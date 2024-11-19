using Robust.Shared.Serialization;

namespace Content.Shared.DeadSpace.Photocopier;

[Serializable, NetSerializable]
public enum PhotocopierVisuals : byte
{
    VisualState,
}

[Serializable, NetSerializable]
public enum PhotocopierVisualState : byte
{
    Normal,
    Scanning,
    Printing
}
