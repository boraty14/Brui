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
        [SerializeField] private SpriteRenderer _image;
        [SerializeField] private NodeCanvas _nodeCanvas;

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

            var minAnchor = _nodeCanvas.GetAnchorPoint(new Vector2(MinX, MinY));
            var maxAnchor = _nodeCanvas.GetAnchorPoint(new Vector2(MaxX, MaxY));
            _image.size = maxAnchor - minAnchor;
        }
    }
}