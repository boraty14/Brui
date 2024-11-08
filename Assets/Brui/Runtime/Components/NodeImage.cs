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
        [field: SerializeField] [field: ReadOnlyNode]
        public float ScaleFactor { get; private set; }

        private Vector2 _latestNodeSize;
        private Vector2 _latestImageSize;
        private bool _latestPreserveAspectState;

        public override void SetComponents()
        {
            base.SetComponents();
            Image = GetComponent<SpriteRenderer>();
            if (Image.sprite == null)
            {
                Image.sprite = Resources.Load<Sprite>("4x4");
            }
            ScaleFactor = 1f;
        }

        public void SetScaleFactor(float scaleFactor)
        {
            ScaleFactor = scaleFactor;
        }

        private void Update()
        {
            if (_latestNodeSize == NodeTransform.NodeSize &&
                _latestImageSize == Image.size &&
                _latestPreserveAspectState == preserveAspect &&
                Image.drawMode == SpriteDrawMode.Sliced)
            {
                return;
            }

            Vector2 imageSize = Image.size;
            
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
                    imageSize = new Vector2(nodeSize.x, nodeSize.y * (nodeAspectRatio / spriteAspectRatio));
                }
                else if(nodeAspectRatio > spriteAspectRatio)
                {
                    imageSize = new Vector2(nodeSize.x * (spriteAspectRatio / nodeAspectRatio), nodeSize.y);
                }
                else
                {
                    imageSize = nodeSize;
                }
            }
            else
            {
                imageSize = NodeTransform.NodeSize;
            }

            Image.size = imageSize * ScaleFactor;
            
            _latestNodeSize = NodeTransform.NodeSize;
            _latestImageSize = Image.size;
            _latestPreserveAspectState = preserveAspect;
        }
    }
}