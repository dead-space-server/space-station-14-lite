using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.DeadSpace.HardsuitIdentification;

[RegisterComponent]
public sealed partial class HardsuitIdentificationComponent : Component
{
    [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string Action = "ActionHardsuitSaveDNA";

    [DataField]
    public EntityUid? ActionEntity;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public string DNA = String.Empty;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool DNAWasStored = false;

    [DataField]
    public bool Activated = false;

    /// <summary>
    /// Emag sound effects.
    /// </summary>
    [DataField]
    public SoundSpecifier SparkSound = new SoundCollectionSpecifier("sparks")
    {
        Params = AudioParams.Default.WithVolume(8),
    };
}
