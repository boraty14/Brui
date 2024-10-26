using Brui.EventHandlers;
using UnityEngine;

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

        private RaycastHit[] _hits;
        private INodeInputFetcher _nodeInputFetcher;

        private INodeDrag _nodeDrag;
        private INodePointerDown _nodePointerDown;
        private INodePointerUp _nodePointerUp;
        private INodePointerClick _nodePointerClick;
        private float _clickTimer;
        private Vector2 _latestPointerPosition;

        public float VerticalSize => _camera.orthographicSize;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _camera.orthographic = true;
            _hits = new RaycastHit[_maxHits];

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            _nodeInputFetcher = new MobileInputFetcher();
#else
            _nodeInputFetcher = new DesktopInputFetcher();
#endif
        }

        void Update()
        {
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

            Vector2 pointerPosition = _nodeInputFetcher.GetPointerPosition();
            Ray ray = _camera.ScreenPointToRay(pointerPosition);
            int hitCount = Physics.RaycastNonAlloc(ray, _hits, Mathf.Infinity, _nodeUILayerMask);

            if (hitCount == 0)
            {
                return;
            }

            bool isPointerDownSet = _nodePointerDown != null;
            bool isPointerUpSet = _nodePointerUp != null;
            bool isPointerClickSet = _nodePointerClick != null;
            bool isDragSet = _nodeDrag != null;

            if (isPointerClickSet)
            {
                _clickTimer += Time.deltaTime;
            }

            for (int i = 0; i < hitCount; i++)
            {
                var hit = _hits[i];
                var hitCollider = hit.collider;

                if (isPointerJustPressed && !isDragSet)
                {
                    _latestPointerPosition = pointerPosition;
                    
                    if (!isPointerDownSet && !isPointerClickSet &&
                        hitCollider.TryGetComponent<INodePointerDown>(out _nodePointerDown))
                    {
                        _nodePointerDown.OnPointerDown(pointerPosition);
                    }

                    if (!isPointerDownSet && !isPointerClickSet)
                    {
                        hitCollider.TryGetComponent<INodePointerClick>(out _nodePointerClick);
                    }

                    if (hit.collider.TryGetComponent<INodeDrag>(out _nodeDrag))
                    {
                        _nodeDrag.OnBeginDrag(pointerPosition);
                    }
                }
                else if (isPointerJustReleased)
                {
                    if (!isPointerUpSet && hitCollider.TryGetComponent<INodePointerUp>(out _nodePointerUp))
                    {
                        _nodePointerUp.OnPointerUp(pointerPosition);
                    }

                    if (isDragSet)
                    {
                        _nodeDrag.OnEndDrag(pointerPosition);
                    }

                    if (isPointerClickSet && _clickTimer < _clickInterval &&
                        hitCollider.TryGetComponent<INodePointerClick>(out var nodePointerClick) &&
                        nodePointerClick == _nodePointerClick)
                    {
                        _nodePointerClick.OnClick();
                    }
                }
                else if (isPointerDown)
                {
                    if (isDragSet)
                    {
                        _nodeDrag.OnDrag(pointerPosition, pointerPosition - _latestPointerPosition);
                        _latestPointerPosition = pointerPosition;
                    }
                }
            }
        }
    }
}