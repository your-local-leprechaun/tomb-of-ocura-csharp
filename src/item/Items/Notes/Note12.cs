namespace Items
{
    class Note12 : ItemBase
    {
        public Note12() : base(
            "Note #12",
            "'How were we supposed to know? They told us he " +
            "was evil and we trusted them. It was them all " +
            "along.\nThere's no way out for us...'",
            ItemType.Misc
        )
        {
            Parser.Parser.RegisterNoun("Note #12");
        }
    }
}