/*
Used for a number of functions that need to be used by different stuff, but doesn't really fit anywhere. Think
of this namespace as a number of general functions
*/

namespace Utilities
{
    public static class General
    {
        /*print out is used to print text out letter by letter to give some more style*/
        public static void PrintOut(string text)
        {
            foreach (char character in text)
            {
                Console.Write(character);
                Thread.Sleep(10);
            }
            Console.WriteLine();
        }
    }
}