using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osu.Framework.Input;
using osu.Framework.Input.Bindings;

namespace Transfer.Game.Input.Bindings
{
    public partial class GlobalActionContainer : KeyBindingContainer<GlobalAction>, IHandleGlobalKeyboardInput
    {

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
        public override IEnumerable<IKeyBinding> DefaultKeyBindings => generalBindings.Concat(watchingBindings).Concat(videoEditorBindings);
        public static IEnumerable<GlobalAction> GetGlobalActionsFor(GlobalActionCategory category)
            => GetBindingsFor(category).Select(binding => binding.Action).Cast<GlobalAction>().Distinct();
        private static IEnumerable<KeyBinding> generalBindings => new[]
        {
            new KeyBinding(new[] { InputKey.Control, InputKey.R }, GlobalAction.Explorer),
            new KeyBinding(new[] { InputKey.LShift, InputKey.C}, GlobalAction.ConfigurationMenu),
            new KeyBinding(new[] { InputKey.Control,InputKey.LShift, InputKey.D}, GlobalAction.ShortVideoMetaData),
            new KeyBinding(new[] { InputKey.Shift, InputKey.V}, GlobalAction.OpenEditor),
            new KeyBinding(InputKey.F5, GlobalAction.TakeScreenshot)
        };
        private static IEnumerable<KeyBinding> watchingBindings => new[]
        {
            new KeyBinding(InputKey.F2, GlobalAction.WatchingRestart)
        };
        private static IEnumerable<KeyBinding> videoEditorBindings => new[]
        {
            new KeyBinding(InputKey.F3, GlobalAction.EditorVideoFullMetaData),
            new KeyBinding(InputKey.F1, GlobalAction.EditorConvertMenu)
        };
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
    }
    public enum GlobalActionCategory
    {
        General,
        Watching,
        Editor
    }
}
