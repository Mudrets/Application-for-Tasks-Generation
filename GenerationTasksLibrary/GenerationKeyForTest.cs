using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerationTasksLibrary
{
    public class GenerationKeyForTest
    {
        public GenerationKeyForTest(string key)
        {
            StringRepresentation = key;
            KeysStringRepresentations = new List<string>() { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
            if (!IsKeyCorrect())
            {
                throw new ArgumentException("переданный ключ некорректен", "key");
            }
            GenerationKeys = GetGenerationKeys();
        }

        public GenerationKeyForTest(List<GenerationKey> generationKeys, int countOfTasks)
        {
            GenerationKeys = generationKeys;
            CountOfTasks = countOfTasks;
            StringRepresentation = GetStringRepresentation(countOfTasks);
            Seed = generationKeys[0].Seed;
        }

        public int Seed { get; set; }
        
        public int CountOfTasks { get; private set; }

        public string StringRepresentation { get; private set; }

        public List<GenerationKey> GenerationKeys { get; private set; }

        public List<string> KeysStringRepresentations { get; protected set; }

        string GetStringRepresentation(int countOfTasks)
        {
            string result = string.Empty;
            List<int> values = new List<int>();
            for (int i = 0; i < 12; i++)
            {
                if (GenerationKeys.ElementAt(0).StringRepresentation[i] == '.')
                {
                    continue;
                }
                int number = 0;
                int j = 4;
                foreach (GenerationKey key in GenerationKeys)
                {
                    int digit = int.Parse(key.StringRepresentation[i].ToString());
                    number += IntPow(10, j) * digit;
                    j--;
                }
                if (i == 2 || i == 7)
                {
                    if (i == 2)
                    {
                        int newNum = 0;
                        int k = 0;
                        while (number > 0)
                        {
                            newNum += number % 10 * IntPow(3, k);
                            number /= 10;
                            k++;
                        }
                        number = newNum;
                    }

                    if (i == 7)
                    {
                        int newNum = 0;
                        int k = 0;
                        while (number > 0)
                        {
                            newNum += number % 10 * IntPow(4, k);
                            number /= 10;
                            k++;
                        }
                        number = newNum;
                    }

                    result += $"{ConvertNumberToWord(number, false, true, i == 2 ? 2 : 3)}.";
                }
                else
                {
                    result += $"{ConvertNumberToWord(number)}.";
                }
            }

            result += $"{countOfTasks.ToString().PadLeft(4, '0')}.{GenerationKeys.ElementAt(0).Seed.ToString().PadLeft(6, '0')}.{(GenerationKeys.ElementAt(0).Settings.ShowAnsers ? '1' : '0')}";
            return result;
        }

        string ConvertNumberToWord(int num, bool smallLetter = true, bool bigLetter = true, int countLetters = 2)
        {
            string strNum = num.ToString().PadLeft(5, '0');
            string result = string.Empty;
            if (smallLetter && bigLetter)
            {
                while (num > 0)
                {
                    int remainder = num % 52;
                    bool upperLetter = remainder > 25;
                    result = (char)(upperLetter ? ('A' + remainder - 26) : 'a' + remainder) + result;
                    num /= 52;
                }
                result = result.PadLeft(3, 'a');
            }
            else if (bigLetter)
            {
                while (num > 0)
                {
                    int remainder = num % 26;
                    result = (char)('A' + remainder) + result;
                    num /= 26;
                }
                result = result.PadLeft(countLetters, 'A');
            }

            return result;
        }

        bool IsKeyCorrect()
        {
            return IsKeyLengthCorrect() && IsKeyStructureCorrect();
        }

        bool IsKeyLengthCorrect() => StringRepresentation.Length == 44;

        bool IsKeyStructureCorrect()
        {
            List<string> values = StringRepresentation.Split('.').ToList();
            if (values.Count != 11)
            {
                return false;
            }

            foreach (string value in values)
            {
                foreach (char ch in value)
                {
                    if (ch < 'A' && ch > '9' || ch > 'Z' && ch < 'a' || ch > 'z' || ch < '0')
                    {
                        return false;
                    }
                }
            }

            bool correctSize = values[0].Length == 3 && values[1].Length == 2 && values[2].Length == 3 &&
                               values[3].Length == 3 && values[4].Length == 3 && values[5].Length == 3 &&
                               values[6].Length == 3 && values[7].Length == 3 && values[8].Length == 4 &&
                               values[9].Length == 6 && values[10].Length == 1;
            return correctSize;
        }

        List<GenerationKey> GetGenerationKeys()
        {
            List<GenerationKey> result = new List<GenerationKey>();
            GetCountOfRoots();
            GetMaxRootValue();
            GetMaxPolyPower();
            GetBoolSettings();
            GetCountOfInequality();
            string strSeed = StringRepresentation.Split('.')[9];
            string answerFlag = StringRepresentation.Split('.')[10];

            try
            {
                Seed = int.Parse(strSeed);
                CountOfTasks = int.Parse(StringRepresentation.Split('.')[8]);
            }
            catch (FormatException)
            {
                throw new ArgumentException("переданный ключ некорректен", "key");
            }

            KeysStringRepresentations[0] += $"{answerFlag}.{strSeed}";
            result.Add(new GenerationKey(KeysStringRepresentations[0]));

            for (int i = 1; i < 5; i++)
            {
                int newSeed = (result[i - 1].Seed + 1) % 1000000;
                KeysStringRepresentations[i] += $"{answerFlag}.{newSeed.ToString().PadLeft(6, '0')}";
                result.Add(new GenerationKey(KeysStringRepresentations[i]));
            }

            return result;

        }

        int IntPow(int @base, int pow)
        {
            int result = 1;
            for (int i = 0; i < pow; i++)
            {
                result *= @base;
            }
            return result;
        }

        int ParseFromWordsToNum(string str, bool containsDigits, bool containsUpperLetter, bool containsLowerLetter)
        {
            int result = 0;
            if (containsDigits && containsUpperLetter && containsLowerLetter)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (char.IsDigit(str[i]))
                    {
                        result += (str[i] - '0' + 52) * IntPow(62, str.Length - i - 1);
                    }
                    else if (char.IsUpper(str[i]))
                    {
                        result += (str[i] - 'A' + 26) * IntPow(62, str.Length - i - 1);
                    }
                    else if (char.IsLower(str[i]))
                    {
                        result += (str[i] - 'a') * IntPow(62, str.Length - i - 1);
                    }
                }
            }
            else if (containsUpperLetter && containsLowerLetter)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (char.IsUpper(str[i]))
                    {
                        result += (str[i] - 'A' + 26) * IntPow(52, str.Length - i - 1);
                    }
                    else if (char.IsLower(str[i]))
                    {
                        result += (str[i] - 'a') * IntPow(52, str.Length - i - 1);
                    }
                }
            }
            else if (containsDigits && containsUpperLetter)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (char.IsDigit(str[i]))
                    {
                        result += (str[i] - '0') * IntPow(32, str.Length - i - 1);
                    }
                    else if (char.IsUpper(str[i]))
                    {
                        result += (str[i] - 'A') * IntPow(32, str.Length - i - 1);
                    }
                }
            }
            else if (containsUpperLetter)
            {
                for (int i = 0; i < str.Length; i++)
                {   
                    if (char.IsUpper(str[i]))
                    {
                        result += (str[i] - 'A') * IntPow(26, str.Length - i - 1);
                    }
                }
            }

            return result;
        }

        void GetCountOfRoots()
        {
            string countsOfRoots = StringRepresentation.Split('.')[0];
            bool isLetter;
            foreach (char ch in countsOfRoots)
            {
                if (!char.IsLetter(ch))
                {
                    throw new ArgumentException("Некорректный ключ", "key");
                }
            }
            int result = 0;
            if (countsOfRoots.Length == 3)
            {
                result = ParseFromWordsToNum(countsOfRoots, false, true, true);
            }
            else
            {
                throw new ArgumentException("Некорректный ключ", "key");
            }

            string strResult = result.ToString();
            if (strResult.Length != 5)
            {
                throw new ArgumentException("Некорректный ключ", "key");
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    KeysStringRepresentations[i] += $"{strResult[i]}.";
                }
            }
        }

        void GetMaxRootValue()
        {
            string maxRoots1 = StringRepresentation.Split('.')[1];
            foreach (char ch in maxRoots1)
            {
                if (!char.IsLetter(ch))
                {
                    throw new ArgumentException("Некорректный ключ", "key");
                }
            }
            int result = 0;
            if (maxRoots1.Length == 2)
            {
                result = ParseFromWordsToNum(maxRoots1, false, true, false);
                string newRes = string.Empty;
                while (result > 0)
                {
                    newRes = $"{result % 3}{newRes}";
                    result /= 3;
                }
                result = int.Parse(newRes);
            }
            else
            {
                throw new ArgumentException("Некорректный ключ", "key");
            }

            string strResult = result.ToString().PadLeft(5, '0');
            for (int i = 0; i < 5; i++)
            {
                KeysStringRepresentations[i] += $"{strResult[i]}";
            }

            string maxRoots2 = StringRepresentation.Split('.')[2];
            foreach (char ch in maxRoots2)
            {
                if (!char.IsLetter(ch))
                {
                    throw new ArgumentException("Некорректный ключ", "key");
                }
            }
            result = 0;
            if (maxRoots2.Length == 3)
            {
                result = ParseFromWordsToNum(maxRoots2, false, true, true);
            }
            else
            {
                throw new ArgumentException("Некорректный ключ", "key");
            }

            strResult = result.ToString().PadLeft(5, '0');
            for (int i = 0; i < 5; i++)
            {
                KeysStringRepresentations[i] += $"{strResult[i]}.";
            }
        }

        void GetMaxPolyPower()
        {
            string maxPolyPower = StringRepresentation.Split('.')[3];
            foreach (char ch in maxPolyPower)
            {
                if (!char.IsLetter(ch))
                {
                    throw new ArgumentException("Некорректный ключ", "key");
                }
            }
            int result = 0;
            if (maxPolyPower.Length == 3)
            {
                result = ParseFromWordsToNum(maxPolyPower, false, true, true);
            }
            else
            {
                throw new ArgumentException("Некорректный ключ", "key");
            }

            string strResult = result.ToString().PadLeft(5, '0');
            for (int i = 0; i < 5; i++)
            {
                KeysStringRepresentations[i] += $"{strResult[i]}.";
            }
        }

        void GetBoolSettings()
        {
            string maxRoots1 = StringRepresentation.Split('.')[4];
            foreach (char ch in maxRoots1)
            {
                if (!char.IsLetter(ch))
                {
                    throw new ArgumentException("Некорректный ключ", "key");
                }
            }
            int result = 0;
            if (maxRoots1.Length == 3)
            {
                result = ParseFromWordsToNum(maxRoots1, false, true, false);
                string newRes = string.Empty;
                while (result > 0)
                {
                    newRes = $"{result % 4}{newRes}";
                    result /= 4;
                }
                result = int.Parse(newRes.PadLeft(1, '0'));
            }
            else
            {
                throw new ArgumentException("Некорректный ключ", "key");
            }

            string strResult = result.ToString().PadLeft(5, '0');
            for (int i = 0; i < 5; i++)
            {
                KeysStringRepresentations[i] += $"{strResult[i]}";
            }

            string maxRoots2 = StringRepresentation.Split('.')[5];
            foreach (char ch in maxRoots2)
            {
                if (!char.IsLetter(ch))
                {
                    throw new ArgumentException("Некорректный ключ", "key");
                }
            }
            result = 0;
            if (maxRoots2.Length == 3)
            {
                result = ParseFromWordsToNum(maxRoots2, false, true, true);
            }
            else
            {
                throw new ArgumentException("Некорректный ключ", "key");
            }

            strResult = result.ToString().PadLeft(5, '0');
            for (int i = 0; i < 5; i++)
            {
                KeysStringRepresentations[i] += $"{strResult[i]}.";
            }
        }

        void GetCountOfInequality()
        {
            string count1 = StringRepresentation.Split('.')[6];
            foreach (char ch in count1)
            {
                if (!char.IsLetter(ch))
                {
                    throw new ArgumentException("Некорректный ключ", "key");
                }
            }
            int result = 0;
            if (count1.Length == 3)
            {
                result = ParseFromWordsToNum(count1, false, true, true);
            }
            else
            {
                throw new ArgumentException("Некорректный ключ", "key");
            }

            string strResult = result.ToString().PadLeft(5, '0');
            for (int i = 0; i < 5; i++)
            {
                KeysStringRepresentations[i] += $"{strResult[i]}";
            }

            string countsOfRoots = StringRepresentation.Split('.')[7];
            foreach (char ch in countsOfRoots)
            {
                if (!char.IsLetter(ch))
                {
                    throw new ArgumentException("Некорректный ключ", "key");
                }
            }
            result = 0;
            if (countsOfRoots.Length == 3)
            {
                result = ParseFromWordsToNum(countsOfRoots, false, true, true);
            }
            else
            {
                throw new ArgumentException("Некорректный ключ", "key");
            }

            strResult = result.ToString().PadLeft(5, '0');
            for (int i = 0; i < 5; i++)
            {
                KeysStringRepresentations[i] += $"{strResult[i]}.";
            }
        }

        public override string ToString()
        {
            return StringRepresentation;
        }

        public static GenerationKeyForTest operator ++(GenerationKeyForTest key)
        {
            Random rnd = new Random(key.Seed);
            int newSeed = (key.Seed + rnd.Next(1, 48)) % 1000000;
            string strSeed = newSeed.ToString();
            strSeed = strSeed.PadLeft(6, '0');
            string[] allElem = key.ToString().Split('.');
            allElem[9] = strSeed;
            string newKey = string.Join(".", allElem);
            return new GenerationKeyForTest(newKey);
        }
    }
}
