using Tokens;
using Commands;

namespace Parser
{
    public class ParseException : Exception
    {
        public ParseException(string message) : base(message) {}
    }

    public class CommandException : Exception
    {
        public CommandException(string message) : base(message) {}
    }

    public sealed class Parser
    {
        // Storage of all possible words to be used!
        private static readonly Dictionary<string, (TokenType Type, string Canonical)> Vocab = new()
        {
            // Nouns
            { "sword", (TokenType.NOUN, "sword") },
            { "state", (TokenType.NOUN, "state") },
            { "game", (TokenType.NOUN, "game") },
            { "inventory", (TokenType.NOUN, "inventory") },
            { "room", (TokenType.NOUN, "room") },
            { "key", (TokenType.NOUN, "key") },
            { "choices", (TokenType.NOUN, "choices") },
            { "door", (TokenType.NOUN, "door") },
            { "north", (TokenType.NOUN, "north") },
            { "hay", (TokenType.NOUN, "hay") },


            // Adjectives
            { "rusty", (TokenType.ADJECTIVE, "rusty") },
            { "old", (TokenType.ADJECTIVE, "old") },

            // Verbs
            { "exit", (TokenType.VERB, "exit") },
            { "quit", (TokenType.VERB, "exit") },
            { "start", (TokenType.VERB, "start") },
            { "move", (TokenType.VERB, "move") },
            { "grab", (TokenType.VERB, "grab") },
            { "get", (TokenType.VERB, "grab") },
            { "walk", (TokenType.VERB, "go") },
            { "go", (TokenType.VERB, "go") },
            { "switch", (TokenType.VERB, "switch") },
            { "check", (TokenType.VERB, "check") },
            { "open", (TokenType.VERB, "open") },
            { "close", (TokenType.VERB, "close") },
            { "show", (TokenType.VERB, "show") },

            // Fillers
            { "the", (TokenType.FILLER, "the") },
        };

        /// <summary>
        /// Public facing, takes in raw input and returns a list 
        /// of tokens in proper grammar.
        /// </summary>
        /// <param name="input">Raw string input</param>
        /// <returns>List of Tokens in grammar order</returns>
        public Command ParseInput(string input)
        {
            string[] words = Scanner(input);

            List<Token> tokens = Tokenize(words)
                        .Where(t => t.Type != TokenType.FILLER)
                        .ToList();

            Command command = Parse(tokens);

            return command;
        }

        private string[] Scanner(string input)
        {
            string[] words = input.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(s => s.ToLower()).ToArray();

            return words;
        }

        private IEnumerable<Token> Tokenize(string[] words)
        {
            for (int i = 0; i < words.Length; i++)
            {
                if (Vocab.TryGetValue(words[i], out var entry))
                {
                    yield return new Token(entry.Type, words[i], entry.Canonical, i);
                }
                else
                {
                    throw new ParseException($"Unknown word: {words[i]}");
                }
            }

            yield return new Token(TokenType.EOF, string.Empty, string.Empty, words.Length);
        }

        private Command Parse(List<Token> tokens)
        {
            int i = 0;

            // Starts with VERB
            if (tokens[i].Type != TokenType.VERB)
            {
                throw new ParseException($"Expected a verb, got unknown word'{tokens[i].Value}'");               
            }
            string verb = tokens[i].Canonical;
            i ++;

            // Standalone Verbs
            if (verb == "exit")
            {
                if (tokens[i].Type != TokenType.EOF)
                {
                    throw new ParseException("Did you mean just 'exit'?");
                }
                return new Command{ Verb = verb, Adjective = string.Empty, Noun = string.Empty};
            }

            // Optional ADJECTIVE
            string? adjective = null;
            if (tokens[i].Type == TokenType.ADJECTIVE)
            {
                adjective = tokens[i].Canonical;
                i++;
            }
            
            // Next is a noun
            if (tokens[i].Type != TokenType.NOUN)
            {
                throw new ParseException($"Expected noun, got unknown '{tokens[i].Value}'");
            }
            string noun = tokens[i].Canonical;
            i++;

            // Ends with EOF
            if (tokens[i].Type != TokenType.EOF)
            {
                throw new ParseException($"Unexpected words after '{noun}'");
            }

            return new Command { Verb = verb, Adjective = adjective, Noun = noun};
        }
    }
}