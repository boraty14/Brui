using UnityEngine;

namespace Brui.Components
{
    [ExecuteAlways]
    public class NodeCollider : MonoBehaviour
    {
        [SerializeField] private ENodeCollider _nodeColliderType;
        public NodeTransform NodeTransform { get; private set; }

        private ENodeCollider _latestNodeColliderType;
        private Vector2 _latestNodeSize;
        
        private void OnValidate()
        {
            SetComponents();
        }

        private void Awake()
        {
            SetComponents();
        }

        private void SetComponents()
        {
            NodeTransform = GetComponent<NodeTransform>();
        }
    }

    public enum ENodeCollider
    {
        Box,
        Circle,
        Capsule
    }
}