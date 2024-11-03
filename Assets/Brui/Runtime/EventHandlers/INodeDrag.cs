using UnityEngine;

namespace Brui.Runtime.EventHandlers
{
    public interface INodeDrag
    {
        void OnBeginDrag(Vector2 position);
        void OnDrag(Vector2 position, Vector2 delta);
        void OnEndDrag(Vector2 position);
    }
}