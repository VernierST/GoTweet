ï»¿namespace GoTweet
{
    /// <summary>
    /// The main window of the GoTweet application where status updates and sensor readings
    /// can be viewed
    /// </summary>
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
            vst.uninit();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GoTweet));
            this.panel1 = new System.Windows.Forms.Panel();
            this.nameLabel = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.supportText = new System.Windows.Forms.Label();
            this.supportLink = new System.Windows.Forms.LinkLabel();
            this.unit = new System.Windows.Forms.Label();
            this.reading = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.date = new System.Windows.Forms.Label();
            this.accountStatusLabel = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tweetNowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForNewDevicesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.readingsTimer = new System.Windows.Forms.Timer(this.components);
            this.tweetTimer = new System.Windows.Forms.Timer(this.components);
            this.statusRotationTimer = new System.Windows.Forms.Timer(this.components);
            this.findSensors = new System.Windows.Forms.Timer(this.components);
            this.startDelay = new System.Windows.Forms.Timer(this.components);
            this.internet = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
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
            this.panel1.Size = new System.Drawing.Size(551, 371);
            this.panel1.TabIndex = 0;
            // 
            // nameLabel
            // 
            this.nameLabel.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameLabel.Location = new System.Drawing.Point(3, 67);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(541, 23);
            this.nameLabel.TabIndex = 2;
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(218)))), ((int)(((byte)(224)))));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.supportText);
            this.panel3.Controls.Add(this.supportLink);
            this.panel3.Controls.Add(this.unit);
            this.panel3.Controls.Add(this.reading);
            this.panel3.Location = new System.Drawing.Point(24, 103);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(496, 166);
            this.panel3.TabIndex = 1;
            // 
            // supportText
            // 
            this.supportText.Location = new System.Drawing.Point(6, 9);
            this.supportText.Name = "supportText";
            this.supportText.Size = new System.Drawing.Size(483, 23);
            this.supportText.TabIndex = 3;
            this.supportText.Text = "label1";
            this.supportText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // supportLink
            // 
            this.supportLink.Location = new System.Drawing.Point(3, 32);
            this.supportLink.Name = "supportLink";
            this.supportLink.Size = new System.Drawing.Size(486, 38);
            this.supportLink.TabIndex = 2;
            this.supportLink.TabStop = true;
            this.supportLink.Text = "linkLabel1";
            this.supportLink.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.supportLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.supportLink_LinkClicked);
            // 
            // unit
            // 
            this.unit.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.unit.Location = new System.Drawing.Point(16, 119);
            this.unit.Name = "unit";
            this.unit.Size = new System.Drawing.Size(473, 33);
            this.unit.TabIndex = 1;
            this.unit.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // reading
            // 
            this.reading.Font = new System.Drawing.Font("Arial", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reading.Location = new System.Drawing.Point(3, 32);
            this.reading.Name = "reading";
            this.reading.Size = new System.Drawing.Size(486, 86);
            this.reading.TabIndex = 0;
            this.reading.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Controls.Add(this.date);
            this.panel2.Controls.Add(this.accountStatusLabel);
            this.panel2.Location = new System.Drawing.Point(0, 291);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(547, 75);
            this.panel2.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(7, -16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(140, 103);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.openPrefrences);
            // 
            // date
            // 
            this.date.BackColor = System.Drawing.Color.Transparent;
            this.date.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.date.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.date.Location = new System.Drawing.Point(279, 58);
            this.date.Name = "date";
            this.date.Size = new System.Drawing.Size(265, 22);
            this.date.TabIndex = 2;
            this.date.Text = "Jul 8, 2010 8:42 AM";
            this.date.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // accountStatusLabel
            // 
            this.accountStatusLabel.BackColor = System.Drawing.Color.Transparent;
            this.accountStatusLabel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountStatusLabel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.accountStatusLabel.Location = new System.Drawing.Point(190, 6);
            this.accountStatusLabel.Name = "accountStatusLabel";
            this.accountStatusLabel.Size = new System.Drawing.Size(354, 57);
            this.accountStatusLabel.TabIndex = 0;
            this.accountStatusLabel.Text = "This is a really, really, really, really, close to one hundred and forty characte" +
                "rs which is the limit of a twitter message and is very clos";
            this.accountStatusLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.accountStatusLabel.TextChanged += new System.EventHandler(this.accountStatusLabel_TextChanged);
            this.accountStatusLabel.Resize += new System.EventHandler(this.accountStatusLabel_Resize);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(551, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tweetNowToolStripMenuItem,
            this.checkForNewDevicesToolStripMenuItem,
            this.preToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // tweetNowToolStripMenuItem
            // 
            this.tweetNowToolStripMenuItem.Name = "tweetNowToolStripMenuItem";
            this.tweetNowToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.tweetNowToolStripMenuItem.Text = "Tweet Now!";
            this.tweetNowToolStripMenuItem.Click += new System.EventHandler(this.tweetNowToolStripMenuItem_Click);
            // 
            // checkForNewDevicesToolStripMenuItem
            // 
            this.checkForNewDevicesToolStripMenuItem.Name = "checkForNewDevicesToolStripMenuItem";
            this.checkForNewDevicesToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.checkForNewDevicesToolStripMenuItem.Text = "Search for Devices";
            this.checkForNewDevicesToolStripMenuItem.Click += new System.EventHandler(this.checkForNewDevicesToolStripMenuItem_Click);
            // 
            // preToolStripMenuItem
            // 
            this.preToolStripMenuItem.Name = "preToolStripMenuItem";
            this.preToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.preToolStripMenuItem.Text = "Options...";
            this.preToolStripMenuItem.Click += new System.EventHandler(this.openPrefrences);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.closeApplication);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem1,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            this.helpToolStripMenuItem1.Size = new System.Drawing.Size(107, 22);
            this.helpToolStripMenuItem1.Text = "Help";
            this.helpToolStripMenuItem1.Click += new System.EventHandler(this.helpToolStripMenuItem1_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // readingsTimer
            // 
            this.readingsTimer.Enabled = true;
            this.readingsTimer.Interval = 1000;
            this.readingsTimer.Tick += new System.EventHandler(this.updateReadings);
            // 
            // tweetTimer
            // 
            this.tweetTimer.Interval = 60000;
            this.tweetTimer.Tick += new System.EventHandler(this.tweetTime);
            // 
            // statusRotationTimer
            // 
            this.statusRotationTimer.Enabled = true;
            this.statusRotationTimer.Interval = 5000;
            this.statusRotationTimer.Tick += new System.EventHandler(this.statusRotationTimer_Tick);
            // 
            // findSensors
            // 
            this.findSensors.Enabled = true;
            this.findSensors.Interval = 5000;
            this.findSensors.Tick += new System.EventHandler(this.findSensors_Tick);
            // 
            // startDelay
            // 
            this.startDelay.Interval = 2000;
            this.startDelay.Tick += new System.EventHandler(this.startDelay_Tick);
            // 
            // internet
            // 
            this.internet.Enabled = true;
            this.internet.Interval = 1500;
            this.internet.Tick += new System.EventHandler(this.internet_Tick);
            // 
            // GoTweet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(225)))), ((int)(((byte)(234)))));
            this.ClientSize = new System.Drawing.Size(547, 366);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "GoTweet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GoTweet";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
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
        private System.Windows.Forms.Timer tweetTimer;
        private System.Windows.Forms.Label accountStatusLabel;
        private System.Windows.Forms.Timer statusRotationTimer;
        private System.Windows.Forms.ToolStripMenuItem tweetNowToolStripMenuItem;
        private System.Windows.Forms.Timer findSensors;
        private System.Windows.Forms.Label date;
        private System.Windows.Forms.Timer startDelay;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.LinkLabel supportLink;
        private System.Windows.Forms.Label supportText;
        private System.Windows.Forms.ToolStripMenuItem checkForNewDevicesToolStripMenuItem;
        private System.Windows.Forms.Timer internet;

    }
}

