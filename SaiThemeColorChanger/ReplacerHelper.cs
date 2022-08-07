namespace SaiDarkTheme
{
    public class ReplacerHelper
    {
        public ReplacerHelper(string search, string replace)
        {
            Search = search;
            Replace = replace;
        }

        public string Search { get; }
        public string Replace { get; }
    }
}
