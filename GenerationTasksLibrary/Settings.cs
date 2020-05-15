using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerationTasksLibrary
{
    public class Settings
    {

        internal Settings(){}

        public Settings(int countRoots, int maxRootValue, int maxPowerPolynomial, bool oneFraction, bool log, bool radical, bool rootsOfMultiplicityTwo, bool powerFunc, bool showAnsers)
        {
            if (countRoots < 1 || countRoots > 9)
            {
                throw new ArgumentException("Количество корней должно быть положительным числом не привышающим 9", "countRoots");
            }
            CountRoots = countRoots;
            if (maxRootValue < 1 || maxRootValue > 20)
            {
                throw new ArgumentException("Максимальное по модулю значение корня должно быть положительным числом не привышающим 20", "maxRootValue");
            }
            MaxRootValue = maxRootValue;
            if (maxPowerPolynomial < 1 || maxPowerPolynomial > 5)
            {
                throw new ArgumentException("Максимальная степень многочлена должна быть положительным числом не больше 5", "maxPowerPolynomial");
            }
            MaxPowerPolynomial = maxPowerPolynomial;

            OneFraction = oneFraction;
            Log = log;
            Radical = radical;
            RootsOfMultiplicityTwo = rootsOfMultiplicityTwo;
            PowerFunc = powerFunc;
            ShowAnsers = showAnsers;
        }

        /// <summary>
        /// Максимальное количество различных корней
        /// </summary>
        internal int CountRoots { get; set; }

        /// <summary>
        /// Максимальное значение корня
        /// </summary>
        internal int MaxRootValue { get; set; }

        /// <summary>
        /// Максимальная степень многочлена в задании
        /// </summary>
        internal int MaxPowerPolynomial { get; set; }

        /// <summary>
        /// True - задание вида:
        /// {frac} < / > / <= / >= 0
        /// 
        /// False - задание любого другого вида
        /// </summary>
        internal bool OneFraction { get; set; }

        /// <summary>
        /// Наличие логарифмов в задаче
        /// 
        /// True - логарифмы могут быть;
        /// False - логарифмов нет;
        /// </summary>
        internal bool Log { get; set; }

        /// <summary>
        /// Наличие корней в задаче:
        /// 
        /// True - корень может быть в уравнении;
        /// False - корней не может быть в уравнении;
        /// </summary>
        internal bool Radical { get; set; }

        /// <summary>
        /// Наличие корней кратности 2 в задаче:
        /// 
        /// True - корни кратности 2 есть;
        /// False - корней кратности 2 нет;
        /// </summary>
        internal bool RootsOfMultiplicityTwo { get; set; }

        /// <summary>
        /// Наличие степенных функций в уравнении:
        /// 
        /// True - модули могут быть в уравнении;
        /// False - модулей не может быть в уравнении;
        /// </summary>
        internal bool PowerFunc { get; set; }

        /// <summary>
        /// Нужно ли показывать ответ
        /// </summary>
        internal bool ShowAnsers { get; set; }

    }
}
