
namespace Tokens
{
    /// <summary>
    /// All possible options a token can be
    /// </summary>
    public enum TokenType
    {
        VERB,
        ADJECTIVE,
        NOUN,
        FILLER,
        EOF
    }

    public sealed class Token
    {
        // What type is this token?
        public TokenType Type { get; init; }

        // Exact value of this token
        public string Value { get; init; } = string.Empty;

        // Canonical Value, used for groupings
        public string Canonical { get; init; } = string.Empty;

        // Position of this token
        public int Position { get; init; }

        // Init
        public Token(TokenType type, string value, string canonical, int position)
        {
            Type = type;
            Value = value;
            Canonical = canonical;
            Position = position;
        }
    }
}