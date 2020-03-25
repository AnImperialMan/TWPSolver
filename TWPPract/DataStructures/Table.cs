using System.Collections.Generic;
using System.Text;

namespace TWPPract.DataStructures
{
    public class Table: List<TableRow>
    {
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("      ");
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < TableRow.Spacing - 2; j++)
                {
                    sb.Append(" ");
                }
                sb.Append($"x{i}");
            }

            sb.AppendLine();

            foreach (var tItem in this)
            {
                sb.AppendLine(tItem.ToString());
            }

            return sb.ToString();
            
        }

    }
}