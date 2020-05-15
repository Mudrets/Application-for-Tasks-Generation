using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerationTasksLibrary
{
    internal class Intervals
    {

        readonly Settings settings;

        internal Intervals(GenerationKey generationKey)
        {
            Random rnd = new Random(generationKey.Seed);
            int key = generationKey.Seed;
            int countOfTasks = generationKey.CountOfTasks;
            settings = generationKey.Settings;
            RootsOfMultiplicityTwo = new List<Fraction>();
            NotIncludedPoints = new List<Fraction>();
            IncludedPoints = new List<Fraction>();
            LeftPoints = new List<Fraction>();
            RightPoints = new List<Fraction>();
            StrictInequality = rnd.Next(2) == 0;
            IncludedPoints = new List<Fraction>();
            Points = GenerateIntervalBorders(key + countOfTasks);
            IntervalString = GenerateInterval(key + countOfTasks, Points);
            StartFromFirstGap = IntervalString[IntervalString.Length - 2] == 'f';
            NotIncludedPoints = GetNotIncludedPoints();
            IntervalsList = GetIntervalsList();
        }
        
        internal Intervals(List<Interval> intervals)
        {
            IntervalString = string.Empty;
            IncludedPoints = new List<Fraction>();
            NotIncludedPoints = new List<Fraction>();
            Points = new List<Fraction>();
            IntervalsList = intervals;

            foreach (var interval in intervals)
            {
                if (interval.IsInfinityInterval)
                {
                    if (interval.WichSide)
                    {
                        IntervalString += $"(-inf {interval.Border}{(interval.BorderIsIncluded ? "]" : ")")}U";
                    }
                    else
                    {
                        IntervalString += $"{(interval.BorderIsIncluded ? "[" : "(")}{interval.Border} +inf)";
                    }

                    if (interval.BorderIsIncluded)
                    {
                        IncludedPoints.Add(interval.Border);
                    }
                    else
                    {
                        NotIncludedPoints.Add(interval.Border);
                    }

                    Points.Add(interval.Border);
                }
                else
                {
                    if (!interval.LeftBorder.Equals(interval.RightBorder))
                    {
                        IntervalString += $"{(interval.LeftIsIncluded ? "[" : "(")}{interval.LeftBorder} {interval.RightBorder}{(interval.RightIsIncluded ? "]" : ")")}U"; 
                    }
                    else
                    {
                        IntervalString += $"{{{interval.LeftBorder}}}U";
                    }

                    if (interval.LeftIsIncluded)
                    {
                        IncludedPoints.Add(interval.LeftBorder);
                    }
                    else
                    {
                        NotIncludedPoints.Add(interval.LeftBorder);
                    }

                    if (interval.RightIsIncluded)
                    {
                        IncludedPoints.Add(interval.RightBorder);
                    }
                    else
                    {
                        NotIncludedPoints.Add(interval.RightBorder);
                    }

                    Points.Add(interval.LeftBorder);
                    Points.Add(interval.RightBorder);
                }
            }
            IntervalString = IntervalString.Trim('U');
        }

        /// <summary>
        /// Стороковое представление интервала
        /// </summary>
        internal string IntervalString { get; private set; }

        /// <summary>
        /// Лист с точками, в которых выражение принимает значение 0
        /// и точки в которых знаменатель выражения равен нулю
        /// </summary>
        internal List<Fraction> Points { get; private set; }

        /// <summary>
        /// Точки, включенные в интервал
        /// </summary>
        internal List<Fraction> IncludedPoints { get; private set; }

        /// <summary>
        /// Точки не включенные в интервал
        /// </summary>
        internal List<Fraction> NotIncludedPoints { get; private set; }

        /// <summary>
        /// Точки, которые находятся в левой части своих интервалов
        /// </summary>
        internal List<Fraction> LeftPoints { get; private set; }

        /// <summary>
        /// Точки, которые находяться в правой части своих интервалов
        /// </summary>
        internal List<Fraction> RightPoints { get; private set; }

        /// <summary>
        /// True если интервал начинается с ...+inf) и
        /// False если интервал начинается с чего-то другого
        /// </summary>
        internal bool StartFromFirstGap { get; private set; }

        /// <summary>
        /// Строгое ли неравенство
        /// </summary>
        internal bool StrictInequality { get; private set; }

        internal bool IsPowerBorders { get; private set; }

        internal int PowerBase { get; private set; }

        internal List<Fraction> RootsOfMultiplicityTwo { get; private set; }

        /// <summary>
        /// Хранит интервалы ответа
        /// </summary>
        internal List<Interval> IntervalsList { get; private set; } 

        public override string ToString()
        {
            return IntervalString;
        }

        /// <summary>
        /// Генерирует интервал по  
        /// переданным ключу генерации и границам интервала
        /// </summary>
        /// <param name="key">ключ генерации интервала</param>
        /// <param name="intervalBorders">границы интервалов</param>
        /// <returns>строковое представление интервала</returns>
        string GenerateInterval(int key, List<Fraction> intervalBorders)
        {
            /* Создаем переменную типа Random по ключу key
             * чтобы всегда получать один и тот же результат
             * при одних и тех же границах интервалов и ключе*/
            Random rnd = new Random(key);

            string result = CreateIntervalsWithoutInf(key, intervalBorders);
            result = AddInf(result);

            return result;

        }

        /// <summary>
        /// Случайным образом генерирует скобки разных видов
        /// </summary>
        /// <param name="key">Ключ генерации</param>
        /// <param name="side">
        /// True - правая скобка
        /// False - левая скобка
        /// </param>
        /// <returns>
        /// Круглая или квадртаная скобка
        /// </returns>
        static char GenerateBracket(int key, bool side)
        {
            Random rnd = new Random(key);
            if (side)
            {
                    return rnd.Next(2) == 0 ? ')' : ']'; 
            }
            else
            {
                    return rnd.Next(2) == 0 ? '(' : '['; 
            }
        }

        List<Interval> GetIntervalsList()
        {
            List<Interval> intervals = new List<Interval>();
            List<string> intervalsStr = new List<string>();

            intervalsStr = IntervalString.Split('U').ToList();
            foreach (var interStr in intervalsStr)
            {
                intervals.Add(Interval.Parse(interStr));
            }

            return intervals;
        }

        /// <summary>
        /// Добавляет (+inf... и ...-inf) если это нужно
        /// </summary>
        /// <param name="stringInterval"></param>
        /// <returns></returns>
        static string AddInf(string stringInterval)
        {
            stringInterval = stringInterval.Trim(' ');
            if (stringInterval.Last() != ')' && stringInterval.Last() != ']' && stringInterval.Last() != '}')
            { 
                stringInterval += " +inf)";
            }

            if (stringInterval.First() != '(' && stringInterval.First() != '[' && stringInterval.First() != '{')
            {
                stringInterval = "(-inf " + stringInterval;
            }
            return stringInterval;
        }

        //В интервал добавляет корень кратности 2
        string AddRootsOfMultiplicityTwo(Fraction point, int key, bool isMidInterval, bool isIncluded = false)
        {
            Random rnd = new Random(key);
            string result = string.Empty;

            if (isIncluded)
            {
                StrictInequality = false;
            }

            if (RootsOfMultiplicityTwo.Contains(point))
            {
                if (rnd.Next(2) == 1 || isIncluded)
                {
                    //Добавляем корень в числитель дважды
                    IncludedPoints.Add(point);
                    IncludedPoints.Add(point);
                    //Если точка расположена между интервалами, то ее надо вывести
                    if (!StrictInequality)
                    {
                        result += !isMidInterval ? $"{{{point}}}U" : ""; 
                    }
                }
                else
                {
                    //Добавляем корень в знаменатель и еще куда-то
                    NotIncludedPoints.Add(point);
                    if (rnd.Next(2) == 0)
                    {
                        IncludedPoints.Add(point);
                    }
                    else
                    {
                        NotIncludedPoints.Add(point);
                    }
                    //Если точка расположена внутри интервала, то она делит этот
                    //интервал на две части
                    result += isMidInterval ? $"{point})U({point} " : "";
                } 
            }

            return result;
        }

        /// <summary>
        /// Создает интервал без (+inf... и ...-inf)
        /// </summary>
        /// <param name="key">Ключ генерации интервала</param>
        /// <param name="borders">Границы интервалов</param>
        /// <returns>Интервал без (+inf... и ...-inf)</returns>
        string CreateIntervalsWithoutInf(int key, List<Fraction> borders)
        {
            Random rnd = new Random(key);
            string result = string.Empty;

            int rndValue = rnd.Next(2);
            int startIndex = 0;
            int countRootsMultTwoInEnd = 0;
            int k = borders.Count() - 1;
            while (k >= 1 && borders[k].Equals(borders[k - 1]))
            {
                countRootsMultTwoInEnd += 2;
                k -= 2;
            }
            int endIndex = borders.Count() - rndValue - countRootsMultTwoInEnd;
            endIndex = endIndex < 0 ? 0 : endIndex;

            if (RootsOfMultiplicityTwo.Count() * 2 != borders.Count())
            {
                //Выбирается способ записи точек в интервале
                //borders.count() % 2 == 0 и rndValue == 0: ()()()
                //borders.count() % 2 == 0 и rndValue == 1: )()(
                //borders.count() % 2 == 1 и rndValue == 0: )()()
                //borders.count() % 2 == 1 и rndValue == 1: ()()(
                if (borders.Count() % 2 == 1 ^ rndValue == 1)
                {
                    //Добавляем корень кратности 2 в промежуток
                    while (RootsOfMultiplicityTwo.Contains(borders[startIndex]))
                    {
                        result += AddRootsOfMultiplicityTwo(borders[startIndex], key, true);
                        startIndex += 2;
                    }

                    char bracket = GenerateBracket(key + rnd.Next(10), true);
                    if (bracket == ']')
                    {
                        RightPoints.Add(borders[startIndex]);
                        IncludedPoints.Add(borders[startIndex]);
                    }
                    else
                    {
                        RightPoints.Add(borders[startIndex]);
                        NotIncludedPoints.Add(borders[startIndex]);
                    }
                    result += $" {borders[startIndex++]}{bracket}U";
                }
                //Еще один корень кратности 2
                while (startIndex < borders.Count() && RootsOfMultiplicityTwo.Contains(borders[startIndex]))
                {
                    result += AddRootsOfMultiplicityTwo(borders[startIndex], key, false);
                    startIndex += RootsOfMultiplicityTwo.Contains(borders[startIndex]) ? 2 : 0;
                }

                for (int i = startIndex; i < endIndex; i += 2)
                {
                    //Проверяем содержиться ли корень в списке с
                    //корнями кратности 2
                    if (RootsOfMultiplicityTwo.Contains(borders[i]))
                    {
                        result += AddRootsOfMultiplicityTwo(borders[i], key, false);
                        continue;
                    }

                    char bracketRight = GenerateBracket(key + rnd.Next(10), true);
                    char bracketLeft = GenerateBracket(key + rnd.Next(10), false);
                    if (bracketLeft == '[')
                    {
                        IncludedPoints.Add(borders[i]);
                        LeftPoints.Add(borders[i]);
                    }

                    result += $"{bracketLeft}{borders[i]} ";

                    result += AddRootsOfMultiplicityTwo(borders[i], key, true);
                    i += RootsOfMultiplicityTwo.Contains(borders[i]) ? 2 : 0;

                    if (bracketRight == ']')
                    {
                        IncludedPoints.Add(borders[i + 1]);
                        RightPoints.Add(borders[i + 1]);
                    }
                    result += $"{borders[i + 1]}{bracketRight}U";
                }

                result = result.Trim('U');

                if (rndValue == 1 && endIndex >= startIndex)
                {
                    char bracket = GenerateBracket(key + rnd.Next(10), false);
                    if (bracket == '[')
                    {
                        IncludedPoints.Add(borders[endIndex]);
                        LeftPoints.Add(borders[endIndex]);
                    }
                    else
                    {
                        NotIncludedPoints.Add(borders[endIndex]);
                        LeftPoints.Add(borders[endIndex]);
                    }
                    result += $"U{bracket}{borders[endIndex++]}";
                    while (endIndex < borders.Count() && RootsOfMultiplicityTwo.Contains(borders[endIndex]))
                    {
                        result += AddRootsOfMultiplicityTwo(borders[endIndex], key, true);
                        endIndex += 2;
                    }
                } 
            }

            //Добавляем конечные корни кратности 2
            while (endIndex < borders.Count() && RootsOfMultiplicityTwo.Contains(borders[endIndex]))
            {
                //Если получилось так, что у ответа неравенства нет интервалов, полуинтервалов или промежутков,
                //то корни кратности 2 точно будут включенными, а не выколотыми
                result += AddRootsOfMultiplicityTwo(borders[endIndex], key, false, result == "");
                endIndex += 2;
            }

            if (StrictInequality)
            {
                result = result.Replace('[', '(');
                result = result.Replace(']', ')');
            }

            return result.Trim('U');
        }

        /// <summary>
        /// Генерирует лист точек интервала
        /// </summary>
        /// <param name="key">ключ генерации</param>
        /// <param name="complexity">сложность интервала</param>
        /// <returns></returns>
        List<Fraction> GenerateIntervalBorders(int key)
        {
            Random rnd = new Random(key);
            int count = settings.CountRoots;
            int maxValue = settings.MaxRootValue;
            int minValue = -settings.MaxRootValue;
            int valuePowerFunc = -1;
            int valueRadical = -1;
            int valueLog = -1;
            int countOfTypes = 0;
            bool powBorders = false;
            int powBase = 0;
            if (settings.PowerFunc)
            {
                valuePowerFunc = countOfTypes++;
            }
            if (settings.Radical)
            {
                valueRadical = countOfTypes++;
            }
            if (settings.Log)
            {
                valueLog = countOfTypes++;
            }
            if (countOfTypes != 0)
            {
                int rndValue = rnd.Next(countOfTypes);
                if (rndValue == valueLog)
                {
                    int logRndValue = rnd.Next(2);
                    if (settings.Log && logRndValue == 0)
                    {
                        maxValue = rnd.Next(-5, 10);
                        minValue = rnd.Next(-10, maxValue);
                    }
                    else if (settings.Log && logRndValue == 1)
                    {
                        maxValue = rnd.Next(-2, 5);
                        minValue = rnd.Next(-5, maxValue);
                    }
                }
                else if (rndValue == valueRadical)
                {
                    maxValue = rnd.Next(11, 20);
                    minValue = rnd.Next(0, maxValue);
                }
                else if (rndValue == valuePowerFunc)
                {
                    IsPowerBorders = true;
                    powBorders = true;
                    int powRndValue = rnd.Next(3);
                    if (powRndValue == 0)
                    {
                        powBase = 2;
                        PowerBase = 2;
                        maxValue = 10;
                        minValue = -10;
                    }
                    if (powRndValue == 1)
                    {
                        powBase = 3;
                        PowerBase = 3;
                        maxValue = 5;
                        minValue = -5;
                    }
                    if (powRndValue == 2)
                    {
                        powBase = 5;
                        PowerBase = 5;
                        maxValue = 4;
                        minValue = -4;
                    }
                }
            }
            List<Fraction> result = new List<Fraction>();
            for (int i = 0; i < count; i++)
            {
                Fraction num;
                if (powBorders)
                {
                    int pow = rnd.Next(minValue, maxValue + 1);
                    num = Fraction.Pow(powBase, pow);
                }
                else
                {
                    num = rnd.Next(minValue, maxValue + 1); 
                }
                /*Проверяем содержиться ли точка в списке уже созданных точек
                *И если сгенерированная точка уже существует, то проверяются
                *может ли наш интервал содержать корни кратности 2 и если да
                *то добавляет повторяющуюся точку, но только если количество
                *точек в промежутке не равно 1*/
                if (result.Contains(num) && !settings.RootsOfMultiplicityTwo)
                {
                    i--;
                }
                else
                {
                    if (result.Contains(num))
                    {
                        if (RootsOfMultiplicityTwo.Contains(num))
                        {
                            RootsOfMultiplicityTwo.Remove(num);
                            result.Remove(num);
                            i--;
                            continue;
                        }
                        else
                        {
                            RootsOfMultiplicityTwo.Add(num);
                        }
                    }
                    result.Add(num);
                }
            }
            result.Sort();

            return result;
        }

        /// <summary>
        /// Возвращает лист состоящий из точек не входящих в интервал
        /// </summary>
        /// <param name="includedPoints">Входящие в интервал точки</param>
        /// <param name="allPoints">Все точки</param>
        /// <returns>Не входящие в интервал точки</returns>
        List<Fraction> GetNotIncludedPoints()
        {
            foreach (var point in Points)
            {
                try
                {
                    if (!IncludedPoints.Contains(point) && !NotIncludedPoints.Contains(point))
                    {
                        NotIncludedPoints.Add(point);
                    }
                }
                catch (NullReferenceException)
                {
                    return Points;
                }
            }
            return NotIncludedPoints;
        }

        public static Intervals operator *(Intervals intervals1, Intervals intervals2)
        {
            List<Interval> intervalsList1 = new List<Interval>();
            List<Interval> intervalsList2 = new List<Interval>();

            List<string> intervalsStr1 = intervals1.ToString().Split('U').ToList();
            List<string> intervalsStr2 = intervals2.ToString().Split('U').ToList();

            foreach (string interStr in intervalsStr1)
            {
                intervalsList1.Add(Interval.Parse(interStr));
            }

            foreach (string interStr in intervalsStr2)
            {
                intervalsList2.Add(Interval.Parse(interStr));
            }

            List<Interval> newIntervalsList = new List<Interval>();

            foreach (var interval1 in intervalsList1)
            {
                foreach (var interval2 in intervalsList2)
                {
                    Interval newInterval = interval1 * interval2;

                    if (newInterval != null)
                    {
                        newIntervalsList.Add(newInterval);
                    }
                }
            }

            return new Intervals(newIntervalsList);
        }

        internal Intervals Subs(Function func)
        {
            List<string> intervalsStr;
            List<Interval> intervals = new List<Interval>();

            if (func.Argument.Power == 1)
            {

                intervalsStr = IntervalString.Split('U').ToList();

                foreach (var interStr in intervalsStr)
                {
                    intervals.Add(Interval.Parse(interStr));
                }

                foreach (var interval in intervals)
                {
                    if (!interval.IsInfinityInterval)
                    {
                        Fraction leftBorder = func.Eq(interval.LeftBorder);
                        Fraction rightBorder = func.Eq(interval.RightBorder);
                        interval.ChangeBorders(leftBorder, rightBorder); 
                    }
                    else
                    {
                        Fraction border = func.Eq(interval.Border);
                        interval.ChangeBorderForInfinity(border);
                    }
                }
            }

            Intervals newIntervals = new Intervals(intervals);
            Intervals endInterval;
            if (func.ValuesRange != null)
            {
                endInterval = newIntervals * func.ValuesRange; 
            }
            else
            {
                endInterval = newIntervals;
            }

            return endInterval;
        }

        internal Fraction MaxPoint()
        {
            Fraction max = -10000;
            foreach (var point in Points)
            {
                max = point > max ? point : max;
            }

            return max;
        }

        internal Fraction MinPoint()
        {
            Fraction min = 10000;
            foreach (var point in Points)
            {
                min = point < min ? point : min;
            }

            return min;
        }

        internal bool AreAllPointsInteger()
        {
            bool result = true;
            foreach (var point in Points)
            {
                if (point.IntDenominator > 1)
                {
                    result = false;
                }
            }

            return result;
        }

        internal string ToHTML()
        {
            List<string> intervalsHtml = new List<string>();
            foreach (Interval interval in IntervalsList)
            {
                intervalsHtml.Add(interval.ToHtml());
            }
            string result = String.Join("U", intervalsHtml);
            return $"<span class=\"answer\">{result}</span>";
        }

    }
}
