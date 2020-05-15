using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerationTasksLibrary
{
    internal class Polynomial
    {
        internal Polynomial()
        {
            VariableName = "x";
            Odds = new List<Fraction> { 1 };
            Power = 0;
        }

        internal Polynomial(string nameOfVariable, Fraction solution)
        {
            List<Fraction> solutions = new List<Fraction>();
            solutions.Add(solution);
            Solutions = solutions;
            List<Fraction> odds = new List<Fraction>();
            odds.Add(-solution);
            odds.Add(1);
            Odds = odds;
            Power = odds.Count() - 1;
            VariableName = nameOfVariable;
            Coefficient = Odds.Last();
        }

        internal Polynomial(string nameOfVariable, params Fraction[] arrSolutions)
        {
            List<Fraction> solutions = arrSolutions.ToList();
            Solutions = solutions;
            Polynomial result = new Polynomial();
            foreach (var solution in solutions)
            {
                result *= new Polynomial(nameOfVariable, solution);
            }
            Odds = result.Odds;
            Power = result.Power;
            VariableName = nameOfVariable;
            Coefficient = Odds.Last();
        }

        /// <summary>
        /// Создает многочлен по его коэффицентам
        /// </summary>
        /// <param name="nameOfVariable">название неизвестной</param>
        /// <param name="odds">коэффиценты</param>
        internal Polynomial(string nameOfVariable, List<Fraction> odds)
        {
            Odds = odds;
            Power = odds.Count() - 1;
            VariableName = nameOfVariable;
            Coefficient = Odds.Last();
        }


        /// <summary>
        /// Решения многочлена
        /// </summary>
        internal List<Fraction> Solutions { get; private set; }

        /// <summary>
        /// Степень многочлена
        /// </summary>
        internal int Power { get; private set; }

        /// <summary>
        /// Коэффиценты при x и свободный член
        /// при x^2 коэфф на позиции 2 в списке
        /// при x^3 коэфф на позиции 3 в списке
        /// свободный член на позиции 0 в списке
        /// </summary>
        internal List<Fraction> Odds { get; private set; }

        /// <summary>
        /// Название переменной
        /// </summary>
        internal string VariableName { get; private set; }

        /// <summary>
        /// Коеффицент перед многочленом
        /// </summary>
        internal Fraction Coefficient { get; private set; }

        public static Polynomial operator *(Polynomial poly1, Polynomial poly2)
        {
            List<Fraction> odds1 = poly1.Odds;
            List<Fraction> odds2 = poly2.Odds;
            int newPower = poly1.Power + poly2.Power;
            List<Fraction> newOdds = new List<Fraction>();

            for (int k = 0; k < newPower + 1; k++)
            {
                Fraction sumOfOdds = 0;
                for (int i = 0; i < odds1.Count(); i++)
                {
                    for (int j = 0; j < odds2.Count(); j++)
                    {
                        if (i + j == k)
                        {
                            sumOfOdds += odds1[i] * odds2[j];
                        }
                    }
                }
                newOdds.Add(sumOfOdds);
            }

            Polynomial newPoly = new Polynomial(poly1.VariableName, newOdds);
            newPoly.Power = newPower;
            return newPoly;
        }

        public static Polynomial operator -(Polynomial poly1, Polynomial poly2)
        {
            List<Fraction> odds1 = poly1.Odds;
            List<Fraction> odds2 = poly2.Odds;
            List<Fraction> newOdds = new List<Fraction>();
            int minPoly = Math.Min(poly1.Power, poly2.Power);

            for (int i = 0; i <= minPoly; i++)
            {
                newOdds.Add(odds1[i] - odds2[i]);
            }

            for (int i = minPoly + 1; i <= Math.Max(poly1.Power, poly2.Power); i++)
            {
                if (poly1.Power > poly2.Power)
                {
                    newOdds.Add(odds1[i]);
                }
                else
                {
                    newOdds.Add(odds2[i]);
                }
            }

            Polynomial newPoly = new Polynomial(poly1.VariableName, newOdds);
            return newPoly;
        }

        public static Polynomial operator *(Polynomial poly, int num)
        {
            List<Fraction> odds = poly.Odds;
            int newPower = poly.Power;
            List<Fraction> newOdds = new List<Fraction>();

            foreach (var odd in odds)
            {
                newOdds.Add(odd * num);
            }

            Polynomial newPoly = new Polynomial(poly.VariableName, newOdds);
            newPoly.Power = newPower;
            return newPoly;
        }

        public static Polynomial operator /(Polynomial poly, Fraction num)
        {
            List<Fraction> odds = poly.Odds;
            int newPower = poly.Power;
            List<Fraction> newOdds = new List<Fraction>();

            foreach (var odd in odds)
            {
                newOdds.Add(odd / num);
            }

            Polynomial newPoly = new Polynomial(poly.VariableName, newOdds);
            newPoly.Power = newPower;
            return newPoly;
        }

        public static Polynomial operator -(Polynomial poly, Fraction num)
        {
            Fraction constant = poly.Odds[0] - num;
            List<Fraction> newOdds = poly.Odds;
            newOdds[0] -= num;
            return new Polynomial(poly.VariableName, newOdds);
        }

        internal Polynomial SeparatePart(int endOddNum)
        {
            List<Fraction> newOdds = new List<Fraction>();

            for (int i = 0; i <= endOddNum; i++)
            {
                newOdds.Add(Odds[i]);
            }

            return new Polynomial(VariableName, newOdds);
        }

        internal int OddsGCF()
        {
            bool allInt = true;
            foreach(Fraction odd in Odds)
            {
                if (odd.IntDenominator != 1)
                {
                    allInt = false;
                    break;
                }
            }
            if (allInt)
            {
                List<int> gcfs = new List<int>();
                foreach (Fraction odd in Odds)
                {
                    gcfs.Add(odd.IntNumenator);
                }

                while (gcfs.Count != 1)
                {
                    List<int> newGCFs = new List<int>();
                    if (gcfs.Count % 2 == 1)
                    {
                        newGCFs.Add(gcfs.Last());
                    }
                    for (int i = 0; i < gcfs.Count; i += 2)
                    {
                        if (gcfs.Count - i != 1)
                        {
                            newGCFs.Add(GCF(gcfs[i], gcfs[i + 1]));
                        }
                    }
                    gcfs = newGCFs;
                }
                return gcfs[0];
            }

            return -1;
        }

        /// <summary>
        /// Сокращает два многочлена
        /// </summary>
        /// <param name="poly"></param>
        /// <returns></returns>
        internal (Polynomial, Polynomial) Reduction(Polynomial poly)
        {
            
            List<Fraction> newOdds1 = new List<Fraction> { 1 };
            List<Fraction> newOdds2 = new List<Fraction> { 1 };

            if (poly.Coefficient == poly.Coefficient && Coefficient == Coefficient)
            {
                int polyGCF = poly.OddsGCF();
                int thisGCF = OddsGCF();
                int gcf = GCF(polyGCF, thisGCF);

                newOdds1 = new List<Fraction>();
                newOdds2 = new List<Fraction>();

                for (int i = 0; i < Odds.Count; i++)
                {
                    newOdds1.Add(Odds[i] / gcf);
                }
                for (int i = 0; i < poly.Odds.Count; i++)
                {
                    newOdds2.Add(poly.Odds[i] / gcf);
                }
            }

            Polynomial poly1 = new Polynomial(VariableName, newOdds1);
            Polynomial poly2 = new Polynomial(poly.VariableName, newOdds2);
            return (poly1, poly2);
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

            return (a + b) * (negA || negB ? -1 : 1 );
        }

        internal Polynomial Copy()
        {
            Polynomial CopyPoly = new Polynomial(VariableName, Odds);
            return CopyPoly;
        }

        /// <summary>
        /// Заменяет переменную на переданную строку
        /// </summary>
        /// <param name="str"></param>
        internal void Subs(string str)
        {
            VariableName = str;
        }

        /// <summary>
        /// Заменяет переменную на переданную функцию
        /// </summary>
        /// <param name="func"></param>
        internal void Subs(Function func)
        {
            VariableName = func.ToString();
        }

        internal static Polynomial ParseFromHTML(string str)
        {
            List<Fraction> odds = new List<Fraction>();
            str = str.Replace(" ", "");
            List<bool> signOfVariable = new List<bool>();
            for (int i = str.Length - 1; i >= 0; i--)
            {
                if (str[i] == '+')
                {
                    signOfVariable.Add(true);
                }
                if (str[i] == '-')
                {
                    signOfVariable.Add(false);
                }
            }
            int countOfVariabes;
            if (str.Split('>', '<').Count() > 1)
            {
                countOfVariabes = (int.Parse(str.Split('>', '<')[3]) - 1);
            }
            else
            {
                countOfVariabes = 2;
            }

            if (signOfVariable.Count() < countOfVariabes)
            {
                signOfVariable.Add(true);
            }
            List<string> variablesWithOdds = str.Split('+', '-').ToList();
            int k = 0;
            foreach (string variableWithOdd in variablesWithOdds)
            {
                int i = 0;
                while (i < variableWithOdd.Count() && !char.IsLetter(variableWithOdd[i]))
                {
                    i++;
                }
                string odd = variableWithOdd.Substring(0, i);
                Fraction frac = odd != "" ? int.Parse(odd) : 1;
                odds.Add(signOfVariable[signOfVariable.Count() - k - 1] ? frac : -frac);
                k++;
            }
            odds.Reverse();
            return new Polynomial("x", odds);
        }

        public override string ToString()
        {
            string result = string.Empty;
            for (int i = Odds.Count() - 1; i >= 0; i--)
            {
                if (Odds[i] != (double)0)
                {
                    result += Odds[i] > (Fraction)0 ? "+" : "-";
                    result += i < Odds.Count() - 1 ? " " : "";
                    result += Odds[i].Abs() > (Fraction)1 || i == 0 ? $"{Odds[i].Abs()}" : "";
                    result += i > 0 ? $"{VariableName}" : "";
                    result += i > 1 ? $"^{i} " : " ";
                }
            }
            result = result.Trim('+');
            result = result.Trim(' ');
            return $"({result})";
        }

        internal int CountNotZeroOdds()
        {
            int k = 0;
            foreach (Fraction odd in Odds)
            {
                if (odd != 0)
                {
                    k++;
                }
            }
            return k;
        }

        internal string ToHTML()
        {
            string result = string.Empty;
            if (VariableName.Contains("Log"))
            {
                Log log = Log.ParseFromHTML(VariableName);
                for (int i = Odds.Count() - 1; i >= 0; i--)
                {
                    log.SetPower(i);
                    if (Odds[i] != (double)0)
                    {
                        result += Odds[i] > (Fraction)0 ? "+" : "-";
                        result += i < Odds.Count() - 1 ? " " : "";
                        result += Odds[i].Abs() > (Fraction)1 || i == 0 ? $"{Odds[i].Abs()}" : "";
                        result += i > 0 ? $"{log.ToHTML()} " : "";
                    }
                }
            }
            else if (VariableName.Contains("power"))
            {
                VariableName = VariableName.Replace("power", "");
                PowerFunc powerFunc = PowerFunc.ParseFromHTML(VariableName);
                for (int i = Odds.Count() - 1; i >= 0; i--)
                {
                    powerFunc.SetPower(i);
                    if (Odds[i] != (double)0)
                    {
                        result += Odds[i] > (Fraction)0 ? "+" : "-";
                        result += i < Odds.Count() - 1 ? " " : "";
                        if (Odds[i].IntDenominator == 0)
                        {
                            result += Odds[i].Abs() != (Fraction)1 || i == 0 ? $"{Odds[i].Abs()}{(i != 0 ? "⋅" : "")}" : ""; 
                        }
                        else
                        {
                            result += Odds[i].Abs() != 1 || i == 0 ? $"{Odds[i].Abs().ToHTML()}{(i != 0 ? "⋅" : "")}" : "";
                        }
                        result += i > 0 ? $"{powerFunc.ToHTML()} " : "";
                    }
                }
            }
            else
            {
                for (int i = Odds.Count() - 1; i >= 0; i--)
                {
                    if (Odds[i] != (double)0)
                    {
                        result += Odds[i] > (Fraction)0 ? "+" : "-";
                        result += i < Odds.Count() - 1 ? " " : "";
                        result += Odds[i].Abs() > (Fraction)1 || i == 0 ? $"{Odds[i].Abs()}" : "";
                        result += i > 0 ? $"{VariableName}" : "";
                        result += i > 1 ? $"<sup>{i}</sup> " : " ";
                    }
                } 
            }
            result = result.Trim('+');
            result = result.Trim(' ');
            return $"{result}";
        }

    }
}
