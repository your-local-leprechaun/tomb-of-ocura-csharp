
using Microsoft.VisualBasic;
using Returns;
using Stats;
using Status;

namespace Items.Equipment
{
    public interface ISpell : IEquipment
    {
        IStatus Status { get; }
    }

    public class SpellBase : EquipmentBase, ISpell
    {
        private readonly Func<IStatus> _statusFactory;
        public IStatus Status => _statusFactory();

        public SpellBase(Func<IStatus> statusFactory, string itemName, string description, Tuple<StatType, int>? requirement = null) :
        base (
            description,
            itemName,
            EquipType.Spell,
            requirement
        )
        {
            _statusFactory = statusFactory;
        }
    }
}