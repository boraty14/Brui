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
        [SerializeField] private CanvasScaler _canvasScaler;
        [SerializeField] private RectTransform _inputObjectRectTransform;

        public static NodeTextInputField I { get; private set; }
        public event Action<string> OnSubmit;
        public event Action OnCancel;
        public bool IsOpen { get; private set; }
        private bool _isDeselected;
        private string _latestMessage;

        private void Awake()
        {
            I = this;
        }

        private void Start()
        {
            _inputField.onDeselect.AddListener(OnDeselected);
            _inputField.onEndEdit.AddListener(OnEndEdit);
            Close();
        }
        
        private void OnEndEdit(string message)
        {
            Debug.LogError(4444);
            _isDeselected = false;
            _latestMessage = message;
            Close();
            StopCoroutine(nameof(TrySubmit));
            StartCoroutine(nameof(TrySubmit));
        }

        private IEnumerator TrySubmit()
        {
            yield return null;
            yield return null;
            if (_isDeselected)
            {
                yield break;
            }
            OnSubmit?.Invoke(_latestMessage);
        }


        private void OnDeselected(string message)
        {
            _isDeselected = true;
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
            Debug.Log(keyboardHeight);
            float newY = keyboardHeight + 10f;
            _inputObjectRectTransform.anchoredPosition = new Vector2(_inputObjectRectTransform.anchoredPosition.x, newY);
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
            _inputField.onEndEdit.RemoveListener(OnEndEdit);
        }
    }
}