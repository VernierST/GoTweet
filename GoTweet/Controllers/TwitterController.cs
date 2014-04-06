ï»¿#define TRACE
#define DEBUG

using System;
using System.Collections.Generic;
using System.Text;
using Twitterizer;
using System.Diagnostics;

namespace GoTweet
{
    /// <summary>
    /// Provides a connection to Twitter
    /// </summary>
    class TwitterController
    {
        /// <summary>
        /// Normally null, this token holds the permission to ask Twitter to allow for authorization
        ///  and is necessary to recieve permanent authorization
        /// </summary>
        private OAuthTokenResponse requestAccessTokens;

        /// <summary>
        /// These tokens, which are loaded from the preferences, contain all the information to make
        ///  authorized calls to Twiter
        /// </summary>
        public OAuthTokenResponse accessTokens { get; set; }

        /// <summary>
        /// The singleton Twitter object
        /// </summary>
        private static TwitterController sharedController = null;

        /// <summary>
        /// Inaccessible constructor
        /// </summary>
        private TwitterController()
        {

        }

        /// <summary>
        /// Only way to get an instance of this class
        /// </summary>
        /// <returns>Singleton TwitterController instance</returns>
        public static TwitterController GetSharedController()
        {
            if (sharedController == null) sharedController = new TwitterController();
            return sharedController;
        }

        /// <summary>
        /// Asks Twitter for permission to athorize and provides a URL to authenticate with
        /// </summary>
        /// <returns>the URL of the authorization page</returns>
        public string GetAuthorizationURL(){
            //Step 1: Get a Request Token to ALLOW for a prompt the user for access to his/her profile, tweets, etc.
            trace("Attempting to get request tokens");
            requestAccessTokens = 
                OAuthUtility.GetRequestToken(PreferencesController.consumerKey, 
                                            PreferencesController.consumerSecret);

            //Step 2: Open a URL to visit to give permission to our application
            trace("Building authorization URL");
            String permissionUri = OAuthUtility.BuildAuthorizationUri(requestAccessTokens.Token).AbsoluteUri;

            return permissionUri;
        }

        /// <summary>
        /// Given a PIN number, attempts to ask for access to Twitter until revoked
        /// </summary>
        /// <param name="pinNumber"></param>
        /// <returns>0 if successful, -1 otherwise</returns>
        public int ConnectWithPIN(string pinNumber)
        {
            
            try
            {
                trace("Getting access tokens");
                accessTokens = OAuthUtility.GetAccessToken(
                    PreferencesController.consumerKey, PreferencesController.consumerSecret, 
                    requestAccessTokens.Token, pinNumber);

                return 0;

            }
            catch (TwitterizerException e)
            {
                trace(e.Message);
                return -1;
            }   
        }

        /// <summary>
        /// Uses the faster and easier xAuth process for logging in, skipping the many dialogs
        /// </summary>
        /// <param name="username">Username of the user requesting access</param>
        /// <param name="password">Password of the user</param>
        /// <returns>0 if succesful, -403 for invalid username/password, -1 otherwise</returns>
        public int xAuthConnect(string username, string password)
        {
            Debug.WriteLine("Line 1");
            try
            {
                Debug.WriteLine("Line 2");
                trace("Getting access tokens");
                Debug.WriteLine("Line 3");
                accessTokens = XAuthUtility.GetAccessTokens(
                    PreferencesController.consumerKey, 
                    PreferencesController.consumerSecret,
                    username,password);
                return 0;
            }
            catch (TwitterizerException e)
            {
                //Debug.WriteLine(e.ResponseBody);
                //if (e.ResponseBody.Equals("Invalid user name or password")) return -403;
                return -1;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return -1;
            }
        }

        /// <summary>
        /// Updates the user's status with the given message
        /// </summary>
        /// <param name="message"></param>
        /// <returns>The response from Twitter</returns>
        public RequestStatus updateStatus(String message)
        {
            try
            {
                TwitterStatus status = TwitterStatus.Update(getTokensFromAccess(), message);
                if (status.RequestStatus.Status == RequestResult.Success)
                {
                    trace("Twitter Status Updated! " + message + " at " + DateTime.Now.ToString());
                    trace("The tweet can be found at http://www.twitter.com/" +
                        accessTokens.ScreenName + "/status/" + status.Id);
                }
                else trace(status.RequestStatus.ResponseBody);
                return status.RequestStatus;
            }
            catch (TwitterizerException failWhale)
            {
                trace(failWhale.Message);
                return RequestStatus.LastRequestStatus;
            }
        }

        /// <summary>
        /// Turns the access tokens into OAuth tokens when necessary
        /// </summary>
        /// <returns>Tokens for authenticated requests</returns>
        public OAuthTokens getTokensFromAccess()
        {
            OAuthTokens result = new OAuthTokens();
            result.AccessToken = accessTokens.Token;
            result.AccessTokenSecret = accessTokens.TokenSecret;
            result.ConsumerKey = PreferencesController.consumerKey;
            result.ConsumerSecret = PreferencesController.consumerSecret;
            return result;
        }

        /// <summary>
        /// Logs output
        /// </summary>
        /// <param name="message">The message to log</param>
        private void trace(string message)
        {
            Debug.WriteLine(message);
        }


    }
}
