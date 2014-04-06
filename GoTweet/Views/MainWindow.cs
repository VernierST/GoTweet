ï»¿#define TRACE

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Twitterizer;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Data.Odbc;
using System.Net.NetworkInformation;



namespace GoTweet
{
    public partial class GoTweet : Form
    {
        #region Instance Variables
        /// <summary>
        /// The application's handle on the NGIO
        /// </summary>
        VSTController vst;
        /// <summary>
        /// The application's handle on Twitter
        /// </summary>
        TwitterController twitter;
        /// <summary>
        /// The application's handle on the user preferences
        /// </summary>
        PreferencesController prefs;

        /// <summary>
        /// The current measurment
        /// </summary>
        float measurement = 0.0f;

        /// <summary>
        /// Indicates if the system is ready to collect data
        /// </summary>
        bool allGood = false; //You're not good until you know everything's working
        public static bool internetWorking = false; //The internets are down

        const int SECONDS_PER_MINUTES = 60;
        const int MILLISECONDS_PER_SECOND = 1000;
        #endregion

        #region Application Lifecycle
        /// <summary>
        /// Initializes all components of the GoTweet window according to preferences
        /// </summary>
        public GoTweet()
        {
            InitializeComponent();
            vst = new VSTController();

            twitter = TwitterController.GetSharedController();

            prefs = PreferencesController.GetSharedController();
            this.searchAndConnectToDevice();
            twitter.accessTokens = prefs.tokens;
            tweetTimer.Interval = prefs.minutes * SECONDS_PER_MINUTES * MILLISECONDS_PER_SECOND;
            updateInterface();
            startDelay.Start(); //wait for everything to initialize, then delay

        }
        /// <summary>
        /// Show the preferences window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openPrefrences(object sender, EventArgs e)
        {
            PreferencesPane.GetPreferencesPane(this).Show();
        }

        /// <summary>
        /// Closes the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeApplication(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// A way to intercept Windows messages to the application
        /// </summary>
        /// <param name="m">The message Windows sent</param>
        protected override void WndProc(ref Message m)
        {
            //DID, I RLY HAVE TO REDEFINE THESE?!?!
            const int WM_DEVICECHANGE = 0x0219;
            // system detected a new device 

            const int DBT_DEVNODES_CHANGED = 0x0007;
            const int DBT_DEVICEREMOVECOMPLETE = 0x8004;

            switch (m.Msg)
            {
                case WM_DEVICECHANGE:
                    int change = m.WParam.ToInt32();
                    if (change == DBT_DEVNODES_CHANGED)
                    {
                        allGood = false;
                        try
                        {
                            if (vst.DeviceOpen) vst.closeDevice();
                            searchAndConnectToDevice();
                        }
                        catch (Exception e)
                        {
                        }
                    }

                    break;
            }
            base.WndProc(ref m);
        }


        #endregion

        #region Twitter
        /// <summary>
        /// Replaces tokens in a message with their appropriate values 
        /// </summary>
        /// <param name="message">The message to format</param>
        /// <returns>A formatted message</returns>
        public string formatMessage(string message)
        {
            String value = String.Format("{0:F" + vst.DecimalPlaces + "}", measurement);
            message = message.Replace("<value>", value);
            message = message.Replace("<name>", vst.LongName);
            message = message.Replace("<units>", vst.Units);

            return message;
        }

        /// <summary>
        /// Attempts to tweet a message, handling any possible error conditions
        /// </summary>
        private void tweet()
        {
            UpdateInternetConnectionStatus();
            if (prefs.authorized && vst.DeviceOpen && vst.SensorConnected && internetWorking)
            {
                string message = formatMessage(prefs.message);
                //Controllers.PachubeController.updatePachube(measurement.ToString());
                RequestStatus status = twitter.updateStatus(message);
                RequestResult result = status.Status;
                if (result != RequestResult.Success) //little confusing naming scheme
                {
                    //MessageBox.Show(status.ToString());
                    if (result == RequestResult.Unauthorized)
                    {
                        prefs.authorized = false;
                        prefs.Save();
                    }
                    if (result == RequestResult.TwitterIsDown || result == RequestResult.TwitterIsOverloaded)
                    {
                        MessageBox.Show("Unable to post tweet. Twitter is down or overloaded");
                    }
                    else if (result == RequestResult.RateLimited)
                    {
                        //MessageBox.Show("Unable to tweet. Please try again later.");
                        if (status.ResponseBody.Contains("Status is over 140 characters."))
                            MessageBox.Show("The status " + message + " is over 140 characters. Please shorten it down!");
                        else if (status.ResponseBody.Contains("Status is a duplicate."))
                            MessageBox.Show("You've tweeted this message recently.");
                        else
                            MessageBox.Show(status.ResponseBody);
                    }
                    prefs.Save();
                    return;
                }
                prefs.lastTweet = message;
                prefs.lastTweetTime = DateTime.Now;
                prefs.Save();
                updateInterface();
            }

        }

