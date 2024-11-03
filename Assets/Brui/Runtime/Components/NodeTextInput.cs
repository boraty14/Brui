using System;
using Brui.Runtime.EventHandlers;
using Brui.Runtime.Interaction;
using UnityEngine;

namespace Brui.Runtime.Components
{
    [RequireComponent(typeof(NodeCollider))]
    public class NodeTextInput : NodeComponent, INodePointerClick
    {
        public event Action<string> OnMessage;
        
        public void OnStartClick()
        {
            
        }

        public void OnCompleteClick()
        {
            if (NodeTextInputField.I.IsOpen)
            {
                return;
            }
            NodeTextInputField.I.Open();
            NodeTextInputField.I.OnSubmit += OnSubmit;
            NodeTextInputField.I.OnCancel += OnCancel;
        }

        public void OnCancelClick()
        {
        }

        private void OnSubmit(string message)
        {
            Debug.LogError($"SSSSSS {message}");
            OnMessage?.Invoke(message);
            UnregisterFromInputField();
        }

        private void OnCancel()
        {
            UnregisterFromInputField();
        }

        private void UnregisterFromInputField()
        {
            NodeTextInputField.I.OnSubmit -= OnSubmit;
            NodeTextInputField.I.OnCancel -= OnCancel;
        }
    }
}