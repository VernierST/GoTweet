ï»¿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Twitterizer;

namespace GoTweet
{
    public partial class GoTweet : Form
    {
        NGIOController ngio;
        TwitterController twitter;
        PreferencesController prefs;

        float measurement = 0.0f;
        string name = "";
        string units = "";


        public GoTweet()
        {
            InitializeComponent();
            ngio = new NGIOController();

            twitter = TwitterController.GetSharedController();

            prefs = PreferencesController.GetSharedController();
            twitter.Tokens = prefs.tokens;
        }

        private void updateReadings(object sender, EventArgs e)
        {
            startDelay.Enabled = false;
            readingsTimer.Enabled = true;

            measurement = ngio.getData();
            reading.Text = String.Format("{0:0.##}", measurement);
            unit.Text = ngio.getUnits();
            nameLabel.Text = ngio.getShortName();

            if (!prefs.timedTweets) //a.k.a - we are tweeting at a threshold
            {
                switch (prefs.tweetWhen)
                {
                    case  1: if (measurement > prefs.tweetWhen) tweet(); break;
                    case  0: if (measurement == prefs.tweetWhen) tweet(); break;
                    case -1: if (measurement < prefs.tweetWhen) tweet(); break;
                }
            }
        }

        private void openPrefrences(object sender, EventArgs e)
        {
            Authorize authorize = new Authorize();
            authorize.Show();
        }

        private void closeApplication(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tweetTime(object sender, EventArgs e)
        {
            if (prefs.timedTweets)
            {
                tweet();
            }
        }

        public string formatMessage(string message)
        {
            message = message.Replace("<value>", measurement.ToString());
            message = message.Replace("<name>", name);
            message = message.Replace("<unit>", units);

            return message;
        }
        public void tweet()
        {
            twitter.updateStatus(formatMessage(prefs.message));
        }

    }
}
