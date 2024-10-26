using System;
using Brui.EventHandlers;
using UnityEngine;

namespace Brui.Components
{
    [RequireComponent(typeof(NodeCollider))]
    [DefaultExecutionOrder(NodeExecutionOrders.ScrollExecutionOrder)]
    public class NodeScroll : NodeComponent, INodeDrag
    {
        public NodeScrollSettings ScrollSettings;
        private NodeTransform _scrollView;

        protected override void SetComponents()
        {
            base.SetComponents();
            if (_scrollView == null)
            {
                
            }
        }

        private void Update()
        {
            
        }

        public void OnBeginDrag(Vector2 position)
        {
            Debug.Log("1");
        }

        public void OnEndDrag(Vector2 position)
        {
            Debug.Log("3");
        }

        public void OnDrag(Vector2 position, Vector2 delta)
        {
            Debug.Log("2");
        }
    }

    [Serializable]
    public class NodeScrollSettings
    {
        public bool IsHorizontal;
        public bool IsVertical;
        public float ScrollSpeed = 1f;
    }
}