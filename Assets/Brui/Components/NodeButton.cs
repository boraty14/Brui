using System;
using Brui.EventHandlers;
using UnityEngine;

namespace Brui.Components
{
    [RequireComponent(typeof(NodeImage))]
    [RequireComponent(typeof(NodeCollider))]
    public class NodeButton : NodeComponent, INodePointerClick
    {
        public NodeImage NodeImage { get; private set; }
        
        public NodeButtonSettings ButtonSettings;
        public event Action OnButtonClick;

        protected override void SetComponents()
        {
            base.SetComponents();
            NodeImage = GetComponent<NodeImage>();
        }

        public void OnStartClick()
        {
            transform.localScale = Vector3.one * ButtonSettings.ClickStartScale;
            NodeImage.Image.color = ButtonSettings.ClickStartColor;
        }

        public void OnCompleteClick()
        {
            ResetState();
            OnButtonClick?.Invoke();
        }
        
        public void OnCancelClick()
        {
            ResetState();
            OnButtonClick?.Invoke();
        }

        private void ResetState()
        {
            transform.localScale = Vector3.one;
            NodeImage.Image.color = Color.white;
        }
    }

    [Serializable]
    public class NodeButtonSettings
    {
        [Range(0f, 1f)] public float ClickStartScale = 0.8f;
        public Color ClickStartColor = Color.grey;
    }
}