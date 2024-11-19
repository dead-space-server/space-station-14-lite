using Content.Server.Maps;
using Robust.Shared.Prototypes;

namespace Content.Server.DeadSpace.Typan.Components;

[RegisterComponent]
public sealed partial class StationTypanLoaderComponent : Component
{
    [DataField]
    public ProtoId<GameMapPrototype> Station = "DSTypan";
}
