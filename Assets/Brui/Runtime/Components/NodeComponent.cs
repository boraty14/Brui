using Brui.Runtime.Attributes;
using UnityEngine;

namespace Brui.Runtime.Components
{
    [RequireComponent(typeof(NodeTransform))]
    public abstract class NodeComponent : MonoBehaviour
    {
        [field: SerializeField] [field: ReadOnlyNode]
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