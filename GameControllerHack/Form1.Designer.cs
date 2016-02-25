namespace GameControllerHack
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.outputComboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ModeComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pianoControl1 = new Sanford.Multimedia.Midi.UI.PianoControl();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.chordComboBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.inputComboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.LShoulder = new System.Windows.Forms.Label();
            this.RShoulder = new System.Windows.Forms.Label();
            this.RT = new System.Windows.Forms.Label();
            this.LT = new System.Windows.Forms.Label();
            this.B1 = new System.Windows.Forms.Label();
            this.B3 = new System.Windows.Forms.Label();
            this.B4 = new System.Windows.Forms.Label();
            this.B2 = new System.Windows.Forms.Label();
            this.DUp = new System.Windows.Forms.Label();
            this.DDown = new System.Windows.Forms.Label();
            this.DLeft = new System.Windows.Forms.Label();
            this.DRight = new System.Windows.Forms.Label();
            this.RStickX = new System.Windows.Forms.Label();
            this.LStickY = new System.Windows.Forms.Label();
            this.RStickY = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.Black;
            this.richTextBox1.ForeColor = System.Drawing.Color.LimeGreen;
            this.richTextBox1.Location = new System.Drawing.Point(13, 13);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(120, 446);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // outputComboBox1
            // 
            this.outputComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.outputComboBox1.FormattingEnabled = true;
            this.outputComboBox1.Location = new System.Drawing.Point(146, 35);
            this.outputComboBox1.Name = "outputComboBox1";
            this.outputComboBox1.Size = new System.Drawing.Size(162, 21);
            this.outputComboBox1.TabIndex = 1;
            this.outputComboBox1.SelectedIndexChanged += new System.EventHandler(this.outputComboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(143, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "MIDI Output";
            // 
            // ModeComboBox
            // 
            this.ModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ModeComboBox.FormattingEnabled = true;
            this.ModeComboBox.Location = new System.Drawing.Point(602, 35);
            this.ModeComboBox.Name = "ModeComboBox";
            this.ModeComboBox.Size = new System.Drawing.Size(177, 21);
            this.ModeComboBox.TabIndex = 4;
            this.ModeComboBox.SelectedIndexChanged += new System.EventHandler(this.ModeComboBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(599, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Scale";
            // 
            // pianoControl1
            // 
            this.pianoControl1.HighNoteID = 109;
            this.pianoControl1.Location = new System.Drawing.Point(31, 465);
            this.pianoControl1.LowNoteID = 21;
            this.pianoControl1.Name = "pianoControl1";
            this.pianoControl1.NoteOnColor = System.Drawing.Color.DimGray;
            this.pianoControl1.Size = new System.Drawing.Size(833, 158);
            this.pianoControl1.TabIndex = 6;
            this.pianoControl1.TabStop = false;
            this.pianoControl1.Text = "pianoControl1";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(475, 35);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 7;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(472, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Root";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(822, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Click!";
            // 
            // chordComboBox
            // 
            this.chordComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.chordComboBox.Enabled = false;
            this.chordComboBox.FormattingEnabled = true;
            this.chordComboBox.Location = new System.Drawing.Point(785, 35);
            this.chordComboBox.Name = "chordComboBox";
            this.chordComboBox.Size = new System.Drawing.Size(79, 21);
            this.chordComboBox.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(782, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Chord";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(785, 68);
            this.textBox1.MaxLength = 3;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(28, 20);
            this.textBox1.TabIndex = 12;
            this.textBox1.Text = "120";
            this.textBox1.Visible = false;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(147, 75);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "label7";
            // 
            // inputComboBox1
            // 
            this.inputComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.inputComboBox1.FormattingEnabled = true;
            this.inputComboBox1.Location = new System.Drawing.Point(314, 35);
            this.inputComboBox1.Name = "inputComboBox1";
            this.inputComboBox1.Size = new System.Drawing.Size(155, 21);
            this.inputComboBox1.TabIndex = 14;
            this.inputComboBox1.SelectedIndexChanged += new System.EventHandler(this.inputComboBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(311, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "MIDI Input";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.pictureBox1.Location = new System.Drawing.Point(139, 126);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(705, 288);
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // LShoulder
            // 
            this.LShoulder.AutoSize = true;
            this.LShoulder.Location = new System.Drawing.Point(272, 144);
            this.LShoulder.Name = "LShoulder";
            this.LShoulder.Size = new System.Drawing.Size(43, 13);
            this.LShoulder.TabIndex = 17;
            this.LShoulder.Text = "Ascend";
            this.LShoulder.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // RShoulder
            // 
            this.RShoulder.AutoSize = true;
            this.RShoulder.Location = new System.Drawing.Point(677, 144);
            this.RShoulder.Name = "RShoulder";
            this.RShoulder.Size = new System.Drawing.Size(43, 13);
            this.RShoulder.TabIndex = 17;
            this.RShoulder.Text = "Ascend";
            // 
            // RT
            // 
            this.RT.AutoSize = true;
            this.RT.Location = new System.Drawing.Point(677, 175);
            this.RT.Name = "RT";
            this.RT.Size = new System.Drawing.Size(45, 13);
            this.RT.TabIndex = 17;
            this.RT.Text = "Decend";
            // 
            // LT
            // 
            this.LT.AutoSize = true;
            this.LT.Location = new System.Drawing.Point(270, 175);
            this.LT.Name = "LT";
            this.LT.Size = new System.Drawing.Size(45, 13);
            this.LT.TabIndex = 17;
            this.LT.Text = "Decend";
            this.LT.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // B1
            // 
            this.B1.AutoSize = true;
            this.B1.Location = new System.Drawing.Point(754, 205);
            this.B1.Name = "B1";
            this.B1.Size = new System.Drawing.Size(22, 13);
            this.B1.TabIndex = 17;
            this.B1.Text = "7th";
            // 
            // B3
            // 
            this.B3.AutoSize = true;
            this.B3.Location = new System.Drawing.Point(754, 275);
            this.B3.Name = "B3";
            this.B3.Size = new System.Drawing.Size(30, 13);
            this.B3.TabIndex = 17;
            this.B3.Text = "Root";
            // 
            // B4
            // 
            this.B4.AutoSize = true;
            this.B4.Location = new System.Drawing.Point(715, 237);
            this.B4.Name = "B4";
            this.B4.Size = new System.Drawing.Size(22, 13);
            this.B4.TabIndex = 17;
            this.B4.Text = "5th";
            // 
            // B2
            // 
            this.B2.AutoSize = true;
            this.B2.Location = new System.Drawing.Point(799, 237);
            this.B2.Name = "B2";
            this.B2.Size = new System.Drawing.Size(22, 13);
            this.B2.TabIndex = 17;
            this.B2.Text = "3rd";
            // 
            // DUp
            // 
            this.DUp.AutoSize = true;
            this.DUp.Location = new System.Drawing.Point(246, 205);
            this.DUp.Name = "DUp";
            this.DUp.Size = new System.Drawing.Size(59, 13);
            this.DUp.TabIndex = 17;
            this.DUp.Text = "Octave Up";
            // 
            // DDown
            // 
            this.DDown.AutoSize = true;
            this.DDown.Location = new System.Drawing.Point(246, 275);
            this.DDown.Name = "DDown";
            this.DDown.Size = new System.Drawing.Size(73, 13);
            this.DDown.TabIndex = 17;
            this.DDown.Text = "Octave Down";
            this.DDown.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // DLeft
            // 
            this.DLeft.AutoSize = true;
            this.DLeft.Location = new System.Drawing.Point(192, 237);
            this.DLeft.Name = "DLeft";
            this.DLeft.Size = new System.Drawing.Size(81, 13);
            this.DLeft.TabIndex = 17;
            this.DLeft.Text = "ChromaticMode";
            // 
            // DRight
            // 
            this.DRight.AutoSize = true;
            this.DRight.Location = new System.Drawing.Point(291, 237);
            this.DRight.Name = "DRight";
            this.DRight.Size = new System.Drawing.Size(44, 13);
            this.DRight.TabIndex = 17;
            this.DRight.Text = "Nothing";
            // 
            // RStickX
            // 
            this.RStickX.AutoSize = true;
            this.RStickX.Location = new System.Drawing.Point(715, 361);
            this.RStickX.Name = "RStickX";
            this.RStickX.Size = new System.Drawing.Size(44, 13);
            this.RStickX.TabIndex = 17;
            this.RStickX.Text = "X=Pitch";
            // 
            // LStickY
            // 
            this.LStickY.AutoSize = true;
            this.LStickY.Location = new System.Drawing.Point(207, 388);
            this.LStickY.Name = "LStickY";
            this.LStickY.Size = new System.Drawing.Size(119, 13);
            this.LStickY.TabIndex = 17;
            this.LStickY.Text = " Y=Modify Button Notes";
            this.LStickY.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // RStickY
            // 
            this.RStickY.AutoSize = true;
            this.RStickY.Location = new System.Drawing.Point(715, 388);
            this.RStickY.Name = "RStickY";
            this.RStickY.Size = new System.Drawing.Size(41, 13);
            this.RStickY.TabIndex = 17;
            this.RStickY.Text = "Y=Mod";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(181, 361);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(145, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "X=Button Note Above/Below";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(887, 635);
            this.Controls.Add(this.LT);
            this.Controls.Add(this.DRight);
            this.Controls.Add(this.B2);
            this.Controls.Add(this.DLeft);
            this.Controls.Add(this.B4);
            this.Controls.Add(this.DDown);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.LStickY);
            this.Controls.Add(this.RStickY);
            this.Controls.Add(this.RStickX);
            this.Controls.Add(this.B3);
            this.Controls.Add(this.DUp);
            this.Controls.Add(this.B1);
            this.Controls.Add(this.RT);
            this.Controls.Add(this.RShoulder);
            this.Controls.Add(this.LShoulder);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.inputComboBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.chordComboBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.pianoControl1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ModeComboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.outputComboBox1);
            this.Controls.Add(this.richTextBox1);
            this.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Shred Willard";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ComboBox outputComboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ModeComboBox;
        private System.Windows.Forms.Label label3;
        private Sanford.Multimedia.Midi.UI.PianoControl pianoControl1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox chordComboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox inputComboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label LShoulder;
        private System.Windows.Forms.Label RShoulder;
        private System.Windows.Forms.Label RT;
        private System.Windows.Forms.Label LT;
        private System.Windows.Forms.Label B1;
        private System.Windows.Forms.Label B3;
        private System.Windows.Forms.Label B4;
        private System.Windows.Forms.Label B2;
        private System.Windows.Forms.Label DUp;
        private System.Windows.Forms.Label DDown;
        private System.Windows.Forms.Label DLeft;
        private System.Windows.Forms.Label DRight;
        private System.Windows.Forms.Label RStickX;
        private System.Windows.Forms.Label LStickY;
        private System.Windows.Forms.Label RStickY;
        private System.Windows.Forms.Label label8;
    }
}