        /// <summary>
        /// Starts the tweet timer
        /// </summary>
        public void startTweeting()
        {
            if (prefs.authorized && vst.DeviceOpen)
            {
                if(shouldTweet()) tweet();
                twitter.accessTokens = prefs.tokens;
                tweetTimer.Stop();
                tweetTimer.Interval = prefs.minutes * SECONDS_PER_MINUTES * MILLISECONDS_PER_SECOND;
                tweetTimer.Start();
            }
        }
        #endregion

        #region Go and NGIO
        /// <summary>
        /// Tells Go or NGIO to find the first device. If it can't,the app indicates so
        /// </summary>
        private void searchAndConnectToDevice()
        {
            int status = -1; //assume the worst
            try
            {
                status = vst.searchForAndOpenDevice();
            }
            catch (Exception e)
            {
                status = -1;
            }
            allGood = false; //prevent any readings from happening while searching
            if (status == -1)
            {
                nameLabel.Font = new Font("Arial", 12, FontStyle.Bold);
                nameLabel.Text = "Please connect a LabQuest, LabQuest Mini, or Go! Device";
                updateInterface();
                reading.Text = "";
                unit.Text = "";
                allGood = false;
                return;
            }

            this.connectToSensor();
        }

        /// <summary>
        /// Tells the NGIO to connect to the first sensor on the first device. If it can't, the
        ///  app indicates so
        /// </summary>
        private void connectToSensor()
        {
            int status = -1; //assume the worst
            try
            {
                status = vst.connectToSensor();
            }
            catch (Exception e)
            {
                status = -1;
            }
            if (status == -1)
            {
                nameLabel.Text = "Plug in a sensor into an analog channel on your device";
                updateInterface();
                reading.Text = "";
                unit.Text = "";
                allGood = false;
                updateInterface();
                return;
            }

            allGood = true;
            updateInterface();
            return;
        }
        #endregion

        #region Menu Item Events
        /// <summary>
        /// Preemptivaley tweets. Doesn't affect the timer at all.
        /// </summary>
        /// <param name="sender">The tool strip item</param>
        /// <param name="e">Event Arguments</param>
        private void tweetNowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tweet();
        }

        /// <summary>
        /// Begins logging any info the NGIO sends
        /// </summary>
        /// <param name="sender">The tool srip item</param>
        /// <param name="e">Event Arguments</param>
        #endregion

        #region Status Label Autosizing
        
        //Code adapted from 
        //http://cyotek.com/article/display/creating-a-windows-forms-label-that-wraps-with-csharp
        /// <summary>
        /// Tries to fit the tweet vertically, but stays constant horizontally
        /// </summary>
        private void fitTheTweet()
        {
            Size size;
            size = accountStatusLabel.GetPreferredSize(new Size(accountStatusLabel.Width,0));
            accountStatusLabel.Height = size.Height;
        }

        /// <summary>
        /// When the label is updated, try to fit it in nicely
        /// </summary>
        /// <param name="sender">The label</param>
        /// <param name="e">The Event Arguments</param>
        private void accountStatusLabel_TextChanged(object sender, EventArgs e)
        {
            fitTheTweet();
        }

        /// <summary>
        /// If the label changes size for some reason, be smart about fitting it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void accountStatusLabel_Resize(object sender, EventArgs e)
        {
            fitTheTweet();
        }

        #endregion Label HouseKeeping Stuff

        #region Timer Events
        /// <summary>
        /// An initial delay after the app starts to get a little data to tweet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startDelay_Tick(object sender, EventArgs e)
        {
            startDelay.Stop();
            tweetTimer.Start();
            tweet();
        }

        /// <summary>
        /// Used to rotate between message and time every so often, but now it just updates to 
        ///  whatever the preferences have in store
        /// </summary>
        /// <param name="sender">The timer</param>
        /// <param name="e">Event arguments</param>
        private void statusRotationTimer_Tick(object sender, EventArgs e)
        {
            //this.updateInterface();
        }

