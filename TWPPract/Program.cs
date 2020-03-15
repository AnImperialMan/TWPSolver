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
            Console.Write("      ");
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < TableItem.Spacing - 2; j++)
                {
                    Console.Write(" ");
                }
                Console.Write($"x{i}");
            }
            Console.WriteLine();

            var table = TwpSolver.CreateTable(singleRules);
            foreach (var item in table)
            {
                Console.WriteLine(item.ToString());
            }

            Console.WriteLine("Z       |");

            Console.WriteLine("\n\n\n5.Детерминированная таблица");
            Console.Write("      ");
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < TableItem.Spacing - 2; j++)
                {
                    Console.Write(" ");
                }
                Console.Write($"x{i}");
            }
            Console.WriteLine();
            var deterTable = TwpSolver.CreateDeterTable(table);
            foreach (var item in deterTable)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine("Z       |");

        }
       
    }
}
