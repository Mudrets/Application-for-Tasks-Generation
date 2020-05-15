using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GenerationTasksLibrary;

namespace CoursePaper
{

    public partial class CreateTestForm : Form
    {
        Color incorrectColor = Color.FromArgb(255, 115, 115);
        Color white;
        Color activeTextColor = Color.FromArgb(0, 0, 0);
        Color notActiveTextColor;
        Color btnColor;

        Random rnd = new Random();

        string directoryName = string.Empty;

        public CreateTestForm()
        {
            InitializeComponent();
            white = CountOfRoots1.BackColor;
            notActiveTextColor = CountOfRoots1.ForeColor;
            btnColor = AddSecondPart.BackColor;
            AddSecondPart.BackColor = white;
            AddSecondPart.ForeColor = Color.FromArgb(0, 0, 0);
            AddSecondPart.FlatAppearance.BorderColor = Color.FromArgb(0, 0, 0);
            AddThirdPart.BackColor = white;
            AddThirdPart.ForeColor = Color.FromArgb(0, 0, 0);
            AddThirdPart.FlatAppearance.BorderColor = Color.FromArgb(0, 0, 0);
            AddForthPart.BackColor = white;
            AddForthPart.ForeColor = Color.FromArgb(0, 0, 0);
            AddForthPart.FlatAppearance.BorderColor = Color.FromArgb(0, 0, 0);
            AddFifthPart.BackColor = white;
            AddFifthPart.ForeColor = Color.FromArgb(0, 0, 0);
            AddFifthPart.FlatAppearance.BorderColor = Color.FromArgb(0, 0, 0);
            GenerateBtn.BackColor = Color.FromArgb(39, 174, 96);
            label1.ForeColor = Color.FromArgb(52, 152, 219);
        }

