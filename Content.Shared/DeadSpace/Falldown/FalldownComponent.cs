using Content.Shared.Actions;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.DeadSpace.Falldown;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class FalldownComponent : Component
{
    [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    [AutoNetworkedField]
    public string FalldownAction = "ActionFalldown";

    [DataField, AutoNetworkedField]
    public EntityUid? FalldownActionEntity;

    [DataField, AutoNetworkedField]
    public TimeSpan FallDelay = TimeSpan.FromSeconds(3);

    [DataField, AutoNetworkedField]
    public TimeSpan StandDelay = TimeSpan.FromSeconds(1);

    [AutoNetworkedField]
    public bool IsDown = false;
}

public sealed partial class FalldownActionEvent : InstantActionEvent
{
}
