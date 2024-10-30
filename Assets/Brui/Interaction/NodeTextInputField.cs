#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
#define NATIVE_MOBILE
#endif

using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Brui.Interaction
{
    public class NodeTextInputField : MonoBehaviour
    {
        [SerializeField] private GameObject _inputObject;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _submitButton;
        [SerializeField] private CanvasScaler _canvasScaler;

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
            _submitButton.onClick.AddListener(OnSubmitButtonClick);
            Close();
        }

        private void OnSubmitButtonClick()
        {
            Debug.Log($"submit button clicked {_inputField.text}");
            OnSubmit?.Invoke(_inputField.text);
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
#if NATIVE_MOBILE
            StartCoroutine(nameof(WaitForKeyboard));
#endif
        }

        private IEnumerator WaitForKeyboard()
        {
            while (!TouchScreenKeyboard.visible)
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
            float keyboardHeight = GetKeyboardHeight();
            Debug.Log(keyboardHeight);
            float newY = keyboardHeight + 10f;
            _inputFieldRectTransform.anchoredPosition = new Vector2(_inputFieldRectTransform.anchoredPosition.x, newY);
            _inputObject.SetActive(true);
        }

        private float GetKeyboardHeight()
        {
#if UNITY_EDITOR
            return 0f; // fake TouchScreenKeyboard height for debug in editor        
#elif UNITY_ANDROID
    using (AndroidJavaClass UnityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
    {
        AndroidJavaObject View =
 UnityClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer").Call<AndroidJavaObject>("getView");
        using (AndroidJavaObject rect = new AndroidJavaObject("android.graphics.Rect"))
        {
            View.Call("getWindowVisibleDisplayFrame", rect);
            var screenHeight = Screen.height;
            float pixelHeight = screenHeight - rect.Call<int>("height");
            return (_canvasScaler.referenceResolution.y / screenHeight) * pixelHeight;
        }
    }
#elif UNITY_IOS
    return (float)TouchScreenKeyboard.area.height;
#else
    return 0f;
#endif
        }

        private void Close()
        {
            StopCoroutine(nameof(WaitForKeyboard));
            _inputObject.SetActive(false);
            _inputField.text = string.Empty;
        }

        private void OnDestroy()
        {
            _inputField.onSelect.RemoveListener(OnSelected);
            _inputField.onDeselect.RemoveListener(OnDeselected);
            _inputField.onSubmit.RemoveListener(OnSubmitted);
            _submitButton.onClick.RemoveListener(OnSubmitButtonClick);
        }
    }
}