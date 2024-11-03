using UnityEngine;

namespace Brui.Runtime.Interaction
{
    public interface INodeInputFetcher
    {
        Vector2 GetPointerPosition();
        bool IsPointerDown();
        bool IsPressedThisFrame();
        bool IsReleasedThisFrame();
    }
}