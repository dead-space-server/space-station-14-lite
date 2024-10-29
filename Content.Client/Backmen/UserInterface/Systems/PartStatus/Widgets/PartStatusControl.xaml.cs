using Content.Shared.Targeting;
using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;
using Robust.Shared.Utility;
using System.Linq;

namespace Content.Client.Backmen.UserInterface.Systems.PartStatus.Widgets;

[GenerateTypedNameReferences]
public sealed partial class PartStatusControl : UIWidget
{
    private readonly Dictionary<TargetBodyPart, TextureRect> _partStatusControls;
    private readonly PartStatusUIController _controller;
    public PartStatusControl()
    {
        RobustXamlLoader.Load(this);

        _controller = UserInterfaceManager.GetUIController<PartStatusUIController>();
        _partStatusControls = new Dictionary<TargetBodyPart, TextureRect>
        {
            { TargetBodyPart.Head, DollHead },
            { TargetBodyPart.Torso, DollTorso },
            { TargetBodyPart.LeftArm, DollLeftArm },
            { TargetBodyPart.RightArm, DollRightArm },
            { TargetBodyPart.LeftLeg, DollLeftLeg },
            { TargetBodyPart.RightLeg, DollRightLeg }
        };
    }

    public void SetTextures(Dictionary<TargetBodyPart, TargetIntegrity> state)
    {
        foreach (var (bodyPart, integrity) in state)
        {
            string enumName = Enum.GetName(typeof(TargetBodyPart), bodyPart) ?? "Unknown";
            int enumValue = (int) integrity;
            var texture = new SpriteSpecifier.Rsi(new ResPath($"/Textures/Interface/Targeting/Status/{enumName.ToLowerInvariant()}.rsi"), $"{enumName.ToLowerInvariant()}_{enumValue}");
            _partStatusControls[bodyPart].Texture = _controller.GetTexture(texture);
        }
    }

    public void SetVisible(bool visible)
    {
        this.Visible = visible;
    }
}
