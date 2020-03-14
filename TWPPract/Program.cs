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

            var cipherName = TwpSolver.EncryptName(name);
            if (cipherName == null)
                return;
            Console.WriteLine("\n\n1. Зашифрованное имя и фамилия: ");
            foreach (var item in cipherName)
                Console.Write(" x" + item);
            
            
            var studentRules = TwpSolver.CreateBasicRules(cipherName);
            Console.WriteLine("\n\n\n2. Правила: ");
            foreach (var rule in studentRules)
            {
                Console.WriteLine(rule.ToString());
            }


            var singleRules = TwpSolver.CreateSingleRules(studentRules);
            Console.WriteLine("\n\n\n3.Новые правила: ");
            foreach (var rule in singleRules)
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
            foreach (var rule in singleRules.Where(rule => !noRepeat.Contains(rule.Key)))
            {
                noRepeat.Add(rule.Key);
                Console.Write($"{rule.Key}");
                Console.Write(rule.Key.Length == 1 ? " |" : "|");
                
                var values = singleRules.Where(x => x.Key == rule.Key).ToList();
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
