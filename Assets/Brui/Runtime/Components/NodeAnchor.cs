using UnityEngine;

namespace Brui.Runtime.Components
{
    public class NodeAnchor : MonoBehaviour
    {
        public Vector2 Anchors;
        public Vector2 PositionOffset;
        
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