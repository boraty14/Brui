using UnityEngine;

namespace Brui.Components
{
    [RequireComponent(typeof(NodeTransform))]
    public class NodeImage : MonoBehaviour
    {
        public SpriteRenderer Image;
        public NodeTransform NodeTransform { get; private set; }

        private void Awake()
        {
            NodeTransform = GetComponent<NodeTransform>();
        }
    }
}