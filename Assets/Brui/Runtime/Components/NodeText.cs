using Brui.Runtime.Attributes;
using TMPro;
using UnityEngine;

namespace Brui.Runtime.Components
{
    [RequireComponent(typeof(TextMeshPro))]
    [ExecuteAlways]
    public class NodeText : NodeComponent
    {
        [field: SerializeField] [field: ReadOnlyNode]
        public TextMeshPro TMPText { get; private set; }
        public string Text
        {
            get => TMPText.text;
            set => TMPText.text = value;
        }
        
       [ReadOnlyNode] [SerializeField] private RectTransform _textRect;
        
        private Vector2 _latestNodeSize;

        private static readonly Vector2 TextVector = new Vector2(0.5f, 0.5f);
        
        public override void SetComponents()
        {
            base.SetComponents();
            TMPText = GetComponent<TextMeshPro>();
            _textRect = TMPText.rectTransform;
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