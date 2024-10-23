using UnityEngine;

namespace Brui.EventHandlers
{
    public interface INodeDrag
    {
        void OnDrag(Vector2 position, Vector2 delta);
    }
}