        private void GenerateBtn_Click(object sender, EventArgs e)
        {
            if (CheckСorrectnessInput())
            {
                if (directoryName == string.Empty)
                {
                    MessageBox.Show("Выберите папку в которую будут сохроняться файлы с вариантами", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    List<GenerationKey> keys = new List<GenerationKey>();

                    bool answersCheckBox = checkBoxAnswers.Checked;

                    int seed = rnd.Next(1, 1000000);
                    int countOfRoots1 = int.Parse(CountOfRoots1.Text);
                    int maxValueRoots1 = int.Parse(MaxValueRoots1.Text);
                    int maxPolyPower1 = int.Parse(MaxPolyPower1.Text);
                    int countOfTasks1 = int.Parse(CountOfTasks1.Text);
                    bool containsLog1 = checkBoxLog1.Checked;
                    bool containsRadical1 = checkBoxRadical1.Checked;
                    bool containsPowerFunc1 = checkBoxPowerFunc1.Checked;
                    bool containsRMT1 = checkBoxRMT1.Checked;
                    bool oneFrac1 = checkBoxOneFruc1.Checked;

                    Settings settingsForPart1 = new Settings(countOfRoots1, maxValueRoots1,
                                                             maxPolyPower1, oneFrac1, containsLog1,
                                                             containsRadical1, containsRMT1,
                                                             containsPowerFunc1, answersCheckBox);
                    keys.Add(new GenerationKey(countOfTasks1, seed, settingsForPart1));

                    if (CountOfRoots2.Enabled)
                    {
                        seed = (keys[0].Seed + 1) % 1000000;
                        int countOfRoots2 = int.Parse(CountOfRoots2.Text);
                        int maxValueRoots2 = int.Parse(MaxValueRoots2.Text);
                        int maxPolyPower2 = int.Parse(MaxPolyPower2.Text);
                        int countOfTasks2 = int.Parse(CountOfTasks2.Text);
                        bool containsLog2 = checkBoxLog2.Checked;
                        bool containsRadical2 = checkBoxRadical2.Checked;
                        bool containsPowerFunc2 = checkBoxPowerFunc2.Checked;
                        bool containsRMT2 = checkBoxRMT2.Checked;
                        bool oneFrac2 = checkBoxOneFruc2.Checked;

                        Settings settingsForPart2 = new Settings(countOfRoots2, maxValueRoots2,
                                                                 maxPolyPower2, oneFrac2, containsLog2,
                                                                 containsRadical2, containsRMT2,
                                                                 containsPowerFunc2, answersCheckBox);
                        keys.Add(new GenerationKey(countOfTasks2, seed, settingsForPart2));
                    }
                    else
                    {
                        seed = (keys[0].Seed + 1) % 1000000;
                        Settings settingsForPart2 = new Settings(1, 11, 1, false, false, false, false, false, answersCheckBox);
                        keys.Add(new GenerationKey(0, seed, settingsForPart2));
                    }

                    if (CountOfRoots3.Enabled)
                    {
                        seed = (keys[1].Seed + 1) % 1000000;
                        int countOfRoots3 = int.Parse(CountOfRoots3.Text);
                        int maxValueRoots3 = int.Parse(MaxValueRoots3.Text);
                        int maxPolyPower3 = int.Parse(MaxPolyPower3.Text);
                        int countOfTasks3 = int.Parse(CountOfTasks3.Text);
                        bool containsLog3 = checkBoxLog3.Checked;
                        bool containsRadical3 = checkBoxRadical3.Checked;
                        bool containsPowerFunc3 = checkBoxPowerFunc3.Checked;
                        bool containsRMT3 = checkBoxRMT3.Checked;
                        bool oneFrac3 = checkBoxOneFruc3.Checked;

                        Settings settingsForPart3 = new Settings(countOfRoots3, maxValueRoots3,
                                                                 maxPolyPower3, oneFrac3, containsLog3,
                                                                 containsRadical3, containsRMT3,
                                                                 containsPowerFunc3, answersCheckBox);
                        keys.Add(new GenerationKey(countOfTasks3, seed, settingsForPart3));
                    }
                    else
                    {
                        seed = (keys[1].Seed + 1) % 1000000;
                        Settings settingsForPart3 = new Settings(1, 11, 1, false, false, false, false, false, answersCheckBox);
                        keys.Add(new GenerationKey(0, seed, settingsForPart3));
                    }

                    if (CountOfRoots4.Enabled)
                    {
                        seed = (keys[2].Seed + 1) % 1000000;
                        int countOfRoots4 = int.Parse(CountOfRoots4.Text);
                        int maxValueRoots4 = int.Parse(MaxValueRoots4.Text);
                        int maxPolyPower4 = int.Parse(MaxPolyPower4.Text);
                        int countOfTasks4 = int.Parse(CountOfTasks4.Text);
                        bool containsLog4 = checkBoxLog4.Checked;
                        bool containsRadical4 = checkBoxRadical4.Checked;
                        bool containsPowerFunc4 = checkBoxPowerFunc4.Checked;
                        bool containsRMT4 = checkBoxRMT4.Checked;
                        bool oneFrac4 = checkBoxOneFruc4.Checked;

                        Settings settingsForPart4 = new Settings(countOfRoots4, maxValueRoots4,
                                                                 maxPolyPower4, oneFrac4, containsLog4,
                                                                 containsRadical4, containsRMT4,
                                                                 containsPowerFunc4, answersCheckBox);
                        keys.Add(new GenerationKey(countOfTasks4, seed, settingsForPart4));
                    }
                    else
                    {
                        seed = (keys[2].Seed + 1) % 1000000;
                        Settings settingsForPart4 = new Settings(1, 11, 1, false, false, false, false, false, answersCheckBox);
                        keys.Add(new GenerationKey(0, seed, settingsForPart4));
                    }

                    if (CountOfRoots5.Enabled)
                    {
                        seed = (keys[3].Seed + 1) % 1000000;
                        int countOfRoots5 = int.Parse(CountOfRoots5.Text);
                        int maxValueRoots5 = int.Parse(MaxValueRoots5.Text);
                        int maxPolyPower5 = int.Parse(MaxPolyPower5.Text);
                        int countOfTasks5 = int.Parse(CountOfTasks5.Text);
                        bool containsLog5 = checkBoxLog5.Checked;
                        bool containsRadical5 = checkBoxRadical5.Checked;
                        bool containsPowerFunc5 = checkBoxPowerFunc5.Checked;
                        bool containsRMT5 = checkBoxRMT5.Checked;
                        bool oneFrac5 = checkBoxOneFruc5.Checked;

                        Settings settingsForPart5 = new Settings(countOfRoots5, maxValueRoots5,
                                                                 maxPolyPower5, oneFrac5, containsLog5,
                                                                 containsRadical5, containsRMT5,
                                                                 containsPowerFunc5, answersCheckBox);
                        keys.Add(new GenerationKey(countOfTasks5, seed, settingsForPart5));
                    }
                    else
                    {
                        seed = (keys[3].Seed + 1) % 1000000;
                        Settings settingsForPart5 = new Settings(1, 11, 1, false, false, false, false, false, answersCheckBox);
                        keys.Add(new GenerationKey(0, seed, settingsForPart5));
                    }

                    int countOfTests = int.Parse(CountOfTests.Text);
                    GenerationKeyForTest keyForTest = new GenerationKeyForTest(keys, countOfTests);

                    Test test = new Test(keyForTest);
                    test.CreateTestDirectory(directoryName);
                    MessageTextBox.Text = $"Генерация прошла успешно. Проверьте выбранную вами папку. Ключ Генерации: {keyForTest}";
                    MessageTextBox.ForeColor = Color.FromArgb(39, 174, 96);
                }

            }
        }

        private bool CheckСorrectnessInput()
        {
            int num;
            bool isCorrect = true;
            if (!int.TryParse(CountOfRoots1.Text, out num) || num < 1 || num > 9)
            {
                CountOfRoots1.BackColor = incorrectColor;
                isCorrect = false;
            }
            if (CountOfTasks2.Enabled && (!int.TryParse(CountOfRoots2.Text, out num) || num < 1 || num > 9))
            {
                CountOfRoots2.BackColor = incorrectColor;
                isCorrect = false;
            }
            if (CountOfTasks3.Enabled && (!int.TryParse(CountOfRoots3.Text, out num) || num < 1 || num > 9))
            {
                CountOfRoots3.BackColor = incorrectColor;
                isCorrect = false;
            }
            if (CountOfTasks4.Enabled && (!int.TryParse(CountOfRoots4.Text, out num) || num < 1 || num > 9))
            {
                CountOfRoots4.BackColor = incorrectColor;
                isCorrect = false;
            }
            if (CountOfTasks5.Enabled && (!int.TryParse(CountOfRoots5.Text, out num) || num < 1 || num > 9))
            {
                CountOfRoots5.BackColor = incorrectColor;
                isCorrect = false;
            }

            if (!int.TryParse(MaxValueRoots1.Text, out num) || num < 1 || num > 20)
            {
                MaxValueRoots1.BackColor = incorrectColor;
                isCorrect = false;
            }
            if (MaxValueRoots2.Enabled && (!int.TryParse(MaxValueRoots2.Text, out num) || num < 1 || num > 20))
            {
                MaxValueRoots2.BackColor = incorrectColor;
                isCorrect = false;
            }
            if (MaxValueRoots3.Enabled && (!int.TryParse(MaxValueRoots3.Text, out num) || num < 1 || num > 20))
            {
                MaxValueRoots3.BackColor = incorrectColor;
                isCorrect = false;
            }
            if (MaxValueRoots4.Enabled && (!int.TryParse(MaxValueRoots4.Text, out num) || num < 1 || num > 20))
            {
                MaxValueRoots4.BackColor = incorrectColor;
                isCorrect = false;
            }
            if (MaxValueRoots5.Enabled && (!int.TryParse(MaxValueRoots5.Text, out num) || num < 1 || num > 20))
            {
                MaxValueRoots5.BackColor = incorrectColor;
                isCorrect = false;
            }

            if (!int.TryParse(MaxPolyPower1.Text, out num) || num < 1 || num > 5)
            {
                MaxPolyPower1.BackColor = incorrectColor;
                isCorrect = false;
            }
            if (MaxPolyPower2.Enabled && (!int.TryParse(MaxPolyPower2.Text, out num) || num < 1 || num > 5))
            {
                MaxPolyPower2.BackColor = incorrectColor;
                isCorrect = false;
            }
            if (MaxPolyPower3.Enabled && (!int.TryParse(MaxPolyPower3.Text, out num) || num < 1 || num > 5))
            {
                MaxPolyPower3.BackColor = incorrectColor;
                isCorrect = false;
            }
            if (MaxPolyPower4.Enabled && (!int.TryParse(MaxPolyPower4.Text, out num) || num < 1 || num > 5))
            {
                MaxPolyPower4.BackColor = incorrectColor;
                isCorrect = false;
            }
            if (MaxPolyPower5.Enabled && (!int.TryParse(MaxPolyPower5.Text, out num) || num < 1 || num > 5))
            {
                MaxPolyPower5.BackColor = incorrectColor;
                isCorrect = false;
            }

            if (!int.TryParse(CountOfTasks1.Text, out num) || num < 1 || num > 99)
            {
                CountOfTasks1.BackColor = incorrectColor;
                isCorrect = false;
            }
            if (CountOfTasks2.Enabled && (!int.TryParse(CountOfTasks2.Text, out num) || num < 1 || num > 99))
            {
                CountOfTasks2.BackColor = incorrectColor;
                isCorrect = false;
            }
            if (CountOfTasks3.Enabled && (!int.TryParse(CountOfTasks3.Text, out num) || num < 1 || num > 99))
            {
                CountOfTasks3.BackColor = incorrectColor;
                isCorrect = false;
            }
            if (CountOfTasks4.Enabled && (!int.TryParse(CountOfTasks4.Text, out num) || num < 1 || num > 99))
            {
                CountOfTasks4.BackColor = incorrectColor;
                isCorrect = false;
            }
            if (CountOfTasks5.Enabled && (!int.TryParse(CountOfTasks5.Text, out num) || num < 1 || num > 99))
            {
                CountOfTasks5.BackColor = incorrectColor;
                isCorrect = false;
            }

            if (!int.TryParse(CountOfTests.Text, out num) || num < 1 || num > 9999)
            {
                CountOfTests.BackColor = incorrectColor;
                isCorrect = false;
            }

            return isCorrect;
        }

        private void AddSecondPart_Click(object sender, EventArgs e)
        {
            CountOfRoots2.BackColor = white;
            MaxValueRoots2.BackColor = white;
            MaxPolyPower2.BackColor = white;
            CountOfTasks2.BackColor = white;

            if (!CountOfRoots2.Enabled)
            {
                AddSecondPart.BackColor = Color.FromArgb(52, 152, 219);
                AddSecondPart.ForeColor = white;
                AddSecondPart.FlatAppearance.BorderColor = Color.FromArgb(41, 128, 185);
                label2.ForeColor = Color.FromArgb(52, 152, 219);

                CountOfRoots2.Enabled = true;
                MaxValueRoots2.Enabled = true;
                MaxPolyPower2.Enabled = true;
                CountOfTasks2.Enabled = true;
                checkBoxLog2.Enabled = true;
                checkBoxOneFruc2.Enabled = true;
                checkBoxPowerFunc2.Enabled = true;
                checkBoxRadical2.Enabled = true;
                checkBoxRMT2.Enabled = true;

                CountOfRoots2.Text = "Количество Корней";
                MaxValueRoots2.Text = "Максимальный Корень";
                MaxPolyPower2.Text = "Максимальная Степень";
                CountOfTasks2.Text = "Количество Неравенств";
                CountOfRoots2.ForeColor = notActiveTextColor;
                MaxValueRoots2.ForeColor = notActiveTextColor;
                MaxPolyPower2.ForeColor = notActiveTextColor;
                CountOfTasks2.ForeColor = notActiveTextColor;
            }
            else
            {
                AddSecondPart.BackColor = white;
                AddSecondPart.ForeColor = Color.FromArgb(0, 0, 0);
                AddSecondPart.FlatAppearance.BorderColor = Color.FromArgb(0, 0, 0);
                label2.ForeColor = notActiveTextColor;

                CountOfRoots2.Enabled = false;
                MaxValueRoots2.Enabled = false;
                MaxPolyPower2.Enabled = false;
                CountOfTasks2.Enabled = false;
                checkBoxLog2.Enabled = false;
                checkBoxOneFruc2.Enabled = false;
                checkBoxPowerFunc2.Enabled = false;
                checkBoxRadical2.Enabled = false;
                checkBoxRMT2.Enabled = false;

                CountOfRoots2.Text = "";
                MaxValueRoots2.Text = "";
                MaxPolyPower2.Text = "";
                CountOfTasks2.Text = "";
                checkBoxLog2.Checked = false;
                checkBoxOneFruc2.Checked = false;
                checkBoxPowerFunc2.Checked = false;
                checkBoxRadical2.Checked = false;
                checkBoxRMT2.Checked = false;

                if (CountOfRoots3.Enabled)
                {
                    AddThirdPart_Click(sender, e);
                }
            }
        }

        private void AddThirdPart_Click(object sender, EventArgs e)
        {

            CountOfRoots3.BackColor = white;
            MaxValueRoots3.BackColor = white;
            MaxPolyPower3.BackColor = white;
            CountOfTasks3.BackColor = white;

            if (!CountOfRoots3.Enabled)
            {
                if (!CountOfRoots2.Enabled)
                {
                    AddSecondPart_Click(sender, e);
                };

                AddThirdPart.BackColor = Color.FromArgb(52, 152, 219);
                AddFifthPart.ForeColor = white;
                AddThirdPart.FlatAppearance.BorderColor = Color.FromArgb(41, 128, 185);
                label3.ForeColor = Color.FromArgb(52, 152, 219);

                CountOfRoots3.Enabled = true;
                MaxValueRoots3.Enabled = true;
                MaxPolyPower3.Enabled = true;
                CountOfTasks3.Enabled = true;
                checkBoxLog3.Enabled = true;
                checkBoxOneFruc3.Enabled = true;
                checkBoxPowerFunc3.Enabled = true;
                checkBoxRadical3.Enabled = true;
                checkBoxRMT3.Enabled = true;

                CountOfRoots3.Text = "Количество Корней";
                MaxValueRoots3.Text = "Максимальный Корень";
                MaxPolyPower3.Text = "Максимальная Степень";
                CountOfTasks3.Text = "Количество Неравенств";
                CountOfRoots3.ForeColor = notActiveTextColor;
                MaxValueRoots3.ForeColor = notActiveTextColor;
                MaxPolyPower3.ForeColor = notActiveTextColor;
                CountOfTasks3.ForeColor = notActiveTextColor;
            }
            else
            {
                AddThirdPart.BackColor = white;
                AddThirdPart.ForeColor = Color.FromArgb(0, 0, 0);
                AddThirdPart.FlatAppearance.BorderColor = Color.FromArgb(0, 0, 0);
                label3.ForeColor = notActiveTextColor;

                CountOfRoots3.Enabled = false;
                MaxValueRoots3.Enabled = false;
                MaxPolyPower3.Enabled = false;
                CountOfTasks3.Enabled = false;
                checkBoxLog3.Enabled = false;
                checkBoxOneFruc3.Enabled = false;
                checkBoxPowerFunc3.Enabled = false;
                checkBoxRadical3.Enabled = false;
                checkBoxRMT3.Enabled = false;

                CountOfRoots3.Text = "";
                MaxValueRoots3.Text = "";
                MaxPolyPower3.Text = "";
                CountOfTasks3.Text = "";
                checkBoxLog3.Checked = false;
                checkBoxOneFruc3.Checked = false;
                checkBoxPowerFunc3.Checked = false;
                checkBoxRadical3.Checked = false;
                checkBoxRMT3.Checked = false;

                if (CountOfRoots4.Enabled)
                {
                    AddForthPart_Click(sender, e);
                }
            }
        }

        private void AddForthPart_Click(object sender, EventArgs e)
        {
            CountOfRoots4.BackColor = white;
            MaxValueRoots4.BackColor = white;
            MaxPolyPower4.BackColor = white;
            CountOfTasks4.BackColor = white;

            if (!CountOfRoots4.Enabled)
            {
                if (!CountOfRoots3.Enabled)
                {
                    AddThirdPart_Click(sender, e);
                };

                AddForthPart.BackColor = Color.FromArgb(52, 152, 219);
                AddForthPart.ForeColor = white;
                AddForthPart.FlatAppearance.BorderColor = Color.FromArgb(41, 128, 185);
                label4.ForeColor = Color.FromArgb(52, 152, 219);

                CountOfRoots4.Enabled = true;
                MaxValueRoots4.Enabled = true;
                MaxPolyPower4.Enabled = true;
                CountOfTasks4.Enabled = true;
                checkBoxLog4.Enabled = true;
                checkBoxOneFruc4.Enabled = true;
                checkBoxPowerFunc4.Enabled = true;
                checkBoxRadical4.Enabled = true;
                checkBoxRMT4.Enabled = true;

                CountOfRoots4.Text = "Количество Корней";
                MaxValueRoots4.Text = "Максимальный Корень";
                MaxPolyPower4.Text = "Максимальная Степень";
                CountOfTasks4.Text = "Количество Неравенств";
                CountOfRoots4.ForeColor = notActiveTextColor;
                MaxValueRoots4.ForeColor = notActiveTextColor;
                MaxPolyPower4.ForeColor = notActiveTextColor;
                CountOfTasks4.ForeColor = notActiveTextColor;
            }
            else
            {
                AddForthPart.BackColor = white;
                AddForthPart.ForeColor = Color.FromArgb(0, 0, 0);
                AddForthPart.FlatAppearance.BorderColor = Color.FromArgb(0, 0, 0);
                label4.ForeColor = notActiveTextColor;

                CountOfRoots4.Enabled = false;
                MaxValueRoots4.Enabled = false;
                MaxPolyPower4.Enabled = false;
                CountOfTasks4.Enabled = false;
                checkBoxLog4.Enabled = false;
                checkBoxOneFruc4.Enabled = false;
                checkBoxPowerFunc4.Enabled = false;
                checkBoxRadical4.Enabled = false;
                checkBoxRMT4.Enabled = false;

                CountOfRoots4.Text = "";
                MaxValueRoots4.Text = "";
                MaxPolyPower4.Text = "";
                CountOfTasks4.Text = "";
                checkBoxLog4.Checked = false;
                checkBoxOneFruc4.Checked = false;
                checkBoxPowerFunc4.Checked = false;
                checkBoxRadical4.Checked = false;
                checkBoxRMT4.Checked = false;

                if (CountOfRoots5.Enabled)
                {
                    AddFifthPart_Click(sender, e);
                }

            }
        }

        private void AddFifthPart_Click(object sender, EventArgs e)
        {

            CountOfRoots5.BackColor = white;
            MaxValueRoots5.BackColor = white;
            MaxPolyPower5.BackColor = white;
            CountOfTasks5.BackColor = white;

            if (!CountOfRoots5.Enabled)
            {
                if (!CountOfRoots4.Enabled)
                {
                    AddForthPart_Click(sender, e);
                };

                AddFifthPart.BackColor = Color.FromArgb(52, 152, 219);
                AddFifthPart.ForeColor = white;
                AddFifthPart.FlatAppearance.BorderColor = Color.FromArgb(41, 128, 185);
                label5.ForeColor = Color.FromArgb(52, 152, 219);

                CountOfRoots5.Enabled = true;
                MaxValueRoots5.Enabled = true;
                MaxPolyPower5.Enabled = true;
                CountOfTasks5.Enabled = true;
                checkBoxLog5.Enabled = true;
                checkBoxOneFruc5.Enabled = true;
                checkBoxPowerFunc5.Enabled = true;
                checkBoxRadical5.Enabled = true;
                checkBoxRMT5.Enabled = true;

                CountOfRoots5.Text = "Количество Корней";
                MaxValueRoots5.Text = "Максимальный Корень";
                MaxPolyPower5.Text = "Максимальная Степень";
                CountOfTasks5.Text = "Количество Неравенств";
                CountOfRoots5.ForeColor = notActiveTextColor;
                MaxValueRoots5.ForeColor = notActiveTextColor;
                MaxPolyPower5.ForeColor = notActiveTextColor;
                CountOfTasks5.ForeColor = notActiveTextColor;
            }
            else
            {
                AddFifthPart.BackColor = white;
                AddFifthPart.ForeColor = Color.FromArgb(0, 0, 0);
                AddFifthPart.FlatAppearance.BorderColor = Color.FromArgb(0, 0, 0);
                label5.ForeColor = notActiveTextColor;

                CountOfRoots5.Enabled = false;
                MaxValueRoots5.Enabled = false;
                MaxPolyPower5.Enabled = false;
                CountOfTasks5.Enabled = false;
                checkBoxLog5.Enabled = false;
                checkBoxOneFruc5.Enabled = false;
                checkBoxPowerFunc5.Enabled = false;
                checkBoxRadical5.Enabled = false;
                checkBoxRMT5.Enabled = false;

                CountOfRoots5.Text = "";
                MaxValueRoots5.Text = "";
                MaxPolyPower5.Text = "";
                CountOfTasks5.Text = "";
                checkBoxLog5.Checked = false;
                checkBoxOneFruc5.Checked = false;
                checkBoxPowerFunc5.Checked = false;
                checkBoxRadical5.Checked = false;
                checkBoxRMT5.Checked = false;
            }
        }

        private void CountOfRoots1_TextChanged(object sender, EventArgs e)
        {
            CountOfRoots1.BackColor = white;
        }

        private void MaxValueRoots1_TextChanged(object sender, EventArgs e)
        {
            MaxValueRoots1.BackColor = white;
        }

        private void MaxPolyPower1_TextChanged(object sender, EventArgs e)
        {
            MaxPolyPower1.BackColor = white;
        }

        private void CountOfTasks1_TextChanged(object sender, EventArgs e)
        {
            CountOfTasks1.BackColor = white;
        }

        private void CountOfRoots2_TextChanged(object sender, EventArgs e)
        {
            CountOfRoots2.BackColor = white;
        }

        private void CountOfRoots3_TextChanged(object sender, EventArgs e)
        {
            CountOfRoots3.BackColor = white;
        }

        private void CountOfRoots4_TextChanged(object sender, EventArgs e)
        {
            CountOfRoots4.BackColor = white;
        }

        private void CountOfRoots5_TextChanged(object sender, EventArgs e)
        {
            CountOfRoots5.BackColor = white;
        }

        private void MaxValueRoots2_TextChanged(object sender, EventArgs e)
        {
            MaxValueRoots2.BackColor = white;
        }

        private void MaxValueRoots3_TextChanged(object sender, EventArgs e)
        {
            MaxValueRoots3.BackColor = white;
        }

        private void MaxValueRoots4_TextChanged(object sender, EventArgs e)
        {
            MaxValueRoots4.BackColor = white;
        }

        private void MaxValueRoots5_TextChanged(object sender, EventArgs e)
        {
            MaxValueRoots5.BackColor = white;
        }

        private void MaxPolyPower2_TextChanged(object sender, EventArgs e)
        {
            MaxPolyPower2.BackColor = white;
        }

        private void MaxPolyPower3_TextChanged(object sender, EventArgs e)
        {
            MaxPolyPower3.BackColor = white;
        }

        private void MaxPolyPower4_TextChanged(object sender, EventArgs e)
        {
            MaxPolyPower4.BackColor = white;
        }

        private void MaxPolyPower5_TextChanged(object sender, EventArgs e)
        {
            MaxPolyPower5.BackColor = white;
        }

        private void CountOfTasks2_TextChanged(object sender, EventArgs e)
        {
            CountOfTasks2.BackColor = white;
        }

        private void CountOfTasks3_TextChanged(object sender, EventArgs e)
        {
            CountOfTasks3.BackColor = white;
        }

        private void CountOfTasks4_TextChanged(object sender, EventArgs e)
        {
            CountOfTasks4.BackColor = white;
        }

        private void CountOfTasks5_TextChanged(object sender, EventArgs e)
        {
            CountOfTasks5.BackColor = white;
        }

        private void LoadBtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            FBD.ShowNewFolderButton = false;
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                directoryName = FBD.SelectedPath;
                label21.Text = FBD.SelectedPath;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormForKeyGeneration form = new FormForKeyGeneration();
            form.Show();
        }

