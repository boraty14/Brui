using System.Runtime.InteropServices;
using UnityEngine;

namespace Brui.Utils
{
    public static class PlatformChecker
    {
        // Import JavaScript function to detect OS
        [DllImport("__Internal")]
        private static extern string GetOperatingSystem();

        // Method 1: Using JavaScript
        public static  void CheckOSUsingJavaScript()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            string os = GetOperatingSystem();
            Debug.Log($"Operating System (JS): {os}");
#endif
        }

        // Method 2: Using Application.platform
        public static void CheckOSUsingUnity()
        {
            // These will work during development
            #if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
                Debug.Log("Running on Windows");
            #elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
                Debug.Log("Running on macOS");
            #elif UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX
                Debug.Log("Running on Linux");
            #elif UNITY_ANDROID
                Debug.Log("Running on Android");
            #elif UNITY_IOS
                Debug.Log("Running on iOS");
            #endif
        }

        // Method 3: Using SystemInfo
        public static void CheckOSUsingSystemInfo()
        {
            string os = SystemInfo.operatingSystem;
            Debug.Log($"Operating System (SystemInfo): {os}");
        }
    }
}