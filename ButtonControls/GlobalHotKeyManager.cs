using ArtistHelper.Model;
using ArtistHelper.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ArtistHelper.ButtonControls
{
    // TODO Перепиши это говно уже plz
    public sealed class GlobalHotKeyManager : IDisposable
    {
        #region HotKeyEvent
        //public event EventHandler<bool> StatusProcessEvent;
        public event EventHandler<bool> CanvasActivEvent; 
        public event EventHandler<bool> CreateControlPanelEvent;
        public event EventHandler<bool> CreateImageCreaterEvent;
        public event EventHandler<bool> ImageFixEvent; 
        public event EventHandler<bool> CtrlButtonPressEvent;
        public event EventHandler<bool> MirrorImage;
        public event EventHandler<bool> AltButtonPressEvent;
        private Factory _Factory = new();
        #endregion HotKeyEvent
        internal LowLevelKeyboardListener _listener;
        public bool StartedProcess { get; private set; } = true;
        public bool FixImage { get; private set; }
        private bool PressedLeftCtrl { get; set; }
        private bool PressedLeftShift { get; set; }
        private bool PressedLeftAlt { get; set; }
        private bool PressedNumPad1 { get; set; }
        private bool PressedNumPad2 { get; set; }
        private bool PressedM { get; set; }
        private bool StartHK { get; set; }
        #region InitKey
        private const Key _keyLeftCtrl = Key.LeftCtrl;
        private const Key _keyLeftShift = Key.LeftShift;
        private const Key _keyLeftAlt = Key.LeftAlt;
        private const Key _keyF1 = Key.F1;
        private const Key _keyM = Key.M;
        private const Key _keyNumPad1 = Key.NumPad1;
        private const Key _keyNumPad2 = Key.NumPad2;
        #endregion InitKey
        internal GlobalHotKeyManager()
        {
            _listener = new LowLevelKeyboardListener();
            _listener.OnKeyDown += _listener_OnKeyDown;
            _listener.OnKeyUp += _listener_OnKeyUp;

            _listener.HookKeyboard();
        }
        private async void _listener_OnKeyDown(object sender, KeyDownArgs e)
        {
            switch (e.KeyDown)
            {
                case _keyLeftCtrl:
                    PressedLeftCtrl = true;
                    break;
                case _keyF1:
                    StartHK = true;
                    break;
                case _keyM:
                    PressedM = true;
                    break;
                case _keyLeftShift:
                    PressedLeftShift = true;
                    break;
                case _keyLeftAlt:
                    PressedLeftAlt = true;
                    break;
                case _keyNumPad1:
                    PressedNumPad1 = true;
                    break;
                case _keyNumPad2:
                    PressedNumPad2 = true;
                    break;
            }

            await HotkeyStart();
        }

        private async Task HotkeyStart()
        {
            if (PressedLeftCtrl)
            {
                CtrlButtonPressEvent?.Invoke(null, PressedLeftCtrl);
                //PressedLeftCtrl = false;
                //PrePressedLeftCtrl = PressedLeftCtrl;
            }
            if (PressedLeftAlt)
            {
                AltButtonPressEvent?.Invoke(null, PressedLeftAlt);
            }
            if (PressedLeftCtrl && StartHK && !PressedLeftShift)
            {
                if (!Application.Current.Windows.OfType<Window>().Any(w => w.GetType().Name == "CanvasLayer"))
                {
                    _Factory.CreateCanvasLayerWindow();
                }
                StartedProcess = !StartedProcess;
                CanvasActivEvent?.Invoke(null, StartedProcess);
            }
            if (PressedLeftCtrl && PressedLeftShift && StartHK)
            {
                FixImage = !FixImage;
                ImageFixEvent?.Invoke(null, FixImage);
                PressedLeftCtrl = false;
                PressedLeftShift = false;
                StartHK = false;
            }
            if (PressedLeftCtrl && PressedLeftAlt && PressedNumPad1)
            {
                _Factory.CreateControlPanelWindow();                
            }
            if (PressedLeftCtrl && PressedLeftAlt && PressedNumPad2)
            {
                _Factory.CreateImageCreatorWindow();
            }
            if (PressedLeftCtrl && PressedM)
            {
                MirrorImage?.Invoke(this, PressedM);
            }
        }
        private async void _listener_OnKeyUp(object sender, KeyUpArgs e)
        {
            switch (e.KeyUp)
            {
                case _keyLeftCtrl:
                    PressedLeftCtrl = false;
                    CtrlButtonPressEvent?.Invoke(null, PressedLeftCtrl);
                    break;
                case _keyF1:
                    StartHK = false;
                    break;
                case _keyM:
                    PressedM = false;
                    break;
                case _keyLeftShift:
                    PressedLeftShift = false;
                    break;
                case _keyLeftAlt:
                    PressedLeftAlt = false;
                    AltButtonPressEvent?.Invoke(null, PressedLeftAlt);
                    break;
                case _keyNumPad1:
                    PressedNumPad1 = false;
                    break;
                case _keyNumPad2:
                    PressedNumPad2 = false;
                    break;
            }
        }
        public void Dispose()
        {
            _listener.UnHookKeyboard();
        }
    }
}
