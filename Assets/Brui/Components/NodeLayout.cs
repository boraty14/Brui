namespace Brui.Components
{
    public class NodeLayout : NodeComponent
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