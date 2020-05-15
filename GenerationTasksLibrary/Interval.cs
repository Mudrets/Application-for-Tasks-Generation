using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerationTasksLibrary
{
    internal class Interval
    {
        /// <summary>
        /// Левая граница интервала
        /// </summary>
        internal Fraction LeftBorder { get; private set; }
        /// <summary>
        /// Правая граница интервала
        /// </summary>
        internal Fraction RightBorder { get; private set; }
        /// <summary>
        /// Хранит границу в случае если вторая граница является бесконечностью
        /// </summary>
        internal Fraction Border { get; private set; }
        /// <summary>
        /// Вклучается ли левая граница
        /// </summary>
        internal bool LeftIsIncluded { get; }
        /// <summary>
        /// Включается ли правая граница
        /// </summary>
        internal bool RightIsIncluded { get; }
        /// <summary>
        /// Является ли одна из границ интервала бесконечностью
        /// </summary>
        internal bool IsInfinityInterval { get; }
        /// <summary>
        /// True - создает интервал (-inf border);
        /// False - создает интервал (border +inf);
        /// </summary>
        internal bool WichSide { get; }
        internal bool BorderIsIncluded { get; }


        /// <summary>
        /// Создает интервал
        /// </summary>
        /// <param name="leftBorder">Левая граница</param>
        /// <param name="rightBorder">Правая граница</param>
        /// <param name="leftIsIncluded">Включена ли правая граница</param>
        /// <param name="rightIsIncluded">Включена ли правая граница</param>
        internal Interval(Fraction leftBorder, Fraction rightBorder, bool leftIsIncluded = false, bool rightIsIncluded = false)
        {
            LeftBorder = Fraction.Min(leftBorder, rightBorder);
            RightBorder = Fraction.Max(leftBorder, rightBorder);
            LeftIsIncluded = leftIsIncluded;
            RightIsIncluded = rightIsIncluded;
            IsInfinityInterval = false;
        }

        /// <summary>
        /// Создает интервал вида (-inf border) или (border +inf)
        /// </summary>
        /// <param name="wichBorder">
        /// True - создает интервал (-inf border);
        /// False - создает интервал (border +inf);
        /// </param>
        /// <param name="border">граница интервала</param>
        /// <param name="borderIsIncluded">включена ли эта граница</param>
        internal Interval(bool wichBorder, Fraction border, bool borderIsIncluded = false)
        {
            WichSide = wichBorder;
            Border = border;
            BorderIsIncluded = borderIsIncluded;
            IsInfinityInterval = true;
        }

        /// <summary>
        /// Определяет какой из двух интервалов бесконечный и
        /// возвращает пару в которой первый интервал бесконечный
        /// а второй конечный
        /// </summary>
        /// <param name="interval1"></param>
        /// <param name="interval2"></param>
        /// <returns></returns>
        static (Interval, Interval) WichInfinity(Interval interval1, Interval interval2)
        {
            return interval1.IsInfinityInterval ? (interval1, interval2) : (interval2, interval1);
        }

        /// <summary>
        /// Пересекает два интервала
        /// </summary>
        /// <param name="interval1">первый интервал</param>
        /// <param name="interval2">второй интервал</param>
        /// <returns>пересечение переданных интервалов</returns>
        public static Interval operator *(Interval interval1, Interval interval2)
        {
            if (!interval1.IsInfinityInterval && !interval2.IsInfinityInterval)
            {
                Fraction a = interval1.LeftBorder, b = interval1.RightBorder,
                         c = interval2.LeftBorder, d = interval2.RightBorder;

                
                Fraction newLeftBorder = Fraction.Max(a, c);
                //Определяем какая граница будет слева
                bool leftIsIncluded = newLeftBorder.Equals(a) ? interval1.LeftIsIncluded : interval2.LeftIsIncluded;
                leftIsIncluded = a.Equals(c) ? interval1.LeftIsIncluded && interval2.RightIsIncluded : leftIsIncluded;
                Fraction newRightBorder = Fraction.Min(b, d);
                //Определяет какая граница будет справа
                bool rightIsIncluded = newRightBorder.Equals(b) ? interval1.RightIsIncluded : interval2.RightIsIncluded;
                rightIsIncluded = b.Equals(d) ? interval1.LeftIsIncluded && interval2.RightIsIncluded : rightIsIncluded;

                //Создаем пересечение
                if (newLeftBorder < newRightBorder)
                {
                    return new Interval(newLeftBorder, newRightBorder, leftIsIncluded, rightIsIncluded);
                }
                else if (newLeftBorder.Equals(newRightBorder))
                {
                    return leftIsIncluded && rightIsIncluded ? new Interval(newLeftBorder, newRightBorder, true, true) : null;
                }
                return null;
            }
            else if (interval1.IsInfinityInterval && interval2.IsInfinityInterval)
            {
                if (interval1.WichSide ^ interval2.WichSide)
                {
                    //Находим правый и левый интервалы
                    Interval leftInterval = interval1.WichSide ? interval1 : interval2;
                    Interval rightInterval = interval1.WichSide ? interval2 : interval1;

                    if (leftInterval.Border > rightInterval.Border)
                    {
                        return new Interval(leftInterval.Border, rightInterval.Border,
                            leftInterval.BorderIsIncluded, rightInterval.BorderIsIncluded);
                    }
                    else if (leftInterval.Border.Equals(rightInterval.Border) && leftInterval.BorderIsIncluded && rightInterval.BorderIsIncluded)
                    {
                        return new Interval(leftInterval.Border, rightInterval.Border, true, true);
                    }

                    return null;
                }
                else
                {
                    if (interval1.Border.Equals(interval2.Border))
                    {
                        return new Interval(interval1.WichSide, interval1.Border, interval1.BorderIsIncluded && interval2.BorderIsIncluded);
                    }
                    else
                    {
                        Fraction border = interval1.WichSide ? Fraction.Min(interval1.Border, interval2.Border)
                                                             : Fraction.Max(interval1.Border, interval2.Border);
                        return new Interval(interval1.WichSide, border, interval1.Border.Equals(border)?
                            interval1.BorderIsIncluded : interval2.BorderIsIncluded);
                    }
                }
            }
            else
            {
                Interval infInter, inter;
                (infInter, inter) = WichInfinity(interval1, interval2);
                if (infInter.WichSide)
                {
                    if (inter.LeftBorder > infInter.Border)
                    {
                        return null;
                    }
                    else if (inter.LeftBorder.Equals(infInter.Border))
                    {
                        return inter.LeftIsIncluded && infInter.BorderIsIncluded ? 
                            new Interval(inter.LeftBorder, infInter.Border, true, true) : null;
                    }
                    else if (inter.RightBorder > infInter.Border)
                    {
                        return new Interval(inter.LeftBorder, infInter.Border, 
                            inter.LeftIsIncluded, infInter.BorderIsIncluded);
                    }
                    else if (inter.RightBorder.Equals(infInter.Border))
                    {
                        return new Interval(inter.LeftBorder, inter.RightBorder, inter.LeftIsIncluded, inter.RightIsIncluded && infInter.BorderIsIncluded);
                    }
                    else
                    {
                        return new Interval(inter.LeftBorder, inter.RightBorder, inter.LeftIsIncluded, inter.RightIsIncluded);
                    }
                }
                else
                {
                    if (inter.RightBorder < infInter.Border)
                    {
                        return null;
                    }
                    else if (inter.RightBorder.Equals(infInter.Border))
                    {
                        return inter.RightIsIncluded && infInter.BorderIsIncluded ? 
                            new Interval(inter.RightBorder, infInter.Border, true, true) : null;
                    }
                    else if (inter.LeftBorder < infInter.Border)
                    {
                        return new Interval(infInter.Border, inter.RightBorder,
                            infInter.BorderIsIncluded, inter.RightIsIncluded);
                    }
                    else if (inter.LeftBorder.Equals(infInter.Border))
                    {
                        return new Interval(inter.LeftBorder, inter.RightBorder, inter.LeftIsIncluded, inter.LeftIsIncluded && infInter.BorderIsIncluded);
                    }
                    else
                    {
                        return new Interval(inter.LeftBorder, inter.RightBorder, inter.LeftIsIncluded, inter.RightIsIncluded);
                    }
                }
            }
        }

        /// <summary>
        /// Парсит строковое представление интервала в эквивалентный ему
        /// интервал
        /// </summary>
        /// <param name="str">строковое представление интервала</param>
        /// <returns></returns>
        internal static Interval Parse(string str)
        {
            if (str[0] != '{')
            {
                //Определяем тип границы нашего интервала
                bool leftIsInclude = str.First() == '[';
                bool rightIsInclude = str.Last() == ']';

                List<string> nums = str.Substring(1, str.Length - 2).Split(' ').ToList();
                nums.RemoveAll(x => x == "");
                Fraction frac1, frac2;
                if (Fraction.TryParse(nums[0], out frac1) & Fraction.TryParse(nums[1], out frac2))
                {
                    return new Interval(frac1, frac2, leftIsInclude, rightIsInclude);
                }
                else
                {
                    bool wichBorder = frac1 == null;
                    return new Interval(wichBorder, wichBorder ? frac2 : frac1,
                                                    wichBorder ? rightIsInclude : leftIsInclude);
                }
            }
            else
            {
                string num = str.Substring(1, str.Length - 2);
                Fraction frac;
                Fraction.TryParse(num, out frac);
                return new Interval(frac, frac, true, true);
            }
        }

        /// <summary>
        /// Изменяет границы интервала. Если левая граница больше правой, то
        /// границы интервала не изменяются
        /// </summary>
        /// <param name="leftBorder">левая граница</param>
        /// <param name="rightBorder">правая граница</param>
        internal void ChangeBorders(Fraction leftBorder, Fraction rightBorder)
        {
            if (leftBorder < rightBorder)
            {
                LeftBorder = leftBorder;
                RightBorder = rightBorder;
            }
            else if (leftBorder.Equals(rightBorder))
            {
                LeftBorder = LeftIsIncluded && RightIsIncluded ? leftBorder : LeftBorder;
                RightBorder = LeftIsIncluded && RightIsIncluded ? rightBorder : RightBorder;
            }
        }

        internal void ChangeBorderForInfinity(Fraction border)
        {
            Border = border;
        }

        public override string ToString()
        {
            if (IsInfinityInterval)
            {
                if (WichSide)
                {
                    return $"(-inf {Border}{(BorderIsIncluded ? "]" : ")")}";
                }
                else
                {
                    return $"{(BorderIsIncluded ? "[" : "(")}{Border} +inf)";
                }
            }
            else
            {
                if (LeftBorder.Equals(RightBorder) && LeftIsIncluded && RightIsIncluded)
                {
                    return $"{{{LeftBorder}}}";
                }
                else
                {
                    return $"{(LeftIsIncluded ? "[" : "(")}{LeftBorder} {RightBorder}{(RightIsIncluded ? "]" : ")")}";
                }
            }
        }

        internal string ToHtml()
        {
            if (IsInfinityInterval)
            {
                if (WichSide)
                {
                    return $"(-∞ {Border.ToHTML()}{(BorderIsIncluded ? "]" : ")")}";
                }
                else
                {
                    return $"{(BorderIsIncluded ? "[" : "(")}{Border.ToHTML()} +∞)";
                }
            }
            else
            {
                if (LeftBorder.Equals(RightBorder) && LeftIsIncluded && RightIsIncluded)
                {
                    return $"{{{LeftBorder.ToHTML()}}}";
                }
                else
                {
                    return $"{(LeftIsIncluded ? "[" : "(")}{LeftBorder.ToHTML()} {RightBorder.ToHTML()}{(RightIsIncluded ? "]" : ")")}";
                }
            }
        }

    }
}
