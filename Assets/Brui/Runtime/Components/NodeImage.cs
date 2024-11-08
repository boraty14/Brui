using Brui.Runtime.Attributes;
using UnityEngine;

namespace Brui.Runtime.Components
{
    [RequireComponent(typeof(SpriteRenderer))]
    [ExecuteAlways]
    public class NodeImage : NodeComponent
    {
        public bool preserveAspect;
        [field: SerializeField] [field: ReadOnlyNode]
        public SpriteRenderer Image { get; private set; }

        private Vector2 _latestNodeSize;
        private bool _latestPreserveAspectState;

        public override void SetComponents()
        {
            base.SetComponents();
            Image = GetComponent<SpriteRenderer>();
            if (Image.sprite == null)
            {
                Image.sprite = Resources.Load<Sprite>("4x4");
            }
        }

        private void Update()
        {
            if (_latestNodeSize == NodeTransform.NodeSize &&
                _latestNodeSize == Image.size &&
                _latestPreserveAspectState == preserveAspect &&
                Image.drawMode == SpriteDrawMode.Sliced)
            {
                return;
            }
            
            Image.drawMode = SpriteDrawMode.Sliced;
            if (preserveAspect)
            {
                float spriteWidth = Image.sprite.rect.width;
                float spriteHeight = Image.sprite.rect.height;
                float spriteAspectRatio = spriteHeight < 0.001f ? 0f : spriteWidth / spriteHeight;

                var nodeSize = NodeTransform.NodeSize;
                float nodeWidth = nodeSize.x;
                float nodeHeight = nodeSize.y;
                float nodeAspectRatio = nodeHeight < 0.001f ? 0f : nodeWidth / nodeHeight;
                
                if (spriteAspectRatio > nodeAspectRatio)
                {
                    Image.size = new Vector2(nodeSize.x, nodeSize.y * (nodeAspectRatio / spriteAspectRatio));
                }
                else if(nodeAspectRatio > spriteAspectRatio)
                {
                    Image.size = new Vector2(nodeSize.x * (spriteAspectRatio / nodeAspectRatio), nodeSize.y);
                }
                else
                {
                    Image.size = nodeSize;
                }
            }
            else
            {
                Image.size = NodeTransform.NodeSize;
            }
            
            _latestNodeSize = NodeTransform.NodeSize;
            _latestPreserveAspectState = preserveAspect;
        }
    }
}