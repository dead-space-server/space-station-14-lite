using System.Threading.Tasks;
using Content.Server.Administration.Notes;
using Content.Server.Database;
using Content.Server.Discord;
using Content.Shared.Administration.Notes;
using Content.Shared.CCVar;
using Content.Shared.Database;
using Content.Shared.DeadSpace.CCCCVars;
using Robust.Shared.Configuration;

namespace Content.Server.DeadSpace.AdminNoteWebhook;

public sealed class AdminNoteWebhook : EntitySystem
{
    [Dependency] private readonly IAdminNotesManager _notes = default!;
    [Dependency] private readonly IServerDbManager _db = default!;
    [Dependency] private readonly DiscordWebhook _discord = default!;
    [Dependency] private readonly IConfigurationManager _config = default!;

    public override void Initialize()
    {
        _notes.NoteAdded += note => Task.Run(async () => await ProcessAdminNote(note));
    }

    private async Task ProcessAdminNote(SharedAdminNote note)
    {
        var webhookUrl = _config.GetCVar(CCCCVars.DiscordBansWebhook);
        var webhookEmbedColor = _config.GetCVar(CCCCVars.AdminNoteWebhookEmbedColor);
        var webhookUsername = _config.GetCVar(CCVars.GameHostName);

        if (webhookUrl == "" || note.Secret || note.NoteType != NoteType.Note)
            return;

        var severity = note.NoteSeverity switch
        {
            NoteSeverity.Minor => Loc.GetString("admin-note-webhook-severity-low"),
            NoteSeverity.Medium => Loc.GetString("admin-note-webhook-severity-medium"),
            NoteSeverity.High => Loc.GetString("admin-note-webhook-severity-high"),
            _ => null,
        };

        if (severity is null)
            return;

        var webhook = await _discord.GetWebhook(webhookUrl);
        if (!webhook.HasValue)
        {
            Log.Error("Failed to get webhook");
            return;
        }

        var player = await _db.GetPlayerRecordByUserId(note.Player);
        if (player is null)
        {
            Log.Error($"Can't find player record for Admin Note Webhook {note.Id} ({note.Player})");
            return;
        }

        var payload = new WebhookPayload()
        {
            Username = webhookUsername,
            Embeds =
            [
                new WebhookEmbed()
                {
                    Title = Loc.GetString("admin-note-embed-title"),
                    Color = int.Parse(webhookEmbedColor, System.Globalization.NumberStyles.HexNumber),
                    Description = string.Join("\n",
                        $"**{Loc.GetString("admin-note-desc-player")}:** {player.LastSeenUserName}",
                        $"**{Loc.GetString("admin-note-desc-reason")}:** {note.Message}",
                        $"**{Loc.GetString("admin-note-desc-severity")}:** {severity}",
                        $"**{Loc.GetString("admin-note-desc-staff")}:** {note.CreatedByName}"),
                    Footer = new WebhookEmbedFooter
                    {
                        Text = $"({Loc.GetString("admin-note-embed-footer")} {note.Round})",
                    },
                },

            ],
        };

        await _discord.CreateMessage(webhook.Value.ToIdentifier(), payload);
    }
}
