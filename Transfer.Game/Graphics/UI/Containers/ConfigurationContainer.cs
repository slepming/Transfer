using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Logging;
using osuTK;
using Transfer.Game.Configuration;
using Transfer.Game.Extensions;

namespace Transfer.Game.Graphics.UI.Containers;

public partial class ConfigurationContainer : TransferFocusedOverlayContainer
{
    private FillFlowContainer appSettings;
    private TransferScrollContainer scrollableConfigContainer;

    public ConfigurationContainer()
    {
        RelativeSizeAxes = Axes.Both;
        Anchor = Anchor.CentreLeft;
        Origin = Anchor.Centre;
    }

    [BackgroundDependencyLoader]
    private void load(TransferConfigManager transferConfigManager)
    {
        InternalChildren =
        [
            scrollableConfigContainer = new TransferScrollContainer
            {
                RelativeSizeAxes = Axes.Both,
                Masking = true,
                Children =
                [
                    new SpriteText
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopLeft,
                        Text = "Options",
                        Position = new Vector2(40, 0),
                        Font = new FontUsage(TransferFonts.FiraCodeNerdFontLight, size: 50, fixedWidth: true)
                    },
                    appSettings = new FillFlowContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.BottomLeft,
                        Direction = FillDirection.Vertical,
                        Margin = new MarginPadding() { Vertical = 50 },
                        Spacing = new Vector2(0, 10),
                    }
                ]
            }
        ];

        var configuration = transferConfigManager.GetCurrentConfiguration();

        foreach (var option in configuration)
        {
            Logger.Log($"Option {option.Key} Value {option.Value}");
            appSettings.Add(new Container
            {
                AutoSizeAxes = Axes.Y,
                Children =
                [
                    new FillFlowContainer()
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Direction = FillDirection.Vertical,
                        Spacing = new Vector2(0, 10),
                        Children =
                        [
                            new SpriteText
                            {
                                Text = option.Key.ToString(),
                                Position = new Vector2(40, 0),
                                Font = new FontUsage(TransferFonts.FiraCodeNerdFontLight, size: 30)
                            },
                        ]
                    }
                ]
            });
        }
    }

    protected override void PopIn()
    {
        this.MoveTo(new Vector2(0, Y), 500, Easing.InOutQuad);
    }

    protected override void PopOut()
    {
        this.MoveTo(new Vector2(-250, Y), 500, Easing.InOutQuad);
    }
}
