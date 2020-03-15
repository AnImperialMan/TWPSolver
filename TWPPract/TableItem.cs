using System.Text;

namespace TWPPract
{
    public struct TableItem
    {
        public const int Spacing = 8;
        
        public readonly string Key;
        
        public readonly string[] Links;

        public TableItem(string key, string[] links)
        {
            Key = key;
            Links = links;
        }
        
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append($"{Key}");
            var restSpaces = Spacing - Key.Length;
            for (int i = 0; i < restSpaces; i++)
            {
                sb.Append(" ");
            }

            sb.Append("|");
            foreach (var item in Links)
            {
                var spaces = Spacing - item.Length;

                var first = spaces / 2;
                var second = spaces - first;

                for (var s = 0; s < first; s++)
                {
                    sb.Append(" ");
                }
                    
                sb.Append(item);
                    
                for (var s = 0; s < second ; s++)
                {
                    sb.Append(" ");
                }
            }

            return sb.ToString();
        }
    }
}