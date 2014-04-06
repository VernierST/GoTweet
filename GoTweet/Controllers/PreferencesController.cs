ï»¿using System;
using System.Collections.Generic;
using System.Text;
using Twitterizer;

namespace GoTweet
{
    /// <summary>
    /// A singleton that relays user preferences to the application and vice-versa
    /// </summary>
    class PreferencesController
    {
        private static PreferencesController sharedController;

        /// <summary>
        /// The OAuth tokens that hold information important for Twitter connection. Note,
        ///  no passwords in here!
        /// </summary>
        public OAuthTokenResponse tokens { set; get;}

        /// <summary>
        /// The option to tweet only when the reading is above/below/or equal to <paramref name="threshold"/>
        /// </summary>
        public bool        tweetAtThreshold { set; get; }

        /// <summary>
        /// How often the user wants to send out a tweet
        /// </summary>
        public int         minutes { set; get; }

        /// <summary>
        /// If <paramref name="tweetAtThreshold"/> is true, the reading is compared to this value before
        ///  tweeting
        /// </summary>
        public double      threshold { set; get; }

        /// <summary>
        /// If <paramref name="tweetAtThreshold"/> is true, this value determines if the reading
        ///  should be:
        /// <list type="bullet">
        ///     <item>1 = above,</item>
        ///     <item>0 = equal to, or</item>
        ///     <item>-1 = below</item>
        /// </list>
        /// to the <paramref name="threshold"/>
        /// </summary>
        public int         tweetWhen { set; get; }
        
        /// <summary>
        /// The text of each tweet to be sent. The following tokens will be replaced with actual info:
        /// <list type="bullet">
        ///     <item>&ltname&gt = the long name of the sensor</item>
        ///     <item>&ltvalue&gt = the reading of the sensor</item>
        ///     <item>&ltunits&gt = the units of the sensor</item>
        /// </list>
        /// </summary>
        public string      message{ set; get; }

        /// <summary>
        /// Whether or not the user has authorized Go!Tweet to connect to their account
        /// </summary>
        public bool        authorized { set; get; }

        /// <summary>
        /// Text of the last tweet sent
        /// </summary>
        public string      lastTweet { set; get; }

        /// <summary>
        /// Time the last tweet was sent
        /// </summary>
        public DateTime    lastTweetTime { set; get; }

        /// <summary>
        /// Whether or not to log NGIO events
        /// </summary>
        public bool logNGIO { set; get; }
        /// <summary>
        /// Whether or not to log Twitter events
        /// </summary>
        public bool logTwitter{set; get; }

        /// <summary>
        /// Loads the preferences
        /// </summary>
        private PreferencesController()
        {
            Load();
        }

        /// <summary>
        /// Singleton instance that encapsulates all the preferences
        /// </summary>
        /// <returns>A single PreferencesController</returns>
        public static PreferencesController GetSharedController(){
            if (sharedController == null)
            {
                sharedController = new PreferencesController();
            }
            return sharedController;
        }

        /// <summary>
        /// Saves instance variables into appropriate settings
        /// </summary>
        public void Save()
        {
            Properties.Settings.Default.accessToken = tokens;
            Properties.Settings.Default.tweetAtThreshold = tweetAtThreshold;
            Properties.Settings.Default.minutes = minutes;
            Properties.Settings.Default.threshold = threshold;
            Properties.Settings.Default.tweetWhen = tweetWhen;
            Properties.Settings.Default.message = message;
            Properties.Settings.Default.authorized = authorized;
            Properties.Settings.Default.lastTweet = lastTweet;
            Properties.Settings.Default.lastTweetTime = lastTweetTime;
            Properties.Settings.Default.logTwitter = logTwitter;
            Properties.Settings.Default.logNGIO = logNGIO;

            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Loads setings into apporpriate instance variables
        /// </summary>
        public void Load()
        {
            this.tokens = Properties.Settings.Default.accessToken;
            this.tweetAtThreshold = Properties.Settings.Default.tweetAtThreshold;
            this.threshold = Properties.Settings.Default.threshold;
            this.minutes = Properties.Settings.Default.minutes;
            this.tweetWhen = Properties.Settings.Default.tweetWhen;
            this.message = Properties.Settings.Default.message;
            this.authorized = Properties.Settings.Default.authorized;
            this.lastTweet = Properties.Settings.Default.lastTweet;
            this.lastTweetTime = Properties.Settings.Default.lastTweetTime;
            this.logNGIO = Properties.Settings.Default.logNGIO;
            this.logTwitter = Properties.Settings.Default.logTwitter;

            if (lastTweetTime.Year == 1) lastTweetTime = DateTime.Now;
        }

        /// <summary>
        /// Reloads the tokens after <see cref="TwitterController"/> changes them
        /// </summary>
        public void updateTokens()
        {
            this.tokens = Properties.Settings.Default.accessToken;
        }

        /// <summary>
        /// Top Secret
        /// </summary>
        public static string consumerKey
        {
            get
            {
                return "xS4OhCpfHLfHm03UqFGlQ"; //hard coding these so they aren't modified
            }
        }

        /// <summary>
        /// Top Secret
        /// </summary>
        public static string consumerSecret
        {
            get
            {
                return "wxOvXmZga3hAqkTvgljsOq5SUdk4e8r7ZdWJFV7m0"; //hard coding these so they aren't modified
            }
        }

    }
}
