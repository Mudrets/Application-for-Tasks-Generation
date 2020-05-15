using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GenerationTasksLibrary
{
    public class Test
    {
        public Test(GenerationKeyForTest key)
        {
            Parts = new List<Part>();
            Key = key;
            for (int i = 0; i < key.GenerationKeys.Count(); i++)
            {
                if (key.GenerationKeys[i].CountOfTasks != 0)
                {
                    Part task = new Part(key.GenerationKeys[i]);
                    Parts.Add(task);
                }
            }
        }

        public GenerationKeyForTest Key { get; private set; }

        internal List<Part> Parts { get; private set; }

        public void CreateTestDirectory(string path)
        {
            path = $@"{path}\Контрольная работа ({Key})";
            Directory.CreateDirectory(path);
            GenerationKeyForTest key = Key;
            int countTasks = key.CountOfTasks;
            Test test = this;
            for (int i = 0; i < countTasks; i++)
            {
                string fullPath = $@"{path}\Вариант {i + 1}.html";
                string code = File.ReadAllText("code.txt");
                code += test.ToHTML();
                code += "</body></html>";
                File.WriteAllText(fullPath, code);
                test = new Test(++key);
            }
        }

        public string ToHTML()
        {
            string result = string.Empty;
            for (int i = 0; i < Parts.Count(); i++)
            {
                result += $"<h1>Часть {i + 1}</h1>";
                result += $"{Parts[i].ToHTML()}";
            }
            return result;
        }
    }
}
