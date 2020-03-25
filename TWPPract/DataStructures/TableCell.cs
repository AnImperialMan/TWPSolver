using System.Linq;

namespace TWPPract.DataStructures
{
    public struct TableCell
    {
        public static TableCell Empty => new TableCell(new string[] {});

        public readonly string[] Links;
        
        public TableCell(string[] links)
        {
            Links = links;
        }
        
        public static bool operator ==(TableCell a, TableCell b)
        {
            if (a.Links.Length != b.Links.Length)
                return false;

            return !a.Links.Where((t, i) => t != b.Links[i]).Any();
        }

        public static bool operator !=(TableCell a, TableCell b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return string.Join("", Links);
        }
    }
}