ï»¿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twitterizer;

namespace GoTweet
{
    class PreferencesController
    {
        private static PreferencesController sharedController;

        public OAuthTokens tokens { set; get;}
        public bool        timedTweets { set; get; }
        public int         minutes { set; get; }
        public double      threshold { set; get; }
        public int         tweetWhen { set; get; }
        public string      message{ set; get; }

        private PreferencesController()
        {
            Load();
        }

        public static PreferencesController GetSharedController(){
            return sharedController;
        }

        public void Save()
        {
            Properties.Settings.Default.accessToken = tokens;
            Properties.Settings.Default.timedTweets = timedTweets;
            Properties.Settings.Default.minutes = minutes;
            Properties.Settings.Default.threshold = threshold;
            Properties.Settings.Default.tweetWhen = tweetWhen;
            Properties.Settings.Default.message = message;

            Properties.Settings.Default.Save();
        }

        public void Load()
        {
            this.tokens = Properties.Settings.Default.accessToken;
            this.timedTweets = Properties.Settings.Default.timedTweets;
            this.threshold = Properties.Settings.Default.threshold;
            this.minutes = Properties.Settings.Default.minutes;
            this.tweetWhen = Properties.Settings.Default.tweetWhen;
            this.message = Properties.Settings.Default.message;
        }

        public string consumerKey
        {
            get
            {
                return Properties.Settings.Default.consumerKey;
            }
        }

        public string consumerSecret
        {
            get
            {
                return Properties.Settings.Default.consumerSecret;
            }
        }

    }
}
