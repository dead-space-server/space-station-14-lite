using Robust.Shared.Configuration;

namespace Content.Shared.DeadSpace.CCCCVars;

/// <summary>
///     DeadSpace modules console variables
/// </summary>
[CVarDefs]
// ReSharper disable once InconsistentNaming
public sealed class CCCCVars
{
    /*
	* GCF
	*/

    /// <summary>
    ///     Whether GCF being shown is enabled at all.
    /// </summary>
    public static readonly CVarDef<bool> GCFEnabled =
        CVarDef.Create("gcf_auto.enabled", true);

    /// <summary>
    ///     Notify for admin about GCF Clean.
    /// </summary>
    public static readonly CVarDef<bool> GCFNotify =
        CVarDef.Create("gcf_auto.notify", false);

    /// <summary>
    ///     The number of seconds between each GCF
    /// </summary>
    public static readonly CVarDef<float> GCFFrequency =
        CVarDef.Create("gcf_auto.frequency", 300f);

    /*
	* InfoLinks
	*/

    /// <summary>
    /// Link to wiki page with roles description in Rules menu.
    /// </summary>
    public static readonly CVarDef<string> InfoLinksRoles =
        CVarDef.Create("infolinks.roles", "", CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    /// Link to wiki page with space laws in Rules menu.
    /// </summary>
    public static readonly CVarDef<string> InfoLinksLaws =
        CVarDef.Create("infolinks.laws", "", CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    /// IPs address for reconnect.
    /// </summary>
    public static readonly CVarDef<string> InfoLinksIPs =
        CVarDef.Create("infolinks.ips", "", CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    /// Multiplier for playtime.
    /// </summary>
    public static readonly CVarDef<float> PlayTimeMultiplier =
        CVarDef.Create("playtime.multiplier", 1f, CVar.SERVER | CVar.REPLICATED);

    /*
	* TTS
	*/

    public static readonly CVarDef<bool> RadioTTSSoundsEnabled =
        CVarDef.Create("audio.radio_tts_sounds_enabled", true, CVar.ARCHIVE | CVar.CLIENTONLY);

    /*
    * Typan
    */

    public static readonly CVarDef<bool> TypanEnabled =
        CVarDef.Create("typan.enabled", false, CVar.SERVERONLY);

    /*
    * Bans webhook
    */

    public static readonly CVarDef<string> DiscordBansWebhook =
        CVarDef.Create("discord.bans_webhook", string.Empty, CVar.SERVERONLY | CVar.CONFIDENTIAL);


    /*
    * Notes webhook
    */

    public static readonly CVarDef<string> AdminNoteWebhookEmbedColor =
        CVarDef.Create("admin_note_webhook.embed_color", "ff8c00", CVar.ARCHIVE | CVar.SERVERONLY | CVar.SERVER);
}
