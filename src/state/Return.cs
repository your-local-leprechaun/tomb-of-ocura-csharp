
using Frontend;
using Rooms;

namespace Returns
{
    public class Return
    {
        public string Message { get; init; }
        public string Panel { get; init; }

        public State.IState? UpdateState { get; init; } = null;

        public bool? Previous { get; init; } = null;
        public bool? EarlyReturn { get; init; } = null;
        public bool? SkipInput { get; init; } = null;
        public bool? Checkpoint { get; init; } = null;

        public Return (
            string message,
            State.IState? state = null,
            string? sidePanel = null,
            bool? previous = null,
            bool? earlyReturn = null,
            bool? skipInput = null,
            bool? checkpoint = null)
        {
            Message = message;
            if (state != null)
            {
                UpdateState = state;
            }
            Panel = sidePanel ?? SidePanel.Basic();
            if (previous != null)
            {
                Previous = previous;
            }
            if (earlyReturn != null)
            {
                EarlyReturn = earlyReturn;
            }
            if (skipInput != null)
            {
                SkipInput = skipInput;
            }
            if (checkpoint != null)
            {
                Checkpoint = checkpoint;
            }
        }
    }
}