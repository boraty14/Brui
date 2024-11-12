using System;
using Brui.Runtime.Attributes;
using Brui.Runtime.EventHandlers;
using UnityEngine;

namespace Brui.Runtime.Components
{
    [DefaultExecutionOrder(NodeConstants.ScrollExecutionOrder)]
    public class NodeScroll : MonoBehaviour, INodeDrag
    {
        public NodeScrollSettings ScrollSettings = new();
        [ReadOnlyNode] [SerializeField] private NodeScrollView _scrollView;
        [ReadOnlyNode] [SerializeField] private NodeScroll _parentScroll;

        private void Awake()
        {
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

        private void Start()
        {
            ScrollTo(ScrollSettings.StartScroll);
        }

        private void Update()
        {
            SetLimits();
        }

        private void SetLimits()
        {
            var scrollSize = _scrollView.ScrollSize;
            var scrollViewNode = _scrollView.NodeTransform;
            var inertiaAmount = ScrollSettings.Inertia * Time.deltaTime;

            switch (_scrollView.NodeLayout.layoutType)
            {
                case ENodeLayout.Vertical:
                    float verticalStartLimit = (NodeTransform.NodeSize.y - scrollSize) * 0.5f;
                    float verticalEndLimit = (-NodeTransform.NodeSize.y + scrollSize) * 0.5f;
                    float verticalPosition = 0f;
                    float currentVerticalPosition = scrollViewNode.TransformSettings.PositionOffset.y;

                    if (scrollViewNode.NodeSize.y < NodeTransform.NodeSize.y)
                    {
                        verticalPosition = Mathf.Lerp(verticalStartLimit, verticalEndLimit, _scrollView.PlacementRatio);
                        scrollViewNode.TransformSettings.PositionOffset.y = verticalPosition;
                    }
                    else
                    {
                        verticalPosition = Mathf.Clamp(currentVerticalPosition, verticalStartLimit, verticalEndLimit);
                        if (Mathf.Approximately(verticalPosition, currentVerticalPosition))
                        {
                            scrollViewNode.TransformSettings.PositionOffset.y = verticalPosition;
                        }
                        else
                        {
                            scrollViewNode.TransformSettings.PositionOffset.y =
                                Mathf.Lerp(currentVerticalPosition, verticalPosition, inertiaAmount);
                        }
                    }

                    break;
                case ENodeLayout.Horizontal:
                    float horizontalStartLimit = (-NodeTransform.NodeSize.x + scrollSize) * 0.5f;
                    float horizontalEndLimit = (NodeTransform.NodeSize.y - scrollSize) * 0.5f;
                    float horizontalPosition = 0f;
                    float currentHorizontalPosition = scrollViewNode.TransformSettings.PositionOffset.x;

                    if (scrollViewNode.NodeSize.x < NodeTransform.NodeSize.x)
                    {
                        horizontalPosition = Mathf.Lerp(horizontalStartLimit, horizontalEndLimit,
                            _scrollView.PlacementRatio);
                        scrollViewNode.TransformSettings.PositionOffset.x = horizontalPosition;
                    }
                    else
                    {
                        horizontalPosition = Mathf.Clamp(scrollViewNode.TransformSettings.PositionOffset.x,
                            horizontalEndLimit, horizontalStartLimit);
                        if (Mathf.Approximately(horizontalPosition, currentHorizontalPosition))
                        {
                            scrollViewNode.TransformSettings.PositionOffset.x = horizontalPosition;
                        }
                        else
                        {
                            scrollViewNode.TransformSettings.PositionOffset.x =
                                Mathf.Lerp(currentHorizontalPosition, horizontalPosition, inertiaAmount);
                        }
                    }

                    break;
            }
        }

        public void ScrollTo(float ratio)
        {
            ratio = _scrollView.NodeLayout.isReverse ? 1f - ratio : ratio;
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
                    scrollViewNode.TransformSettings.PositionOffset.y =
                        Mathf.Lerp(verticalStartLimit, verticalEndLimit, ratio);
                    break;
                case ENodeLayout.Horizontal:
                    if (scrollViewNode.NodeSize.x < NodeTransform.NodeSize.x)
                    {
                        return;
                    }

                    float horizontalStartLimit = (-NodeTransform.NodeSize.x + scrollSize) * 0.5f;
                    float horizontalEndLimit = (NodeTransform.NodeSize.y - scrollSize) * 0.5f;

                    scrollViewNode.TransformSettings.PositionOffset.x =
                        Mathf.Lerp(horizontalEndLimit, horizontalStartLimit, ratio);
                    break;
            }
        }

        public void ScrollTo(int itemIndex)
        {
            ScrollTo(Mathf.Clamp01((float)itemIndex / _scrollView.ElementCount));
        }

        public float GetItemScrollRatio(int itemIndex)
        {
            return Mathf.Clamp01((float)itemIndex / _scrollView.ElementCount);
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
        public float Inertia = 10f;
        [Range(0f, 1f)] public float StartScroll = 0f;
        public bool PropagateScroll;
    }
}