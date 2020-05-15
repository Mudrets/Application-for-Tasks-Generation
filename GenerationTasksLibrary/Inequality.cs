using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerationTasksLibrary
{
    internal class Inequality
    {
        readonly Settings settings;

        /// <summary>
        /// Хранит дробь из которой было полученно неравенство
        /// </summary>
        internal Fraction BigFraction { get; }

        /// <summary>
        /// Левая часть неравенства
        /// </summary>
        internal List<Fraction> LeftSide { get; private set; }

        /// <summary>
        /// Правая часть неравенства
        /// </summary>
        internal List<Fraction> RightSide { get; private set; }

        /// <summary>
        /// Ответ неравенства
        /// </summary>
        internal Intervals Answer { get; private set; }

        internal GenerationKey GenerationKey { get; }

        /// <summary>
        /// Знак неравенства
        /// </summary>
        internal string Sign { get; }


        internal Inequality(GenerationKey generationKey)
        {
            Random rnd = new Random(generationKey.Seed);
            settings = generationKey.Settings;

            Fraction bigFraction = new Fraction(generationKey);
            BigFraction = bigFraction;
            Answer = bigFraction.Answer;
            Sign = bigFraction.Sign ? "<" : ">";
            Sign += Answer.StrictInequality ? "" : "=";
            if (Sign == "<=")
            {
                Sign = "⩽";
            }
            else if (Sign == ">=")
            {
                Sign = "⩾";
            }
            GenerationKey = generationKey;

            if (!settings.OneFraction)
            {
                GenerateLeftAndRightSides(generationKey);
            }
            else
            {
                bigFraction.MultiplyPolynominal(generationKey.Seed, settings);
                LeftSide = new List<Fraction> { bigFraction };
                RightSide = new List<Fraction>();
            }

            if (settings.PowerFunc && Answer.IsPowerBorders)
            {
                Fraction root = rnd.Next(-50, 51);
                Polynomial argument = new Polynomial("x", root);
                PowerFunc powFunc = new PowerFunc(argument, Answer.PowerBase);
                Subs(powFunc);
            }
            else if (settings.Radical && Answer.MinPoint() >= (double)0 && Answer.MaxPoint() > (double)10)
            {
                Fraction root = rnd.Next(-200, 201);
                Polynomial argument = new Polynomial("x", root);
                Radical radical = new Radical(argument);
                Subs(radical);
            }
            else if (settings.Log && Answer.MinPoint() >= (double)-10 && Answer.MaxPoint() <= (double)10 && Answer.AreAllPointsInteger())
            {
                Fraction root = rnd.Next(-100, 100);
                Polynomial argument = new Polynomial("x", root);
                int rndBase = rnd.Next(6) == 0 ? 2 : 3;
                Fraction @base = Answer.MinPoint() >= (double)-5 && Answer.MaxPoint() <= (double)5 ? rndBase : 2;
                Log log = new Log(argument, @base);
                Subs(log);
            }
        }

        /// <summary>
        /// Разбивает дробь на левую и правую отзнака неравенства части
        /// </summary>
        /// <param name="generationKey">ключ генерации</param>
        void GenerateLeftAndRightSides(GenerationKey generationKey)
        {
            Random rnd = new Random(generationKey.Seed);
            LeftSide = new List<Fraction>();
            RightSide = new List<Fraction>();
            List<Fraction> fracList = BigFraction.DecomposeAmountOfFractions(generationKey);
            if (fracList.Count() == 1)
            {
                fracList[0].MultiplyPolynominal(generationKey.Seed, settings);
            }
            for (int i = 0; i < fracList.Count; i++)
            {
                int rndValue = rnd.Next(2);
                if (rndValue == 0)
                {
                    LeftSide.Add(fracList[i]);
                }
                else
                {
                    RightSide.Add(-fracList[i]);
                }
            }
        }

        public override string ToString()
        {
            Random rnd = new Random(GenerationKey.Seed);
            string result = string.Empty;

            if (LeftSide.Count != 0)
            {
                for (int i = 0; i < LeftSide.Count; i++)
                {

                    result += $"{LeftSide[i]} ";
                    if (i < LeftSide.Count - 1)
                    {
                        result += "+ ";
                    }
                }
            }
            else
            {
                result += "0 ";
            }

            result += $"{Sign} ";

            if (RightSide.Count != 0)
            {
                for (int i = 0; i < RightSide.Count; i++)
                {

                    result += $"{RightSide[i]} ";
                    if (i < RightSide.Count - 1)
                    {
                        result += "+ ";
                    }
                } 
            }
            else
            {
                result += "0 ";
            }
            result += "\n\n";
            result += $"Ответ: {Answer}";

            return result.Trim(' ');
        }

        /// <summary>
        /// Заменяет переменную в неравенстве на строку
        /// </summary>
        /// <param name="str"></param>
        internal void Subs(string str)
        {
            foreach (var frac in RightSide)
            {
                frac.Subs(str);
            }

            foreach (var frac in LeftSide)
            {
                frac.Subs(str);
            }
        }

        internal void Subs(Function func)
        {
            foreach (var frac in RightSide)
            {
                frac.Subs($"{(func is PowerFunc ? "power" : "")}{func.ToHTML()}");
            }

            foreach (var frac in LeftSide)
            {
                frac.Subs($"{(func is PowerFunc ? "power" : "")}{func.ToHTML()}");
            }

            Answer = Answer.Subs(func);
        }

        internal string ToHTML()
        {
            string result = string.Empty;

            if (LeftSide.Count != 0)
            {
                for (int i = 0; i < LeftSide.Count; i++)
                {

                    result += $"{LeftSide[i].ToHTML()}";
                    if (i < LeftSide.Count - 1)
                    {
                        result += "<span class=\"sign\">+</span>";
                    }
                }
            }
            else
            {
                result += "<span class=\"number\">0</span>";
            }

            result += $"<span class=\"sign\">{Sign}</span>";

            if (RightSide.Count != 0)
            {
                for (int i = 0; i < RightSide.Count; i++)
                {

                    result += $"{RightSide[i].ToHTML()}";
                    if (i < RightSide.Count - 1)
                    {
                        result += "<span class=\"sign\">+</span>";
                    }
                }
            }
            else
            {
                result += "<span class=\"number\">0</span>";
            }
            if (settings.ShowAnsers)
            {
                result += "<br><br>";
                result += $"<span class=\"answer\">Ответ:</span> {Answer.ToHTML()}"; 
            }

            return result.Trim(' ');
        }

    }
}
