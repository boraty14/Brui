using System;
using Brui.Runtime.EventHandlers;
using UnityEngine;

namespace Brui.Runtime.Components
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class NodeScroll : MonoBehaviour, INodeDrag
    {
        public NodeScrollSettings ScrollSettings = new();
        [SerializeField] private NodeLayout _layout;
        [SerializeField] private NodeScroll _parentScroll;
        [SerializeField] private BoxCollider2D _interactionCollider;

        private void OnValidate()
        {
            _interactionCollider = GetComponent<BoxCollider2D>();
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
            var scrollViewSize = _layout.ViewSize;
            var inertiaAmount = ScrollSettings.Inertia * Time.deltaTime;
            var interactionSize = _interactionCollider.size;
            var scrollViewLocalPosition = _layout.transform.localPosition;

            switch (_layout.Layout)
            {
                case ELayout.Vertical:
                    float verticalStartLimit = (interactionSize.y - scrollViewSize) * 0.5f;
                    float verticalEndLimit = (-interactionSize.y + scrollViewSize) * 0.5f;
                    float verticalPosition = 0f;
                    float currentVerticalPosition = scrollViewLocalPosition.y;

                    if (scrollViewSize < interactionSize.y)
                    {
                        verticalPosition = Mathf.Lerp(verticalStartLimit, verticalEndLimit,
                            ScrollSettings.LayoutPlacementRatio);
                        _layout.transform.localPosition = new Vector3(
                            scrollViewLocalPosition.x,
                            verticalPosition,
                            scrollViewLocalPosition.z);
                    }
                    else
                    {
                        verticalPosition = Mathf.Clamp(currentVerticalPosition, verticalStartLimit, verticalEndLimit);
                        if (Mathf.Approximately(verticalPosition, currentVerticalPosition))
                        {
                            _layout.transform.localPosition = new Vector3(
                                scrollViewLocalPosition.x,
                                verticalPosition,
                                scrollViewLocalPosition.z);
                        }
                        else
                        {
                            _layout.transform.localPosition = new Vector3(
                                scrollViewLocalPosition.x,
                                Mathf.Lerp(currentVerticalPosition, verticalPosition, inertiaAmount),
                                scrollViewLocalPosition.z);
                        }
                    }

                    break;
                case ELayout.Horizontal:
                    float horizontalStartLimit = (interactionSize.x - scrollViewSize) * 0.5f;
                    float horizontalEndLimit = (-interactionSize.x + scrollViewSize) * 0.5f;
                    float horizontalPosition = 0f;
                    float currentHorizontalPosition = scrollViewLocalPosition.x;

                    if (scrollViewSize < interactionSize.x)
                    {
                        horizontalPosition = Mathf.Lerp(horizontalStartLimit, horizontalEndLimit,
                            ScrollSettings.LayoutPlacementRatio);
                        _layout.transform.localPosition = new Vector3(
                            horizontalPosition,
                            scrollViewLocalPosition.y,
                            scrollViewLocalPosition.z);
                    }
                    else
                    {
                        horizontalPosition = Mathf.Clamp(currentHorizontalPosition, horizontalStartLimit,
                            horizontalEndLimit);
                        if (Mathf.Approximately(horizontalPosition, currentHorizontalPosition))
                        {
                            _layout.transform.localPosition = new Vector3(
                                horizontalPosition,
                                scrollViewLocalPosition.y,
                                scrollViewLocalPosition.z);
                        }
                        else
                        {
                            _layout.transform.localPosition = new Vector3(
                                Mathf.Lerp(currentHorizontalPosition, horizontalPosition, inertiaAmount),
                                scrollViewLocalPosition.y,
                                scrollViewLocalPosition.z);
                        }
                    }

                    break;
            }
        }

        public void ScrollTo(float ratio)
        {
            ratio = _layout.IsReverse ? 1f - ratio : ratio;
            var interactionSize = _interactionCollider.size;
            var scrollViewSize = _layout.ViewSize;
            var scrollViewLocalPosition = _layout.transform.localPosition;

            switch (_layout.Layout)
            {
                case ELayout.Vertical:
                    if (scrollViewSize < interactionSize.y)
                    {
                        return;
                    }

                    float verticalStartLimit = (interactionSize.y - scrollViewSize) * 0.5f;
                    float verticalEndLimit = (-interactionSize.y + scrollViewSize) * 0.5f;
                    _layout.transform.localPosition = new Vector3(
                        scrollViewLocalPosition.x,
                        Mathf.Lerp(verticalStartLimit, verticalEndLimit, ratio),
                        scrollViewLocalPosition.z);
                    break;
                case ELayout.Horizontal:
                    if (scrollViewSize < interactionSize.x)
                    {
                        return;
                    }

                    float horizontalStartLimit = (interactionSize.x - scrollViewSize) * 0.5f;
                    float horizontalEndLimit = (-interactionSize.x + scrollViewSize) * 0.5f;
                    _layout.transform.localPosition = new Vector3(
                        Mathf.Lerp(horizontalEndLimit, horizontalStartLimit, ratio),
                        scrollViewLocalPosition.y,
                        scrollViewLocalPosition.z);
                    break;
            }
        }

        public void ScrollTo(int itemIndex)
        {
            ScrollTo(Mathf.Clamp01((float)itemIndex / _layout.ElementCount));
        }

        public float GetItemScrollRatio(int itemIndex)
        {
            return Mathf.Clamp01((float)itemIndex / _layout.ElementCount);
        }

        public void OnBeginDrag(Vector2 position)
        {
            if (ScrollSettings.PropagateScroll)
            {
                _parentScroll?.OnBeginDrag(position);
            }
        }

        public void OnEndDrag(Vector2 position)
        {
            if (ScrollSettings.PropagateScroll)
            {
                _parentScroll?.OnEndDrag(position);
            }
        }

        public void OnDrag(Vector2 position, Vector2 delta)
        {
            if (ScrollSettings.PropagateScroll)
            {
                _parentScroll?.OnDrag(position, delta);
            }

            switch (_layout.Layout)
            {
                case ELayout.Vertical:
                    _layout.transform.localPosition +=
                        delta.y * ScrollSettings.ScrollSpeed * Vector3.up;
                    break;
                case ELayout.Horizontal:
                    _layout.transform.localPosition +=
                        delta.x * ScrollSettings.ScrollSpeed * Vector3.right;
                    break;
            }
        }
    }

    [Serializable]
    public class NodeScrollSettings
    {
        [Range(0f, 1f)] public float LayoutPlacementRatio;
        public float ScrollSpeed = 1f;
        public float Inertia = 10f;
        [Range(0f, 1f)] public float StartScroll = 0f;
        public bool PropagateScroll;
    }
}