using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using Transfer.Game.Extensions;
using Transfer.Game.Graphics.UIv2;

namespace Transfer.Game.Graphics.UI.Containers;

public partial class LoadingOverlay : OverlayContainer
{
    protected override bool BlockScrollInput { get; }

    private string header = "None";

    public string Header
    {
        get => header;
        set
        {
            if (header == value) return;
            header = value;
        }
    }

    private LoadingContainer loadingComponent;

    public LoadingOverlay(bool blockScrollInput = true)
    {
        BlockScrollInput = blockScrollInput;
        RelativeSizeAxes = Axes.Both;
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();
        InternalChildren =
        [
            new SpriteText
            {
                Text = Header,
                Font = new FontUsage(TransferFonts.FiraCodeNerdFontBold)
            },
            loadingComponent = new()
            {
                Width = 200,
                Height = 25,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            }
        ];
    }

    protected override void PopIn()
    {
        this.FadeTo(1, 200, Easing.InOutCubic);
    }

    protected override void PopOut()
    {
        this.FadeTo(0, 400, Easing.InOutCubic);
    }
}
