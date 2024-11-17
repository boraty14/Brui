using UnityEngine;

namespace Brui.Runtime.Components
{
    [ExecuteAlways]
    public class NodeAnchor : MonoBehaviour
    {
        public Vector2 Anchor;
        public Vector2 PositionOffset;
        [SerializeField] private NodeCanvas _nodeCanvas;

        private void Update()
        {
            float anchorX = (Anchor.x - 0.5f) * _nodeCanvas.CanvasSize.x;
            float anchorY = (Anchor.y - 0.5f) * _nodeCanvas.CanvasSize.y;

            transform.localPosition =
                new Vector3(
                    anchorX + PositionOffset.x,
                    anchorY + PositionOffset.y,
                    transform.localPosition.z
                );
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Vector3 position = transform.position;
            var localScale = transform.localScale;
            Vector3 size = new Vector3(localScale.x, localScale.y, 0);
            Gizmos.DrawWireCube(position, size);
        }
    }
}