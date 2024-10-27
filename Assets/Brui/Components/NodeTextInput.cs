#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
#define NATIVE_MOBILE 
#endif

using Brui.EventHandlers;
using UnityEngine;

namespace Brui.Components
{
    [RequireComponent(typeof(NodeCollider))]
    [RequireComponent(typeof(NodeText))]
    public class NodeTextInput : NodeComponent, INodePointerClick
    {
        public NodeText NodeText { get; set; }
        private TouchScreenKeyboard _touchScreenKeyboard;

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
#if NATIVE_MOBILE
            _touchScreenKeyboard = TouchScreenKeyboard.Open(NodeText.Text, TouchScreenKeyboardType.Default,
                false, false, false, true, "");
#else
            
#endif
        }

        public void OnCancelClick()
        {
        }

        private void Update()
        {
#if NATIVE_MOBILE
            if (_touchScreenKeyboard == null)
            {
                return;
            }
            
            var status = _touchScreenKeyboard.status;

            if (status == TouchScreenKeyboard.Status.Done ||
                status == TouchScreenKeyboard.Status.Canceled ||
                status == TouchScreenKeyboard.Status.LostFocus)
            {
                _touchScreenKeyboard = null;
                return;
            }
            
            if (_touchScreenKeyboard.active)
            {
                NodeText.Text = _touchScreenKeyboard.text;
            }
#else
            
#endif
        }
    }
}