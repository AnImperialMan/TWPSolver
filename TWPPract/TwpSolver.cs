using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TWPPract
{
    public static class TwpSolver
    {
        public static byte[] EncryptName(string text)
        {
            var cipher = new byte[18];
            try
            {
                for (var i = 0; i < cipher.Length; i++)
                {
                    if (i < text.Length)
                        cipher[i] = TwpDataProvider.Cipher[char.ToUpper(text[i])];
                    else
                        cipher[i] = TwpDataProvider.Cipher[' '];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Неверные символы в имени и фамилии!");
                return null;
            }

            return cipher;
        }

        public static Rule[] CreateBasicRules(byte[] cipher)
        {
            var studentRules = TwpDataProvider.Rules;
            for (var i = 0; i < studentRules.Length; i++)
            {
                for (var j = 0; j < studentRules[i].Symbols.Length; j++)
                {
                    studentRules[i].Symbols[j] = cipher[studentRules[i].Symbols[j] - 1];
                }
            }

            return studentRules;
        }


        public static List<SingleRule> CreateSingleRules(IEnumerable<Rule> basicRules)
        {
            var newRules = new List<SingleRule>();
            var iters = new Dictionary<string, int>();
            foreach (var rule in basicRules)
            {
                var symbols = rule.Symbols.ToList();
                if (symbols.Count <= 1)
                {
                    newRules.Add(new SingleRule(rule.Key, rule.Symbols[0], rule.LastSymbol));
                    continue;
                }

                if (!iters.ContainsKey(rule.Key))
                    iters[rule.Key] = 0;

                var first = true;
                while (symbols.Count > 1)
                {
                    var newSymbol = rule.Key + (iters[rule.Key] + 1);
                    var newRule = new SingleRule(first ? rule.Key : rule.Key + iters[rule.Key] , symbols[0], newSymbol);
                    newRules.Add(newRule);
                    symbols.RemoveAt(0);
                    iters[rule.Key]++;
                    first = false;
                }
                var lastRule = new SingleRule(rule.Key + iters[rule.Key], symbols[0], rule.LastSymbol);
                newRules.Add(lastRule);
            }

            return newRules;
        }
        
        public static List<TableItem> CreateTable(List<SingleRule> singleRules)
        {
            var table = new List<TableItem>();
            var noRepeat = new List<string>();
            
            foreach (var rule in singleRules.Where(rule => !noRepeat.Contains(rule.Key)))
            {
                noRepeat.Add(rule.Key);
                var tableItem = new TableItem(rule.Key, new string[8]);
                var rules = singleRules.Where(x => x.Key == rule.Key).ToList();
                for (var i = 0; i < 8; i++)
                {
                    var currentValues = rules.Where(x => x.Symbol == (byte) i).ToList();
                    
                    var link = "";
                    foreach (var item in currentValues)
                    {
                        if (item.LastSymbol == "" || item.LastSymbol == "\0")
                            link += "Z";
                        else
                        {
                            link += item.LastSymbol;
                        }
                    }

                    tableItem.Links[i] = link;
                }
            }

            return table;
        }
    }
}