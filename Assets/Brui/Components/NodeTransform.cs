using System;
using Brui.Attributes;
using UnityEngine;

namespace Brui.Components
{
    public class NodeTransform : MonoBehaviour
    {
        public NodeTransformSettings TransformSettings = new();
        private NodeCanvas _nodeCanvas;
        [field: SerializeField] [field: ReadOnlyNode]
        public Vector2 NodeSize { get; private set; }
        [field: SerializeField] [field: ReadOnlyNode]
        public int NodeOrder { get; private set; }

        public void SetNodeSize(Vector2 nodeSize) => NodeSize = nodeSize;
        public void SetNodeOrder(int nodeOrder) => NodeOrder = nodeOrder;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;

            Vector3 position = transform.position;
            var localScale = transform.localScale;
            Vector3 size = new Vector3(NodeSize.x * localScale.x, NodeSize.y * localScale.y, 0);
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