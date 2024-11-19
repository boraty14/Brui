using Brui.Runtime.Interaction;
using UnityEngine;

namespace Brui.Runtime.Components
{
    [ExecuteAlways]
    public class NodeCanvas : MonoBehaviour
    {
        public NodeCamera nodeCamera;
        public bool ApplySafeAreaX;
        public bool ApplySafeAreaY;

        private int _order = 0;
        private Vector2 _screenSize;
        private Rect _safeArea;
        private Vector2 _canvasSize;
        private Vector3 _offset = Vector3.zero;

        public static Vector2 CanvasSize { get; private set; }
        
        private const float NodeOrderOffset = -0.0001f;

        public Vector2 GetAnchorPoint(Vector2 anchor)
        {
            float posX = (anchor.x - 0.5f) * _canvasSize.x;
            float posY = (anchor.y - 0.5f) * _canvasSize.y;
            return new Vector2(posX, posY);
        }

        private void Update()
        {
            _order = 0;
            _screenSize = new Vector2(Screen.width, Screen.height);
            _safeArea = Screen.safeArea;

            var cameraPosition = nodeCamera.transform.position;
            var cameraVerticalSize = nodeCamera.VerticalSize;
            var cameraDistance = nodeCamera.DistanceToCamera;

            Vector2 screenSize = _screenSize;
            float width = cameraVerticalSize * (screenSize.x / screenSize.y) * 2f;
            float height = cameraVerticalSize * 2f;

            _offset = Vector3.zero;
            if (ApplySafeAreaX)
            {
                float leftRatio = _safeArea.x / _screenSize.x;
                float rightRatio = (_screenSize.x - (_safeArea.x + _safeArea.width)) / _screenSize.x;
                _offset.x += -(rightRatio - leftRatio) * 0.5f * width;
                width *= 1 - (leftRatio + rightRatio);
            }

            if (ApplySafeAreaY)
            {
                float bottomRatio = _safeArea.y / _screenSize.y;
                float topRatio = (_screenSize.y - (_safeArea.y + _safeArea.height)) / _screenSize.y;
                _offset.y += -(topRatio - bottomRatio) * 0.5f * height;
                height *= 1 - (bottomRatio + topRatio);
            }

            _canvasSize = new Vector2(width, height);
            CanvasSize = _canvasSize;

            transform.position = cameraPosition + Vector3.forward * cameraDistance + _offset;

            ResolveChildren(transform);
        }

        private void ResolveChildren(Transform childTransform)
        {
            int childCount = childTransform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var child = childTransform.GetChild(i);
                SetChildOffset(child);
                ResolveChildren(child);
            }
        }

        private void SetChildOffset(Transform childTransform)
        {
            _order++;
            var currentPosition = childTransform.position;
            childTransform.position =
                new Vector3(
                    currentPosition.x,
                    currentPosition.y,
                    NodeOrderOffset * _order
                );
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Vector3 position = transform.position;
            Gizmos.DrawWireCube(position, _canvasSize);
        }
    }
}