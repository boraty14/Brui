using Brui.EventHandlers;
using UnityEngine;

namespace Brui.Components
{
    [RequireComponent(typeof(NodeCollider))]
    [RequireComponent(typeof(NodeText))]
    public class NodeTextInput : NodeComponent, INodePointerClick
    {
        public NodeText NodeText { get; set; }
        private TouchScreenKeyboard _keyboard;

        public override void SetComponents()
        {
            base.SetComponents();
            NodeText = GetComponent<NodeText>();
        }

        public void OnStartClick()
        {
        }

        public void OnCompleteClick()
        {
            if (_keyboard != null)
            {
                return;
            }
            _keyboard = TouchScreenKeyboard.Open(NodeText.Text, TouchScreenKeyboardType.Default,
                false, false, false, false, "");
            Debug.Log("opened");
        }

        public void OnCancelClick()
        {
        }

        private void Update()
        {
            if (_keyboard == null)
            {
                return;
            }
            
            var status = _keyboard.status;
            
            if (status == TouchScreenKeyboard.Status.Done ||
                status == TouchScreenKeyboard.Status.Canceled ||
                status == TouchScreenKeyboard.Status.LostFocus)
            {
                _keyboard = null;
                return;
            }

            if (_keyboard.active)
            {
                NodeText.Text = _keyboard.text;
            }
        }
    }
}