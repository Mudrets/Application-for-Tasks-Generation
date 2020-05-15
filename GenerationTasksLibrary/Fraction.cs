using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerationTasksLibrary
{
    public class Fraction : IEquatable<Fraction>, IComparable<Fraction>
    {
        readonly Settings settings;

        internal Fraction(GenerationKey generationKey)
        {
            Answer = new Intervals(generationKey);
            Sign = !Answer.StartFromFirstGap;
            settings = generationKey.Settings;

            Numenator = new List<Polynomial>();
            foreach (var point in Answer.IncludedPoints)
            {
                Numenator.Add(new Polynomial("x", point));
            }

            Denominator = new List<Polynomial>();
            foreach (var point in Answer.NotIncludedPoints)
            {
                Denominator.Add(new Polynomial("x", point));
            }

            //Создаем числитель
            //Numenator = CreateExpression(generationKey, Answer.IncludedPoints.GetRange(0, Answer.IncludedPoints.Count()));
            //Создаем занаменатель
            //Denominator = CreateExpression(generationKey, Answer.NotIncludedPoints.GetRange(0, Answer.NotIncludedPoints.Count()));

        }

        internal Fraction(List<Polynomial> numenator, List<Polynomial> denomenator)
        {
            Numenator = numenator;
            Denominator = denomenator;
        }

        internal Fraction(Fraction numenator, Polynomial denomenator)
        {
            Polynomial poly = new Polynomial("x", new List<Fraction> { numenator * numenator.IntDenominator });
            Numenator = new List<Polynomial> { poly };
            Denominator = new List<Polynomial> { denomenator * numenator.IntDenominator };
        }

        internal Fraction(int numenator, int denomenator)
        {
            int gcf = GCF(numenator, denomenator);
            IntNumenator = numenator / gcf;
            IntDenominator = denomenator / gcf;
            if (IntDenominator < 0)
            {
                IntDenominator *= -1;
                IntNumenator *= -1;
            }
        }


        /// <summary>
        /// Интервал являющийся ответом неравенства
        /// </summary>
        internal Intervals Answer { get; private set; }

        /// <summary>
        /// Числитель
        /// </summary>
        internal List<Polynomial> Numenator { get; private set; }

        /// <summary>
        /// Знаменатель
        /// </summary>
        internal List<Polynomial> Denominator { get; private set; }

        /// <summary>
        /// Целочисленный Числитель
        /// </summary>
        internal int IntNumenator { get; private set; }

        /// <summary>
        /// Целочисленный Знаменатель
        /// </summary>
        internal int IntDenominator { get; private set; }

        /// <summary>
        /// Знак неравенства
        /// </summary>
        internal bool Sign { get; private set; }

        /// <summary>
        /// Создает строковое представление неравенства
        /// </summary>
        /// <returns></returns>
        string CreateStringRepresentation()
        {
            string result = string.Empty;
            if (Numenator != null && Denominator != null)
            {
                if (Numenator.Count() > 0)
                {
                    foreach (Polynomial poly in Numenator)
                    {
                        result += poly;
                    }
                }
                else
                {
                    result += "1";
                }
                if (Denominator.Count() > 0)
                {
                    result += "/";
                    if (Denominator.Count() > 1)
                    {
                        result += "(";
                    }
                    foreach (Polynomial poly in Denominator)
                    {
                        result += poly;
                    }
                    if (Denominator.Count() > 1)
                    {
                        result += ")";
                    }

                }
            }
            else
            {
                result += $"{IntNumenator}{(IntDenominator != 1 ? $"/{IntDenominator}" : "")}";
            }
            return result;
        }

        static int SumOfPowersPoly(List<Polynomial> polynomials)
        {
            int sum = 0;
            foreach (var poly in polynomials)
            {
                sum += poly.Power;
            }
            return sum;
        }


        internal List<Fraction> DecomposeAmountOfFractions(GenerationKey generationKey)
        {
            List<Fraction> result = new List<Fraction> { this };
            Random rnd = new Random(generationKey.Seed);

            int sumNumenatorPowers = SumOfPowersPoly(Numenator);
            if (sumNumenatorPowers >= 1 && Denominator.Count == 1 && sumNumenatorPowers <= settings.MaxPowerPolynomial)
            {
                result = new List<Fraction>();

                List<Polynomial> numenator1 = new List<Polynomial>();
                List<Polynomial> numenator2 = new List<Polynomial>();
                List<Polynomial> denomenator1 = new List<Polynomial>();
                List<Polynomial> denomenator2 = new List<Polynomial>();

                Polynomial bigPoly = new Polynomial();
                foreach (Polynomial poly in Numenator)
                {
                    bigPoly *= poly;
                }
                if (bigPoly.CountNotZeroOdds() > 1)
                {
                    int coef = rnd.Next(-10, 10);
                    coef = coef >= 0 ? coef + 1 : coef;
                    Denominator[0] *= coef;
                    Numenator[0] *= coef;
                    int endOdd = rnd.Next(bigPoly.Power);
                    numenator2.Add(bigPoly.SeparatePart(endOdd));
                    Polynomial newPoly = bigPoly - numenator2[0];
                    numenator1.Add(newPoly);
                    denomenator1.Add(Denominator[0]);
                    denomenator2.Add(Denominator[0]);

                    (denomenator1[0], numenator1[0]) = denomenator1[0].Reduction(numenator1[0]);
                    (denomenator2[0], numenator2[0]) = denomenator2[0].Reduction(numenator2[0]);

                    Fraction newFraction1 = new Fraction(numenator1, denomenator1);
                    Fraction newFraction2 = new Fraction(numenator2, denomenator2);
                    result.Add(newFraction1);
                    result.Add(newFraction2); 
                }
                else
                {
                    return new List<Fraction> { this };
                }
            }
            else if (Denominator.Count == 1 && sumNumenatorPowers >= settings.MaxPowerPolynomial)
            {
                
            }
            else if (Denominator.Count > 1)
            {

                //Многочлены, на которые умножаются параметры 
                List<Polynomial> factorsСommonВenominator = new List<Polynomial>();
                //Знаменатели дробей полученные после разложения
                List<Polynomial> resultDenominators = new List<Polynomial>();
                int maxPower = 0;
                //Многочлен, который на который мы будем равнятся при поиске параметров
                Polynomial bigPoly = new Polynomial();
                List<Polynomial> outOfBrackets = new List<Polynomial>();
                try
                {
                    foreach (var poly in Denominator)
                    {
                        resultDenominators.Add(poly);
                        int power = 0;
                        Polynomial benominator = new Polynomial();
                        foreach (var poly1 in Denominator)
                        {
                            if (poly1 != poly)
                            {
                                benominator *= poly1;
                                power += poly1.Power;
                            }
                        }
                        factorsСommonВenominator.Add(benominator);

                        maxPower = power > maxPower ? power : maxPower;
                    }

                    foreach (Polynomial poly in Numenator)
                    {
                        if (bigPoly.Power < maxPower)
                        {
                            bigPoly *= poly;
                        }
                        else
                        {
                            outOfBrackets.Add(poly);
                        }
                    }
                }
                catch (OverflowException)
                {
                    return result;
                }

                //Составляем уравнения системы
                List<Dictionary<char, Fraction>> vcList = new List<Dictionary<char, Fraction>>();
                for (int i = 0; i <= maxPower; i++)
                {
                    vcList.Add(new Dictionary<char, Fraction>());
                }

                for (int i = 0; i < factorsСommonВenominator.Count; i++)
                {
                    Polynomial poly = factorsСommonВenominator[i];
                    for (int j = 0; j <= poly.Power; j++)
                    {
                        vcList[j].Add((char)('A' + i), poly.Odds[j]);
                    }
                }

                //Составляем систему и решаем ее
                List<Equation> equations = new List<Equation>();
                for (int i = 0; i < vcList.Count; i++)
                {
                    if (bigPoly.Power >= i)
                    {
                        equations.Add(new Equation(vcList[i], -bigPoly.Odds[i]));
                    }
                    else
                    {
                        equations.Add(new Equation(vcList[i], 0));
                    }
                }
                EquationSystem eqSys = new EquationSystem(equations);
                Dictionary<char, Fraction> systemAnswers;
                try
                {
                    systemAnswers = eqSys.Solve();
                }
                catch (OverflowException)
                {
                    return result;
                }

                if (systemAnswers.Count == 0)
                {
                    return result;
                }

                //Составляем начальные дроби
                result = new List<Fraction>();
                List<Polynomial> zeroFractionsDenoms = new List<Polynomial>();
                for (int i = 0; i < systemAnswers.Count; i++)
                {
                    if (systemAnswers[(char)('A' + i)] != (double)0)
                    {
                        result.Add(new Fraction(systemAnswers[(char)('A' + i)], resultDenominators[i]));
                    }
                    else
                    {
                        zeroFractionsDenoms.Add(resultDenominators[i]);
                    }
                }

                //Приводим все к красивому вижу и предотвращаем потерю корней
                for (int i = 0; i < result.Count; i++)
                {
                    for (int j = 0; j < zeroFractionsDenoms.Count; j++)
                    {
                        result[i].Denominator.Add(zeroFractionsDenoms[j]);
                        result[i].Numenator.Add(zeroFractionsDenoms[j]);
                    }
                }

                for (int i = 0; i < result.Count; i++)
                {
                    foreach (var outFraction in outOfBrackets)
                    {
                        result[i].Numenator.Add(outFraction);
                    }
                }
            }

            bool isOddsBig = false;
            foreach(Fraction frac in result)
            {
                foreach (Polynomial poly in frac.Numenator)
                {
                    foreach(Fraction odd in poly.Odds)
                    {
                        if (Math.Abs(odd.IntNumenator) >= 1000 || Math.Abs(odd.IntDenominator) >= 1000)
                        {
                            isOddsBig = true;
                        }
                    }
                }
            }

            if (isOddsBig)
            {
                return new List<Fraction> { this };
            }
            else
            {
                return result; 
            }
        }
        
        /// <summary>
        /// Рандомно перемножает множители числителя и знаменателя
        /// </summary>
        /// <param name="key">ключ генерации</param>
        internal void MultiplyPolynominal(int key, Settings settings)
        {
            Random rnd = new Random(key);
            List<Polynomial> newNumenator = new List<Polynomial>();
            List<Polynomial> newDenomenator = new List<Polynomial>();
            int polynominalPower = settings.MaxPowerPolynomial;

            while (Numenator.Count > 0)
            {
                /* Находим максимальное количество многочленов степени polynominalPower,
                 * которые возможно создать из оставшихся корней*/
                int maxCountOfPoly = Numenator.Count() / polynominalPower;
                //Определяемся с количеством создаваеммых многочленнов степени polynominalPower
                int countOfPoly = polynominalPower > 1 ? rnd.Next(Math.Max(0, maxCountOfPoly - 1), maxCountOfPoly + 1) : maxCountOfPoly;

                for (int i = 0; i < countOfPoly; i++)
                {
                    //Массив выбранных корней создаваемого многочлена
                    Polynomial newPoly = new Polynomial();
                    for (int j = 0; j < polynominalPower; j++)
                    {
                        int index = rnd.Next(Numenator.Count());
                        newPoly *= Numenator[index];
                        Numenator.RemoveAt(index);
                    }
                    //Создаем многочлен
                    newNumenator.Add(newPoly);
                }
                //Уменьшаем степень создаваемых многочленов
                polynominalPower--;
            }

            Numenator = newNumenator;

            polynominalPower = settings.MaxPowerPolynomial;

            while (Denominator.Count > 0)
            {
                /* Находим максимальное количество многочленов степени polynominalPower,
                 * которые возможно создать из оставшихся корней*/
                int maxCountOfPoly = Denominator.Count() / polynominalPower;
                //Определяемся с количеством создаваеммых многочленнов степени polynominalPower
                int countOfPoly = polynominalPower > 1 ? rnd.Next(Math.Max(0, maxCountOfPoly - 1), maxCountOfPoly + 1) : maxCountOfPoly;

                for (int i = 0; i < countOfPoly; i++)
                {
                    //Массив выбранных корней создаваемого многочлена
                    Polynomial newPoly = new Polynomial();
                    for (int j = 0; j < polynominalPower; j++)
                    {
                        int index = rnd.Next(Denominator.Count());
                        newPoly *= Denominator[index];
                        Denominator.RemoveAt(index);
                    }
                    //Создаем многочлен
                    newDenomenator.Add(newPoly);
                }
                //Уменьшаем степень создаваемых многочленов
                polynominalPower--;
            }

            Denominator = newDenomenator;
        }

        /// <summary>
        /// Работает только для числовых дробей
        /// </summary>
        /// <param name="fract1"></param>
        /// <param name="fract2"></param>
        /// <returns></returns>
        public static Fraction operator *(Fraction fract1, Fraction fract2)
        {
            int newIntNum = fract1.IntNumenator * fract2.IntNumenator;
            int newIntDenom = fract1.IntDenominator * fract2.IntDenominator;
            int gcf = GCF(newIntDenom, newIntNum);
            newIntNum /= gcf;
            newIntDenom /= gcf;
            return new Fraction(newIntNum, newIntDenom);
        }

        public static Fraction operator *(Fraction fract1, int num)
        {
            int newIntNum = fract1.IntNumenator * num;
            int newIntDenom = fract1.IntDenominator;
            int gcf = GCF(newIntDenom, newIntNum);
            newIntNum /= gcf;
            newIntDenom /= gcf;
            return new Fraction(newIntNum, newIntDenom);
        }

        public static Fraction operator /(Fraction fract1, Fraction fract2)
        {
            int newIntNum = fract1.IntNumenator * fract2.IntDenominator;
            int newIntDenom = fract1.IntDenominator * fract2.IntNumenator;
            int gcf = GCF(newIntDenom, newIntNum);
            newIntNum /= gcf;
            newIntDenom /= gcf;
            return new Fraction(newIntNum, newIntDenom);
        }

        public static Fraction operator +(Fraction fract1, Fraction fract2)
        {
            int newIntNum = fract1.IntNumenator * fract2.IntDenominator;
            newIntNum += fract2.IntNumenator * fract1.IntDenominator;
            int newIntDenom = fract1.IntDenominator * fract2.IntDenominator;
            int gcf = GCF(newIntDenom, newIntNum);
            newIntNum /= gcf;
            newIntDenom /= gcf;
            return new Fraction(newIntNum, newIntDenom);
        }
        public static Fraction operator -(Fraction fract1, Fraction fract2)
        {
            int newIntNum = fract1.IntNumenator * fract2.IntDenominator;
            newIntNum -= fract2.IntNumenator * fract1.IntDenominator;
            int newIntDenom = fract1.IntDenominator * fract2.IntDenominator;
            int gcf = GCF(newIntDenom, newIntNum);
            newIntNum /= gcf;
            newIntDenom /= gcf;
            return new Fraction(newIntNum, newIntDenom);
        }

        public static bool operator !=(Fraction frac, double num)
        {
            return frac.IntNumenator / (double)frac.IntDenominator != num;
        }

        public static bool operator ==(Fraction frac, double num)
        {
            return frac.IntNumenator / (double)frac.IntDenominator == num;
        }

        public static bool operator >=(Fraction frac, double num)
        {
            return frac.IntNumenator / (double)frac.IntDenominator >= num;
        }

        public static bool operator <=(Fraction frac, double num)
        {
            return frac.IntNumenator / (double)frac.IntDenominator <= num;
        }

        public static bool operator <(Fraction frac, double num)
        {
            return frac.IntNumenator / (double)frac.IntDenominator < num;
        }

        public static bool operator >(Fraction frac, double num)
        {
            return frac.IntNumenator / (double)frac.IntDenominator > num;
        }

        public static bool operator >=(Fraction frac1, Fraction frac2)
        {
            return frac1 - frac2 >= (double)0;
        }

        public static bool operator <=(Fraction frac1, Fraction frac2)
        {
            return frac1 - frac2 <= (double)0; ;
        }

        public static bool operator <(Fraction frac1, Fraction frac2)
        {
            return frac1 - frac2 < (double)0; ;
        }

        public static bool operator >(Fraction frac1, Fraction frac2)
        {
            return frac1 - frac2 > (double)0; ;
        }

        public static Fraction operator -(Fraction fract1)
        {
            if (fract1.Numenator == null)
            {
                int newIntNum = -fract1.IntNumenator;
                return new Fraction(newIntNum, fract1.IntDenominator); 
            }
            else
            {
                List<Polynomial> newNumenator = new List<Polynomial>();
                List<Polynomial> newDenominator = new List<Polynomial>();
                foreach (var poly in fract1.Numenator)
                {
                    newNumenator.Add(poly);
                }
                foreach (var poly in fract1.Denominator)
                {
                    newDenominator.Add(poly);
                }
                if (newNumenator.Count == 0)
                {
                    newNumenator.Add(new Polynomial());
                }
                newNumenator[0] *= -1;

                return new Fraction(newNumenator, newDenominator);
            }
        }

        public static implicit operator Fraction(int num) => new Fraction(num, 1);

        internal Fraction Abs()
        {
            return new Fraction(Math.Abs(IntNumenator), Math.Abs(IntDenominator));
        }

        static int GCF(int a, int b)
        {
            bool negA = a < 0;
            bool negB = b < 0;
            a = Math.Abs(a);
            b = Math.Abs(b);

            while (a != 0 && b != 0)
            {
                if (a > b)
                {
                    a = a % b;
                }
                else
                {
                    b = b % a;
                }
            }

            int gcf = (a + b) * (negA || negB ? -1 : 1);
            return gcf == 0 ? 1 : gcf;
        }


        public override string ToString()
        {
            return CreateStringRepresentation();
        }

        public bool Equals(Fraction other)
        {
            if (other == null)
            {
                return false;
            }

            return IntNumenator == other.IntNumenator && IntDenominator == other.IntDenominator;
        }

        internal static Fraction Max(Fraction fraction1, Fraction fraction2)
        {
            return fraction1 > fraction2 ? fraction1 : fraction2;
        }

        internal static Fraction Min(Fraction fraction1, Fraction fraction2)
        {
            return fraction1 < fraction2 ? fraction1 : fraction2;
        }

        /// <summary>
        /// Преобразует строковое представление дроби в эквивалетное ему
        /// дробное число со знаком. Возвращает значение, указывающее успешно
        /// ли выполнено преобразование
        /// </summary>
        /// <param name="str">Строка содержащая преобразуемое число</param>
        /// <param name="num">
        /// Переменная в которую будет записано число.
        /// В случае если преобразование невозможно, то
        /// в переменную записывается null
        /// </param>
        /// <returns></returns>
        internal static bool TryParse(string str, out Fraction num)
        {
            if (str.Contains('/'))
            {
                string numenator = str.Split('/')[0];
                string denomenator = str.Split('/')[1];
                int intNum;
                int intDenom;
                bool successNum = int.TryParse(numenator, out intNum);
                bool successDenom = int.TryParse(denomenator, out intDenom);

                if (successNum && successDenom)
                {
                    num = new Fraction(intNum, intDenom);
                }
                else
                {
                    num = null;
                }

                return successDenom && successNum;
            }
            else
            {
                int number;
                bool success = int.TryParse(str, out number);

                num = success ? (Fraction)number : null;

                return success;
            }
        }
        
        internal static Fraction Abs(Fraction frac)
        {
            return frac > (double)0 ? frac : frac * (-1);
        }


        internal static Fraction Pow(Fraction num, int pow)
        {
            Fraction result = 1;
            if (pow >= 0)
            {
                for (int i = 0; i < pow; i++)
                {
                    result *= num;
                } 
            }
            else
            {
                for (int i = 0; i < Math.Abs(pow); i++)
                {
                    result /= num;
                }
            }

            return result;
        }

        /// <summary>
        /// Заменяет переменную в дроби на переданную строку
        /// </summary>
        /// <param name="str"></param>
        internal void Subs(string str)
        {
            foreach (var poly in Numenator)
            {
                poly.Subs(str);
            }
            foreach (var poly in Denominator)
            {
                poly.Subs(str);
            }
        }

        /// <summary>
        /// Заменяет переменную в дроби на переданную функцию
        /// </summary>
        /// <param name="log"></param>
        internal void Subs(Function func)
        {
            foreach (var poly in Numenator)
            {
                poly.Subs(func.ToHTML());
            }
            foreach (var poly in Denominator)
            {
                poly.Subs(func.ToHTML());
            }
        }

        public int CompareTo(Fraction other)
        {
            if(this < other)
            {
                return -1;
            }
            else if (this > other)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        internal Fraction GetIntPart()
        {
            return IntNumenator / IntDenominator;
        }

        internal Fraction GetFloatPart()
        {
            return this - GetIntPart();
        }

        internal string ToHTML()
        {
            if (Numenator != null && Denominator != null)
            {
                string numenatorHTML = string.Empty;
                if (Numenator.Count() > 1)
                {
                    foreach (var poly in Numenator)
                    {
                        numenatorHTML += $"({poly.ToHTML()})";
                    }
                }
                else if(Numenator.Count() == 1)
                {
                    numenatorHTML += $"{Numenator[0].ToHTML()}";
                }
                else
                {
                    numenatorHTML += "1";
                }

                string denominatorHTML = string.Empty;
                if (Denominator.Count() > 1)
                {
                    foreach (var poly in Denominator)
                    {
                        denominatorHTML += $"({poly.ToHTML()})";
                    }
                }
                else if(Denominator.Count() == 1)
                {
                    denominatorHTML += $"{Denominator[0].ToHTML()}";
                }
                else
                {
                    denominatorHTML = "1";
                }
                if (denominatorHTML == "1")
                {
                    return $"<span class=\"func\">{numenatorHTML}</span>";
                }
                else
                {
                    return $"<span class=\"fraction\"><span class=\"top\">{numenatorHTML}</span><span class=\"bottom\">{denominatorHTML}</span></span>";
                }
            }
            else
            {
                if (GetIntPart() > (double)0)
                {
                    if (IntNumenator % IntDenominator != 0)
                    {
                        return $"<span class=\"answerNumber\">{GetIntPart()}</span><span class=\"fraction answerFraction\"><span class=\"top\">{IntNumenator % IntDenominator}</span>{(IntDenominator != 1 ? $"<span class=\"bottom\">{IntDenominator}</span>" : "")}</span>"; 
                    }
                    else
                    {
                        return $"{GetIntPart()}";
                    }
                }
                else if(GetIntPart() < (double)0)
                {
                    if (IntNumenator % IntDenominator != 0)
                    {
                        return $"<span class=\"answerNumber\">{GetIntPart()}</span><span class=\"fraction answerFraction\"><span class=\"top\">{-IntNumenator % IntDenominator}</span>{(IntDenominator != 1 ? $"<span class=\"bottom\">{IntDenominator}</span>" : "")}</span>";
                    }
                    else
                    {
                        return $"{GetIntPart()}";
                    }
                }
                else
                {
                    if (IntNumenator == 0)
                    {
                        return "0";
                    }
                    else
                    {
                        return $"<span class=\"fraction answerFraction\" ><span class=\"top\">{IntNumenator}</span>{(IntDenominator != 1 ? $"<span class=\"bottom\">{IntDenominator}</span>" : "")}</span>";
                    }
                }
            }
        }
    }
}
