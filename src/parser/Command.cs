
namespace Commands
{
    public class Command
    {
        public required string Verb { get; init; }
        public string? Adjective { get; init; }
        public required string Noun { get; init; }
    }
}