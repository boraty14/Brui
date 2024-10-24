using System.Collections.Generic;
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
        private readonly List<NodeLayout> _layouts = new();

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
            _layouts.Clear();
            
            float cameraVerticalSize = nodeCamera.VerticalSize;
            Vector2 screenSize = _screenSize;
            float width = cameraVerticalSize * (screenSize.x / screenSize.y);
            float height = cameraVerticalSize;
            Vector2 canvasSize = new Vector2(width * 2f, height * 2f);

            ResolveChildNodes(transform,canvasSize);
            ResolveLayouts();
        }
        
        private void ResolveChildNodes(Transform nodeParent, Vector2 parentSize)
        {
            int childCount = nodeParent.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i);
                if (!child.TryGetComponent<NodeTransform>(out var nodeTransform))
                {
                    continue;
                }
                ResolveNode(nodeTransform, parentSize);
            }
        }

        private void ResolveNode(NodeTransform nodeTransform, Vector2 parentSize)
        {
            if (nodeTransform.transform.TryGetComponent<NodeLayout>(out var nodeLayout))
            {
                _layouts.Add(nodeLayout);
            }
            SetNodeTransform(nodeTransform, parentSize);
            ResolveChildNodes(nodeTransform.transform, nodeTransform.NodeSize);
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
            Vector3 nodeLocation = nodeCamera.transform.position +
                                   new Vector3(location.x, location.y, nodeCamera.DistanceToCamera);
            nodeTransform.transform.localPosition = nodeLocation;

            // size
            nodeTransform.NodeSize = new Vector2(anchorXMax - anchorXMin, anchorYMax - anchorYMin) +
                                     nodeTransform.TransformSettings.SizeOffset;
        }

        private void ResolveLayouts()
        {
            foreach (var layout in _layouts)
            {
                layout.SetLayout();
            }
        }
    }
}