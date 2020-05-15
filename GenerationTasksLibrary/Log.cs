using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerationTasksLibrary
{
    internal class Log : Function
    {
        internal Log(Polynomial argument, Fraction @base)
        {
            Argument = argument;
            Base = @base;
        }

        internal Fraction Base { get; }
        internal Fraction Power { get; private set; }

        internal override Intervals GetValuesRange()
        {
            bool isPos = Argument.Coefficient / Fraction.Abs(Argument.Coefficient) == 1;
            List<Interval> intervalsList = new List<Interval>();
            if (Argument.Power == 2)
            {
                if (isPos)
                {
                    intervalsList.Add(new Interval(true, Argument.Solutions[0]));
                    intervalsList.Add(new Interval(false, Argument.Solutions[1]));
                }
                else
                {
                    intervalsList.Add(new Interval(Argument.Solutions[0], Argument.Solutions[1]));
                }
            }
            else if (Argument.Power == 1)
            {
                intervalsList.Add(new Interval(!isPos, Argument.Solutions[0]));
            }

            return new Intervals(intervalsList);
        }

        internal void SetPower(Fraction pow)
        {
            Power = pow;
        }

        internal override Fraction Eq(Fraction num)
        {
            Fraction newNum = Fraction.Pow(Base, num.IntNumenator);
            return (newNum - Argument.Odds[0]) / Argument.Odds[1];
        }

        internal static Log ParseFromHTML(string str)
        {
            int i = 0;
            while (str[i] != '>')
            {
                i++;
            }

            int j = i;
            while (str[j] != '<')
            {
                j++;
            }

            int power = 1;
            if (str[i-1] == 'p')
            {
                power = int.Parse(str.Substring(i + 1, j - i - 1));
                i = j + 10;
                j += 10;
                while (str[j] != '<')
                {
                    j++;
                }
            }

            int @base = 0;
            if (str[i-1] == 'b')
            {
                @base = int.Parse(str.Substring(i + 1, j - i - 1));
            }

            i = j + 6;
            j += 6;
            while (str[j] != ')')
            {
                j++;
            }
            Polynomial argument = Polynomial.ParseFromHTML(str.Substring(i + 1, j - i - 1));

            Log log = new Log(argument, @base);
            log.SetPower(power);
            return log;
        }

        public override string ToString()
        {
            return $"Log{(Power != null && Power != 1 ? $"^({Power})" : "")}_{{{Base}}}({Argument})";
        }

        internal override string ToHTML()
        {
            return $"Log{(Power != null && Power != 1 ? $"<sup>{Power}</sup>" : "")}<sub>{Base}</sub>({Argument.ToHTML()})";
        }
    }
}
