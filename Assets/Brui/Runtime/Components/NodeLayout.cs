using UnityEngine;

namespace Brui.Runtime.Components
{
    [ExecuteAlways]
    public class NodeLayout : MonoBehaviour
    {
        [SerializeField] private ELayout _layout;
        public bool IsReverse;
        public float Interval;
        public int ElementCount => transform.childCount;
        public float ViewSize => ElementCount * Interval;
        public ELayout Layout => _layout;

        private void Update()
        {
            float startPosition = IsReverse ? (ViewSize - Interval) * 0.5f : (-ViewSize + Interval) * 0.5f;
            float iterator = IsReverse ? -Interval : Interval;

            for (int i = 0; i < ElementCount; i++)
            {
                var element = transform.GetChild(i);
                float currentPosition = startPosition + (i * iterator);
                switch (_layout)
                {
                    case ELayout.Vertical:
                        element.localPosition = new Vector3(
                            0,
                            currentPosition,
                            0);
                        break;
                    case ELayout.Horizontal:
                        element.localPosition = new Vector3(
                            currentPosition,
                            0,
                            0);
                        break;
                }
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Vector3 position = transform.position;
            Vector3 startPosition = position;
            Vector3 endPosition = position;
            float offset = IsReverse ? ViewSize * 0.5f : -ViewSize * 0.5f;
            
            switch (_layout)
            {
                case ELayout.Vertical:
                    startPosition.y -= offset;
                    endPosition.y += offset;
                    break;
                case ELayout.Horizontal:
                    startPosition.x -= offset;
                    endPosition.x += offset;
                    break;
            }
            Gizmos.DrawLine(startPosition,endPosition);
        }
    }

    public enum ELayout
    {
        Vertical,
        Horizontal
    }
}