namespace Content.Server.DeadSpace.Photocopier;

[RegisterComponent]
public sealed partial class TonerCartridgeComponent : Component
{
    [DataField("maxAmount")]
    public int MaxAmount = 30;

    [DataField("currentAmount")]
    public int CurrentAmount = 30;
}
