using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ArtistHelper.ButtonControls
{
    public sealed class GlobalHotKeyManager : IDisposable
    {
        public event EventHandler<bool> StatusProcessEvent;
        public bool StartedProcess { get; private set; }
        internal LowLevelKeyboardListener _listener;
        private bool PressedLeftCtrl { get;  set; }
        private bool StopHK { get;  set; }
        private bool StartHK { get;  set; }

        private const Key _keyLeftCtrl = Key.LeftCtrl;
        private const Key _keyF1 = Key.F1;


        internal GlobalHotKeyManager()
        {
            _listener = new LowLevelKeyboardListener();
            _listener.OnKeyDown += _listener_OnKeyDown;
            _listener.OnKeyUp += _listener_OnKeyUp;

            _listener.HookKeyboard();
        }
        async void _listener_OnKeyDown(object sender, KeyDownArgs e)
        {
            switch (e.KeyDown)
            {
                case _keyLeftCtrl:
                    PressedLeftCtrl = true;
                    break;

                case _keyF1:
                    StartHK = true;
                    break;
            }

            if (PressedLeftCtrl)
            {
                if (StartedProcess == false && StartHK)
                {
                    StartedProcess = true;
                    StatusProcessEvent?.Invoke(null, StartedProcess);
                    PressedLeftCtrl = false;
                    StartHK = false;
                }
                if (StartedProcess == true && StartHK)
                {
                    StartedProcess = false;
                    StatusProcessEvent?.Invoke(null, StartedProcess);
                    PressedLeftCtrl = false;
                    StopHK = false;
                }
            }
        }
        async void _listener_OnKeyUp(object sender, KeyUpArgs e)
        {
            switch (e.KeyUp)
            {
                case _keyLeftCtrl:
                    PressedLeftCtrl = false;
                    break;
                case _keyF1:
                    StartHK = false;
                    break;
            }
        }
        public void Dispose()
        {
            _listener.UnHookKeyboard();
        }
    }
}
