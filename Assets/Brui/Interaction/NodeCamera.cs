using System.Collections.Generic;
using UnityEngine;

namespace Brui.Interaction
{
    [RequireComponent(typeof(Camera))]
    public class NodeCamera : MonoBehaviour
    {
        public float DistanceToCamera = 10;
        [SerializeField] private LayerMask _nodeUILayerMask;
        [SerializeField] private int _maxHits = 50;
        [SerializeField] private Camera _camera;

        private RaycastHit[] _hits;
        private RaycastHit _dragHit;
        private INodeInputFetcher _nodeInputFetcher;

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
            // interaction
            Ray ray = _camera.ScreenPointToRay(_nodeInputFetcher.GetPointerPosition());
            int hitCount = Physics.RaycastNonAlloc(ray, _hits, Mathf.Infinity, _nodeUILayerMask);
        }
    }
}