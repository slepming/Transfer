using System.IO;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osuTK;
using Transfer.Game.Extensions;
using Transfer.Game.Graphics.UI;
using Transfer.Game.Graphics.UI.Containers;
using Transfer.Game.Graphics.UIv2.Buttons;

namespace Transfer.Game.Graphics.UIv2;

public partial class TransferFileSelector(string initialPath = null, string[] validFileExtensions = null) : FileSelector(initialPath, validFileExtensions)
{
    protected override Drawable CreateHiddenToggleButton() => new TransferCheckbox(s => s.Font = new FontUsage(family: TransferFonts.Oswald, size: 20f))
    {
        LabelText = "Toggle hidden items",
        FadeDuration = 200,
        FadeEasing = Easing.OutQuint,
        CheckboxSize = new Vector2(25)
    }.With(b => b.Current.BindValueChanged(_ => ShowHiddenItems.Toggle()));

    protected override ScrollContainer<Drawable> CreateScrollContainer() => new TransferScrollContainer();

    protected override DirectorySelectorBreadcrumbDisplay CreateBreadcrumb() => new TransferDirectorySelectorBreadcrumbDisplay();

    protected override DirectorySelectorDirectory CreateDirectoryItem(DirectoryInfo directory, LocalisableString? displayName = null) => new TransferDirectorySelectorDirectory(directory, displayName.ToString());

    protected override DirectorySelectorDirectory CreateParentDirectoryItem(DirectoryInfo directory) => new TransferDirectorySelectorParentDirectory(directory);

    protected override DirectoryListingFile CreateFileItem(FileInfo file) => new TransferListingFile(file);

    private partial class TransferListingFile(FileInfo file) : DirectoryListingFile(file)
    {
        protected override IconUsage? Icon => FontAwesome.Regular.FileVideo;

        private Box background;

        [BackgroundDependencyLoader]
        private void load()
        {
            Flow.AutoSizeAxes = Axes.X;
            Flow.Height = 16;
            Masking = true;
            CornerRadius = 5;

            AddInternal(background = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.FromHSL(0, 0, .6f, .1f),
            });
        }

        protected override SpriteText CreateSpriteText() => new SpriteText()
        {
            Font = new FontUsage(family: TransferFonts.FiraCodeNerdFont, size: FONT_SIZE)
        };

        protected override bool OnHover(HoverEvent e)
        {
            background.FadeColour(Colour4.FromHSL(0, 0, .6f, .3f), 200, Easing.OutQuint);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            background.FadeColour(Colour4.FromHSL(0, 0, .6f, .1f), 200, Easing.OutQuint);
            base.OnHoverLost(e);
        }
    }
}
