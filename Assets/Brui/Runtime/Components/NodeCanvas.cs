using Brui.Runtime.Interaction;
using UnityEngine;

namespace Brui.Runtime.Components
{
    [ExecuteAlways]
    [DefaultExecutionOrder(NodeConstants.CanvasExecutionOrder)]
    public class NodeCanvas : MonoBehaviour
    {
        public NodeCamera nodeCamera;
        public bool ApplySafeAreaX;
        public bool ApplySafeAreaY;

        private Vector2 _screenSize;
        private Rect _safeArea;
        private Vector2 _canvasSize;

        private void Update()
        {
            _screenSize = new Vector2(Screen.width, Screen.height);
            _safeArea = Screen.safeArea;

            var cameraPosition = nodeCamera.transform.position;
            var cameraVerticalSize = nodeCamera.VerticalSize;
            var cameraDistance = nodeCamera.DistanceToCamera;

            Vector2 screenSize = _screenSize;
            float width = cameraVerticalSize * (screenSize.x / screenSize.y) * 2f;
            float height = cameraVerticalSize * 2f;

            Vector3 offset = Vector3.zero;
            if (ApplySafeAreaX)
            {
                float leftRatio = _safeArea.x / _screenSize.x;
                float rightRatio = (_screenSize.x - (_safeArea.x + _safeArea.width)) / _screenSize.x;
                offset.x += -(rightRatio - leftRatio) * 0.5f * width;
                width *= 1 - (leftRatio + rightRatio);
            }

            if (ApplySafeAreaY)
            {
                float bottomRatio = _safeArea.y / _screenSize.y;
                float topRatio = (_screenSize.y - (_safeArea.y + _safeArea.height)) / _screenSize.y;
                offset.y += -(topRatio - bottomRatio) * 0.5f * height;
                height *= 1 - (bottomRatio + topRatio);
            }

            _canvasSize = new Vector2(width, height);

            transform.position = cameraPosition + Vector3.forward * cameraDistance + offset;

            // resolve child nodes

            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i);
                var childNode = child.GetComponent<NodeAnchor>();
                if (childNode == null)
                {
                    childNode = child.gameObject.AddComponent<NodeAnchor>();
                }

                SetNodeAnchor(childNode, new Vector2(width, height));
            }
        }

        private void SetNodeAnchor(NodeAnchor nodeAnchor, Vector2 parentSize)
        {
            float anchorXMin = (nodeAnchor.XAnchors.x - 0.5f) * parentSize.x;
            float anchorXMax = (nodeAnchor.XAnchors.y - 0.5f) * parentSize.x;
            float anchorYMin = (nodeAnchor.YAnchors.x - 0.5f) * parentSize.y;
            float anchorYMax = (nodeAnchor.YAnchors.y - 0.5f) * parentSize.y;

            nodeAnchor.transform.localPosition =
                new Vector2((anchorXMin + anchorXMax) * 0.5f, (anchorYMin + anchorYMax) * 0.5f) +
                nodeAnchor.PositionOffset;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;

            Vector3 position = transform.position;
            Vector3 size = new Vector3(_canvasSize.x, _canvasSize.y, 0);
            Gizmos.DrawWireCube(position, size);
        }
    }
}