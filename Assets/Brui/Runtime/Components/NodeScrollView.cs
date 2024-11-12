using Brui.Runtime.Attributes;
using UnityEngine;

namespace Brui.Runtime.Components
{
    [RequireComponent(typeof(NodeLayout))]
    public class NodeScrollView : MonoBehaviour
    {
        [ReadOnlyNode] public float ScrollSize;
        [field:Range(0f, 1f)] [field: SerializeField]
        public float PlacementRatio { get; private set; }

        [field: SerializeField] [field: ReadOnlyNode]
        public NodeLayout NodeLayout { get; private set; }
        [field: SerializeField] [field: ReadOnlyNode]
        public NodeScroll NodeScroll { get; private set; }
        public int ElementCount => transform.childCount;

        private void Awake()
        {
            NodeLayout = GetComponent<NodeLayout>();
            NodeScroll = GetComponentInParent<NodeScroll>(true);
        }
    }
}