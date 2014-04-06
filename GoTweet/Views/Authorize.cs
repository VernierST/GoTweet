ï»¿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GoTweet
{
    /// <summary>
    /// The OAuth authorization process window
    /// </summary>
    public partial class Authorize : Form
    {
        TwitterController twitter;
        PreferencesController prefs;
        PreferencesPane pane;

        private static Authorize authorizationWindow;
        private Authorize()
        {
            InitializeComponent();
            twitter = TwitterController.GetSharedController();
            prefs = PreferencesController.GetSharedController();
            browser.Navigate(twitter.GetAuthorizationURL());

        }

        private void confirm_Click(object sender, EventArgs e)
        {
            int response = twitter.ConnectWithPIN(pinNumber.Text.Trim());
            if (response != 0)
            {
                MessageBox.Show("Unable to authorize Twitter account!", "GoTweet Twitter Authorization");
                prefs.authorized = false;
                prefs.Save();
                this.Close();
            }
            else
            {
                MessageBox.Show("Twitter account authorized!", "GoTweet Twitter Authorization");
                prefs.updateTokens();
                prefs.authorized = true;
                prefs.tokens = twitter.accessTokens;
                prefs.Save();
                this.Close();
            }

        }

        /// <summary>
        /// Returns a singleton AuthorizationWindow
        /// </summary>
        /// <param name="pane">The parent PreferencesPane window</param>
        /// <returns>a singleton AuthorizationWindow</returns>
        public static Authorize GetAuthorizationWindow(PreferencesPane pane)
        {
            if (authorizationWindow == null) authorizationWindow = new Authorize();
            authorizationWindow.pane = pane;
            return authorizationWindow;
        }

        private void Authorize_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            pane.updateInterface();
            this.Hide();
        }

        private void Authorize_VisibleChanged(object sender, EventArgs e)
        {
            if(this.Visible) browser.Navigate(twitter.GetAuthorizationURL());
        }

        private void pinNumber_MouseDown(object sender, MouseEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = "";
        }
    }
}
