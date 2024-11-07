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

        private int _order;

        private void Update()
        {
            _screenSize = new Vector2(Screen.width, Screen.height);
            _safeArea = Screen.safeArea;
            _order = 0;

            float cameraVerticalSize = nodeCamera.VerticalSize;
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

            transform.position = nodeCamera.transform.position + Vector3.forward * nodeCamera.DistanceToCamera + offset;
            ResolveChildNodes(transform, _canvasSize);
        }

        private void ResolveChildNodes(Transform nodeParent, Vector2 parentSize)
        {
            int childCount = nodeParent.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var child = nodeParent.GetChild(i);
                if (child.TryGetComponent<NodeLayout>(out var nodeLayout))
                {
                    ResolveLayout(nodeLayout, parentSize);
                    continue;
                }


                var childNode = child.GetComponent<NodeTransform>();
                if (childNode == null)
                {
                    childNode = child.gameObject.AddComponent<NodeTransform>();
                }
                ResolveNode(childNode, parentSize);
            }
        }

        private void ResolveNode(NodeTransform nodeTransform, Vector2 parentSize)
        {
            SetNodeTransform(nodeTransform, parentSize);
            ResolveChildNodes(nodeTransform.transform, nodeTransform.NodeSize);
        }

        private void ResolveLayout(NodeLayout nodeLayout, Vector2 parentSize)
        {
            bool isVertical = nodeLayout.layoutType == ENodeLayout.Vertical;

            if (nodeLayout.TryGetComponent<NodeScrollView>(out var nodeScrollView))
            {
                int scrollViewChildCount = nodeScrollView.transform.childCount;
                float scrollSize = scrollViewChildCount * nodeScrollView.NodeScroll.ScrollSettings.ItemSize;
                nodeScrollView.ScrollSize = scrollSize;
                if (isVertical)
                {
                    nodeScrollView.NodeTransform.TransformSettings.AnchorX.Min = 0f;
                    nodeScrollView.NodeTransform.TransformSettings.AnchorX.Max = 1f;
                    nodeScrollView.NodeTransform.TransformSettings.AnchorY.Min = 0.5f;
                    nodeScrollView.NodeTransform.TransformSettings.AnchorY.Max = 0.5f;
                    nodeScrollView.NodeTransform.TransformSettings.SizeOffset =
                        new Vector2(0f, scrollSize);
                }
                else
                {
                    nodeScrollView.NodeTransform.TransformSettings.AnchorX.Min = 0.5f;
                    nodeScrollView.NodeTransform.TransformSettings.AnchorX.Max = 0.5f;
                    nodeScrollView.NodeTransform.TransformSettings.AnchorY.Min = 0f;
                    nodeScrollView.NodeTransform.TransformSettings.AnchorY.Max = 1f;
                    nodeScrollView.NodeTransform.TransformSettings.SizeOffset =
                        new Vector2(scrollSize, 0f);
                }
            }

            SetNodeTransform(nodeLayout.NodeTransform, parentSize);

            int childCount = nodeLayout.transform.childCount;
            if (childCount == 0)
            {
                return;
            }

            float interval = isVertical
                ? nodeLayout.NodeTransform.NodeSize.y / (childCount + 1)
                : nodeLayout.NodeTransform.NodeSize.x / (childCount + 1);
            bool isReverse = nodeLayout.isReverse;

            Vector2 startPosition = isVertical
                ? new Vector2(0f, -nodeLayout.NodeTransform.NodeSize.y * 0.5f)
                : new Vector2(-nodeLayout.NodeTransform.NodeSize.x * 0.5f, 0f);

            for (int i = 0; i < childCount; i++)
            {
                var child = nodeLayout.transform.GetChild(i);
                var childNode = child.GetComponent<NodeTransform>();
                if (isVertical)
                {
                    childNode.TransformSettings.AnchorY.Min = 0f;
                    childNode.TransformSettings.AnchorY.Max = 0f;
                }
                else
                {
                    childNode.TransformSettings.AnchorX.Min = 0f;
                    childNode.TransformSettings.AnchorX.Max = 0f;
                }
                
                float anchorXMin = (childNode.TransformSettings.AnchorX.Min - 0.5f) * parentSize.x;
                float anchorXMax = (childNode.TransformSettings.AnchorX.Max - 0.5f) * parentSize.x;
                float anchorYMin = (childNode.TransformSettings.AnchorY.Min - 0.5f) * parentSize.y;
                float anchorYMax = (childNode.TransformSettings.AnchorY.Max - 0.5f) * parentSize.y;
                
                childNode.SetNodeSize(new Vector2(anchorXMax - anchorXMin, anchorYMax - anchorYMin) +
                                      childNode.TransformSettings.SizeOffset);
                childNode.SetNodeOrder(_order);
                _order++;

                Vector2 offset = isVertical
                    ? isReverse
                        ? new Vector2(0f, (childCount - i) * interval)
                        : new Vector2(0f, (i + 1) * interval)
                    : isReverse
                        ? new Vector2((childCount - i) * interval, 0f)
                        : new Vector2((i + 1) * interval, 0f);

                var combinedPosition = startPosition + offset;
                childNode.TransformSettings.PositionOffset = combinedPosition;
                child.localPosition = combinedPosition;

                var currentPosition = child.position;
                var nodeOffset = childNode.NodeOrder * NodeConstants.NodeSortOffset;
                child.position = new Vector3(currentPosition.x, currentPosition.y, nodeOffset);
                
                ResolveChildNodes(child, childNode.NodeSize);
            }
        }


        private void SetNodeTransform(NodeTransform nodeTransform, Vector2 parentSize)
        {
            nodeTransform.SetNodeOrder(_order);
            _order++;

            float anchorXMin = (nodeTransform.TransformSettings.AnchorX.Min - 0.5f) * parentSize.x;
            float anchorXMax = (nodeTransform.TransformSettings.AnchorX.Max - 0.5f) * parentSize.x;
            float anchorYMin = (nodeTransform.TransformSettings.AnchorY.Min - 0.5f) * parentSize.y;
            float anchorYMax = (nodeTransform.TransformSettings.AnchorY.Max - 0.5f) * parentSize.y;

            // position
            Vector2 location = new Vector2((anchorXMin + anchorXMax) * 0.5f, (anchorYMin + anchorYMax) * 0.5f) +
                               nodeTransform.TransformSettings.PositionOffset;

            nodeTransform.transform.localPosition = location;
            
            var currentPosition = nodeTransform.transform.position;
            var nodeOffset = nodeTransform.NodeOrder * NodeConstants.NodeSortOffset;
            nodeTransform.transform.position = new Vector3(currentPosition.x, currentPosition.y, nodeOffset);

            // size
            nodeTransform.SetNodeSize(new Vector2(anchorXMax - anchorXMin, anchorYMax - anchorYMin) +
                                      nodeTransform.TransformSettings.SizeOffset);
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