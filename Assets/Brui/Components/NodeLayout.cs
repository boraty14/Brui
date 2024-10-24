using System.Collections.Generic;
using UnityEngine;

namespace Brui.Components
{
    [RequireComponent(typeof(NodeTransform))]
    [ExecuteAlways]
    public class NodeLayout : MonoBehaviour
    {
        public ENodeLayout layoutType;
        public bool isReverse;
        public NodeTransform NodeTransform { get; private set; }

        private readonly List<NodeTransform> _childNodes;
        
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
        }
    }

    public enum ENodeLayout
    {
        Vertical,
        Horizontal
    }
}