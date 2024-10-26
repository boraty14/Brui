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
        private NodeScroll _parentScroll;
        
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

                var firstChild = transform.GetChild(0);
                _scrollView = firstChild.GetComponent<NodeScrollView>();
                if (_scrollView == null)
                {
                    _scrollView = firstChild.gameObject.AddComponent<NodeScrollView>();
                }
            }

            if (ScrollSettings.PropagateScroll)
            {
                _parentScroll = GetComponentInParent<NodeScroll>(true);
            }
        }

        private void Update()
        {
            
        }

        public void OnBeginDrag(Vector2 position)
        {
            if (ScrollSettings.PropagateScroll)
            {
                _parentScroll.OnBeginDrag(position);
            }
        }

        public void OnEndDrag(Vector2 position)
        {
            if (ScrollSettings.PropagateScroll)
            {
                _parentScroll.OnEndDrag(position);
            }
        }

        public void OnDrag(Vector2 position, Vector2 delta)
        {
            if (ScrollSettings.PropagateScroll)
            {
                _parentScroll.OnDrag(position, delta);
            }
            
            var viewSize = _scrollView.NodeTransform.NodeSize;
            var viewPosition = _scrollView.transform.localPosition;

            switch (_scrollView.NodeLayout.layoutType)
            {
                case ENodeLayout.Vertical:
                    break;
                case ENodeLayout.Horizontal:
                    break;
            }
        }
    }

    [Serializable]
    public class NodeScrollSettings
    {
        public float ScrollSpeed = 1f;
        public float ItemSize = 1f;
        public bool PropagateScroll;
    }
}