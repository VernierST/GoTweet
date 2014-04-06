ï»¿namespace GoTweet
{
    partial class GoTweet
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
            ngio.dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.nameLabel = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.unit = new System.Windows.Forms.Label();
            this.reading = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.readingsTimer = new System.Windows.Forms.Timer(this.components);
            this.startDelay = new System.Windows.Forms.Timer(this.components);
            this.tweetTimer = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(225)))), ((int)(((byte)(234)))));
            this.panel1.Controls.Add(this.nameLabel);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.menuStrip1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(547, 366);
            this.panel1.TabIndex = 0;
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Font = new System.Drawing.Font("Nina", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameLabel.Location = new System.Drawing.Point(225, 67);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(59, 23);
            this.nameLabel.TabIndex = 2;
            this.nameLabel.Text = "Name";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(218)))), ((int)(((byte)(224)))));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.unit);
            this.panel3.Controls.Add(this.reading);
            this.panel3.Location = new System.Drawing.Point(24, 103);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(496, 166);
            this.panel3.TabIndex = 1;
            // 
            // unit
            // 
            this.unit.AutoSize = true;
            this.unit.Font = new System.Drawing.Font("Nina", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.unit.Location = new System.Drawing.Point(197, 119);
            this.unit.Name = "unit";
            this.unit.Size = new System.Drawing.Size(74, 33);
            this.unit.TabIndex = 1;
            this.unit.Text = "units";
            this.unit.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // reading
            // 
            this.reading.AutoSize = true;
            this.reading.Font = new System.Drawing.Font("Nina", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reading.Location = new System.Drawing.Point(113, 0);
            this.reading.Name = "reading";
            this.reading.Size = new System.Drawing.Size(263, 77);
            this.reading.TabIndex = 0;
            this.reading.Text = "Reading";
            this.reading.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.Location = new System.Drawing.Point(0, 291);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(547, 75);
            this.panel2.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(547, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.preToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // preToolStripMenuItem
            // 
            this.preToolStripMenuItem.Name = "preToolStripMenuItem";
            this.preToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.preToolStripMenuItem.Text = "Preferences...";
            this.preToolStripMenuItem.Click += new System.EventHandler(this.openPrefrences);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.closeApplication);
            // 
            // readingsTimer
            // 
            this.readingsTimer.Interval = 1000;
            this.readingsTimer.Tick += new System.EventHandler(this.updateReadings);
            // 
            // startDelay
            // 
            this.startDelay.Enabled = true;
            this.startDelay.Interval = 500;
            this.startDelay.Tick += new System.EventHandler(this.updateReadings);
            // 
            // tweetTimer
            // 
            this.tweetTimer.Interval = 60000;
            this.tweetTimer.Tick += new System.EventHandler(this.tweetTime);
            // 
            // GoTweet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(225)))), ((int)(((byte)(234)))));
            this.ClientSize = new System.Drawing.Size(547, 366);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "GoTweet";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GoTweet";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label reading;
        private System.Windows.Forms.Timer readingsTimer;
        private System.Windows.Forms.Label unit;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem preToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Timer startDelay;
        private System.Windows.Forms.Timer tweetTimer;

    }
}

