using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TWPPract.DataStructures;
using Rule = TWPPract.DataStructures.Rule;

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

                        newTable.RemoveAll(x => matchingRows.Contains(x) && x.Key != "Z");
                        newTable.Insert(r + 1, new TableRow(combinedKey, newCells.ToArray()));
                    }

                    iter++;
                }
            }

            return newTable;
        }

        public static List<Group> CalculateGroups(Table table)
        {
            var newTable = table;
            var groups = new List<Group>();

            var findGroup = true;
            while (findGroup)
            {
                findGroup = false;
                for (var i = 0; i < newTable.Count; i++)
                {
                    for (var j = 0; j < newTable.Count; j++)
                    {
                        // нашли группу
                        if (i != j && newTable[j] == newTable[i])
                        {
                            var keyA = newTable[i].Key;
                            var keyB = newTable[j].Key;
                            var groupA = groups.FirstOrDefault(x => x.Contains(keyA)); // references
                            var groupB = groups.FirstOrDefault(x => x.Contains(keyB));
                            
                            if (groupA != null || groupB != null)
                            {
                                if (groupA != null && groupB != null)
                                {
                                    if (groupA == groupB)
                                        continue;
                                    
                                    for (var k = 0; k < newTable.Count; k++)
                                    {
                                        newTable[k].Cells.Where(x => groupB.Contains(x.Links[0])).ToList().ForEach(x => x.Links[0] = groupA.Min());
                                    }
                                    
                                    groupA.AddRange(groupB);
                                    groups.Remove(groupB);
                                    findGroup = true;
                                }
                                

                                if (groupA == null)
                                {
                                    for (var k = 0; k < newTable.Count; k++)
                                    {
                                        newTable[k].Cells.Where(x => x.Links.Length > 0 && x.Links[0] == keyA).ToList().ForEach(x => x.Links[0] = groupB.Min());
                                    }
                                    groupB.Add(keyA);
                                    findGroup = true;
                                }
                                
                                if (groupB == null)
                                {
                                    for (var k = 0; k < newTable.Count; k++)
                                    {
                                        newTable[k].Cells.Where(x => x.Links.Length > 0 && x.Links[0] == keyB).ToList().ForEach(x => x.Links[0] = groupA.Min());
                                    }
                                    groupA.Add(keyB);
                                    findGroup = true;
                                }
                            }
                            else
                            {
                                groups.Add(new Group { keyA, keyB });
                                for (var k = 0; k < newTable.Count; k++)
                                {
                                    newTable[k].Cells.Where(x => x.Links.Length > 0 && x.Links[0] == keyB).ToList().ForEach(x => x.Links[0] = keyA);
                                }
                                findGroup = true;
                            }

                        }
                    }
                }
            }

            groups.ForEach(g => g.Sort());

            var totalGroups = new List<Group>();
            foreach (var row in table)
            {
                if (totalGroups.Any(g => g.Contains(row.Key)))
                    continue;
                
                var group = groups.FirstOrDefault(g => g.Contains(row.Key));
                if (group != null)
                {
                    totalGroups.Add(group);
                    groups.Remove(group);
                }
                else
                {
                    totalGroups.Add(new Group {row.Key});
                }
            }

            return totalGroups;
        }

        public static Table CreateMinimizedTable(Table deterTable, List<Group> groups, bool useQNamings)
        {
            var newTable = new Table();
            for (int i = 0; i < groups.Count; i++)
            {
                var group = groups[i];
                var name = useQNamings ? $"q{i}" : string.Join(",", group);
                for (var c = 0; c < TwpDataProvider.ColumnCount; c++)
                {
                    for (var j = 0; j < deterTable.Count; j++)
                    {
                        if (deterTable[j].Cells[c].Links.Length <= 0) 
                            continue;
                        
                        var item = deterTable[j].Cells[c].Links[0];
                        if (group.Contains(item))
                        {
                            deterTable[j].Cells[c].Links[0] = name;
                        }
                    }
                }

                var matchRow = deterTable.FirstOrDefault(x => group.Contains(x.Key));
                var newRow = new TableRow(name, matchRow.Cells);
                newTable.Add(newRow);
            }
            
            return newTable;
        }
        
    }
}