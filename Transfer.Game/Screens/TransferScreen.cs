using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        protected IconButton IconButton;
        public virtual string Title => GetType().Name;
        public string Description => Title;
        protected bool IsShowOptionsButton = true;




        public TransferScreen(){
            Logger.Log($"Initialization {Description}");
        }
        protected virtual void OnExit() => this.Exit();


        public override void OnEntering(ScreenTransitionEvent e)
        {
            base.OnEntering(e);
            this.FadeInFromZero(1000, Easing.InOutElastic);

        }

        protected override void LoadComplete()
        {
            if(IsShowOptionsButton){
                // IconButton.Show();
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
