ï»¿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoTweet
{
    public partial class Authorize : Form
    {
        TwitterController twitter;
        public Authorize()
        {
            InitializeComponent();
            twitter = TwitterController.GetSharedController();
            browser.Navigate(twitter.GetAuthorizationURL());

        }

        private void confirm_Click(object sender, EventArgs e)
        {
            int response = twitter.ConnectWithPIN(pinNumber.Text);
            if (response != 0)
            {
                MessageBox.Show("Unable to authorize Twitter account!", "GoTweet Twitter Authorization");
                this.Close();
            }
            else
            {
                MessageBox.Show("Twitter account authorized!", "GoTweet Twitter Authorization");
                this.Close();
            }

        }
    }
}
