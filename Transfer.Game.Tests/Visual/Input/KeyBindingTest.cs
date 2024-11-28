using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Testing.Input;
using Transfer.Game.Input.Bindings;

namespace Transfer.Game.Tests.Visual.Input
{
    public partial class TestSceneKeyBinding : TransferTestScene
    {
        private readonly KeyBindings keyBindings;
        private ManualInputManager inputManager;
        private GlobalActionContainer globalActionContainer;
        public TestSceneKeyBinding()
        {
            Add(keyBindings = new KeyBindings());
            base.Content.Add(inputManager = new ManualInputManager
            {
                UseParentInput = true,
                Child = globalActionContainer = new GlobalActionContainer(null)
            });
        }
        [Test]
        public void TestDefaultKeyBindings()
        {
            AddStep("Fire key", () => inputManager.Key(osuTK.Input.Key.F1));
            AddAssert("Action", () => keyBindings.Action);
        }
    }
    public partial class KeyBindings : CompositeDrawable, IKeyBindingHandler<GlobalAction>
    {
        public bool Action;
        public bool OnPressed(KeyBindingPressEvent<GlobalAction> e)
        {
            Action = e.Action == GlobalAction.EditorConvertMenu;
            Logger.Log($"KeyBindingTest Pressed: {e.Action.ToString()}");
            return true;
        }

        public void OnReleased(KeyBindingReleaseEvent<GlobalAction> e)
        {
            
        }
    }
}
