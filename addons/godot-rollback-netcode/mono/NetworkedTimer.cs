using System;

namespace GodotRollbackNetcode
{
    public class NetworkedAnimationPlayer : GDScriptWrapper
    {
        public bool AutoReset
        {
            get => (bool)Source.Get("auto_reset");
            set => Source.Set("auto_reset", value);
        }
    }

    public class NetworkedTimer : GDScriptWrapper
    {
        public bool Autostart
        {
            get => (bool)Source.Get("autostart");
            set => Source.Set("autostart", value);
        }

        public bool OneShot
        {
            get => (bool)Source.Get("one_shot");
            set => Source.Set("one_shot", value);
        }

        public bool WaitTicks
        {
            get => (bool)Source.Get("wait_ticks");
            set => Source.Set("wait_ticks", value);
        }

        public bool HashState
        {
            get => (bool)Source.Get("hash_state");
            set => Source.Set("hash_state", value);
        }

        public NetworkedTimer(Godot.Object source) : base(source)
        {
            source.Connect("timeout", this, nameof(OnTimeout));
        }

        public event Action Timeout;

        private void OnTimeout()
        {
            Timeout?.Invoke();
        }
    }
}
