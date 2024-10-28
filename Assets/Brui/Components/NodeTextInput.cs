#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
#define NATIVE_MOBILE
#endif

using Brui.Attributes;
using Brui.EventHandlers;
using UnityEngine;

namespace Brui.Components
{
    [RequireComponent(typeof(NodeCollider))]
    [RequireComponent(typeof(NodeText))]
    public class NodeTextInput : NodeComponent, INodePointerClick
    {
        [field: SerializeField] [field: ReadOnlyNode]
        public NodeText NodeText { get; private set; }
        public int characterLimit = 12;
        private TouchScreenKeyboard _touchScreenKeyboard;
        private string _baseText;
        private float _blinkTimer;
        private bool _showBlink;

        private const string CursorSymbol = "|";

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
                ResetBlink();
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

            if (!_touchScreenKeyboard.active)
            {
                return;
            }

            if (_touchScreenKeyboard.text.Length <= characterLimit)
            {
                _baseText = _touchScreenKeyboard.text;
            }
            else
            {
                _touchScreenKeyboard.text = _baseText;
            }
            
            if (_showBlink)
            {
                NodeText.Text = _baseText+ CursorSymbol;
            }
            else
            {
                NodeText.Text = _baseText;
                    
            }
            _blinkTimer += Time.deltaTime;
            if (_blinkTimer > NodeConstants.InputBlinkInterval)
            {
                _blinkTimer = 0f;
                _showBlink = !_showBlink;
            }
#else

#endif
        }

        private void ResetBlink()
        {
            _blinkTimer = 0f;
            _showBlink = false;
        }
    }
}