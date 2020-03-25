using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TWPPract.DataStructures;

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
                
                var tableRow = new TableRow(rule.Key);
                var rules = singleRules.Where(x => x.Key == rule.Key).ToList();
                for (var i = 0; i < TwpDataProvider.ColumnCount; i++)
                {
                    var notEmptyValues = rules.Where(x => x.Symbol == (byte) i).ToList();

                    var links = new List<string>();
                    foreach (var item in notEmptyValues)
                    {
                        if (item.LastSymbol == "" || item.LastSymbol == "\0")
                        {
                            links.Add("Z");
                        }
                        else
                        {
                            links.Add(item.LastSymbol);
                        }
                    }
                    
                    tableRow.Cells[i] = new TableCell(links.ToArray());
                }
                
                table.Add(tableRow);
            }

            table.Add(new TableRow("Z"));

            return table;
        }

        public static Table CreateDeterTable(Table table)
        {
            var newTable = table;
            for (var r = 0; r < newTable.Count; r++)
            {                
                var item = newTable[r];
                var iter = 0;
                foreach (var tableCell in item.Cells)
                {
                    if (tableCell.Links.Length > 1)
                    {
                        var combinedKey = string.Join(".", tableCell.Links);
                        
                        // находим все строки где ключ один из текущих ссылок
                        var matchingRows = newTable.Where(x => tableCell.Links.Contains(x.Key)).ToArray();
                        
                        // инициализируем новые клетки для новой строки
                        var newCells =  new List<TableCell>();
                        for (var i = 0; i < TwpDataProvider.ColumnCount; i++)
                        {
                            var manySelected = matchingRows.SelectMany(s => s.Cells[i].Links).ToArray();
                            newCells.Add(new TableCell(manySelected));
                        }
                        newTable[r].Cells[iter] = new TableCell(new [] {combinedKey});

                        newTable.RemoveAll(x => matchingRows.Contains(x));
                        newTable.Insert(r + 1, new TableRow(combinedKey, newCells.ToArray()));
                    }

                    iter++;
                }
            }
            newTable.Add(new TableRow("Z"));

            return newTable;
        }

        /*public static Table MinimizeTable(Table table)
        {
            for (var i = 0; i < table.Count; i++)
            {
                var source = new TableRow("q" + i, table[i].Cells);
                var duplicatesIds = table.Where(x => x == source).Select(k => k.Key).ToArray();
                
                for (var j = 0; j < table.Count; j++)
                {
                    for (var l = 0; l < table[j].Cells.Length; l++)
                    {
                        if (duplicatesIds.Contains(table[j].Cells[l]))
                        {
                            table[j].Cells[l] = source.Key;
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
        }*/
    }
}