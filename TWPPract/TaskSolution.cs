using System.Text;

namespace TWPPract
{
    public static class TaskSolution
    {
        private static readonly StringBuilder StringBuilder = new StringBuilder();

        public static string ReadAll => StringBuilder.ToString().Replace("\0", "");
        
        public static void Write(string text)
        {
            StringBuilder.Append(text);
        }
        
        public static void WriteLine(string text)
        {
            StringBuilder.AppendLine(text);
        }
        
        public static void WriteLine()
        {
            StringBuilder.AppendLine();
        }
        
    }
}