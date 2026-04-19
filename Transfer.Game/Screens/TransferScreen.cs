using System;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Logging;
using osu.Framework.Screens;
using Transfer.Game.Graphics.UI;
using Transfer.Game.Graphics.UI.Containers.Dialogs;

namespace Transfer.Game.Screens
{
    public abstract partial class TransferScreen : Screen, ITransferScreen, IHasDescription
    {
        protected Loading LoadingComponent;
        public virtual string Title => GetType().Name;
        public string Description => Title;

        private readonly Box backgroundFill;

        // ReSharper disable once PublicConstructorInAbstractClass
        protected TransferScreen()
        {
            Logger.Log($"Initialization {Description}");
            AddInternal(backgroundFill = CreateBackground().With(b => b.RelativeSizeAxes = Axes.Both));
        }

        protected virtual void OnExit() => this.Exit();

        public override void OnEntering(ScreenTransitionEvent e)
        {
            this.FadeInFromZero(1000, Easing.Out);
            base.OnEntering(e);
        }

        public override bool OnExiting(ScreenExitEvent e)
        {
            this.FadeIn(1000, Easing.InOutQuad).ScaleTo(0.1f, 2000, Easing.In);
            return base.OnExiting(e);
        }

        protected virtual Box CreateBackground() => new Box()
        {
            RelativeSizeAxes = Axes.Both,
            Depth = float.MaxValue,
            Colour = Colour4.Transparent
        };

        protected void Exception(Exception transferException)
        {
            AddInternal(new ErrorDialog()
            {
                Title = Title,
                Message = transferException.Message,
                Depth = 1000
            });
        }
    }
}
