using System.Linq;
using System.Text;

namespace TWPPract.DataStructures
{
    public struct TableRow
    {
        public const int Spacing = 8;
        public const int CellsCount = 8;
        public static TableCell[] EmptyCells => Enumerable.Repeat(TableCell.Empty, CellsCount).ToArray();
        public readonly string Key;
        
        public readonly TableCell[] Cells;

        public TableRow(string key, TableCell[] cells = null)
        {
            Key = key;
            
            if (cells == null)
                cells = EmptyCells;
            Cells = cells;
        }

        public static bool operator ==(TableRow a, TableRow b)
        {
            for (int i = 0; i < 8; i++)
            {
                if (a.Cells[i] != b.Cells[i])
                    return false;
            }

            return true;
        }

        public static bool operator !=(TableRow a, TableRow b)
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
            foreach (var item in Cells)
            {
                var spaces = Spacing - item.Links.Sum(x => x.Length);

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