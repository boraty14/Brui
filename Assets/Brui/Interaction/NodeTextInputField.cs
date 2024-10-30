#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
#define NATIVE_MOBILE
#endif

using System;
using TMPro;
using UnityEngine;

namespace Brui.Interaction
{
    public class NodeTextInputField : MonoBehaviour
    {
        [SerializeField] private GameObject _inputObject;
        [SerializeField] private TMP_InputField _inputField;

        private RectTransform _inputFieldRectTransform;
        
        public static NodeTextInputField I { get; private set; }
        public event Action<string> OnSubmit;
        public event Action OnCancel;

        private void Awake()
        {
            I = this;
        }

        private void Start()
        {
            _inputFieldRectTransform = _inputField.GetComponent<RectTransform>();
            _inputField.onSelect.AddListener(OnSelected);
            _inputField.onDeselect.AddListener(OnDeselected);
            _inputField.onSubmit.AddListener(OnSubmitted);
            Close();
        }

        private void OnSubmitted(string message)
        {
            Debug.Log($"submitted {message}");
            OnSubmit?.Invoke(message);
            Close();
        }

        private void OnSelected(string message)
        {
            Debug.Log($"selected {message}");
        }
        
        private void OnDeselected(string message)
        {
            Debug.Log($"deselected {message}");
            OnCancel?.Invoke();
            Close();
        }

        public void Open()
        {
            _inputObject.SetActive(true);
            _inputField.ActivateInputField();
#if !NATIVE_MOBILE
            float keyboardHeight = TouchScreenKeyboard.area.height;

            // Position the input field just above the keyboard
            float newY = keyboardHeight + (_inputFieldRectTransform.rect.height / 2) + 10f;
            _inputFieldRectTransform.anchoredPosition = new Vector2(_inputFieldRectTransform.anchoredPosition.x, newY);
#endif
        }

        private void Close()
        {
            _inputObject.SetActive(false);
        }
        
        private void OnDestroy()
        {
            _inputField.onSelect.RemoveListener(OnSelected);
            _inputField.onDeselect.RemoveListener(OnDeselected);
            _inputField.onSubmit.RemoveListener(OnSubmitted);
        }
    }
}