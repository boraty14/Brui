using UnityEngine;

namespace Brui.Components
{
    [RequireComponent(typeof(NodeTransform))]
    public abstract class NodeComponent : MonoBehaviour
    {
        public NodeTransform NodeTransform { get; private set; }

        private void OnValidate()
        {
            SetComponents();
        }

        private void Awake()
        {
            SetComponents();
        }

        public virtual void SetComponents()
        {
            NodeTransform = GetComponent<NodeTransform>();
        }
    }
}