
namespace Commands
{
    public record Command
    {
        public string Verb { get; init; }
        public string? Adjective { get; init; }
        public string Noun { get; init; }

        public Command(string verb, string noun, string? adjective = null)
        {
            Verb = verb;
            Adjective = adjective;
            Noun = noun;

            // Add all to dictionary
            if (adjective is not null)
            {
                Parser.Parser.RegisterAdjective(adjective);
            }
            Parser.Parser.RegisterNoun(noun);
        }
    }
}