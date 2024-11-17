using UnityEngine;

namespace Brui.Runtime.Components
{
    [ExecuteAlways]
    public class NodeLayout : MonoBehaviour
    {
        public ELayout Layout;
        public bool IsReverse;
        public float Interval;
        public int ElementCount => transform.childCount;
        public float ViewSize => ElementCount * Interval;

        private void Update()
        {
            float startPosition = IsReverse ? (ViewSize - Interval) * 0.5f  : (-ViewSize + Interval) * 0.5f;
            float iterator = IsReverse ? -Interval : Interval;
            
            for (int i = 0; i < ElementCount; i++)
            {
                var element = transform.GetChild(i);
                var elementLocalPosition = element.localPosition;
                float currentPosition = startPosition + (i * iterator);
                switch (Layout)
                {
                    case ELayout.Vertical:
                        element.localPosition = new Vector3(
                            elementLocalPosition.x,
                            currentPosition,
                            elementLocalPosition.z);
                        break;
                    case ELayout.Horizontal:
                        element.localPosition = new Vector3(
                            currentPosition,
                            elementLocalPosition.y,
                            elementLocalPosition.z);
                        break;
                }
            }
            
            
            

        }
    }
    
    public enum ELayout
    {
        Vertical,
        Horizontal
    }
}