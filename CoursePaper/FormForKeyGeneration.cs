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
    public partial class FormForKeyGeneration : Form
    {
        string directoryName = string.Empty;

        public FormForKeyGeneration()
        {
            InitializeComponent();
        }

        private void Generate_Click(object sender, EventArgs e)
        {
            if (directoryName != string.Empty)
            {
                GenerationKeyForTest key = null;
                try
                {
                    string strKey = keyBox.Text;
                    if (checkBox1.Checked)
                    {
                        string[] values = strKey.Split('.');
                        values[10] = "1";
                        strKey = string.Join(".", values);
                    }
                    key = new GenerationKeyForTest(strKey);
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


                if (key != null)
                {
                    Test test = new Test(key);
                    test.CreateTestDirectory(directoryName);
                } 
            }
            else
            {
                MessageBox.Show("Выберите папку в которую будут сохроняться файлы с вариантами", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
    }
}
