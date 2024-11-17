using System;
using Brui.Runtime.EventHandlers;
using UnityEngine;

namespace Brui.Runtime.Components
{
    public class NodeButton : MonoBehaviour, INodePointerClick
    {
        public bool IsClickable = true;
        [Range(0f, 1f)] public float ClickStartScale = 0.95f;
        public event Action OnButtonClickComplete;
        public event Action OnButtonClickCancel;

        public void OnStartClick()
        {
            if (!IsClickable)
            {
                return;
            }
            transform.localScale = Vector3.one * ClickStartScale;
        }

        public void OnCompleteClick()
        {
            if (!IsClickable)
            {
                return;
            }
            ResetState();
            OnButtonClickComplete?.Invoke();
        }

        public void OnCancelClick()
        {
            if (!IsClickable)
            {
                return;
            }
            ResetState();
            OnButtonClickCancel?.Invoke();
        }

        private void ResetState()
        {
            transform.localScale = Vector3.one;
        }
    }

    [Serializable]
    public class NodeButtonSettings
    {
        public bool IsClickable = true;
        [Range(0f, 1f)] public float ClickStartScale = 0.9f;
    }
}