        private void CountOfRoots1_Enter(object sender, EventArgs e)
        {
            if (CountOfRoots1.Text == "Количество Корней")
            {
                CountOfRoots1.Text = "";
                CountOfRoots1.ForeColor = activeTextColor;
            }
        }

        private void CountOfRoots1_Leave(object sender, EventArgs e)
        {
            if (CountOfRoots1.Text == "")
            {
                CountOfRoots1.Text = "Количество Корней";
                CountOfRoots1.ForeColor = notActiveTextColor;
            }
        }

        private void MaxValueRoots1_Enter(object sender, EventArgs e)
        {
            if (MaxValueRoots1.Text == "Максимальный Корень")
            {
                MaxValueRoots1.Text = "";
                MaxValueRoots1.ForeColor = activeTextColor;
            }
        }

        private void MaxValueRoots1_Leave(object sender, EventArgs e)
        {
            if (MaxValueRoots1.Text == "")
            {
                MaxValueRoots1.Text = "Максимальный Корень";
                MaxValueRoots1.ForeColor = notActiveTextColor;
            }
        }

        private void MaxPolyPower1_Enter(object sender, EventArgs e)
        {
            if (MaxPolyPower1.Text == "Максимальная Степень")
            {
                MaxPolyPower1.Text = "";
                MaxPolyPower1.ForeColor = activeTextColor;
            }
        }

