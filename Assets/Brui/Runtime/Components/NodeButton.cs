using System;
using Brui.Runtime.Attributes;
using Brui.Runtime.EventHandlers;
using UnityEngine;

namespace Brui.Runtime.Components
{
    [RequireComponent(typeof(NodeImage))]
    [RequireComponent(typeof(NodeCollider))]
    public class NodeButton : NodeComponent, INodePointerClick
    {
        [field: SerializeField] [field: ReadOnlyNode]
        public NodeImage NodeImage { get; private set; }

        public NodeButtonSettings ButtonSettings;
        public event Action OnButtonClick;

        public override void SetComponents()
        {
            base.SetComponents();
            NodeImage = GetComponent<NodeImage>();
        }

        public void OnStartClick()
        {
            if (!ButtonSettings.IsClickable)
            {
                return;
            }
            NodeImage.SetScaleFactor(ButtonSettings.ClickStartScale);
            NodeImage.Image.color = ButtonSettings.ClickStartColor;
        }

        public void OnCompleteClick()
        {
            if (!ButtonSettings.IsClickable)
            {
                return;
            }
            ResetState();
            OnButtonClick?.Invoke();
        }

        public void OnCancelClick()
        {
            if (!ButtonSettings.IsClickable)
            {
                return;
            }
            ResetState();
        }

        private void ResetState()
        {
            NodeImage.SetScaleFactor(1f);
            NodeImage.Image.color = Color.white;
        }
    }

    [Serializable]
    public class NodeButtonSettings
    {
        public bool IsClickable = true;
        [Range(0f, 1f)] public float ClickStartScale = 0.9f;
        public Color ClickStartColor = Color.grey;
    }
}