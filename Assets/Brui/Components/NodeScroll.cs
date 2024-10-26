using System;
using Brui.EventHandlers;
using UnityEngine;

namespace Brui.Components
{
    [RequireComponent(typeof(NodeCollider))]
    public class NodeScroll : NodeComponent, INodeDrag
    {
        public NodeScrollSettings ScrollSettings;

        public void OnBeginDrag(Vector2 position)
        {
        }

        public void OnEndDrag(Vector2 position)
        {
        }

        public void OnDrag(Vector2 position, Vector2 delta)
        {
        }
    }

    [Serializable]
    public class NodeScrollSettings
    {
        public bool IsHorizontal;
        public bool IsVertical;
    }
}