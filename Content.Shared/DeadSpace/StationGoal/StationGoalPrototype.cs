using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.DeadSpace.StationGoal;

[Serializable, Prototype("stationGoal")]
public sealed partial class StationGoalPrototype : IPrototype
{
    [IdDataField] public string ID { get; } = default!;

    [DataField("name", required: true)] public string Name = default!;

    [DataField("text", required: true)] public ResPath Text = default!;
}
