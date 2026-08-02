
namespace Items.Equipment
{
    public interface IMelee : IEquipment
    {
        bool Swing();
        int Damage();
    }
}
