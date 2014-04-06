ï»¿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GoTweet
{
    public partial class LoginPrompt : Form
    {
        TwitterController twitter;
        PreferencesController prefs;

        public LoginPrompt()
        {
            InitializeComponent();
            twitter = TwitterController.GetSharedController();
            prefs = PreferencesController.GetSharedController();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (username.Text.Equals("") || password.Text.Equals(""))
            {
                MessageBox.Show("Please complete all fields!");
                return;
            }
            int response = twitter.xAuthConnect(username.Text, password.Text);
            if (response == -1)
            {
                MessageBox.Show("Unable to connect Go!Tweet to your account ", "Go!Tweet Twitter Authorization");
                username.Text = "";
                password.Text = "";
                prefs.authorized = false;
                return;
            }
            else if (response == -403)
            {
                MessageBox.Show("Invalid username or password.");
                password.Text = "";
                prefs.authorized = false;
                return;
            }
            else
            {
                MessageBox.Show("Your account has been successfully connected to Twitter!", "Go!Tweet Twitter Authorization");
                prefs.updateTokens();
                prefs.authorized = true;
                prefs.tokens = twitter.accessTokens;
            }
            prefs.Save();
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
