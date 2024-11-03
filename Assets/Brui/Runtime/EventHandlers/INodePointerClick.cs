namespace Brui.Runtime.EventHandlers
{
    public interface INodePointerClick
    {
        void OnStartClick();
        void OnCompleteClick();
        void OnCancelClick();
    }
}