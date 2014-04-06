ï»¿namespace GoTweet
{
    partial class Authorize
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
            this.browser = new System.Windows.Forms.WebBrowser();
            this.pinNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.confirm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // browser
            // 
            this.browser.Location = new System.Drawing.Point(0, 49);
            this.browser.MinimumSize = new System.Drawing.Size(20, 20);
            this.browser.Name = "browser";
            this.browser.ScrollBarsEnabled = false;
            this.browser.Size = new System.Drawing.Size(787, 421);
            this.browser.TabIndex = 0;
            // 
            // pinNumber
            // 
            this.pinNumber.Location = new System.Drawing.Point(59, 15);
            this.pinNumber.Name = "pinNumber";
            this.pinNumber.Size = new System.Drawing.Size(612, 20);
            this.pinNumber.TabIndex = 1;
            this.pinNumber.Text = "Enter your PIN number here";
            this.pinNumber.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pinNumber_MouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "PIN:";
            // 
            // confirm
            // 
            this.confirm.Location = new System.Drawing.Point(677, 12);
            this.confirm.Name = "confirm";
            this.confirm.Size = new System.Drawing.Size(100, 23);
            this.confirm.TabIndex = 3;
            this.confirm.Text = "Confirm";
            this.confirm.UseVisualStyleBackColor = true;
            this.confirm.Click += new System.EventHandler(this.confirm_Click);
            // 
            // Authorize
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 470);
            this.Controls.Add(this.confirm);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pinNumber);
            this.Controls.Add(this.browser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Authorize";
            this.ShowInTaskbar = false;
            this.Text = "Authorize";
            this.VisibleChanged += new System.EventHandler(this.Authorize_VisibleChanged);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Authorize_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser browser;
        private System.Windows.Forms.TextBox pinNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button confirm;
    }
}