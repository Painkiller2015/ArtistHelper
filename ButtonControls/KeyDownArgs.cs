using System;
using System.Windows.Input;

namespace ArtistHelper.ButtonControls
{
    internal class KeyDownArgs : EventArgs
    {
        public Key KeyDown { get; private set; }

        internal KeyDownArgs(Key key) => KeyDown = key;
    }
}
