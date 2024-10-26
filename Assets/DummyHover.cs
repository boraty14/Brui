using Brui.EventHandlers;
using UnityEngine;

namespace DefaultNamespace
{
    public class DummyHover : MonoBehaviour, INodePointerHover
    {
        public void OnPointerEnter()
        {
            Debug.Log("a");            
        }

        public void OnPointerExit()
        {
            Debug.Log("b");            
        }
    }
}