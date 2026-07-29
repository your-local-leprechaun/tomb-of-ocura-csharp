using Commands;
using Returns;


namespace State
{
    public interface IState
    {
        /// <summary>
        /// Takes in command and returns dicitonary with information on updates and dispaly
        /// </summary>
        /// <param name="command">Command to make happen</param>
        /// <returns>Returns a Return object with message and updates</returns>
        Return Execute(Command command);

        /// <summary>
        /// Function that triggers when this state activates within Game
        /// </summary>
        /// <returns>Dictionary with information/updates.</returns>
        Return Activate();
    }
}