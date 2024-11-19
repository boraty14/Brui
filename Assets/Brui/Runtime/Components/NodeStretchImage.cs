using UnityEngine;

namespace Brui.Runtime.Components
{
    [RequireComponent(typeof(SpriteRenderer))]
    [ExecuteAlways]
    public class NodeStretchImage : MonoBehaviour
    {
        [Range(0f, 1f)] public float MinX;
        [Range(0f, 1f)] public float MaxX;
        [Range(0f, 1f)] public float MinY;
        [Range(0f, 1f)] public float MaxY;
        public Vector2 PositionOffset;
        public Vector2 SizeOffset;
        [SerializeField] private SpriteRenderer _image;

        private void OnValidate()
        {
            _image = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (_image.drawMode != SpriteDrawMode.Sliced)
            {
                _image.drawMode = SpriteDrawMode.Sliced;
            }

            var minAnchor = GetAnchorPoint(new Vector2(MinX, MinY));
            var maxAnchor = GetAnchorPoint(new Vector2(MaxX, MaxY));
            _image.size = (maxAnchor - minAnchor) + SizeOffset;
            var currentLocalPosition = transform.localPosition;
            var middlePoint = (maxAnchor + minAnchor) * 0.5f;
            transform.localPosition = new Vector3(
                middlePoint.x + PositionOffset.x,
                middlePoint.y + PositionOffset.y,
                currentLocalPosition.z
            );
        }

        private Vector2 GetAnchorPoint(Vector2 anchor)
        {
            float posX = (anchor.x - 0.5f) * NodeCanvas.CanvasSize.x;
            float posY = (anchor.y - 0.5f) * NodeCanvas.CanvasSize.y;
            return new Vector2(posX, posY);
        }
    }
}