
namespace Returns
{
    public class Return
    {
        public string Message { get; init; }

        public State.IState? UpdateState { get; init; } = null;

        public bool? Previous { get; init; } = null;

        public Return (string message, State.IState? state = null, bool? previous = null)
        {
            Message = message;
            if (state != null)
            {
                UpdateState = state;
            }
            if (previous != null)
            {
                Previous = previous;
            }
        } 
    }
}