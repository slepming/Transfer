using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace Transfer.Game.Graphics.UIv2;

public partial class LoadingContainer : Container
{
    private Box loadingBox;
    private Box loadingBackground;
    private Container loadingComponent;
    private Container loadingBorderContainer;

    [BackgroundDependencyLoader]
    private void load()
    {
        loadingBox = new Box
        {
            RelativeSizeAxes = Axes.Both,
            Colour = Colour4.White,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
        };
        loadingBackground = new Box
        {
            RelativeSizeAxes = Axes.Both,
            Colour = Colour4.Gray.Opacity(0.2f),
        };
        loadingComponent = new Container()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativeSizeAxes = Axes.Both,
            Size = new Vector2(0.7f, 0.7f),
            Child = loadingBox
        };
        loadingBorderContainer = new Container()
        {
            RelativeSizeAxes = Axes.Both,
            Masking = true,
            BorderColour = Colour4.White.Opacity(0.6f),
            BorderThickness = 2,
            CornerRadius = 2,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Children =
            [
                loadingBackground,
                loadingComponent
            ]
        };
        InternalChild = loadingBorderContainer;
        loadingComponent.Loop(a => a
                                   .ScaleTo(new Vector2(0.1f, 0.7f)).ScaleTo(new Vector2(1f, 0.7f), 1200, Easing.OutBounce).Then().ScaleTo(new Vector2(0.1f, 0.7f), 1800, Easing.InBounce));
    }
}
