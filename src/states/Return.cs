
namespace Returns
{
    public class Return
    {
        public string Message { get; init; }

        public State.IState? UpdateState { get; init; } = null;

        public Return (string message, State.IState? state = null)
        {
            Message = message;
            if (state != null)
            {
                UpdateState = state;
            }
        } 
    }
}