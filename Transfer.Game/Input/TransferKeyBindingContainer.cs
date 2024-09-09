using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Input.Bindings;
using osuTK.Input;

namespace Transfer.Game.Input
{
    public partial class TransferKeyBindingContainer : KeyBindingContainer<VideoInputAction>
    {
        public override IEnumerable<IKeyBinding> DefaultKeyBindings => new[]
        {
            new KeyBinding(new[] { InputKey.Space}, VideoInputAction.Stop)
        };

    }
}