using Commands;


namespace State
{
    interface IState
    {
        /// <summary>
        /// Takes in command and returns dicitonary with information on updates and dispaly
        /// </summary>
        /// <param name="command">Command to make happen</param>
        /// <returns>Dictionary with information/updates</returns>
        Dictionary<string, object> Execute(Command command);
    }
}