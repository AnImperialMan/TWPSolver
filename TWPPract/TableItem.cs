namespace TWPPract
{
    public struct TableItem
    {
        public readonly string Key;
        
        public readonly string[] Links;

        public TableItem(string key, string[] links)
        {
            Key = key;
            Links = links;
        }
    }
}