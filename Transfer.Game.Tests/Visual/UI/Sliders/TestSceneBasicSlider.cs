using NUnit.Framework;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osuTK;
using Transfer.Game.Graphics.UI;
using Transfer.Game.Tests.Visual;

namespace Transfer.Game.Tests.UI.Sliders;

public partial class TestSceneBasicSlider : TransferTestScene
{
    private TransferBasicSliderBar<double> slider;

    public TestSceneBasicSlider()
    {
    }

    [Test]
    public void TestInit()
    {
        AddStep("Initialize", () => Add(slider = new TransferBasicSliderBar<double>()
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
        }));
    }

    [Test]
    public void TestSizes()
    {
        AddAssert("Slider is not null", () => slider != null);
        AddStep("Resize", () => slider.ResizeTo(new Vector2(150, 25), 300, Easing.Out));
    }
}