        private void MaxPolyPower1_Leave(object sender, EventArgs e)
        {
            if (MaxPolyPower1.Text == "")
            {
                MaxPolyPower1.Text = "Максимальная Степень";
                MaxPolyPower1.ForeColor = notActiveTextColor;
            }
        }

        private void CountOfTasks1_Enter(object sender, EventArgs e)
        {
            if (CountOfTasks1.Text == "Количество Неравенств")
            {
                CountOfTasks1.Text = "";
                CountOfTasks1.ForeColor = activeTextColor;
            }
        }

        private void CountOfTasks1_Leave(object sender, EventArgs e)
        {
            if (CountOfTasks1.Text == "")
            {
                CountOfTasks1.Text = "Количество Неравенств";
                CountOfTasks1.ForeColor = notActiveTextColor;
            }
        }

        private void CountOfRoots2_Enter(object sender, EventArgs e)
        {
            if (CountOfRoots2.Text == "Количество Корней")
            {
                CountOfRoots2.Text = "";
                CountOfRoots2.ForeColor = activeTextColor;
            }
        }

        private void CountOfRoots2_Leave(object sender, EventArgs e)
        {
            if (CountOfRoots2.Text == "")
            {
                CountOfRoots2.Text = "Количество Корней";
                CountOfRoots2.ForeColor = notActiveTextColor;
            }
        }

