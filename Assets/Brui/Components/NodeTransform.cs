using System;
using UnityEngine;

namespace Brui.Components
{
    public class NodeTransform : MonoBehaviour
    {
        public NodeTransformSettings TransformSettings;
        private NodeCanvas _nodeCanvas;

        public Vector2 NodeSize;
        public int NodeOrder;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;

            Vector3 position = transform.position;
            Vector3 size = new Vector3(NodeSize.x, NodeSize.y, 0);
            Gizmos.DrawWireCube(position, size);
        }
    }

    [Serializable]
    public class NodeTransformSettings
    {
        public Vector2 SizeOffset = new Vector2(1f, 1f);
        public Vector2 PositionOffset = Vector2.zero;
        public NodeAnchor AnchorX = new NodeAnchor { Min = 0.5f, Max = 0.5f };
        public NodeAnchor AnchorY = new NodeAnchor { Min = 0.5f, Max = 0.5f };
    }

    [Serializable]
    public class NodeAnchor
    {
        public float Min;
        public float Max;
    }
}