using System.Text;

namespace TWPPract.DataStructures
{
    public struct Rule
    {
        public readonly string Key;
        public readonly byte[] Symbols;
        public readonly string LastSymbol;

        public Rule(char key, byte[] symbols, char lastSymbol)
        {
            Key = key.ToString();
            Symbols = symbols;
            LastSymbol = lastSymbol.ToString();
        }
        

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(Key);
            sb.Append(" -> ");
            foreach (var symbol in Symbols)
            {
                sb.Append("x" + symbol + " ");
            }

            sb.Append(LastSymbol);
            return sb.ToString();
        }
    }

    public struct SingleRule
    {
        public readonly string Key;
        public readonly byte Symbol;
        public readonly string LastSymbol;
        
        public SingleRule(string key, byte symbol, string lastSymbol)
        {
            Key = key;
            Symbol = symbol;
            LastSymbol = lastSymbol;
        }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(Key);
            sb.Append(" -> ");
            sb.Append("x" + Symbol + " ");

            sb.Append(LastSymbol);
            return sb.ToString();
        }
    }
}