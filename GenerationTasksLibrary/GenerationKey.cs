using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerationTasksLibrary
{
    public class GenerationKey
    {
        /// <summary>
        /// Создает объект ключа генерации варианта
        /// </summary>
        /// <param name="key">ключ генерации варианта</param>
        public GenerationKey(string key)
        {
            Settings = new Settings();
            if (IsKeyCorrect(key))
            {
                StringRepresentation = key;
            }
            else
            {
                throw new ArgumentException("переданный ключ некорректен", "key");
            }
        }

        public GenerationKey(int countOfTasks, int seed, Settings settings)
        {
            CountOfTasks = countOfTasks;
            Seed = seed;
            Settings = settings;
            StringRepresentation = CreateStringRepresentation();
        }

        /// <summary>
        /// Количество заданий в варианте 
        /// </summary>
        public int CountOfTasks { get; private set; }

        /// <summary>
        /// Ключ для генерации варианта
        /// </summary>
        public int Seed { get; private set; }

        /// <summary>
        /// Строковое представление ключа
        /// </summary>
        public string StringRepresentation { get; private set; }

        /// <summary>
        /// Характеристики неравенства
        /// </summary>
        internal Settings Settings { get; private set;  }


        /// <summary>
        /// Проверяет корректность переданного ключа
        /// </summary>
        /// <param name="key">Ключ генерации варианта</param>
        /// <returns>
        /// True - если ключ корректный
        /// False - если ключ некорректный
        /// </returns>
        private bool IsKeyCorrect(string key)
        {
            return IsKeyLengthCorrect(key) 
                && IsKeyStructureCorrect(key);
        }

        /// <summary>
        /// Проверяет длинну ключа
        /// </summary>
        /// <param name="key">Ключ генерации варианта</param>
        /// <returns>
        /// True - длинная ключа корректна
        /// False - длинна ключа некорректна
        /// </returns>
        private static bool IsKeyLengthCorrect(string key) => key.Length == 21;

        /// <summary>
        /// Проверяет корректна ли структура ключа
        /// </summary>
        /// <param name="key">Ключ генерации</param>
        /// Если ключ корректный, то в этот параметр будет передан ключ генерации
        /// </param>
        /// <returns>
        /// True - структура ключа корректна
        /// False - структура ключа некорректна
        /// </returns>
        private bool IsKeyStructureCorrect(string key)
        {
            return IsCountOfRootsCorrect(key) && IsMaxRootValueCorrect(key) &&
                   IsMaxPolyPowerCorrect(key) && IsBoolSettingsCorrect(key) &&
                   IsCountOfTasksCorrect(key) && IsShowAnswersFlagCorrect(key) &&
                   IsSeedCorrect(key);
        }

        /// <summary>
        /// Проверяет корректность блока сложности
        /// </summary>
        /// <param name="key">Ключ генерации</param>
        /// <returns>
        /// True - блок сложности корректен
        /// False - блок сложности некорректен
        /// </returns>
        private bool IsCountOfRootsCorrect(string key)
        {
            string strCountOfRoots = key.Split('.')[0];
            int countOfRoots = -1;
            try
            {
                countOfRoots = int.Parse(strCountOfRoots);
            }
            catch (FormatException)
            {
                throw new ArgumentException("переданный ключ некорректен", "key");
            }

            Settings.CountRoots = countOfRoots;
            return countOfRoots > 0 && countOfRoots <= 9;
        }

        private bool IsMaxRootValueCorrect(string key)
        {
            string strMaxRootValue = key.Split('.')[1];
            int maxRootValue = -1;
            try
            {
                maxRootValue = int.Parse(strMaxRootValue);
            }
            catch (FormatException)
            {
                throw new ArgumentException("переданный ключ некорректен", "key");
            }

            Settings.MaxRootValue = maxRootValue;
            return maxRootValue > 0 && maxRootValue <= 20;
        }

        private bool IsMaxPolyPowerCorrect(string key)
        {
            string strMaxPolyPower = key.Split('.')[2];
            int maxPolyPower = -1;
            try
            {
                maxPolyPower = int.Parse(strMaxPolyPower);
            }
            catch (FormatException)
            {
                throw new ArgumentException("переданный ключ некорректен", "key");
            }

            Settings.MaxPowerPolynomial = maxPolyPower;
            return maxPolyPower > 0 && maxPolyPower <= 5;
        }

        private bool IsBoolSettingsCorrect(string key)
        {
            string strBoolSettings = key.Split('.')[3];
            int boolSettings = -1;
            try
            {
                boolSettings = int.Parse(strBoolSettings);
            }
            catch (FormatException)
            {
                throw new ArgumentException("переданный ключ некорректен", "key");
            }
            if (!(boolSettings >= 0 && boolSettings <= 31))
            {
                return false;
            }

            string settings = string.Empty;
            while (boolSettings > 0)
            {
                settings = (boolSettings % 2).ToString() + settings;
                boolSettings /= 2;
            }
            settings = settings.PadLeft(5, '0');
            Settings.Log = settings[0] == '1';
            Settings.Radical = settings[1] == '1';
            Settings.RootsOfMultiplicityTwo = settings[2] == '1';
            Settings.PowerFunc = settings[3] == '1';
            Settings.OneFraction = settings[4] == '1';
            return true;
        }

        /// <summary>
        /// Проверяет корректность блока количества заданий
        /// </summary>
        /// <param name="key">Ключ генерации</param>
        /// <returns>
        /// True - блок количества заданий корректен
        /// False - блок количества заданий некорректен
        /// </returns>
        private bool IsCountOfTasksCorrect(string key)
        {
            string stringCountOfTasks = key.Split('.')[4];
            int countOfTasks = -1;
            try
            {
                countOfTasks = int.Parse(stringCountOfTasks);
            }
            catch (FormatException)
            {
                throw new ArgumentException("переданный ключ некорректен", "key");
            }

            CountOfTasks = countOfTasks;
            return countOfTasks >= 0 && countOfTasks <= 99;
        }

        private bool IsShowAnswersFlagCorrect(string key)
        {
            string stringShowAnswersFlag = key.Split('.')[5];
            int showAnswersFlag = -1;
            try
            {
                showAnswersFlag = int.Parse(stringShowAnswersFlag);
            }
            catch (FormatException)
            {
                throw new ArgumentException("переданный ключ некорректен", "key");
            }

            Settings.ShowAnsers = showAnswersFlag == 1;
            return showAnswersFlag >= 0 && showAnswersFlag <= 1;
        }

        /// <summary>
        /// Проверяет корректность блока ключа генерации задания
        /// </summary>
        /// <param name="key">Ключ генерации</param>
        /// <returns>
        /// True - блок ключа генерации задания корректен
        /// False - блок ключа генерации задания некорректен
        /// </returns>
        private bool IsSeedCorrect(string key)
        {
            string stringSeed = key.Split('.')[6];
            int seed = -1;
            try
            {
                seed = int.Parse(stringSeed);
            }
            catch (FormatException)
            {
                throw new ArgumentException("переданный ключ некорректен", "key");
            }

            Seed = seed;
            return seed >= 0 && seed <= 999999;
        }

        public override string ToString()
        {
            return StringRepresentation;
        }

        string CreateStringRepresentation()
        {
            string result = string.Empty;
            result += $"{Settings.CountRoots.ToString()}.";
            result += $"{Settings.MaxRootValue.ToString().PadLeft(2, '0')}.";
            result += $"{Settings.MaxPowerPolynomial}.";

            int binSettings = 0;
            binSettings += Settings.Log ? 16 : 0;
            binSettings += Settings.Radical ? 8 : 0;
            binSettings += Settings.RootsOfMultiplicityTwo ? 4 : 0;
            binSettings += Settings.PowerFunc ? 2 : 0;
            binSettings += Settings.OneFraction ? 1 : 0;

            result += $"{binSettings.ToString().PadLeft(2, '0')}.";
            result += $"{CountOfTasks.ToString().PadLeft(2, '0')}.";
            result += $"{(Settings.ShowAnsers ? 1 : 0)}.";
            result += $"{Seed.ToString().PadLeft(6, '0')}";

            return result;
        }

        public static GenerationKey operator ++(GenerationKey key)
        {
            Random rnd = new Random(key.Seed);
            int newSeed = (key.Seed + rnd.Next(1, 50)) % 1000000;
            string strSeed = newSeed.ToString();
            strSeed = strSeed.PadLeft(6, '0');
            string[] allElem = key.ToString().Split('.');
            allElem[6] = strSeed;
            string newKey = string.Join(".", allElem);
            return new GenerationKey(newKey);
        }
    }
}
