using NUnit.Framework;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osuTK;
using Transfer.Game.Graphics.UI;

namespace Transfer.Game.Tests.Visual.UI.Sliders;

public partial class TestSceneBasicSlider : TransferTestScene
{
    private TransferBasicSliderBar<double> slider;

    public TestSceneBasicSlider()
    {
    }

    [SetUp]
    public void TestInit() =>
        Schedule(() => Child = slider = new TransferBasicSliderBar<double>()
        {
            Width = 400,
            Height = 200,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Current = new BindableDouble()
            {
                MinValue = 0,
                MaxValue = 1,
                Default = 0.5,
            }
        });

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