        private void MaxValueRoots2_Enter(object sender, EventArgs e)
        {
            if (MaxValueRoots2.Text == "Максимальный Корень")
            {
                MaxValueRoots2.Text = "";
                MaxValueRoots2.ForeColor = activeTextColor;
            }
        }

        private void MaxValueRoots2_Leave(object sender, EventArgs e)
        {
            if (MaxValueRoots2.Text == "")
            {
                MaxValueRoots2.Text = "Максимальный Корень";
                MaxValueRoots2.ForeColor = notActiveTextColor;
            }
        }

        private void MaxPolyPower2_Enter(object sender, EventArgs e)
        {
            if (MaxPolyPower2.Text == "Максимальная Степень")
            {
                MaxPolyPower2.Text = "";
                MaxPolyPower2.ForeColor = activeTextColor;
            }
        }

        private void MaxPolyPower2_Leave(object sender, EventArgs e)
        {
            if (MaxPolyPower2.Text == "")
            {
                MaxPolyPower2.Text = "Максимальная Степень";
                MaxPolyPower2.ForeColor = notActiveTextColor;
            }
        }

        private void CountOfTasks2_Enter(object sender, EventArgs e)
        {
            if (CountOfTasks2.Text == "Количество Неравенств")
            {
                CountOfTasks2.Text = "";
                CountOfTasks2.ForeColor = activeTextColor;
            }
        }

