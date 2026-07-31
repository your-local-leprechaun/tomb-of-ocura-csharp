
namespace Items.Equipment
{
    public class RustySword : IEquipment
    {
        public string Description { get; init; }
        public ItemType Type { get; init; }
        public string ItemName { get; init; }

        public RustySword()
        {
            Description = "A old, rusty sword. It doesn't look powerful.";
            Type = ItemType.Equipment;
            ItemName = "Rusty Sword";

            Parser.Parser.RegisterNoun("Sword");
            Parser.Parser.RegisterAdjective("Rusty");
        }
    }
}