        /// <summary>
        /// Fits the date in the right place, and checks if authorization still exists
        /// </summary>
        private void updateInterface()
        {
            if (prefs.authorized && internetWorking)
            {
                accountStatusLabel.Text = prefs.lastTweet;
                date.Location = new Point(date.Location.X, accountStatusLabel.Location.Y + accountStatusLabel.Size.Height);
                if(!accountStatusLabel.Text.Equals("Go!Tweet hasn't tweeted yet"))
                date.Text =
                    "Tweeted: "+prefs.lastTweetTime.ToString("MMM d, yyyy h:mm tt");
                else date.Text = "";
                //Month abbreviation, 1 or 2 digit day, 4 digit year, 1 or 2 digit hour, 2 digit minute,
                //2 letter AM/PM
            }
            else if (prefs.authorized && !internetWorking)
            {
                accountStatusLabel.Text = "Unable to connect to internet. Check your connection";
                date.Text = "";
                tweetNowToolStripMenuItem.Enabled = false;
            }
            else
            {
                accountStatusLabel.Text = "Click the bird to link Go!Tweet to a Twitter account";
                date.Text = "";
                tweetNowToolStripMenuItem.Enabled = false;
            }

            if (vst.DeviceOpen && prefs.authorized) tweetNowToolStripMenuItem.Enabled = true;
            else tweetNowToolStripMenuItem.Enabled = false;

            if (!vst.DeviceOpen)
            {
                reading.Visible = false;
                supportText.Text = "For help connecting your interface to Go!Tweet, please visit";
                supportLink.Text = "http://www.vernier.com/soft/gotweet/help/troubleshooting.html";
                supportLink.Visible = true;
                supportText.Visible = true;
            }
            else if (!vst.SensorConnected)
            {
                reading.Visible = false;
                supportText.Text = "For a list of supported sensors,  please see ";
                supportLink.Text = "http://www.vernier.com/soft/gotweet/help/sensors.html";
                supportText.Visible = true;
                supportLink.Visible = true;
            }
            else
            {
                supportText.Text = "";
                supportLink.Text = "";
                supportText.Visible = false;
                supportLink.Visible = false;
                reading.Visible = true;
            }
        }

        /// <summary>
        /// Tweet at the user's specified interval if everything's good
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tweetTime(object sender, EventArgs e)
        {
            if (shouldTweet()) tweet();
        }

        private bool shouldTweet()
        {
            if (prefs.tweetAtThreshold)
            {
                switch (prefs.tweetWhen)
                {
                    case 1: if (measurement >= prefs.threshold) return true; else return false;
                    case 0: if (measurement == prefs.threshold) return true; else return false;
                    case -1: if (measurement <= prefs.threshold) return true; else return false;
                    default: return false;
                }
            }
            else return true;
        }

        /// <summary>
        /// Check every so often to see if the sensors still exist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void findSensors_Tick(object sender, EventArgs e)
        {
            if (vst.DeviceOpen) connectToSensor();
        }

        /// <summary>
        /// Get new data from the sensor every half a second
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateReadings(object sender, EventArgs e)
        {
            if (allGood)
            {
                try
                {
                    measurement = vst.getData();
                    if (measurement != float.MaxValue)
                    {
                        string formatString = "{0:F" + vst.DecimalPlaces + "}";
                        string text = "";
                        //helps prevent movement of the decimal regardless of sign
                        if (measurement >= 0) text += " ";
                        text += String.Format(formatString, measurement);
                        reading.Text = text;
                        unit.Text = vst.Units;
                        nameLabel.Font = new Font("Arial", 14.5f, FontStyle.Bold);
                        nameLabel.Text = vst.LongName;
                    }
                }
                catch
                {

                }
            }
        }

        #endregion

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new Help().ShowDialog();
        }

        private void supportLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(supportLink.Text);
        }

        private void checkForNewDevicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            allGood = false;
            if (vst.DeviceOpen) vst.closeDevice();
            searchAndConnectToDevice();
        }

        public static void UpdateInternetConnectionStatus()
        {
            try
            {
                Ping internet = new Ping();
                PingReply response = internet.Send("www.google.com");

                if (response.Status == IPStatus.Success) internetWorking = true;
                else internetWorking = false;
            }
            catch
            {
                internetWorking = false;
            }
        }

        private void internet_Tick(object sender, EventArgs e)
        {
            UpdateInternetConnectionStatus();
        }
    }
}
