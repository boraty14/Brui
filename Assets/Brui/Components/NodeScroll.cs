using System;
using Brui.EventHandlers;
using UnityEngine;

namespace Brui.Components
{
    [RequireComponent(typeof(NodeCollider))]
    [RequireComponent(typeof(NodeImage))]
    [RequireComponent(typeof(SpriteMask))]
    public class NodeScroll : NodeComponent, INodeDrag
    {
        public NodeScrollSettings ScrollSettings;
        public NodeImage NodeImage { get; private set; }
        public SpriteMask SpriteMask { get; private set; }
        private NodeScrollView _scrollView;
        
        protected override void SetComponents()
        {
            base.SetComponents();
            NodeImage = GetComponent<NodeImage>();
            SpriteMask = GetComponent<SpriteMask>();
            SpriteMask.maskSource = SpriteMask.MaskSource.SupportedRenderers;
            if (_scrollView == null)
            {
                if (transform.childCount == 0)
                {
                    GameObject viewObject = new GameObject("View");
                    viewObject.AddComponent<NodeScrollView>();
                    viewObject.transform.SetParent(transform);
                }

                _scrollView = transform.GetChild(0).GetComponent<NodeScrollView>();
            }
        }

        private void Update()
        {
            
        }

        public void OnBeginDrag(Vector2 position)
        {
        }

        public void OnEndDrag(Vector2 position)
        {
        }

        public void OnDrag(Vector2 position, Vector2 delta)
        {
            var viewSize = _scrollView.NodeTransform.NodeSize;
            var viewPosition = _scrollView.transform.localPosition;

            if (ScrollSettings.IsHorizontal)
            {
                
            }
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