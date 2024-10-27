using TMPro;
using UnityEngine;

namespace Brui.Components
{
    [RequireComponent(typeof(TextMeshPro))]
    [ExecuteAlways]
    public class NodeText : NodeComponent
    {
        public TextMeshPro Text { get; private set; }
        private RectTransform _textRect;
        
        private Vector2 _latestNodeSize;

        private static readonly Vector2 TextVector = new Vector2(0.5f, 0.5f);
        
        public override void SetComponents()
        {
            base.SetComponents();
            Text = GetComponent<TextMeshPro>();
            _textRect = Text.rectTransform;
        }

        private void Update()
        {
            if (_latestNodeSize == NodeTransform.NodeSize &&
                _latestNodeSize == _textRect.sizeDelta &&
                _textRect.anchorMin == TextVector &&
                _textRect.anchorMax == TextVector &&
                _textRect.pivot == TextVector)
            {
                return;
            }

            _textRect.anchorMin = TextVector;
            _textRect.anchorMax = TextVector;
            _textRect.pivot = TextVector;
            _textRect.sizeDelta = NodeTransform.NodeSize;

            _latestNodeSize = NodeTransform.NodeSize;
        }
    }
}