using System;
using Brui.EventHandlers;
using UnityEngine;

namespace Brui.Components
{
    [RequireComponent(typeof(NodeCollider))]
    [RequireComponent(typeof(NodeImage))]
    [RequireComponent(typeof(SpriteMask))]
    [DefaultExecutionOrder(NodeConstants.ScrollExecutionOrder)]
    [ExecuteAlways]
    public class NodeScroll : NodeComponent, INodeDrag
    {
        public NodeScrollSettings ScrollSettings = new();
        public NodeImage NodeImage { get; private set; }
        public SpriteMask SpriteMask { get; private set; }
        private NodeScrollView _scrollView;
        private NodeScroll _parentScroll;

        public override void SetComponents()
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
            _scrollView.SetComponents();


            if (ScrollSettings.PropagateScroll)
            {
                _parentScroll = GetComponentInParent<NodeScroll>(true);
            }
        }

        private void Update()
        {
            SetLimits();
        }

        private void SetLimits()
        {
            var scrollSize = _scrollView.ScrollSize;
            var scrollViewNode = _scrollView.NodeTransform;

            switch (_scrollView.NodeLayout.layoutType)
            {
                case ENodeLayout.Vertical:
                    float verticalStartLimit = (NodeTransform.NodeSize.y - scrollSize) * 0.5f;
                    float verticalEndLimit = (-NodeTransform.NodeSize.y + scrollSize) * 0.5f;
                    float verticalPosition = 0f;

                    if (scrollViewNode.NodeSize.y < NodeTransform.NodeSize.y)
                    {
                        verticalPosition = Mathf.Lerp(verticalStartLimit, verticalEndLimit, _scrollView.PlacementRatio);
                    }
                    else
                    {
                        verticalPosition = Mathf.Clamp(scrollViewNode.TransformSettings.PositionOffset.y,
                            verticalStartLimit, verticalEndLimit);
                    }

                    scrollViewNode.TransformSettings.PositionOffset.y = verticalPosition;
                    break;
                case ENodeLayout.Horizontal:
                    float horizontalStartLimit = (-NodeTransform.NodeSize.x + scrollSize) * 0.5f;
                    float horizontalEndLimit = (NodeTransform.NodeSize.y - scrollSize) * 0.5f;
                    float horizontalPosition = 0f;

                    if (scrollViewNode.NodeSize.x < NodeTransform.NodeSize.x)
                    {
                        horizontalPosition = Mathf.Lerp(horizontalStartLimit, horizontalEndLimit,
                            _scrollView.PlacementRatio);
                    }
                    else
                    {
                        horizontalPosition = Mathf.Clamp(scrollViewNode.TransformSettings.PositionOffset.x,
                            horizontalEndLimit, horizontalStartLimit);
                    }

                    scrollViewNode.TransformSettings.PositionOffset.x = horizontalPosition;
                    break;
            }
        }

        public void ScrollTo(float ratio)
        {
            var scrollSize = _scrollView.ScrollSize;
            var scrollViewNode = _scrollView.NodeTransform;

            switch (_scrollView.NodeLayout.layoutType)
            {
                case ENodeLayout.Vertical:
                    if (scrollViewNode.NodeSize.y < NodeTransform.NodeSize.y)
                    {
                        return;
                    }

                    float verticalStartLimit = (NodeTransform.NodeSize.y - scrollSize) * 0.5f;
                    float verticalEndLimit = (-NodeTransform.NodeSize.y + scrollSize) * 0.5f;
                    float verticalPosition = 0f;


                    verticalPosition = Mathf.Clamp(scrollViewNode.TransformSettings.PositionOffset.y,
                        verticalStartLimit, verticalEndLimit);
                    scrollViewNode.TransformSettings.PositionOffset.y = verticalPosition;
                    break;
                case ENodeLayout.Horizontal:
                    if (scrollViewNode.NodeSize.x < NodeTransform.NodeSize.x)
                    {
                        return;
                    }

                    float horizontalStartLimit = (-NodeTransform.NodeSize.x + scrollSize) * 0.5f;
                    float horizontalEndLimit = (NodeTransform.NodeSize.y - scrollSize) * 0.5f;
                    float horizontalPosition = 0f;


                    horizontalPosition = Mathf.Clamp(scrollViewNode.TransformSettings.PositionOffset.x,
                        horizontalEndLimit, horizontalStartLimit);
                    scrollViewNode.TransformSettings.PositionOffset.x = horizontalPosition;
                    break;
            }
        }

        public void ScrollTo(int itemIndex)
        {
        }

        public float GetItemScrollRatio(int itemIndex)
        {
            return 0f;
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

            var scrollViewNode = _scrollView.NodeTransform;

            switch (_scrollView.NodeLayout.layoutType)
            {
                case ENodeLayout.Vertical:
                    scrollViewNode.TransformSettings.PositionOffset +=
                        delta.y * ScrollSettings.ScrollSpeed * Vector2.up;
                    break;
                case ENodeLayout.Horizontal:
                    scrollViewNode.TransformSettings.PositionOffset +=
                        delta.x * ScrollSettings.ScrollSpeed * Vector2.right;
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