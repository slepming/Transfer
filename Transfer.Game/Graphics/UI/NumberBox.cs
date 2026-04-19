using osu.Framework.Bindables;

namespace Transfer.Game.Graphics.UI;

public partial class NumberBox(bool handleNonPositionalInput = false, bool requestsFocus = false) : TransferTextBox(handleNonPositionalInput, requestsFocus)
{
    public override bool OnlyNumbers { get; } = true;

    public BindableInt bindableNumber { get; set; }

    protected override bool CanAddCharacter(char character)
    {
        if (character == '\0') return false;

        if (OnlyNumbers)
        {
            if (bindableNumber != null)
            {
                return char.IsDigit(character) && bindableNumber.MinValue <= character && character <= bindableNumber.MaxValue;
            }

            return char.IsDigit(character) || character == '.';
        }

        return true;
    }
}
