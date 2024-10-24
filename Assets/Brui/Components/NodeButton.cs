using System;
using Brui.EventHandlers;
using UnityEngine;

namespace Brui.Components
{
    [RequireComponent(typeof(NodeTransform))]
    [RequireComponent(typeof(NodeImage))]
    public class NodeButton : MonoBehaviour, INodePointerClick, INodePointerDown, INodePointerUp
    {
        public NodeTransform NodeTransform { get; private set; }
        public NodeImage NodeImage { get; private set; }
        
        public NodeButtonSettings ButtonSettings;
        public event Action OnButtonClick;

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
            NodeImage = GetComponent<NodeImage>();
            NodeTransform = GetComponent<NodeTransform>();
        }

        public void OnClick()
        {
            OnButtonClick?.Invoke();
        }

        public void OnPointerDown(Vector2 position)
        {
            transform.localScale = Vector3.one * ButtonSettings.PointerDownScale;
            NodeImage.Image.color = ButtonSettings.PointerDownColor;
        }

        public void OnPointerUp(Vector2 position)
        {
            transform.localScale = Vector3.one;
            NodeImage.Image.color = Color.white;
        }
    }

    [Serializable]
    public class NodeButtonSettings
    {
        [Range(0f, 1f)] public float PointerDownScale;
        public Color PointerDownColor;
    }
}