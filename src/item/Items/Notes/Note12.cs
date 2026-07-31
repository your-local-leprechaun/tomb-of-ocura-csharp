namespace Items
{
    class Note12 : IItem
    {
        public string Description { get; init; }
        public ItemType Type { get; init; }
        public string ItemName { get; init; }

        public Note12()
        {
            Description = "'How were we supposed to know? They told us he " +
                        "was evil and we trusted them. It was them all " +
                        "along.\nThere's no way out for us...'";
            Type = ItemType.Misc;
            ItemName = "Note #12";

            Parser.Parser.RegisterNoun("Note #12");
        }
    }
}