using System.Linq;
using System.Text;

namespace TWPPract
{
    public struct TableItem
    {
        public const int Spacing = 8;
        public const int LinksCount = 8;
        public static string[] EmptyLinks => Enumerable.Repeat("", LinksCount).ToArray();
        public readonly string Key;
        
        public readonly string[] Links;

        public TableItem(string key, string[] links = null)
        {
            Key = key;
            
            if (links == null)
                links = EmptyLinks;
            Links = links;
        }

        public static bool operator ==(TableItem a, TableItem b)
        {
            for (int i = 0; i < 8; i++)
            {
                if (a.Links[i] != b.Links[i])
                    return false;
            }

            return true;
        }

        public static bool operator !=(TableItem a, TableItem b)
        {
            return !(a == b);
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