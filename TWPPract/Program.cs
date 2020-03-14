using System;
using System.Collections.Generic;
using System.Linq;

namespace TWPPract
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите свою фамилию и имя 18 символов или более (остальное будет обрезано)");
            Console.WriteLine();
            var name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Строка пуста!");
                return;
            }

            var cipherName = new byte[18];
            try
            {
                for (var i = 0; i < cipherName.Length; i++)
                {
                    if (i < name.Length)
                        cipherName[i] = TwpDataProvider.Cipher[char.ToUpper(name[i])];
                    else
                        cipherName[i] = TwpDataProvider.Cipher[' '];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Неверные символы в имени и фамилии!");
                return;
            }
            
            Console.WriteLine("\n\n1. Зашифрованное имя и фамилия: ");
            foreach (var item in cipherName)
                Console.Write(" x" + item);

            var studentRules = TwpDataProvider.Rules;
            for (var i = 0; i < studentRules.Length; i++)
            {
                for (var j = 0; j < studentRules[i].Symbols.Length; j++)
                {
                    studentRules[i].Symbols[j] = cipherName[studentRules[i].Symbols[j] - 1];
                }
            }

            Console.WriteLine("\n\n\n2. Правила: ");
            foreach (var rule in studentRules)
            {
                Console.WriteLine(rule.ToString());
            }

            var newRules = new List<FixedRule>();
            var iters = new Dictionary<string, int>();
            foreach (var rule in studentRules)
            {
                var symbols = rule.Symbols.ToList();
                if (symbols.Count <= 1)
                {
                    newRules.Add(new FixedRule(rule.Key, rule.Symbols[0], rule.LastSymbol));
                    continue;
                }

                if (!iters.ContainsKey(rule.Key))
                    iters[rule.Key] = 0;

                var first = true;
                while (symbols.Count > 1)
                {
                    var newSymbol = rule.Key + (iters[rule.Key] + 1);
                    var newRule = new FixedRule(first ? rule.Key : rule.Key + iters[rule.Key] , symbols[0], newSymbol);
                    newRules.Add(newRule);
                    symbols.RemoveAt(0);
                    iters[rule.Key]++;
                    first = false;
                }
                var lastRule = new FixedRule(rule.Key + iters[rule.Key], symbols[0], rule.LastSymbol);
                newRules.Add(lastRule);
            }

            Console.WriteLine("\n\n\n3.Новые правила: ");
            foreach (var rule in newRules)
            {
                Console.WriteLine(rule.ToString());
            }
            
            Console.WriteLine("\n\n\n4.Таблица");
            const int spacesCount = 8;
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < spacesCount - 2; j++)
                {
                    Console.Write(" ");
                }
                Console.Write($"x{i}");
            }

            Console.WriteLine();
            var noRepeat = new List<string>();
            foreach (var rule in newRules.Where(rule => !noRepeat.Contains(rule.Key)))
            {
                noRepeat.Add(rule.Key);
                Console.Write($"{rule.Key}");
                Console.Write(rule.Key.Length == 1 ? " |" : "|");
                
                var values = newRules.Where(x => x.Key == rule.Key).ToList();
                for (var i = 0; i < 8; i++)
                {
                    var currentValues = values.Where(x => x.Symbol == (byte) i).ToList();
                    
                    var addStr = "";
                    foreach (var item in currentValues)
                    {
                        if (item.LastSymbol == "" || item.LastSymbol == "\0")
                            addStr += "Z";
                        else
                        {
                            addStr += item.LastSymbol;
                        }
                    }
                    var spaces = spacesCount - addStr.Length;

                    var first = spaces / 2;
                    var second = spaces - first;

                    for (var s = 0; s < first; s++)
                    {
                        Console.Write(" ");
                    }
                    
                    Console.Write(addStr);
                    
                    for (var s = 0; s < second ; s++)
                    {
                        Console.Write(" ");
                    }
                }
                
                Console.WriteLine();
            }
            Console.WriteLine("Z |");
        }
    }
}
