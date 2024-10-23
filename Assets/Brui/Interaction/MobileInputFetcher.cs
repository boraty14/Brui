using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Brui.Interaction
{
    public class MobileInputFetcher : INodeInputFetcher
    {
        public Vector2 GetPointerPosition()
        {
            return Touch.activeTouches.Count > 0 ? Touch.activeTouches[0].screenPosition : Vector2.zero;
        }

        public bool IsPointerDown()
        {
            return Touch.activeTouches.Count > 0;
        }

        public bool IsPressedThisFrame()
        {
            return Touch.activeTouches.Count > 0 && Touch.activeTouches[0].phase == TouchPhase.Began;
        }

        public bool IsReleasedThisFrame()
        {
            return Touch.activeTouches.Count > 0 && Touch.activeTouches[0].phase == TouchPhase.Ended;
        }
    }
}