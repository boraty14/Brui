using UnityEngine;

namespace Brui.Runtime.Components
{
    public class NodeLayout : MonoBehaviour
    {
        public ENodeLayout layoutType;
        public bool isReverse;
    }

    public enum ENodeLayout
    {
        Vertical,
        Horizontal
    }
}