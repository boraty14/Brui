#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
#define GAME_WINDOWS
#endif

#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
#define GAME_MAC
#endif

#if UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX
#define GAME_LINUX
#endif

#if GAME_WINDOWS
using System.Runtime.InteropServices;
#endif

namespace Brui.Utils
{
    public static class CapsLockDetector 
    {
#if GAME_WINDOWS
    [DllImport("user32.dll")]
    private static extern short GetKeyState(int keyCode);
    
    // Virtual key code for Caps Lock
    private const int VK_CAPITAL = 0x14;
#endif

#if GAME_MAC
    [DllImport("__Internal")]
    private static extern bool IsCapsLockOnMac();
#endif

        public static bool IsCapsLockActive()
        {
            
#if GAME_WINDOWS
            return (((ushort)GetKeyState(VK_CAPITAL)) & 0x0001) != 0;
            
#elif GAME_MAC
            return IsCapsLockOnMac();
            
#elif GAME_LINUX
            return false;
#endif
#pragma warning disable CS0162 // Unreachable code detected
            return false;
#pragma warning restore CS0162 // Unreachable code detected
        }
    }
}