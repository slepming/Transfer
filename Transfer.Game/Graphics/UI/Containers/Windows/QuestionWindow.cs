using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Localisation;
using osu.Framework.Logging;
using osuTK;

namespace Transfer.Game.Graphics.UI.Containers.Windows;

public partial class QuestionWindow : MenuWindow
{
    public LocalisableString QuestionText;
    public StringButton Ok, Cancel;
    private TextFlowContainer text;

    public Action<bool> Answer;

    public QuestionWindow()
    {
        Size = new Vector2(1 / 2f, 1 / 4f);
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        WindowContent.AddRange([
            Ok = new StringButton
            {
                Width = 50,
                Height = 25,
                Text = "Ok",
                Anchor = Anchor.BottomLeft,
                Origin = Anchor.BottomLeft,
                Action = okAnswer
            },
            Cancel = new StringButton
            {
                Width = 50,
                Height = 25,
                Text = "Cancel",
                Anchor = Anchor.BottomRight,
                Origin = Anchor.BottomRight,
                Action = cancelAnswer
            },
            text = new TextFlowContainer()
            {
                // Font = new FontUsage(size: 15, family: TransferFonts.FiraCodeNerdFontLight),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
            }
        ]);
        text.Text = QuestionText;
    }

    private void okAnswer()
    {
        Logger.Log("User click on 'OK'", level: LogLevel.Debug);
        Answer?.Invoke(true);
        Hide();
        this.Expire();
    }

    private void cancelAnswer()
    {
        Logger.Log("User click on 'Cancel'", level: LogLevel.Debug);
        Answer?.Invoke(false);
        Hide();
        this.Expire();
    }
}