        private void CountOfTasks2_Leave(object sender, EventArgs e)
        {
            if (CountOfTasks2.Text == "")
            {
                CountOfTasks2.Text = "Количество Неравенств";
                CountOfTasks2.ForeColor = notActiveTextColor;
            }
        }

        private void CountOfRoots3_Enter(object sender, EventArgs e)
        {
            if (CountOfRoots3.Text == "Количество Корней")
            {
                CountOfRoots3.Text = "";
                CountOfRoots3.ForeColor = activeTextColor;
            }
        }

        private void CountOfRoots3_Leave(object sender, EventArgs e)
        {
            if (CountOfRoots3.Text == "")
            {
                CountOfRoots3.Text = "Количество Корней";
                CountOfRoots3.ForeColor = notActiveTextColor;
            }
        }

        private void MaxValueRoots3_Enter(object sender, EventArgs e)
        {
            if (MaxValueRoots3.Text == "Максимальный Корень")
            {
                MaxValueRoots3.Text = "";
                MaxValueRoots3.ForeColor = activeTextColor;
            }
        }

        private void MaxValueRoots3_Leave(object sender, EventArgs e)
        {
            if (MaxValueRoots3.Text == "")
            {
                MaxValueRoots3.Text = "Максимальный Корень";
                MaxValueRoots3.ForeColor = notActiveTextColor;
            }
        }

