using System.Collections.Generic;
using Brui.EventHandlers;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Brui.Interaction
{
    [RequireComponent(typeof(Camera))]
    public class NodeCamera : MonoBehaviour
    {
        public float DistanceToCamera = 10;
        [SerializeField] private LayerMask _nodeUILayerMask;
        [SerializeField] private int _maxHits = 10;
        [SerializeField] private float _clickInterval = 1f;
        [SerializeField] private Camera _camera;

        private RaycastHit2D[] _hits;
        private INodeInputFetcher _nodeInputFetcher;

        // hover
        private List<INodePointerHover> _hoveredNodes;
        private List<INodePointerHover> _currentlyHoveredNodes;
        private List<INodePointerHover> _exitingHoverNodes;

        // click
        private INodeDrag _nodeDrag;
        private INodePointerDown _nodePointerDown;
        private INodePointerUp _nodePointerUp;
        private INodePointerClick _nodePointerClick;
        private float _clickTimer;
        private Vector2 _latestPointerPosition;

        public float VerticalSize => _camera.orthographicSize;
        private bool IsPointerDownSet => _nodePointerDown != null;
        private bool IsPointerUpSet => _nodePointerUp != null;
        private bool IsPointerClickSet => _nodePointerClick != null;
        private bool IsDragSet => _nodeDrag != null;

        private void Awake()
        {
            EnhancedTouchSupport.Enable();
            _camera = GetComponent<Camera>();
            _camera.orthographic = true;
            _hits = new RaycastHit2D[_maxHits];
            _hoveredNodes = new List<INodePointerHover>();
            _currentlyHoveredNodes = new List<INodePointerHover>();
            _exitingHoverNodes = new List<INodePointerHover>();

#if (UNITY_ANDROID || UNITY_IOS)
            _nodeInputFetcher = new MobileInputFetcher();
#else
            _nodeInputFetcher = new DesktopInputFetcher();
#endif
        }

        void Update()
        {
            Vector2 pointerPosition = _camera.ScreenToWorldPoint(_nodeInputFetcher.GetPointerPosition());
            int hitCount =
                Physics2D.RaycastNonAlloc(pointerPosition, Vector2.zero, _hits, Mathf.Infinity, _nodeUILayerMask);

            // hover
            _currentlyHoveredNodes.Clear();
            _exitingHoverNodes.Clear();
            for (int i = 0; i < hitCount; i++)
            {
                var hit = _hits[i];
                var hitCollider = hit.collider;
                if (!hitCollider.TryGetComponent<INodePointerHover>(out var nodePointerHover))
                {
                    continue;
                }

                if (!_hoveredNodes.Contains(nodePointerHover))
                {
                    _hoveredNodes.Add(nodePointerHover);
                    nodePointerHover.OnPointerEnter();
                }

                _currentlyHoveredNodes.Add(nodePointerHover);
            }

            foreach (var hoveredNode in _hoveredNodes)
            {
                if (!_currentlyHoveredNodes.Contains(hoveredNode))
                {
                    _exitingHoverNodes.Add(hoveredNode);
                    hoveredNode.OnPointerExit();
                }
            }

            foreach (var exitingHoverNode in _exitingHoverNodes)
            {
                _hoveredNodes.Remove(exitingHoverNode);
            }

            // click interactions
            bool isPointerDown = _nodeInputFetcher.IsPointerDown();
            bool isPointerJustPressed = _nodeInputFetcher.IsPressedThisFrame();
            bool isPointerJustReleased = _nodeInputFetcher.IsReleasedThisFrame();

            if (!isPointerDown && !isPointerJustPressed && !isPointerJustReleased)
            {
                _nodeDrag = null;
                _nodePointerDown = null;
                _nodePointerUp = null;
                _nodePointerClick = null;
                _clickTimer = 0f;
                return;
            }

            if (IsPointerClickSet)
            {
                _clickTimer += Time.deltaTime;
            }

            if (isPointerJustReleased)
            {
                var hitCollider = _hits[0].collider;
                if (!IsPointerUpSet && hitCount > 0 &&
                    hitCollider.TryGetComponent<INodePointerUp>(out _nodePointerUp))
                {
                    _nodePointerUp.OnPointerUp(pointerPosition);
                }

                if (IsDragSet)
                {
                    _nodeDrag.OnEndDrag(pointerPosition);
                }

                if (IsPointerClickSet)
                {
                    if (_clickTimer < _clickInterval && hitCount > 0 &&
                        hitCollider.TryGetComponent<INodePointerClick>(out var nodePointerClick) &&
                        nodePointerClick == _nodePointerClick)
                    {
                        _nodePointerClick.OnCompleteClick();
                    }
                    else
                    {
                        _nodePointerClick.OnCancelClick();
                    }
                }

                return;
            }

            if (isPointerDown && !isPointerJustPressed)
            {
                if (IsDragSet)
                {
                    _nodeDrag.OnDrag(pointerPosition, pointerPosition - _latestPointerPosition);
                    _latestPointerPosition = pointerPosition;
                }

                return;
            }

            for (int i = 0; i < hitCount; i++)
            {
                var hit = _hits[i];
                var hitCollider = hit.collider;

                if (isPointerJustPressed && !IsDragSet)
                {
                    _latestPointerPosition = pointerPosition;

                    if (!IsPointerDownSet && !IsPointerClickSet &&
                        hitCollider.TryGetComponent<INodePointerDown>(out _nodePointerDown))
                    {
                        _nodePointerDown.OnPointerDown(pointerPosition);
                    }

                    if (!IsPointerDownSet && !IsPointerClickSet &&
                        hitCollider.TryGetComponent<INodePointerClick>(out _nodePointerClick))
                    {
                        _nodePointerClick.OnStartClick();
                    }

                    if (hit.collider.TryGetComponent<INodeDrag>(out _nodeDrag))
                    {
                        if (IsPointerClickSet)
                        {
                            _nodePointerClick.OnCancelClick();
                            _nodePointerClick = null;
                        }
                        _nodeDrag.OnBeginDrag(pointerPosition);
                    }
                }
            }
        }
    }
}