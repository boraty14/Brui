using Brui.Interaction;
using UnityEngine;

namespace Brui.Components
{
    [ExecuteAlways]
    [DefaultExecutionOrder(-100)]
    public class NodeCanvas : MonoBehaviour
    {
        private NodeCamera _nodeCamera;
        private NodeCamera NodeCamera
        {
            get
            {
                if (_nodeCamera == null)
                {
                    _nodeCamera = FindAnyObjectByType<NodeCamera>();
                }

                return _nodeCamera;
            }
        }

        private Vector2 _screenSize;
        private Rect _safeArea;

        private int _order;

        private void Update()
        {
            _screenSize = new Vector2(Screen.width, Screen.height);
            _safeArea = Screen.safeArea;
            _order = 0;
            float cameraVerticalSize = NodeCamera.VerticalSize;
            Vector2 screenSize = _screenSize;
            float width = cameraVerticalSize * (screenSize.x / screenSize.y);
            float height = cameraVerticalSize;
            Vector2 canvasSize = new Vector2(width * 2f, height * 2f);

            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i);
                if (!child.TryGetComponent<NodeTransform>(out var nodeTransform))
                {
                    continue;
                }

                ResolveNode(nodeTransform, canvasSize);
            }
        }

        private void ResolveNode(NodeTransform nodeTransform, Vector2 parentSize)
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
            Vector3 nodeLocation = NodeCamera.transform.position +
                                   new Vector3(location.x, location.y, NodeCamera.DistanceToCamera);
            nodeTransform.transform.localPosition = nodeLocation;
            
            // size
            nodeTransform.NodeSize = new Vector2(anchorXMax - anchorXMin, anchorYMax - anchorYMin) +
                                     nodeTransform.TransformSettings.SizeOffset;
            
            // reoccur
            int childCount = nodeTransform.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var child = nodeTransform.transform.GetChild(i);
                if (!child.TryGetComponent<NodeTransform>(out var childTransform))
                {
                    continue;
                }
                
                ResolveNode(childTransform, nodeTransform.NodeSize);
            }
        }
    }
}