using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerationTasksLibrary
{
    internal class PowerFunc : Function
    {
        internal PowerFunc(Polynomial powerFunc, int @base)
        {
            Argument = powerFunc;
            Base = @base; 
        }

        internal int Base { get; }

        internal int Power;

        internal override Fraction Eq(Fraction num)
        {
            Fraction numenator = Log(Base, num) - Argument.Odds[0];
            Fraction denomenator = Argument.Odds[1];
            int intNumenator = numenator.IntNumenator * denomenator.IntDenominator;
            int intDenomenator = numenator.IntDenominator * denomenator.IntNumenator;
            return new Fraction(intNumenator, intDenomenator);
        }

        static int Log(int @base, Fraction num)
        {
            if (num.IntNumenator == 1 && num.IntDenominator == 1)
            {
                return 0;
            }
            else if (num.IntNumenator > 1)
            {
                int number = 1;
                int k = 0;
                while (number != num.IntNumenator)
                {
                    number *= @base;
                    k++;
                }
                return k;
            }
            else
            {
                int number = 1;
                int k = 0;
                while (number != num.IntDenominator)
                {
                    number *= @base;
                    k++;
                }
                return -k;
            }
        }

        internal override Intervals GetValuesRange()
        {
            return null;
        }

        internal void SetPower(int power)
        {
            Power = power;
        }

        internal static PowerFunc ParseFromHTML(string str)
        {
            int i = 0;
            while (str[i] != '<')
            {
                i++;
            }

            int @base = int.Parse(str.Substring(0, i));
            i += 5;
            Polynomial argument;
            int power = 1;
            if (str[i] != '(')
            {
                int k = i + 1;
                while (str[k] == '(')
                {
                    k++;
                }
                power = int.Parse(str.Substring(i, k - i - 1));
                i = k;
            }

            int j = i + 1;
            while (str[j] != ')')
            {
                j++;
            }
            argument = Polynomial.ParseFromHTML(str.Substring(i + 1, j - i - 1));

            PowerFunc result = new PowerFunc(argument, @base);
            result.SetPower(power);
            return result;
        }

        internal override string ToHTML()
        {
            return $"{Base}<sup>{(Power > 1 ? $"{Power}" : "")}({Argument.ToHTML()})</sup>";
        }
    }
}
