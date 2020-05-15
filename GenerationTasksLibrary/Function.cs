using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerationTasksLibrary
{
    internal abstract class Function
    {
        internal Polynomial Argument { get; private protected set; }
        internal Intervals ValuesRange
        {
            get {
                return GetValuesRange();
            }
        }

        abstract internal Fraction Eq(Fraction num);
        abstract internal Intervals GetValuesRange();
        abstract internal string ToHTML();
    }
}
