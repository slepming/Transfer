using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics;
using osu.Framework.Bindables;
using System;
using osuTK;
using osu.Framework.Graphics.UserInterface;
using System.Globalization;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Textures;
using osu.Framework.Logging;

namespace Transfer.Game.Graphics.UI.Containers
{
    public partial class EditToolsContainer : TransferContainer
    {
        private readonly SpeedUpSlider speedUpSlider;
        private readonly TransferTextBox transferTextBox;
        private readonly ConfirmButton confirmButton;
        private Sprite lightningSprite;

        public Action ConfirmAction;

        public BindableDouble SpeedUpValue = new BindableDouble();

        public EditToolsContainer()
        {
            RelativeSizeAxes = Axes.Both;
            InternalChildren =
            [
                speedUpSlider = new SpeedUpSlider(),
                transferTextBox = new NumberBox(true)
                {
                    Width = 40,
                    Height = 43,
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopLeft,
                    BackgroundColour = Colour4.Transparent,
                    BorderColour = Colour4.Transparent,
                    FontSize = 20,
                    Position = new Vector2(50, 0)
                },
                confirmButton = new ConfirmButton
                {
                    Width = 80,
                    Height = 20,
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    Position = new Vector2(0, -5),
                }
            ];
            confirmButton.Enabled.Value = true;
            confirmButton.Action += onFinishEdition;
            speedUpSlider.Current.ValueChanged += speedUpValueChanged;
            transferTextBox.OnCommit += textBoxCommit;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textureStore)
        {
            Texture lightning = textureStore.Get("Lightning");
            if (lightning == null) Logger.Log("Lightning is null");
            AddInternal(lightningSprite = new Sprite
            {
                Texture = lightning,
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Width = 50,
                Height = 50
            });
        }

        private void onFinishEdition()
        {
            ConfirmAction?.Invoke();
            confirmButton.Enabled.Value = false;
        }

        private void textBoxCommit(TextBox sender, bool newText)
        {
            if (newText) speedUpSlider.Current.Value = Convert.ToDouble(sender.Text, CultureInfo.InvariantCulture);
        }

        private void speedUpValueChanged(ValueChangedEvent<double> e)
        {
            SpeedUpValue.Value = e.NewValue;
            transferTextBox.Text = Math.Round(e.NewValue, 2).ToString(CultureInfo.InvariantCulture);
        }
    }
}
