using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace craftersmine.GameEngine.Input
{
    /// <summary>
    /// Allows to get Keyboard input
    /// </summary>
    public sealed class Keyboard
    {
        private static Keys keys { get; set; }
        private static Keys modKeys { get; set; }

        /// <summary>
        /// [Internal Engine Method] Updates information about keys
        /// </summary>
        /// <param name="pressedKeys"></param>
        /// <param name="modifiers"></param>
        public static void UpdateKeys(Keys pressedKeys, Keys modifiers)
        {
            keys = pressedKeys;
            modKeys = modifiers;
        }

        /// <summary>
        /// Returns pressed keys
        /// </summary>
        /// <returns>Pressed <see cref="Keys"/></returns>
        public static Keys GetKeys()
        {
            return keys;
        }

        /// <summary>
        /// Returns pressed keys modifiers
        /// </summary>
        /// <returns>Pressed <see cref="Keys"/> modifiers</returns>
        public static Keys GetKeyModifiers()
        {
            return modKeys;
        }

        /// <summary>
        /// <code>true</code> if <paramref name="key"/> pressed, else <code>false</code>
        /// </summary>
        /// <param name="key">Key for check</param>
        /// <returns></returns>
        public static bool IsKeyDown(Keys key)
        {
            return (GetKeyState(Convert.ToInt16(key)) & 0X80) == 0X80;
        }

        /// <summary>
        ///  If the high-order bit is 1, the key is down; otherwise, it is up.
        ///  If the low-order bit is 1, the key is toggled. 
        ///  A key, such as the CAPS LOCK key, is toggled if it is turned on. 
        ///  The key is off and untoggled if the low-order bit is 0. 
        ///  A toggle key's indicator light (if any) on the keyboard will be on when 
        ///  the key is toggled, and off when the key is untoggled.
        /// </summary>
        /// <param name="nVirtKey"></param>
        [DllImport("user32.dll")]
        public extern static Int16 GetKeyState(Int16 nVirtKey);
    }
}
