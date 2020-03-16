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
        
        public static Table CreateTable(List<SingleRule> singleRules)
        {
            var table = new Table();
            var noRepeat = new List<string>();
            
            foreach (var rule in singleRules.Where(rule => !noRepeat.Contains(rule.Key)))
            {
                noRepeat.Add(rule.Key);
                var tableItem = new TableItem(rule.Key);
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
                table.Add(tableItem);
            }
            table.Add(new TableItem("Z"));

            return table;
        }

        public static Table CreateDeterTable(Table table)
        {
            var newTable = new Table();
            foreach (var item in table.Where(item => newTable.Count(x => x.Key.Contains(item.Key)) <= 0))
            {
                newTable.Add(item);
                int iter = 0;
                foreach (var link in item.Links)
                {
                    if (link.Length > 2)
                    {
                        var links = new List<string>();
                        for (int i = 0; i < link.Length / 2; i++)
                        {
                            links.Add(link.Substring(i * 2, 2));
                        }                       
                        var redirect = string.Join("_", links);


                        var tableItems = new Table();
                        foreach (var lnk in links)
                        {
                            tableItems.Add(table.FirstOrDefault(x => x.Key == lnk));
                        }

                        var newLinks = new[] { "", "", "", "", "", "", "", ""};
                        foreach (var tblItem in tableItems)
                        {
                            for (var i = 0; i < newLinks.Length; i++)
                            {
                                newLinks[i] += tblItem.Links[i];
                            }
                        }

                        newTable[^1].Links[iter] = redirect;
                        var newTableItem = new TableItem(redirect, newLinks);
                        newTable.Add(newTableItem);
                    }

                    iter++;
                }
            }

            return newTable;
        }

        public static Table MinimizeTable(Table table)
        {
            for (var i = 0; i < table.Count; i++)
            {
                var source = new TableItem("q" + i, table[i].Links);
                var duplicatesIds = table.Where(x => x == source).Select(k => k.Key).ToArray();
                
                for (var j = 0; j < table.Count; j++)
                {
                    for (var l = 0; l < table[j].Links.Length; l++)
                    {
                        if (duplicatesIds.Contains(table[j].Links[l]))
                        {
                            table[j].Links[l] = source.Key;
                        }
                    }
                }

                table[table.FindIndex(x => x.Key == table[i].Key)] = source;
                table.RemoveAll(x => duplicatesIds.Contains(x.Key) && x.Key != source.Key);

                if (duplicatesIds.Length > 1 || duplicatesIds.Length == 1 && duplicatesIds[0] != source.Key)
                {
                    TaskSolution.Write(source.Key + " = {");
                    var first = true;
                    foreach (var dup in duplicatesIds)
                    {
                        if (first)
                            first = false;
                        else
                            TaskSolution.Write(", ");
                        TaskSolution.Write(dup);
                    }
                    TaskSolution.Write("}");
                    TaskSolution.WriteLine();
                }

                if (duplicatesIds.Length > 1)
                {
                    i = 0;
                }
            }


            return table;
        }
    }
}