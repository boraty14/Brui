using UnityEngine;

namespace Brui.Components
{
    [RequireComponent(typeof(NodeTransform))]
    [ExecuteAlways]
    public class NodeLayout : MonoBehaviour
    {
        public ENodeLayout layoutType;
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

        public void SetLayout()
        {
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i);
                if (!child.TryGetComponent<NodeTransform>(out var nodeTransform))
                {
                    continue;
                }
                
                // todo get position according to node size and orientation
            }
        }
    }

    public enum ENodeLayout
    {
        Vertical,
        Horizontal
    }
}