using UnityEngine;

namespace Brui.Components
{
    [RequireComponent(typeof(NodeLayout))]
    public class NodeScrollView : NodeComponent
    {
        public NodeLayout NodeLayout { get; private set; }
        public NodeScroll NodeScroll { get; private set; }

        protected override void SetComponents()
        {
            base.SetComponents();
            NodeLayout = GetComponent<NodeLayout>();
            NodeScroll = GetComponentInParent<NodeScroll>(true);
        }
    }
}