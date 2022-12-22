namespace CourseWork
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Program
    {
        public const string Space_Position = "99";

        public static void Main()
        {
            #region Configurations & Constants
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;
            const string alphabet = "АБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЬЮЯ_";

            char[,] alphabetAsMatrix = new char[,]
            {
                {'А', 'Б', 'В', 'Г', 'Д', 'Е'},
                {'Ж', 'З', 'И', 'Й', 'К', 'Л'},
                {'М', 'Н', 'О', 'П', 'Р', 'С'},
                {'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч'},
                {'Ш', 'Щ', 'Ъ', 'Ь', 'Ю', 'Я'},
                {'-','-','-','-','-','-' },
            };
            #endregion

            #region Encryption

            #region многоазбучно заместване
            //Console.Write("Въведете текст: ");
            var input = "КОМБИНИРАН_АЛГОРИТЪМ"; //Console.ReadLine().ToUpper();
            //Console.Write("Въведете ключ: ");
            var key1 = "ЗОРА"; //Console.ReadLine().ToUpper();
            var cipher = AlphabeticalSubstituionEncryption(alphabet, input, key1);
            Console.WriteLine($"След криптиране по метода на многоазбучно заместване: {cipher}");
            Console.WriteLine();
            #endregion

            #region Блокуване на текста
            Console.Write("Въведете вторият ключ: ");
            var key2 = "КОРАЛ"; //Console.ReadLine().ToUpper();
            cipher = BlockTextEncryption(alphabet, cipher, key2);
            Console.WriteLine($"След криптиране по метода на блокуване на текста: {cipher}");
            Console.WriteLine();
            #endregion

            #region Директно заместване чрез квадрат на Полибий
            cipher = PolybiousEncryption(alphabetAsMatrix, cipher);
            Console.WriteLine($"След криптиране по метода директно заместване чрез квадрат на Полибий: {cipher}");
            Console.WriteLine();
            #endregion

            #endregion

            #region Decryption
            #region Декриптиране на директно заместване чрез квадрат на Полибий
            var plainText = PolybiousDecryption(alphabetAsMatrix, cipher);
            Console.WriteLine($"Текст след декриптиране метода на директно заместване в чрез квадрат на Полибий: {plainText}");
            Console.WriteLine();
            #endregion

            #region Декриптиране метода на блокуване на тескта
            plainText = BlockTextDecryption(alphabet, plainText, key2);
            Console.WriteLine($"Текст след декриптиране метода на метода на блокуване на тескта: {plainText}");
            Console.WriteLine();
            #endregion

            #region Декриптиране метода на многоазбучно заместване
            plainText = AlphabeticalSubstitutionDecryption(alphabet, plainText, key1);
            Console.WriteLine($"Текст след декриптиране метода на многоазбучно заместване: {plainText}");
            Console.WriteLine();
            #endregion
            #endregion
        }

        #region многоазбучно заместване
        public static string AlphabeticalSubstituionEncryption(string alphabet, string plainText, string key)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < plainText.Length; i++)
            {
                // Ic = (Ip + Ik) mod n
                var indexP = alphabet.IndexOf(plainText[i]) + 1;
                var indexK = alphabet.IndexOf(key[i % key.Length]) + 1;
                var indexC = (indexP + indexK) % 31;
                sb.Append(alphabet[indexC - 1]);
            }
            return sb.ToString();
        }
        public static string AlphabeticalSubstitutionDecryption(string alphabet, string cipher, string key)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < cipher.Length; i++)
            {
                // Ip = (Ic-Ik) mod n
                var indexC = alphabet.IndexOf(cipher[i]);
                var indexK = alphabet.IndexOf(key[i % key.Length]);
                var temp = (indexC - indexK);
                if(temp < 0)
                {
                    temp += 31;
                }
                var indexP =  temp % alphabet.Length;
                if (indexP == 0)
                {
                    sb.Append(alphabet[alphabet.Length - 1]);
                }
                else
                {
                    sb.Append(alphabet[indexP - 1]);
                }
            }
            return sb.ToString();
        }
        #endregion

        #region Блокуване на текста
        public static string BlockTextEncryption(string alphabet, string plainText, string key)
        {
            StringBuilder sb = new StringBuilder();
            var blocks = string
                .Join(string.Empty,
                    plainText.Select((x, i) => i > 0 && i % key.Length == 0 ? $" {x}" : x.ToString()))
                .Split(' ')
                .ToList();

            var indexesOfKey = GetOrderOfKey(alphabet, key);

            for (int i = 0; i < blocks.Count; i++)
            {
                var block = blocks[i];
                for (int j = 0; j < indexesOfKey.Count; j++)
                {
                    var index = indexesOfKey.IndexOf(j);
                    sb.Append(block.ElementAt(index));
                }
            }

            return sb.ToString();
        }
        public static string BlockTextDecryption(string alphabet, string cipher, string key)
        {
            StringBuilder sb = new StringBuilder();

            var blocks = string
               .Join(string.Empty,
                   cipher.Select((x, i) => i > 0 && i % key.Length == 0 ? $" {x}" : x.ToString()))
               .Split(' ')
               .ToList();
            var indexesOfKey = GetOrderOfKey(alphabet, key);
            for (int i = 0; i < blocks.Count; i++)
            {
                var blockCipher = blocks[i];
                for (int j = 0; j < indexesOfKey.Count; j++)
                {
                    var element = blockCipher.ElementAt(indexesOfKey[j]);
                    sb.Append(element);
                }
            }

            return sb.ToString();
        }
        #endregion

        #region Директно заместване чрез квадрат на Полибий
        public static string PolybiousEncryption(char[,] alphabet, string text)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var character in text)
            {
                var characterPosition = FindCharacterEncryptedPosition(alphabet, character);
                sb.Append(characterPosition);
            }

            return sb.ToString();
        }
        public static string PolybiousDecryption(char[,] alphabet, string cipher)
        {
            var doubles = string
                .Join(string.Empty,
                      cipher.Select((x, i) => i > 0 && i % 2 == 0 ? $" {x}" : x.ToString()))
                .Split(' ')
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (string position in doubles)
            {
                var letter = DecryptLetter(alphabet, position);
                sb.Append(letter);
            }

            return sb.ToString();
        }
        #endregion


        #region Helpers
        private static string FindCharacterEncryptedPosition(char[,] alphabet, char character)
        {
            for (int i = 0; i < alphabet.GetLength(0); i++)
            {
                for (int j = 0; j < alphabet.GetLength((1)); j++)
                {
                    if (character == alphabet[i, j])
                    {
                        return $"{i + 1}{j + 1}";
                    }
                }
            }

            return Space_Position;
        }

        private static string DecryptLetter(char[,] alphabet, string position)
        {
            if (position == Space_Position)
            {
                return " ";
            }

            var indexes = position.ToCharArray();
            int row = (indexes[0] - '0') - 1;
            int column = (indexes[1] - '0') - 1;

            return alphabet[row, column].ToString();
        }

        private static List<int> GetOrderOfKey(string alphabet, string key)
        {
            var indexes = new List<int>();

            for (int i = 0; i < key.Length; i++)
            {
                var indexK = alphabet.IndexOf(key[i]);
                indexes.Add(indexK);
            }

            var tempList = indexes
                .OrderBy(x => x)
                .ToList();

            for (int i = 0; i < tempList.Count; i++)
            {
                var sortedIndex = tempList.IndexOf(indexes[i]);
                indexes[i] = sortedIndex;
            }

            return indexes;
        }
        #endregion
    }
}
