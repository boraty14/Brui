#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
#define NATIVE_MOBILE
#endif

using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Brui.Runtime.Interaction
{
    public class NodeTextInputField : MonoBehaviour
    {
        [SerializeField] private GameObject _inputObject;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private CanvasScaler _canvasScaler;
        [SerializeField] private RectTransform _inputObjectRectTransform;
        [SerializeField] private float _keyboardOffset = 10f;

        public static NodeTextInputField I { get; private set; }
        public event Action<string> OnSubmit;
        public event Action OnCancel;
        public bool IsOpen { get; private set; }

        private void Awake()
        {
            I = this;
        }

        private void Start()
        {
            _inputField.onDeselect.AddListener(OnDeselected);
            _inputField.onSubmit.AddListener(OnSubmitted);
            Close();
        }

        private void Update()
        {
            if (!IsOpen)
            {
                return;
            }

#if NATIVE_MOBILE
            if (_inputField.isFocused && !TouchScreenKeyboard.visible)
            {
                OnSubmitMobile(_inputField.text);
            }
#endif
        }
        
        private void OnSubmitted(string message)
        {
            OnSubmit?.Invoke(message);
            Close();
        }

        private void OnSubmitMobile(string message)
        {
            OnSubmit?.Invoke(message);
            Close();
        }

        private void OnDeselected(string message)
        {
            OnCancel?.Invoke();
            Close();
        }

        public void Open()
        {
            IsOpen = true;
            _inputObject.SetActive(true);
            _inputField.ActivateInputField();
#if NATIVE_MOBILE
            StopCoroutine(nameof(WaitForKeyboard));
            StartCoroutine(nameof(WaitForKeyboard));
#endif
        }

        private IEnumerator WaitForKeyboard()
        {
            while (!TouchScreenKeyboard.visible)
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);
            float keyboardHeight = GetKeyboardHeight();
            float newY = keyboardHeight + (_keyboardOffset * Screen.height /  _canvasScaler.referenceResolution.y);
            _inputObjectRectTransform.anchoredPosition =
                new Vector2(_inputObjectRectTransform.anchoredPosition.x, newY);
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
            IsOpen = false;
            StopCoroutine(nameof(WaitForKeyboard));
            _inputField.DeactivateInputField();
            _inputObject.SetActive(false);
            _inputField.text = string.Empty;
        }

        private void OnDestroy()
        {
            _inputField.onDeselect.RemoveListener(OnDeselected);
            _inputField.onSubmit.RemoveListener(OnSubmitted);
        }
    }
}