        private void MaxPolyPower3_Enter(object sender, EventArgs e)
        {
            if (MaxPolyPower3.Text == "Максимальная Степень")
            {
                MaxPolyPower3.Text = "";
                MaxPolyPower3.ForeColor = activeTextColor;
            }
        }

        private void MaxPolyPower3_Leave(object sender, EventArgs e)
        {
            if (MaxPolyPower3.Text == "")
            {
                MaxPolyPower3.Text = "Максимальная Степень";
                MaxPolyPower3.ForeColor = notActiveTextColor;
            }
        }

        private void CountOfTasks3_Enter(object sender, EventArgs e)
        {
            if (CountOfTasks3.Text == "Количество Неравенств")
            {
                CountOfTasks3.Text = "";
                CountOfTasks3.ForeColor = activeTextColor;
            }
        }

        private void CountOfTasks3_Leave(object sender, EventArgs e)
        {
            if (CountOfTasks3.Text == "")
            {
                CountOfTasks3.Text = "Количество Неравенств";
                CountOfTasks3.ForeColor = notActiveTextColor;
            }
        }

        private void CountOfRoots4_Enter(object sender, EventArgs e)
        {
            if (CountOfRoots4.Text == "Количество Корней")
            {
                CountOfRoots4.Text = "";
                CountOfRoots4.ForeColor = activeTextColor;
            }
        }

        private void CountOfRoots4_Leave(object sender, EventArgs e)
        {
            if (CountOfRoots4.Text == "")
            {
                CountOfRoots4.Text = "Количество Корней";
                CountOfRoots4.ForeColor = notActiveTextColor;
            }
        }

        private void MaxValueRoots4_Enter(object sender, EventArgs e)
        {
            if (MaxValueRoots4.Text == "Максимальный Корень")
            {
                MaxValueRoots4.Text = "";
                MaxValueRoots4.ForeColor = activeTextColor;
            }
        }

        private void MaxValueRoots4_Leave(object sender, EventArgs e)
        {
            if (MaxValueRoots4.Text == "")
            {
                MaxValueRoots4.Text = "Максимальный Корень";
                MaxValueRoots4.ForeColor = notActiveTextColor;
            }
        }

        private void MaxPolyPower4_Enter(object sender, EventArgs e)
        {
            if (MaxPolyPower4.Text == "Максимальная Степень")
            {
                MaxPolyPower4.Text = "";
                MaxPolyPower4.ForeColor = activeTextColor;
            }
        }

        private void MaxPolyPower4_Leave(object sender, EventArgs e)
        {
            if (MaxPolyPower4.Text == "")
            {
                MaxPolyPower4.Text = "Максимальная Степень";
                MaxPolyPower4.ForeColor = notActiveTextColor;
            }
        }

        private void CountOfTasks4_Enter(object sender, EventArgs e)
        {
            if (CountOfTasks4.Text == "Количество Неравенств")
            {
                CountOfTasks4.Text = "";
                CountOfTasks4.ForeColor = activeTextColor;
            }
        }

        private void CountOfTasks4_Leave(object sender, EventArgs e)
        {
            if (CountOfTasks4.Text == "")
            {
                CountOfTasks4.Text = "Количество Неравенств";
                CountOfTasks4.ForeColor = notActiveTextColor;
            }
        }

        private void CountOfRoots5_Enter(object sender, EventArgs e)
        {
            if (CountOfRoots5.Text == "Количество Корней")
            {
                CountOfRoots5.Text = "";
                CountOfRoots5.ForeColor = activeTextColor;
            }
        }

        private void CountOfRoots5_Leave(object sender, EventArgs e)
        {
            if (CountOfRoots5.Text == "")
            {
                CountOfRoots5.Text = "Количество Корней";
                CountOfRoots5.ForeColor = notActiveTextColor;
            }
        }

