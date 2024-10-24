using UnityEngine;

namespace Brui.Components
{
    [RequireComponent(typeof(NodeTransform))]
    [RequireComponent(typeof(SpriteRenderer))]
    [ExecuteAlways]
    
    public class NodeImage : MonoBehaviour
    {
        public bool preserveAspect;
        public NodeTransform NodeTransform { get; private set; }
        public SpriteRenderer Image { get; private set; }

        private Vector2 _latestNodeSize;
        private Vector2 _latestSpriteSize;
        private bool _latestPreserveAspectState;
        private int _latestNodeOrder;

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
            Image = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (_latestNodeSize == NodeTransform.NodeSize &&
                _latestPreserveAspectState == preserveAspect &&
                _latestSpriteSize == Image.size &&
                _latestNodeOrder == Image.sortingOrder &&
                _latestNodeOrder == NodeTransform.NodeOrder)
            {
                return;
            }
            
            Image.drawMode = SpriteDrawMode.Sliced;
            Image.sortingOrder = NodeTransform.NodeOrder;
            if (preserveAspect)
            {
                float spriteWidth = Image.sprite.rect.width;
                float spriteHeight = Image.sprite.rect.height;
                float spriteAspectRatio = spriteWidth / spriteHeight;

                var nodeSize = NodeTransform.NodeSize;
                float nodeWidth = nodeSize.x;
                float nodeHeight = nodeSize.y;
                float nodeAspectRatio = nodeWidth / nodeHeight;

                if (spriteAspectRatio > nodeAspectRatio) // (4,4) (2,4)
                {
                    Image.size = new Vector2(nodeSize.x, nodeSize.y * (nodeAspectRatio / spriteAspectRatio));
                }
                else
                {
                    Image.size = new Vector2(nodeSize.x * (spriteAspectRatio / nodeAspectRatio), nodeSize.y);
                }
            }
            else
            {
                Image.size = NodeTransform.NodeSize;
            }
            
            _latestNodeSize = NodeTransform.NodeSize;
            _latestPreserveAspectState = preserveAspect;
            _latestSpriteSize = Image.size;
            _latestNodeOrder = Image.sortingOrder;
        }
    }
}