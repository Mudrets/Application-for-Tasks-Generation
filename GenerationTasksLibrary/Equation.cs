using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerationTasksLibrary
{
    public class Equation
    {

        /// <summary>
        /// Создает уравнение на основе переданного словаря, ключами которого являются 
        /// названия переменных, а значениями являются коэффициенты у этих переменных
        /// </summary>
        /// <param name="variableAndCoef"></param>
        /// <param name="freeMember"></param>
        public Equation(Dictionary<char, Fraction> variableAndCoef, Fraction freeMember = null)
        {
            RemoveZero(variableAndCoef);
            VariableAndCoef = variableAndCoef;
            if (freeMember == null)
            {
                FreeMember = 0;
            }
            else
            {
                FreeMember = freeMember;
            }
        }

        internal Dictionary<char, Fraction> VariableAndCoef { get; private set; }

        internal Fraction FreeMember { get; private set; }

        /// <summary>
        /// Подстваляет вместо переданной переменной 
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="value"></param>
        internal void Subs(char variable, Fraction value)
        {
            Fraction freeMember = VariableAndCoef[variable] * value;
            FreeMember += freeMember;
            VariableAndCoef.Remove(variable);
        }

        /// <summary>
        /// Подставялет вместо переданной переменной какое-то выражение
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="eq"></param>
        internal void Subs(char variable, Equation eq)
        {
            try
            {
                Fraction coef = VariableAndCoef[variable];
                VariableAndCoef.Remove(variable);

                //Копируем словарь
                Dictionary<char, Fraction> vcCopy = new Dictionary<char, Fraction>();
                foreach (var vc in eq.VariableAndCoef)
                {
                    vcCopy.Add(vc.Key, vc.Value);
                }

                for (int i = 0; i < vcCopy.Count; i++)
                {
                    char key = vcCopy.Keys.ElementAt(i);
                    vcCopy[key] *= coef;
                }

                foreach (var vs in vcCopy)
                {
                    if (VariableAndCoef.ContainsKey(vs.Key))
                    {
                        VariableAndCoef[vs.Key] += vs.Value;
                    }
                    else
                    {
                        VariableAndCoef.Add(vs.Key, vs.Value);
                    }
                }

                VariableAndCoef = RemoveZero(VariableAndCoef);

                FreeMember += eq.FreeMember * coef;
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException("Переданной переменной нет в уравнении", "variable");
            }
        }

        /// <summary>
        /// Выражает переданную переменную через другие
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        internal Equation Express(char variable)
        {
            if (VariableAndCoef.Keys.Contains(variable))
            {
                Dictionary<char, Fraction> newEq = new Dictionary<char, Fraction>();
                foreach (var vc in VariableAndCoef)
                {
                    if (vc.Key != variable)
                    {
                        newEq.Add(vc.Key, -vc.Value / VariableAndCoef[variable]);
                    }
                }

                return new Equation(newEq, -FreeMember / VariableAndCoef[variable]);
            }
            else
            {
                throw new ArgumentException("Переданной переменной нет в уравнении", "variable");
            }
        }

        /// <summary>
        /// Удаляет все переменные с нулевыми коэффицентами
        /// </summary>
        /// <param name="vc"></param>
        /// <returns></returns>
        static Dictionary<char, Fraction> RemoveZero(Dictionary<char, Fraction> vc)
        {
            for (int i = 0; i < vc.Count; i++)
            {
                if (vc.Values.ElementAt(i) == (double)0)
                {
                    char key = vc.Keys.ElementAt(i);
                    vc.Remove(key);
                    i--;
                }
            }

            return vc;
        }

        public override string ToString()
        {
            string result = string.Empty;
            foreach (var vc in VariableAndCoef)
            {
                result += $"+ ({vc.Value})*{vc.Key} ";
            }
            result += $"+ {FreeMember}";
            return result.Trim('+').Trim(' ');
        }
    }
}
