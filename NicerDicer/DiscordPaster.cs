using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using WindowsInput;

namespace NicerDicer
{
    public class DiscordPaster
    {

        [DllImport("user32.dll", SetLastError = true)]
        static extern void SwitchToThisWindow(IntPtr hWnd, bool turnOn);
        [STAThread]
        public static bool PostInDiscord(string text, string channelName) {
            try {
                Process bProcess = Process.GetProcessesByName("Discord").FirstOrDefault();
                if (bProcess != null && bProcess.MainWindowTitle == $"{channelName} - Discord") {
                    SwitchToThisWindow(bProcess.MainWindowHandle, true);
                    new InputSimulator().Keyboard.TextEntry(text);
                    new InputSimulator().Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RETURN);
                    return true;
                }
                return false;
            }
            catch { return false; }
        }

    }
}