using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerationTasksLibrary
{
    internal class Radical : Function
    {
        internal Radical(Polynomial argument)
        {
            Argument = argument;
        }

        internal override Fraction Eq(Fraction num)
        {
            return num >= (double)0 ? Fraction.Pow(num, 2) : null;
        }

        internal override Intervals GetValuesRange()
        {
            bool isPos = Argument.Coefficient / Fraction.Abs(Argument.Coefficient) == 1;
            List<Interval> intervalsList = new List<Interval>();
            if (Argument.Power == 2)
            {
                if (isPos)
                {
                    intervalsList.Add(new Interval(true, Argument.Solutions[0], true));
                    intervalsList.Add(new Interval(false, Argument.Solutions[1], true));
                }
                else
                {
                    intervalsList.Add(new Interval(Argument.Solutions[0], Argument.Solutions[1], true, true));
                }
            }
            else if (Argument.Power == 1)
            {
                intervalsList.Add(new Interval(!isPos, Argument.Solutions[0], true));
            }

            return new Intervals(intervalsList);
        }

        public override string ToString()
        {
            return $"√({Argument})";
        }

        internal override string ToHTML()
        {
            return $"√({Argument.ToHTML()})";
        }
    }
}
