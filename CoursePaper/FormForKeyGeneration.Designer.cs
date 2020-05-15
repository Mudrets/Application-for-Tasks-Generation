namespace CoursePaper
{
    partial class FormForKeyGeneration
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.keyBox = new System.Windows.Forms.TextBox();
            this.Generate = new System.Windows.Forms.Button();
            this.label21 = new System.Windows.Forms.Label();
            this.LoadBtn = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(143, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Введите ключ генерации";
            // 
            // keyBox
            // 
            this.keyBox.Location = new System.Drawing.Point(43, 138);
            this.keyBox.Name = "keyBox";
            this.keyBox.Size = new System.Drawing.Size(343, 20);
            this.keyBox.TabIndex = 1;
            // 
            // Generate
            // 
            this.Generate.Location = new System.Drawing.Point(146, 438);
            this.Generate.Name = "Generate";
            this.Generate.Size = new System.Drawing.Size(109, 43);
            this.Generate.TabIndex = 2;
            this.Generate.Text = "Генерировать";
            this.Generate.UseVisualStyleBackColor = true;
            this.Generate.Click += new System.EventHandler(this.Generate_Click);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(150, 305);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(0, 13);
            this.label21.TabIndex = 75;
            // 
            // LoadBtn
            // 
            this.LoadBtn.Location = new System.Drawing.Point(146, 321);
            this.LoadBtn.Name = "LoadBtn";
            this.LoadBtn.Size = new System.Drawing.Size(109, 33);
            this.LoadBtn.TabIndex = 74;
            this.LoadBtn.Text = "Загрузить";
            this.LoadBtn.UseVisualStyleBackColor = true;
            this.LoadBtn.Click += new System.EventHandler(this.LoadBtn_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(165, 373);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(64, 17);
            this.checkBox1.TabIndex = 76;
            this.checkBox1.Text = "Ответы";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // FormForKeyGeneration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 533);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.LoadBtn);
            this.Controls.Add(this.Generate);
            this.Controls.Add(this.keyBox);
            this.Controls.Add(this.label1);
            this.Name = "FormForKeyGeneration";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox keyBox;
        private System.Windows.Forms.Button Generate;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Button LoadBtn;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}