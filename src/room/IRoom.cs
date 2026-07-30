using Commands;
using Items;
using Returns;

namespace Rooms
{
    public interface IRoom
    {
        string Description { get; }
        List<string> Choices { get; }
        List<IItem> Items { get; }
    }
}