using System;
using UnityEngine;

namespace Brui.Components
{
    [ExecuteAlways]
    public class NodeCollider : NodeComponent
    {
        [SerializeField] private ENodeCollider _nodeColliderType;
        [SerializeField] private NodeColliderReferences _colliderReferences;

        private ENodeCollider _latestNodeColliderType;
        private Vector2 _latestNodeSize;
        private int _latestNodeOrder;

        private float _circleColliderSize;
        private Vector2 _boxColliderSize;
        private Vector2 _capsuleColliderSize;
        private CapsuleDirection2D _capsuleDirection;

        private void Update()
        {
            if (_latestNodeColliderType == _nodeColliderType &&
                _latestNodeSize == NodeTransform.NodeSize &&
                _latestNodeOrder == NodeTransform.NodeOrder &&
                IsColliderSizeSame())
            {
                return;
            }

            var nodeSize = NodeTransform.NodeSize;
            var nodeOrder = NodeTransform.NodeOrder;
            switch (_nodeColliderType)
            {
                case ENodeCollider.Box:
                    if (_colliderReferences.CapsuleCollider != null)
                    {
                        DestroyCollider(_colliderReferences.CapsuleCollider);
                    }

                    if (_colliderReferences.CircleCollider != null)
                    {
                        DestroyCollider(_colliderReferences.CircleCollider);
                    }

                    if (_colliderReferences.BoxCollider == null)
                    {
                        _colliderReferences.BoxCollider = gameObject.AddComponent<BoxCollider2D>();
                    }

                    _colliderReferences.BoxCollider.size = nodeSize;
                    _colliderReferences.BoxCollider.layerOverridePriority = nodeOrder;
                    _boxColliderSize = nodeSize;
                    break;
                case ENodeCollider.Circle:
                    if (_colliderReferences.CapsuleCollider != null)
                    {
                        DestroyCollider(_colliderReferences.CapsuleCollider);
                    }

                    if (_colliderReferences.BoxCollider != null)
                    {
                        DestroyCollider(_colliderReferences.BoxCollider);
                    }

                    if (_colliderReferences.CircleCollider == null)
                    {
                        _colliderReferences.CircleCollider = gameObject.AddComponent<CircleCollider2D>();
                    }

                    _colliderReferences.CircleCollider.radius = nodeSize.x * 0.5f;
                    _colliderReferences.CircleCollider.layerOverridePriority = nodeOrder;
                    _circleColliderSize = nodeSize.x * 0.5f;
                    break;
                case ENodeCollider.Capsule:
                    if (_colliderReferences.CircleCollider != null)
                    {
                        DestroyCollider(_colliderReferences.CircleCollider);
                    }

                    if (_colliderReferences.BoxCollider != null)
                    {
                        DestroyCollider(_colliderReferences.BoxCollider);
                    }

                    if (_colliderReferences.CapsuleCollider == null)
                    {
                        _colliderReferences.CapsuleCollider = gameObject.AddComponent<CapsuleCollider2D>();
                    }

                    _colliderReferences.CapsuleCollider.direction = nodeSize.x > nodeSize.y
                        ? CapsuleDirection2D.Horizontal
                        : CapsuleDirection2D.Vertical;
                    _colliderReferences.CapsuleCollider.size = nodeSize;
                    _colliderReferences.CapsuleCollider.layerOverridePriority = nodeOrder;
                    _capsuleColliderSize = nodeSize;
                    break;
            }

            _latestNodeColliderType = _nodeColliderType;
            _latestNodeSize = NodeTransform.NodeSize;
            _latestNodeOrder = NodeTransform.NodeOrder;
        }

        private bool IsColliderSizeSame()
        {
            return _nodeColliderType switch
            {
                ENodeCollider.Box => _boxColliderSize == _colliderReferences.BoxCollider.size,
                ENodeCollider.Circle => Mathf.Approximately(_circleColliderSize,
                    _colliderReferences.CircleCollider.radius),
                ENodeCollider.Capsule => _capsuleColliderSize == _colliderReferences.CapsuleCollider.size &&
                                         _capsuleDirection == _colliderReferences.CapsuleCollider.direction,
                _ => false
            };
        }

        private void DestroyCollider(Component colliderComponent)
        {
            if (Application.isPlaying)
            {
                Destroy(colliderComponent);
            }
            else
            {
                DestroyImmediate(colliderComponent);
            }
        }
    }

    public enum ENodeCollider
    {
        Box,
        Circle,
        Capsule
    }

    [Serializable]
    public class NodeColliderReferences
    {
        public BoxCollider2D BoxCollider;
        public CircleCollider2D CircleCollider;
        public CapsuleCollider2D CapsuleCollider;
    }
}