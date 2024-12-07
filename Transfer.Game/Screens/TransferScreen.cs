using osu.Framework.Screens;
using osu.Framework.Graphics;
using osu.Framework.Logging;
using osu.Framework.Bindables;
using Transfer.Game.UserInterface;
using Transfer.Game.UserInterface.Containers;
using Transfer.Game.Extensions;
using Transfer.Game.Graphics.UI;

namespace Transfer.Game.Screens
{
    public abstract partial class TransferScreen : Screen, ITransferScreen, IHasDescription
    {
        protected Loading LoadingComponent;
        protected DisappearingIconButton OptionsButton;
        public virtual string Title => GetType().Name;
        public string Description => Title;
        public bool OptionsButtonVisible = true;

        public bool CursorVisible = true;

        protected Bindable<string> NotifyError = new Bindable<string>();


        public TransferScreen(){
            Logger.Log($"Initialization {Description}");
        }
        protected virtual void OnExit() => this.Exit();


        public override void OnEntering(ScreenTransitionEvent e)
        {
            this.FadeInFromZero(1000, Easing.InOutElastic);
            base.OnEntering(e);

        }

        protected override void LoadComplete()
        {
            if(OptionsButtonVisible){
                // OptionsButton.Show();
            }
            base.LoadComplete();
        }

        public override bool OnExiting(ScreenExitEvent e)
        {

            this.FadeIn(1000,Easing.InOutQuad).ScaleTo(0.1f,2000,Easing.InOutQuad);
            return base.OnExiting(e);
        }
        protected void OnException(TransformException transformException)
        {
            AddInternal(new ExceptionContainer{
                AutoSizeAxes = Axes.X,
                Height = 50,
            });
        }


    }
}
