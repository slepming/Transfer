#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osu.Framework.Input;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;

namespace Transfer.Game.Input.Bindings
{
    public partial class GlobalActionContainer : KeyBindingContainer<GlobalAction>, IHandleGlobalKeyboardInput, IKeyBindingHandler<GlobalAction>
    {
        private IKeyBindingHandler<GlobalAction>? handler;
        public GlobalActionContainer(TransferGameBase? game) : base(matchingMode: KeyCombinationMatchingMode.Modifiers) {
            if(game is IKeyBindingHandler<GlobalAction> h){
                handler = h;
            }
        }

        public static IEnumerable<KeyBinding> GetBindingsFor(GlobalActionCategory globalActionCategory)
        {
            switch(globalActionCategory)
            {
                case GlobalActionCategory.General:
                    return generalBindings;
                case GlobalActionCategory.Watching:
                    return watchingBindings;
                case GlobalActionCategory.Editor:
                    return videoEditorBindings;
                default:
                    throw new ArgumentOutOfRangeException(nameof(globalActionCategory), globalActionCategory, $"Unexpected {nameof(GlobalActionCategory)}");
            }
        }
        public bool OnPressed(KeyBindingPressEvent<GlobalAction> e) => handler?.OnPressed(e) == true;

        public void OnReleased(KeyBindingReleaseEvent<GlobalAction> e) => handler?.OnReleased(e);
        public override IEnumerable<IKeyBinding> DefaultKeyBindings => generalBindings.Concat(watchingBindings).Concat(videoEditorBindings);
        public static IEnumerable<GlobalAction> GetGlobalActionsFor(GlobalActionCategory category)
            => GetBindingsFor(category).Select(binding => binding.Action).Cast<GlobalAction>().Distinct();
        private static IEnumerable<KeyBinding> generalBindings =>
        [
            new KeyBinding(new[] { InputKey.Control, InputKey.R }, GlobalAction.Explorer),
            new KeyBinding(new[] { InputKey.Control, InputKey.O}, GlobalAction.ConfigurationMenu),
            new KeyBinding(new[] { InputKey.Control,InputKey.LShift, InputKey.D}, GlobalAction.ShortVideoMetaData),
            new KeyBinding(new[] { InputKey.Shift, InputKey.V}, GlobalAction.OpenEditor),
            new KeyBinding(InputKey.F5, GlobalAction.TakeScreenshot)
        ];
        private static IEnumerable<KeyBinding> watchingBindings =>
        [
            new KeyBinding(InputKey.F2, GlobalAction.WatchingRestart),
            new KeyBinding(InputKey.Space, GlobalAction.PauseVideo),
            new KeyBinding(new[] { InputKey.Shift, InputKey.R}, GlobalAction.MoveToInternalExplorer)
        ];
        private static IEnumerable<KeyBinding> videoEditorBindings =>
        [
            new KeyBinding(InputKey.F3, GlobalAction.EditorVideoFullMetaData),
            new KeyBinding(InputKey.F1, GlobalAction.EditorConvertMenu)
        ];
    }

    public enum GlobalAction
    {
        Explorer,

        ConfigurationMenu,

        OpenEditor,

        ShortVideoMetaData,

        EditorVideoFullMetaData,

        EditorConvertMenu,

        WatchingRestart,
        TakeScreenshot,
        PauseVideo,
        MoveToInternalExplorer
    }
    public enum GlobalActionCategory
    {
        General,
        Watching,
        Editor
    }
}
