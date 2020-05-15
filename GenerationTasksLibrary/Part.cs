using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerationTasksLibrary
{
    internal class Part
    {

        internal Part(GenerationKey key)
        {
            //Передаем характиристики варианта
            CountOfTasks = key.CountOfTasks; //Количество заданий
            MetSeeds = new List<int>();

            //Лист с заданиями
            List<Inequality> tasks = new List<Inequality>();
            //Заполняем лист заданиями
            for (int i = 0; i < key.CountOfTasks; i++)
            {
                if (!MetSeeds.Contains(key.Seed))
                {
                    MetSeeds.Add(key.Seed);
                    Inequality task = null;
                    try
                    {
                        task = new Inequality(key);
                    }
                    catch (OverflowException)
                    {
                        i--;
                        continue;
                    }
                    catch (DivideByZeroException)
                    {
                        i--;
                        continue;
                    }
                    if (task.ToHTML().Contains("/0"))
                    {
                        i--;
                        continue;
                    }
                    tasks.Add(task);
                }
                else
                {
                    i--;
                }

                key++;
            }
            Tasks = tasks;
        }

        /// <summary>
        /// Лист использованных ключей генерации заданий.
        /// Создан для того, чтобы избежать генерации одного
        /// задания более одного раза
        /// </summary>
        List<int> MetSeeds { get; }

        /// <summary>
        /// Количество заданий в тесте
        /// </summary>
        internal int CountOfTasks { get; private set; }

        /// <summary>
        /// Лист с заданиями из работы
        /// </summary>
        internal List<Inequality> Tasks { get; private set; }

        public override string ToString()
        {
            string result = string.Empty;

            for (int i = 0; i < CountOfTasks; i++)
            {
                result += $"\nЗадание {i + 1}\n{Tasks[i]}\n";
            }

            return result;
        }

        internal string ToHTML()
        {
            string result = string.Empty;

            for (int i = 0; i < CountOfTasks; i++)
            {
                result += $"<div class=\"task\"><h3>Задание {i + 1}</h3><br>{Tasks[i].ToHTML()}</div>";
            }

            return result;
        }

    }
}
