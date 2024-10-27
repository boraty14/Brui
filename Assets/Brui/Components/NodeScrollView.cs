using UnityEngine;

namespace Brui.Components
{
    [RequireComponent(typeof(NodeLayout))]
    public class NodeScrollView : NodeComponent
    {
        public float ScrollSize;
        [Range(0f, 1f)] [field: SerializeField]
        public float PlacementRatio { get; private set; }

        [field:SerializeField] public NodeLayout NodeLayout { get; private set; }
        [field:SerializeField] public NodeScroll NodeScroll { get; private set; }
        public int ElementCount => transform.childCount;

        public override void SetComponents()
        {
            base.SetComponents();
            NodeLayout = GetComponent<NodeLayout>();
            NodeScroll = GetComponentInParent<NodeScroll>(true);
        }
    }
}