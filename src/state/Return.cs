
using Combatants;
using Rooms;

namespace Returns
{
    public class Return
    {
        public string Message { get; init; }

        public State.IState? UpdateState { get; init; } = null;

        public bool? Previous { get; init; } = null;
        public bool? EarlyReturn { get; init; } = null;
        public List<ICombatant>? Combatants { get; init; } = null;
        public bool? Equipment { get; init; } = null;
        public bool? Checkpoint { get; init; } = null;

        public Return (
            string message,
            State.IState? state = null,
            bool? previous = null,
            bool? earlyReturn = null,
            List<ICombatant>? combatants = null,
            bool? equipment = null,
            bool? checkpoint = null)
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
            if (earlyReturn != null)
            {
                EarlyReturn = earlyReturn;
            }
            if (combatants != null)
            {
                Combatants = combatants;
            }
            if (equipment != null)
            {
                Equipment = equipment;
            }
            if (checkpoint != null)
            {
                Checkpoint = checkpoint;
            }
        } 
    }
}