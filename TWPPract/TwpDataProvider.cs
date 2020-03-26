using System.Collections.Generic;
using TWPPract.DataStructures;

namespace TWPPract
{
    public static class TwpDataProvider
    {
        public const byte ColumnCount = 8;
        
        public static readonly Dictionary<char, byte> Cipher = new Dictionary<char, byte>()
        {
            {'А', 1},
            {'Б', 5},
            {'В', 2},
            {'Г', 4},
            {'Д', 6},
            {'Е', 6},
            {'Ё', 6},
            {'Ж', 4},
            {'З', 3},
            {'И', 3},
            {'Й', 0},
            {'К', 7},
            {'Л', 0},
            {'М', 3},
            {'Н', 7},
            {'О', 4},
            {'П', 5},
            {'Р', 0},
            {'С', 4},
            {'Т', 5},
            {'У', 7},
            {'Ф', 2},
            {'Х', 5},
            {'Ц', 4},
            {'Ч', 2},
            {'Ш', 2},
            {'Щ', 0},
            {'Ъ', 6},
            {'Ь', 6},
            {'Ы', 1},
            {'Э', 1},
            {'Ю', 3},
            {'Я', 7},
            {' ', 5},
        };

        public static readonly Rule[] Rules = 
        {
            new Rule('S', new byte[] {1, 2, 3},  'A'),
            new Rule('S', new byte[] {1, 4, 5},  'B'),
            new Rule('S', new byte[] {6},  'C'),
            new Rule('S', new byte[] {7},  'F'),
            new Rule('A', new byte[] {8},  'D'),
            new Rule('A', new byte[] {9},  '\0'),
            new Rule('B', new byte[] {8},  'E'),
            new Rule('B', new byte[] {9},  '\0'),
            new Rule('C', new byte[] {8},  'E'),
            new Rule('C', new byte[] {9},  '\0'),
            new Rule('D', new byte[] {10},  'S'),
            new Rule('D', new byte[] {11},  '\0'),
            new Rule('E', new byte[] {10},  'S'),
            new Rule('E', new byte[] {11},  '\0'),
            new Rule('F', new byte[] {12, 13, 14, 15},  '\0'),
            new Rule('F', new byte[] {10, 13, 14, 16},  '\0'),
            new Rule('F', new byte[] {17, 18, 15},  '\0'),
        };

    }
}