using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerationTasksLibrary
{
    internal class EquationSystem
    {
        internal EquationSystem (List<Equation> eqList)
        {
            Equations = eqList;
        }

        private List<Equation> Equations { get; }

        internal Dictionary<char, Fraction> Solve()
        {
            Dictionary<char, Fraction> result = new Dictionary<char, Fraction>();

            //Проверка решаемости системы
            int maxVariableCount = 0;
            foreach (var eq in Equations)
            {
                if(eq.VariableAndCoef.Count > maxVariableCount)
                {
                    maxVariableCount = eq.VariableAndCoef.Count;
                }
            }
            //Если максимальное количество переменных в уравнении больше
            //чем количетво уравнений, то система имеет более одного решения
            //или не имеет их совсем
            if(maxVariableCount > Equations.Count)
            {
                return result;
            }

            for (int i = 0; i < maxVariableCount; i++)
            {
                Equation express;
                char variableName;
                if (Equations[i].VariableAndCoef.Count > 0)
                {
                    variableName = Equations[i].VariableAndCoef.Keys.ElementAt(0);
                    express = Equations[i].Express(variableName); 
                }
                else
                {
                    continue;
                }

                for (int j = 0; j < Equations.Count; j++)
                {
                    if (j != i && Equations[j].VariableAndCoef.Keys.Contains(variableName))
                    {
                        Equations[j].Subs(variableName, express);
                    }
                }
            }
            List<char> zeroVariables = new List<char>();
            //Заполняем словарь с результатами
            for (int i = 0; i < Equations.Count; i++)
            {
                //Находим коэффицент перед параметром
                if (Equations[i].VariableAndCoef.Count > 0)
                {
                    for (int j = 1; j < Equations[i].VariableAndCoef.Count; j++)
                    {
                        char key = Equations[i].VariableAndCoef.Keys.ElementAt(j);
                        if (!zeroVariables.Contains(key))
                        {
                            zeroVariables.Add(key);
                        }
                        Equations[i].Subs(key, 0);
                    }
                    char variableName = Equations[i].VariableAndCoef.Keys.ElementAt(0);
                    Fraction coef = Equations[i].VariableAndCoef[variableName];
                    result.Add(variableName, -Equations[i].FreeMember / coef);
                }
                else if (Equations[i].FreeMember != (double)0)
                {
                    return new Dictionary<char, Fraction>();
                }
            }

            foreach (char variable in zeroVariables)
            {
                result.Add(variable, 0);
            }

            return result;
        }

    }
}
