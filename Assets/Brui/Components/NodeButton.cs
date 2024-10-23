using System;
using Brui.EventHandlers;
using UnityEngine;

namespace Brui.Components
{
    [RequireComponent(typeof(NodeTransform))]
    public class NodeButton : MonoBehaviour, INodePointerClick, INodePointerDown, INodePointerUp
    {
        public NodeButtonSettings ButtonSettings;
        public event Action OnButtonClick; 
        public NodeTransform NodeTransform { get; private set; }

        private void Awake()
        {
            NodeTransform = GetComponent<NodeTransform>();
        }

        public void OnClick()
        {
            OnButtonClick?.Invoke();
        }

        public void OnPointerDown(Vector2 position)
        {
            
        }

        public void OnPointerUp(Vector2 position)
        {
            
        }
    }

    [Serializable]
    public class NodeButtonSettings
    {
        [Range(0f, 1f)] public float PointerDownScale;
        public Color PointerDownColor;
    }
}