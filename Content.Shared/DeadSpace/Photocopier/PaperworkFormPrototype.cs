using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Robust.Shared.Utility;

namespace Content.Shared.DeadSpace.Photocopier;

[Prototype("paperworkForm")][Serializable, NetSerializable]
public sealed partial class PaperworkFormPrototype : IPrototype
{
    [ViewVariables][IdDataField] public string ID { get; private set; } = default!;

    [DataField("category")] public string Category { get; private set; } = default!;

    [DataField("name", required: true)] public string Name = default!;

    [DataField("text", required: true)] public ResPath Text = default!;

    [DataField("paperPrototype", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string PaperPrototype = default!;
}
