using NUnit.Framework;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osuTK;
using Transfer.Game.Graphics.UIv2;

namespace Transfer.Game.Tests.Visual.UI.Sliders;

public partial class VolumeSliderTestScene : TransferTestScene
{
    private VolumeSlider slider;
    private BindableDouble current;

    [SetUp]
    public void Setup()
    {
        Clear();
        Add(slider = new VolumeSlider()
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Width = 400,
            Height = 100,
            Current = current = new BindableDouble()
            {
                MinValue = 0,
                MaxValue = 1,
                Value = 0.5
            }
        });
    }

    [Test]
    public void TestSizes()
    {
        AddStep("Add Resize sliders", () =>
        {
            AddSliderStep("Resize W", 0, 500, 500, v =>
            {
                slider.ResizeTo(new Vector2(v, slider.Height), 200, Easing.OutBack);
            });
            AddSliderStep("Resize H", 0, 500, 100, v =>
            {
                slider.ResizeTo(new Vector2(slider.Width, v), 200, Easing.OutBack);
            });
        });
    }
}
