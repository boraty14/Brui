#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
#define NATIVE_MOBILE
#endif

using Brui.Attributes;
using Brui.EventHandlers;
using UnityEngine;
#if !NATIVE_MOBILE
using UnityEngine.InputSystem;
#endif

namespace Brui.Components
{
    [RequireComponent(typeof(NodeCollider))]
    [RequireComponent(typeof(NodeText))]
    public class NodeTextInput : NodeComponent, INodePointerClick
    {
        [field: SerializeField] [field: ReadOnlyNode]
        public NodeText NodeText { get; private set; }
        public int characterLimit = 12;
        public bool isSpaceEnabled;
        public string BaseText { get; private set; } = string.Empty;

        private TouchScreenKeyboard _touchScreenKeyboard;
        private float _blinkTimer;
        private bool _showBlink;

#if NATIVE_MOBILE
        private bool _isMobileOpen;
#else
        private bool _isKeyboardOpen;
#endif

        private const string CursorSymbol = "|";

        public override void SetComponents()
        {
            base.SetComponents();
            NodeText = GetComponent<NodeText>();
        }

        public void CloseInput()
        {
#if !NATIVE_MOBILE
            _isKeyboardOpen = false;
#endif
            NodeText.Text = BaseText;
        }

        public void OnStartClick()
        {
        }

        public void OnCompleteClick()
        {
#if NATIVE_MOBILE
            _touchScreenKeyboard = TouchScreenKeyboard.Open(NodeText.Text, TouchScreenKeyboardType.Default,
                false, false, false, true, "");
            _isMobileOpen = true;
#else
            _isKeyboardOpen = true;
#endif
        }

        public void OnCancelClick()
        {
        }

        private void Update()
        {
#if NATIVE_MOBILE
            if (!_isMobileOpen)
            {
                ResetBlink();
                return;
            }

            if (_touchScreenKeyboard == null)
            {
                _isMobileOpen = false;
                NodeText.Text = BaseText;
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
                BaseText = _touchScreenKeyboard.text;
            }
            else
            {
                _touchScreenKeyboard.text = BaseText;
            }
#else
            if (!_isKeyboardOpen)
            {
                ResetBlink();
                return;
            }

            ProcessKeyboard();
#endif
            if (_showBlink)
            {
                NodeText.Text = BaseText + CursorSymbol;
            }
            else
            {
                NodeText.Text = BaseText;
            }

            _blinkTimer += Time.deltaTime;
            if (_blinkTimer > NodeConstants.InputBlinkInterval)
            {
                _blinkTimer = 0f;
                _showBlink = !_showBlink;
            }
        }

#if !NATIVE_MOBILE
        private void ProcessKeyboard()
        {
            var keyboard = Keyboard.current;

            int baseTextLength = BaseText.Length;
            if (keyboard.backspaceKey.wasPressedThisFrame)
            {
                if (baseTextLength > 0)
                {
                    BaseText = BaseText.Substring(0, baseTextLength - 1);
                }

                return;
            }

            if (baseTextLength > characterLimit)
            {
                return;
            }

            foreach (var key in Keyboard.current.allKeys)
            {
                if (BaseText.Length > characterLimit)
                {
                    return;
                }

                if (!key.wasPressedThisFrame)
                {
                    continue;
                }

                string keyName = key.displayName;

                if (keyName.Length != 1)
                {
                    continue;
                }

                char keyChar = keyName[0];
                if (char.IsLetterOrDigit(keyChar) || (isSpaceEnabled && char.IsWhiteSpace(keyChar)))
                {
                    BaseText += keyChar;
                }

            }
        }
#endif

        private void ResetBlink()
        {
            _blinkTimer = 0f;
            _showBlink = false;
        }
    }
}