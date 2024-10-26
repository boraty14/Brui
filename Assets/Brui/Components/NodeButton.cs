using System;
using Brui.EventHandlers;
using UnityEngine;

namespace Brui.Components
{
    [RequireComponent(typeof(NodeImage))]
    [RequireComponent(typeof(NodeCollider))]
    public class NodeButton : NodeComponent, INodePointerClick, INodePointerDown, INodePointerUp
    {
        public NodeImage NodeImage { get; private set; }
        
        public NodeButtonSettings ButtonSettings;
        public event Action OnButtonClick;

        protected override void SetComponents()
        {
            base.SetComponents();
            NodeImage = GetComponent<NodeImage>();
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