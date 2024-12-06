using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osuTK;
using Transfer.Game.Configuration;
using Transfer.Game.Input.Bindings;

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
        InternalChildren = new Drawable[]
        {
            scrollableConfigContainer = new TransferScrollContainer
            {
                RelativeSizeAxes = Axes.Both,
                Masking = true,
                Children = new Drawable[]
                {
                    new SpriteText{
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopLeft,
                        Text = "Options",
                        Position = new Vector2(40, Y),
                        Font = new FontUsage("Oswald", size: 50, fixedWidth: true)
                    },
                    appSettings = new FillFlowContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.TopLeft,
                        Direction = FillDirection.Vertical,
                        Position = new Vector2(X, Y+5),
                        Spacing = new Vector2(0, 20),
                        Children = new Drawable[]{
                            new SpriteText{
                                Text = TransferOptions.AudioBitrate.ToString()
                            }
                        }
                    }
                }
            }

        };
        foreach (var option in Enum.GetValues(typeof(TransferOptions)))
        {
            var configurationValue = transferConfigManager.Get<string>((TransferOptions)option);
            appSettings.Add(new Container
            {
                AutoSizeAxes = Axes.Y,
                Children = new Drawable[]
                {
                    new SpriteText
                    {
                        Text = option.ToString(),
                        Position = new Vector2(40, 0),
                        Font = new FontUsage("Oswald", size: 30, fixedWidth: true)
                    }
                }
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
