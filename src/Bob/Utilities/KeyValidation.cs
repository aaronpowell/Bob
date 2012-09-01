using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Bob.Utilities
{
    public static class KeyValidation
    {
        public static bool IsModifier(VirtualKey key)
        {
            switch (key)
            {
                case VirtualKey.Shift:
                case VirtualKey.Control:
                case VirtualKey.Tab:
                case VirtualKey.Enter:
                    return true;
            }

            return false;
        }

        public static bool IsNumeric(VirtualKey key, bool checkModifiers = true)
        {
            if (checkModifiers && Window.Current.CoreWindow.GetKeyState(VirtualKey.Shift).HasFlag(CoreVirtualKeyStates.Down))
                return false;

            switch (key)
            {
                case VirtualKey.NumberPad0:
                case VirtualKey.NumberPad1:
                case VirtualKey.NumberPad2:
                case VirtualKey.NumberPad3:
                case VirtualKey.NumberPad4:
                case VirtualKey.NumberPad5:
                case VirtualKey.NumberPad6:
                case VirtualKey.NumberPad7:
                case VirtualKey.NumberPad8:
                case VirtualKey.NumberPad9:
                case VirtualKey.Number0:
                case VirtualKey.Number1:
                case VirtualKey.Number2:
                case VirtualKey.Number3:
                case VirtualKey.Number4:
                case VirtualKey.Number5:
                case VirtualKey.Number6:
                case VirtualKey.Number7:
                case VirtualKey.Number8:
                case VirtualKey.Number9:
                    return true;
            }

            return false;
        }

        public static bool IsLetter(VirtualKey key)
        {
            switch (key)
            {
                case VirtualKey.A:
                case VirtualKey.B:
                case VirtualKey.C:
                case VirtualKey.D:
                case VirtualKey.E:
                case VirtualKey.F:
                case VirtualKey.G:
                case VirtualKey.H:
                case VirtualKey.I:
                case VirtualKey.J:
                case VirtualKey.K:
                case VirtualKey.L:
                case VirtualKey.M:
                case VirtualKey.N:
                case VirtualKey.O:
                case VirtualKey.P:
                case VirtualKey.Q:
                case VirtualKey.R:
                case VirtualKey.S:
                case VirtualKey.T:
                case VirtualKey.U:
                case VirtualKey.V:
                case VirtualKey.W:
                case VirtualKey.X:
                case VirtualKey.Y:
                case VirtualKey.Z:
                case VirtualKey.Space:
                    return true;
            }
            return false;
        }
    }
}