using UnityEngine;
using UnityEngine.InputSystem;

namespace Brui.Runtime.Interaction
{
    public class DesktopInputFetcher : INodeInputFetcher
    {
        public Vector2 GetPointerPosition()
        {
            return Mouse.current.position.ReadValue();
        }

        public bool IsPointerDown()
        {
            return Mouse.current.leftButton.isPressed;
        }

        public bool IsPressedThisFrame()
        {
            return Mouse.current.leftButton.wasPressedThisFrame;
        }

        public bool IsReleasedThisFrame()
        {
            return Mouse.current.leftButton.wasReleasedThisFrame;
        }
    }
}