        private void MaxValueRoots5_Enter(object sender, EventArgs e)
        {
            if (MaxValueRoots5.Text == "Максимальный Корень")
            {
                MaxValueRoots5.Text = "";
                MaxValueRoots5.ForeColor = activeTextColor;
            }
        }

        private void MaxValueRoots5_Leave(object sender, EventArgs e)
        {
            if (MaxValueRoots5.Text == "")
            {
                MaxValueRoots5.Text = "Максимальный Корень";
                MaxValueRoots5.ForeColor = notActiveTextColor;
            }
        }

        private void MaxPolyPower5_Enter(object sender, EventArgs e)
        {
            if (MaxPolyPower5.Text == "Максимальная Степень")
            {
                MaxPolyPower5.Text = "";
                MaxPolyPower5.ForeColor = activeTextColor;
            }
        }

        private void MaxPolyPower5_Leave(object sender, EventArgs e)
        {
            if (MaxPolyPower5.Text == "")
            {
                MaxPolyPower5.Text = "Максимальная Степень";
                MaxPolyPower5.ForeColor = notActiveTextColor;
            }
        }

        private void CountOfTasks5_Enter(object sender, EventArgs e)
        {
            if (CountOfTasks5.Text == "Количество Неравенств")
            {
                CountOfTasks5.Text = "";
                CountOfTasks5.ForeColor = activeTextColor;
            }
        }

        private void CountOfTasks5_Leave(object sender, EventArgs e)
        {
            if (CountOfTasks5.Text == "")
            {
                CountOfTasks5.Text = "Количество Неравенств";
                CountOfTasks5.ForeColor = notActiveTextColor;
            }
        }

        private void AddSecondPart_MouseEnter(object sender, EventArgs e)
        {
            if (!CountOfRoots2.Enabled)
            {
                AddSecondPart.BackColor = Color.FromArgb(52, 152, 219); 
            }
            else
            {
                AddSecondPart.FlatAppearance.BorderColor = Color.FromArgb(41, 128, 185);
            }
        }

        private void AddSecondPart_MouseLeave(object sender, EventArgs e)
        {
            if (!CountOfRoots2.Enabled)
            {
                AddSecondPart.BackColor = btnColor; 
            }
            else
            {
                AddSecondPart.FlatAppearance.BorderColor = Color.FromArgb(0, 0, 0);
            }
        }

        private void AddThirdPart_MouseEnter(object sender, EventArgs e)
        {
            if (!CountOfRoots3.Enabled)
            {
                AddThirdPart.BackColor = Color.FromArgb(52, 152, 219); 
            }
            else
            {
                AddThirdPart.FlatAppearance.BorderColor = Color.FromArgb(41, 128, 185);
            }
        }

        private void AddThirdPart_MouseLeave(object sender, EventArgs e)
        {
            if (!CountOfRoots3.Enabled)
            {
                AddThirdPart.BackColor = btnColor; 
            }
            else
            {
                AddThirdPart.FlatAppearance.BorderColor = Color.FromArgb(0, 0, 0);
            }
        }

        private void AddForthPart_MouseEnter(object sender, EventArgs e)
        {
            if (!CountOfRoots4.Enabled)
            {
                AddForthPart.BackColor = Color.FromArgb(52, 152, 219); 
            }
            else
            {
                AddForthPart.FlatAppearance.BorderColor = Color.FromArgb(41, 128, 185);
            }
        }

        private void AddForthPart_MouseLeave(object sender, EventArgs e)
        {
            if (!CountOfRoots4.Enabled)
            {
                AddForthPart.BackColor = btnColor; 
            }
            else
            {
                AddForthPart.FlatAppearance.BorderColor = Color.FromArgb(0, 0, 0);
            }
        }

        private void AddFifthPart_MouseEnter(object sender, EventArgs e)
        {
            if (!CountOfRoots5.Enabled)
            {
                AddFifthPart.BackColor = Color.FromArgb(52, 152, 219); 
            }
            else
            {
                AddFifthPart.FlatAppearance.BorderColor = Color.FromArgb(41, 128, 185);
            }
        }

        private void AddFifthPart_MouseLeave(object sender, EventArgs e)
        {
            if (!CountOfRoots5.Enabled)
            {
                AddFifthPart.BackColor = btnColor; 
            }
            else
            {
                AddFifthPart.FlatAppearance.BorderColor = Color.FromArgb(0, 0, 0);
            }
        }

        private void CountOfTests_Enter(object sender, EventArgs e)
        {
            if (CountOfTests.Text == "Количество Вариантов")
            {
                CountOfTests.Text = "";
                CountOfTests.ForeColor = activeTextColor;
            }
        }

        private void CountOfTests_Leave(object sender, EventArgs e)
        {
            if (CountOfTests.Text == "")
            {
                CountOfTests.Text = "Количество Вариантов";
                CountOfTests.ForeColor = notActiveTextColor;
            }
        }

        private void GenerateBtn_MouseEnter(object sender, EventArgs e)
        {
            GenerateBtn.BackColor = Color.FromArgb(76, 209, 55);
        }

        private void GenerateBtn_MouseLeave(object sender, EventArgs e)
        {
            GenerateBtn.BackColor = Color.FromArgb(39, 174, 96);
        }

        private void CountOfTests_TextChanged(object sender, EventArgs e)
        {
            CountOfTests.BackColor = white;
        }
    }
}
