using UnityEngine;

namespace Brui.Interaction
{
    public interface INodeInputFetcher
    {
        Vector2 GetPointerPosition();
        bool IsPointerDown();
        bool IsPressedThisFrame();
        bool IsReleasedThisFrame();
    }
}