using UnityEngine;

namespace Brui.Components
{
    [RequireComponent(typeof(NodeTransform))]
    public class NodeLayout : MonoBehaviour
    {
        public ENodeLayout layoutType;
        public bool isReverse;
        public NodeTransform NodeTransform { get; private set; }
        
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

    public enum ENodeLayout
    {
        Vertical,
        Horizontal
    }
}