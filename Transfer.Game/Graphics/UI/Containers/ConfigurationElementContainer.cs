using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using Transfer.Game.Extensions;

namespace Transfer.Game.Graphics.UI.Containers;

public partial class ConfigurationElementContainer : Container
{
    private SpriteText optionSpriteText = new SpriteText(){
        Font = new FontUsage(TransferFonts.Oswald, size:30, fixedWidth: true)
    };

    /// <summary>
    /// For configuration setting(Can be null)
    /// </summary>
    public Drawable Option;

    private string optionName = "None";
    public string OptionName {
        get => optionName;
        set{
            if(optionName == value) return;
            optionName = value;
        }
    }
    public ConfigurationElementContainer()
    {
        optionSpriteText.Text = optionName;
        Option ??= new TransferBasicSliderBar<int>
        {
            Width = 300,
            Height = 5,
            TransferValueOnCommit = true,
            KeyboardStep = 0f,
            SelectionColour = Colour4.AliceBlue,
            Current = new BindableInt(){
                MaxValue = 5,
                MinValue = 0,
                Value = 1,
            },
        };

        InternalChildren = [
            optionSpriteText,
            Option
        ];
    }

}
