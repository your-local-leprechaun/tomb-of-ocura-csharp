using System.Runtime.CompilerServices;
using Combatants;
using Returns;

namespace Status
{
    public interface IStatus
    {
        string Name { get; }
        int Time { get; }
        /// <summary>
        /// Inflicts this status upon the target, must be ICombatant, giving access to most things, such
        /// as stats, equipment, etc. Called once per turn.
        /// </summary>
        /// <param name="target">Who is being inflicted with the status?</param>
        Return Inflict(ICombatant target);
        void ExtendTime(int amount);
    }

    public class StatusBase : IStatus
    {
        public int Time { get; protected set; }
        public string Name { get; init; }

        public virtual Return Inflict(ICombatant target)
        {
            return new Return("Status inflicted!");
        }

        public void ExtendTime(int amount)
        {
            Time += amount;
        }

        public StatusBase(int time, string name)
        {
            Time = time;
            Name = name;
        }
    }
}