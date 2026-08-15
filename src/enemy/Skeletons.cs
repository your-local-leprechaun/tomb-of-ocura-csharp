using Combatants;
using Items;
using Items.Equipment;
using Stats;

namespace Enemies
{
    public class Skeleton : EnemyBase, ICombatant
    {
        public Skeleton(string name) : base(
            name,
            new Range(2, 4).Roll(Random.Shared),
            "This bone white creature has no eyes, muscles, or brain, yet moves just like a human would. Many have broken bones that will never heal.",
            1,
            75,
            new Dictionary<StatType, int>
            {
                { StatType.Might, 6},
                { StatType.Arcana, 0},
                { StatType.Fortitude, 4},
                { StatType.Vitality, 2},
                { StatType.Chance, 1},
            }
        )
        { }

        protected override List<EnemyMove> Moves { get; } = new()
        {
            new EnemyMove("Slam", 75, 50, 3, 4),
            new EnemyMove("Swing", 100, 80, 1, 2),
        };
    }

    public class SkeletonCommander : EnemyBase, ICombatant
    {
        public SkeletonCommander(string name) : base (
            name,
            new Range(4, 6).Roll(Random.Shared),
            "A larger skeleton with a mark on it's skull. It commands other skeletons and is stronger than a base skeleton.",
            2,
            200,
            new Dictionary<StatType, int>
            {
                { StatType.Might, 7},
                { StatType.Arcana, 0},
                { StatType.Fortitude, 8},
                { StatType.Vitality, 15},
                { StatType.Chance, 1},
            },
            new Dictionary<EquipType, IEquipment>
            {
                { EquipType.Melee, new BasicSword()},
                { EquipType.Gloves, new BrokenGloves()},
            }
        )
        { }

        protected override int WeaponAttackWeight => 100;

        protected override List<ItemDrop>? Drops => new()
        {
            new ItemDrop(() => new BasicSword(), 100),
            // TODO: scale this with the Chance stat once that's wired up, instead of a flat 25%.
            new ItemDrop(() => new BrokenGloves(), 25),
        };
    }
}