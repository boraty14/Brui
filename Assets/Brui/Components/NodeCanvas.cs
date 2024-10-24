using Brui.Interaction;
using UnityEngine;

namespace Brui.Components
{
    [ExecuteAlways]
    [DefaultExecutionOrder(NodeExecutionOrders.CanvasExecutionOrder)]
    public class NodeCanvas : MonoBehaviour
    {
        public NodeCamera nodeCamera;

        private Vector2 _screenSize;
        private Rect _safeArea;

        private int _order;

        private void OnEnable()
        {
            nodeCamera = FindAnyObjectByType<NodeCamera>();
        }

        private void Update()
        {
            _screenSize = new Vector2(Screen.width, Screen.height);
            _safeArea = Screen.safeArea;
            _order = 0;

            float cameraVerticalSize = nodeCamera.VerticalSize;
            Vector2 screenSize = _screenSize;
            float width = cameraVerticalSize * (screenSize.x / screenSize.y);
            float height = cameraVerticalSize;
            Vector2 canvasSize = new Vector2(width * 2f, height * 2f);

            transform.position = nodeCamera.transform.position + Vector3.forward * nodeCamera.DistanceToCamera;
            ResolveChildNodes(transform, canvasSize);
        }

        private void ResolveChildNodes(Transform nodeParent, Vector2 parentSize)
        {
            int childCount = nodeParent.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i);
                if (child.TryGetComponent<NodeLayout>(out var nodeLayout))
                {
                    ResolveLayout(nodeLayout, parentSize);
                    continue;
                }

                ResolveNode(child.GetComponent<NodeTransform>(), parentSize);
            }
        }

        private void ResolveNode(NodeTransform nodeTransform, Vector2 parentSize)
        {
            SetNodeTransform(nodeTransform, parentSize);
            ResolveChildNodes(nodeTransform.transform, nodeTransform.NodeSize);
        }

        private void ResolveLayout(NodeLayout nodeLayout, Vector2 parentSize)
        {
            SetNodeTransform(nodeLayout.NodeTransform, parentSize);

            int childCount = nodeLayout.transform.childCount;
            if (childCount == 0)
            {
                return;
            }

            bool isVertical = nodeLayout.layoutType == ENodeLayout.Vertical;
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
                childNode.TransformSettings.AnchorX.Min = isVertical ? 0.5f : 0f;
                childNode.TransformSettings.AnchorX.Max = isVertical ? 0.5f : 0f;
                childNode.TransformSettings.AnchorY.Min = isVertical ? 0f : 0.5f;
                childNode.TransformSettings.AnchorY.Max = isVertical ? 0f : 0.5f;
                childNode.NodeSize = childNode.TransformSettings.SizeOffset;

                Vector2 offset = isVertical
                    ? isReverse
                        ? new Vector2(0f, (childCount - i) * interval)
                        : new Vector2(0f, (i + 1) * interval)
                    : isReverse
                        ? new Vector2((childCount - i) * interval, 0f)
                        : new Vector2((i + 1) * interval, 0f);

                childNode.transform.localPosition = startPosition + offset;
                ResolveChildNodes(child, childNode.NodeSize);
            }
        }


        private void SetNodeTransform(NodeTransform nodeTransform, Vector2 parentSize)
        {
            nodeTransform.NodeOrder = _order;
            _order++;

            float anchorXMin = (nodeTransform.TransformSettings.AnchorX.Min - 0.5f) * parentSize.x;
            float anchorXMax = (nodeTransform.TransformSettings.AnchorX.Max - 0.5f) * parentSize.x;
            float anchorYMin = (nodeTransform.TransformSettings.AnchorY.Min - 0.5f) * parentSize.y;
            float anchorYMax = (nodeTransform.TransformSettings.AnchorY.Max - 0.5f) * parentSize.y;

            // position
            Vector2 location = new Vector2((anchorXMin + anchorXMax) * 0.5f, (anchorYMin + anchorYMax) * 0.5f) +
                               nodeTransform.TransformSettings.PositionOffset;
            
            nodeTransform.transform.localPosition = location;

            // size
            nodeTransform.NodeSize = new Vector2(anchorXMax - anchorXMin, anchorYMax - anchorYMin) +
                                     nodeTransform.TransformSettings.SizeOffset;
            
        }
    }
}