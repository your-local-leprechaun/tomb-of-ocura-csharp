using Basic;
using Commands;
using Items;
using Parser;
using Returns;
using State;

namespace Rooms
{
    public class RoomBase<T> : Singleton<T>, IState, IRoom
        where T : RoomBase<T>
    {
        // Singleton Information
        private RoomBase() {}

        protected static T Get => Instance;

        // State Information
        public Return Execute(Command command)
        {
            // Shared Commands that all rooms share.
            if (command.Verb == "check" && command.Noun == "room")
            {
                // Show the description again.
                return new Return(Description);
            }

            // Custom Choices
            Return? result = CustomChoices(command);
            if (result != null)
            {
                return result;
            }

            // Return an Unknown
            throw new CommandException("--Unknown Command--");
        }

        protected virtual Return? CustomChoices(Command command)
        {
            return null;
        }

        public virtual Return Activate()
        {
            return new Return("Room is not setup");
        }

        // Room Information
        public string Description { get; private set; } = "";
        public List<string> Choices { get; private set; } = [];
        public List<IItem> Items { get; private set; } = [];

        protected void UpdateDescription(string newDescription)
        {
            Description = newDescription;
        }

        protected void AddChoice(string newChoice)
        {
            Choices.Add(newChoice);
        }

        protected void RemoveChoice(string removeChoice)
        {
            Choices.Remove(removeChoice);
        }

        protected void AddItem(IItem item)
        {
            Items.Add(item);
        }

        protected void RemoveItem(IItem item)
        {
            Items.Remove(item);
        }

        protected void CollectItem(IItem item)
        {
            // Check if Item is in the room

            // Add to Inventory

            // Remove from room
        }
    }
}