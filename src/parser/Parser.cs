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
            { "choices", (TokenType.NOUN, "choices") },
            { "door", (TokenType.NOUN, "door") },
            { "north", (TokenType.NOUN, "north") },
            { "hay", (TokenType.NOUN, "hay") },
            { "player", (TokenType.NOUN, "player") },


            // Adjectives
            { "rusty", (TokenType.ADJECTIVE, "rusty") },
            { "old", (TokenType.ADJECTIVE, "old") },

            // Verbs
            { "exit", (TokenType.VERB, "exit") },
            { "quit", (TokenType.VERB, "exit") },
            { "start", (TokenType.VERB, "start") },
            { "grab", (TokenType.VERB, "grab") },
            { "get", (TokenType.VERB, "grab") },
            { "pick up", (TokenType.VERB, "grab")},
            { "move", (TokenType.VERB, "move") },
            { "walk", (TokenType.VERB, "move") },
            { "go", (TokenType.VERB, "move") },
            { "light", (TokenType.VERB, "light") },
            { "switch", (TokenType.VERB, "switch") },
            { "check", (TokenType.VERB, "check") },
            { "open", (TokenType.VERB, "open") },
            { "close", (TokenType.VERB, "close") },
            { "show", (TokenType.VERB, "show") },
            { "status", (TokenType.VERB, "status") },
            { "fix", (TokenType.VERB, "fix") },
            { "straighten", (TokenType.VERB, "fix") },
            { "equip", (TokenType.VERB, "equip") },
            { "unequip", (TokenType.VERB, "unequip") },
            { "attack", (TokenType.VERB, "attack") },
            { "swing", (TokenType.VERB, "attack") },
            { "melee", (TokenType.VERB, "attack") },
            { "hold", (TokenType.VERB, "hold") },
            { "cast", (TokenType.VERB, "cast") },
            { "magic", (TokenType.VERB, "cast") },
            { "leave", (TokenType.VERB, "leave") },
            { "use", (TokenType.VERB, "use") },

            // Fillers
            { "the", (TokenType.FILLER, "the") },
        };

        public static void RegisterNoun(string word)
        {
            word = word.ToLower();
            Vocab[word] = (TokenType.NOUN, word);
        }

        public static void RegisterAdjective(string adjective)
        {
            adjective = adjective.ToLower();
            Vocab[adjective] = (TokenType.ADJECTIVE, adjective);
        }

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
            int i = 0;
            while (i < words.Length)
            {
                // Try to match two-word verbs
                if (i + 1 < words.Length)
                {
                    string twoWord = $"{words[i]} {words[i+1]}";
                    if (Vocab.TryGetValue(twoWord, out var twoWordEntry))
                    {
                        yield return new Token(twoWordEntry.Type, twoWord, twoWordEntry.Canonical, i);
                        i += 2;
                        continue;
                    }
                }

                // Match with a single word instead
                if (Vocab.TryGetValue(words[i], out var entry))
                {
                    yield return new Token(entry.Type, words[i], entry.Canonical, i);
                    i++;
                    continue;
                }
                
                throw new ParseException($"Unknown word: {words[i]}");
            }

            yield return new Token(TokenType.EOF, string.Empty, string.Empty, words.Length);
        }

        private Command Parse(List<Token> tokens)
        {
            int i = 0;

            // Starts with VERB
            if (tokens[i].Type != TokenType.VERB)
            {
                throw new ParseException($"Expected a verb, got unknown word '{tokens[i].Value}'");               
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
                return new Command(verb, string.Empty);
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

            return new Command(verb, noun, adjective);
        }
    }
}