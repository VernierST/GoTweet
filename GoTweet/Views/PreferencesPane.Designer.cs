ï»¿namespace GoTweet
{
    /// <summary>
    /// A dialog where preferences can be altered
    /// </summary>
    partial class PreferencesPane
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreferencesPane));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.thresholdBox = new System.Windows.Forms.NumericUpDown();
            this.minutesBox = new System.Windows.Forms.NumericUpDown();
            this.tweetWhenBox = new System.Windows.Forms.ComboBox();
            this.tweetAtThresholdBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.authorizeButton = new System.Windows.Forms.Button();
            this.twitterStatusLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tweetText = new System.Windows.Forms.TextBox();
            this.disclaimerLabel = new System.Windows.Forms.Label();
            this.doneButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thresholdBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minutesBox)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox1.Controls.Add(this.thresholdBox);
            this.groupBox1.Controls.Add(this.minutesBox);
            this.groupBox1.Controls.Add(this.tweetWhenBox);
            this.groupBox1.Controls.Add(this.tweetAtThresholdBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(23, 67);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(401, 82);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // thresholdBox
            // 
            this.thresholdBox.Location = new System.Drawing.Point(273, 44);
            this.thresholdBox.Name = "thresholdBox";
            this.thresholdBox.Size = new System.Drawing.Size(88, 20);
            this.thresholdBox.TabIndex = 5;
            // 
            // minutesBox
            // 
            this.minutesBox.Location = new System.Drawing.Point(172, 18);
            this.minutesBox.Name = "minutesBox";
            this.minutesBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.minutesBox.Size = new System.Drawing.Size(44, 20);
            this.minutesBox.TabIndex = 2;
            // 
            // tweetWhenBox
            // 
            this.tweetWhenBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tweetWhenBox.FormattingEnabled = true;
            this.tweetWhenBox.Items.AddRange(new object[] {
            "above",
            "equal to",
            "below"});
            this.tweetWhenBox.Location = new System.Drawing.Point(172, 44);
            this.tweetWhenBox.Name = "tweetWhenBox";
            this.tweetWhenBox.Size = new System.Drawing.Size(94, 21);
            this.tweetWhenBox.TabIndex = 4;
            // 
            // tweetAtThresholdBox
            // 
            this.tweetAtThresholdBox.AutoSize = true;
            this.tweetAtThresholdBox.Location = new System.Drawing.Point(22, 49);
            this.tweetAtThresholdBox.Name = "tweetAtThresholdBox";
            this.tweetAtThresholdBox.Size = new System.Drawing.Size(144, 17);
            this.tweetAtThresholdBox.TabIndex = 3;
            this.tweetAtThresholdBox.Text = "Only tweet when value is";
            this.tweetAtThresholdBox.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(223, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "minutes";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(96, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tweet Every:";
            // 
            // authorizeButton
            // 
            this.authorizeButton.Location = new System.Drawing.Point(301, 21);
            this.authorizeButton.Name = "authorizeButton";
            this.authorizeButton.Size = new System.Drawing.Size(123, 23);
            this.authorizeButton.TabIndex = 2;
            this.authorizeButton.Text = "Link Go!Tweet";
            this.authorizeButton.UseVisualStyleBackColor = true;
            this.authorizeButton.Click += new System.EventHandler(this.authorizeButton_Click);
            // 
            // twitterStatusLabel
            // 
            this.twitterStatusLabel.AutoSize = true;
            this.twitterStatusLabel.Location = new System.Drawing.Point(20, 26);
            this.twitterStatusLabel.Name = "twitterStatusLabel";
            this.twitterStatusLabel.Size = new System.Drawing.Size(231, 13);
            this.twitterStatusLabel.TabIndex = 2;
            this.twitterStatusLabel.Text = "Go!Tweet is not yet linked to a Twitter account.";
            this.twitterStatusLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 171);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Tweet Text:";
            // 
            // tweetText
            // 
            this.tweetText.Location = new System.Drawing.Point(92, 171);
            this.tweetText.Multiline = true;
            this.tweetText.Name = "tweetText";
            this.tweetText.Size = new System.Drawing.Size(332, 89);
            this.tweetText.TabIndex = 6;
            // 
            // disclaimerLabel
            // 
            this.disclaimerLabel.AutoSize = true;
            this.disclaimerLabel.Location = new System.Drawing.Point(92, 267);
            this.disclaimerLabel.Name = "disclaimerLabel";
            this.disclaimerLabel.Size = new System.Drawing.Size(53, 13);
            this.disclaimerLabel.TabIndex = 5;
            this.disclaimerLabel.Text = "disclaimer";
            // 
            // doneButton
            // 
            this.doneButton.Location = new System.Drawing.Point(300, 375);
            this.doneButton.Name = "doneButton";
            this.doneButton.Size = new System.Drawing.Size(124, 23);
            this.doneButton.TabIndex = 7;
            this.doneButton.Text = "Done";
            this.doneButton.UseMnemonic = false;
            this.doneButton.UseVisualStyleBackColor = true;
            this.doneButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // PreferencesPane
            // 
            this.AcceptButton = this.doneButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 421);
            this.Controls.Add(this.doneButton);
            this.Controls.Add(this.disclaimerLabel);
            this.Controls.Add(this.tweetText);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.twitterStatusLabel);
            this.Controls.Add(this.authorizeButton);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PreferencesPane";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Options";
            this.VisibleChanged += new System.EventHandler(this.PreferencesPane_VisibleChanged);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PreferencesPane_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thresholdBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minutesBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox tweetAtThresholdBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button authorizeButton;
        private System.Windows.Forms.Label twitterStatusLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tweetText;
        private System.Windows.Forms.Label disclaimerLabel;
        private System.Windows.Forms.Button doneButton;
        private System.Windows.Forms.ComboBox tweetWhenBox;
        private System.Windows.Forms.NumericUpDown minutesBox;
        private System.Windows.Forms.NumericUpDown thresholdBox;
    }
}