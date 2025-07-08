using System;
using osu.Framework.Screens;
using osu.Framework.Graphics;
using osu.Framework.Logging;
using osu.Framework.Bindables;
using Transfer.Game.Extensions;
using Transfer.Game.Graphics.UI;
using Transfer.Game.Graphics.UI.Containers;
using Transfer.Game.Graphics.UI.Containers.Dialogs;

namespace Transfer.Game.Screens
{
    public abstract partial class TransferScreen : Screen, ITransferScreen, IHasDescription
    {
        protected Loading LoadingComponent;
        public virtual string Title => GetType().Name;
        public string Description => Title;
        protected bool CursorVisible = true;

        protected Bindable<string> NotifyError = new Bindable<string>();

        // ReSharper disable once PublicConstructorInAbstractClass
        public TransferScreen()
        {
            Logger.Log($"Initialization {Description}");
            NotifyError.ValueChanged += onNotifyError;
        }

        private void onNotifyError(ValueChangedEvent<string> e)
        {
            OnNotifyError(e.NewValue);
        }

        public virtual void CursorHide()
        {
            CursorVisible = !CursorVisible;
        }

        protected virtual void OnExit() => this.Exit();

        public override void OnEntering(ScreenTransitionEvent e)
        {
            this.FadeInFromZero(1000, Easing.Out);
            base.OnEntering(e);
        }

        public override bool OnExiting(ScreenExitEvent e)
        {
            this.FadeIn(1000, Easing.InOutQuad).ScaleTo(0.1f,2000,Easing.In);
            return base.OnExiting(e);
        }

        protected void Exception(Exception transferException) // WHY I DON't USE IT?????!!!
        {
            AddInternal(new ErrorDialog()
            {
                Title = Title,
                Message = transferException.Message,
                Depth = 1000
            });
        }

        protected virtual void OnNotifyError(string text)
        {
        }
    }
}
