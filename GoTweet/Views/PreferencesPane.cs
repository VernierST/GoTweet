ï»¿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GoTweet
{
    public partial class PreferencesPane : Form
    {
        private static PreferencesPane preferencesPane;
        private PreferencesController prefs;

        private TwitterController twitter;

        private GoTweet main;

        private bool allGood = true;

        private PreferencesPane()
        {
            InitializeComponent();
            disclaimerLabel.Text =
                "This text will be used when posting tweets.The placeholders " +
                "<name>, \n<value>, and <units> will be replaced by the actual information " +
                "from the \nconnected sensor. Remember Twitter limits updates to 140 characters " +
                "\nand each placeholder may represent more characters than its name.";

            prefs = PreferencesController.GetSharedController();
            twitter = TwitterController.GetSharedController();
            updateInterface();

            minutesBox.Maximum = Int32.MaxValue;
            minutesBox.Minimum = 0;

            thresholdBox.Maximum = Int32.MaxValue;
            thresholdBox.Minimum = Int32.MinValue;
                
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void authorizeButton_Click(object sender, EventArgs e)
        {
            GoTweet.UpdateInternetConnectionStatus();
            if (GoTweet.internetWorking == false)
            {
                MessageBox.Show("Unable to connect to internet. Please check connection");
            }
            else new LoginPrompt().ShowDialog();
            updateInterface();
        }

        /// <summary>
        /// Static method for getting the singleton preferences pane object
        /// </summary>
        /// <param name="app">The parent GoTweet window</param>
        /// <returns>A PreferencesPane singleton object</returns>
        public static PreferencesPane GetPreferencesPane(GoTweet app)
        {
            if (preferencesPane == null) preferencesPane = new PreferencesPane();
            preferencesPane.main = app;
            return preferencesPane;
        }

        /// <summary>
        /// Updates the dialog according to the user's preferences
        /// </summary>
        public void updateInterface()
        {
            minutesBox.Text = prefs.minutes.ToString();
            tweetAtThresholdBox.Checked = prefs.tweetAtThreshold;
            thresholdBox.Text = prefs.threshold.ToString();
            tweetText.Text = prefs.message;

            string twitterAuthorizationStatus = "";
            if (prefs.authorized)
            {
                twitterAuthorizationStatus = "Go!Tweet is linked to the account "+twitter.accessTokens.ScreenName;
                authorizeButton.Enabled = false;
            }
            else
            {
                twitterAuthorizationStatus = "Go!Tweet is not yet linked to a Twitter account.";
                authorizeButton.Enabled = true;
            }
            twitterStatusLabel.Text = twitterAuthorizationStatus;

            string tweetWhenText = "";
            switch (prefs.tweetWhen)
            {
                case 1: tweetWhenText = "above"; break;
                case 0: tweetWhenText = "equal to"; break;
                case -1: tweetWhenText = "below"; break;
                default: tweetWhenText = ""; break;
            }
            tweetWhenBox.Text = tweetWhenText;
        }

        private void savePreferences()
        {
            try
            {
                prefs.minutes = Int32.Parse(minutesBox.Text);
                minutesBox.BackColor = Color.White;
                System.Threading.Thread.Sleep(250); //theatrics
                prefs.tweetAtThreshold = tweetAtThresholdBox.Checked;
            }
            catch (FormatException e)
            {
                minutesBox.BackColor = Color.Red;
                allGood = false;
                return;
            }
            try
            {
                prefs.threshold = Int32.Parse(thresholdBox.Text);
                thresholdBox.BackColor = Color.White;
                System.Threading.Thread.Sleep(250);
            }
            catch (FormatException e)
            {
                thresholdBox.BackColor = Color.Red;
                allGood = false;
                return;
            }
            allGood = true;
            prefs.message = tweetText.Text;
            prefs.authorized = !twitterStatusLabel.Text.Contains("not"); //roundabout way of doing it

            string text = tweetWhenBox.Text;
            if (text.Equals("above")) prefs.tweetWhen = 1;
            else if (text.Equals("below")) prefs.tweetWhen = -1;
            else if (text.Equals("equal to")) prefs.tweetWhen = 0;

            prefs.Save();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            savePreferences();
            main.startTweeting();
            this.Close();
        }

        private void PreferencesPane_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            if(allGood) this.Hide();
        }

        private void PreferencesPane_VisibleChanged(object sender, EventArgs e)
        {
            if(this.Visible) updateInterface();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(twitter.GetAuthorizationURL());
        }
    }
}
