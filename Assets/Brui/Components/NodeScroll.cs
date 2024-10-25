using System;
using Brui.EventHandlers;
using UnityEngine;

namespace Brui.Components
{
    [RequireComponent(typeof(NodeTransform))]
    [RequireComponent(typeof(NodeCollider))]
    public class NodeScroll : MonoBehaviour, INodeBeginDrag, INodeEndDrag, INodeDrag
    {
        public NodeScrollSettings ScrollSettings;
        public NodeTransform NodeTransform { get; private set; }

        private void OnValidate()
        {
            SetComponents();
        }

        private void Awake()
        {
            SetComponents();
        }

        private void SetComponents()
        {
            NodeTransform = GetComponent<NodeTransform>();
        }

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