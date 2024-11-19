using Content.Shared.Examine;

namespace Content.Server.DeadSpace.Photocopier;

public sealed class TonerCartridgeSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<TonerCartridgeComponent, ExaminedEvent>(OnExamined);
    }

    private void OnExamined(EntityUid uid, TonerCartridgeComponent component, ExaminedEvent args)
    {
        if (!args.IsInDetailsRange)
            return;

        args.PushText(component.CurrentAmount == 0 ? Loc.GetString("toner-component-examine-empty")
            : Loc.GetString("toner-component-examine", ("left", component.CurrentAmount), ("max", component.MaxAmount)));
    }
}
