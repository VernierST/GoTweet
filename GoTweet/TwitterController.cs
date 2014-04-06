ï»¿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twitterizer;

namespace GoTweet
{
    class TwitterController
    {

        private OAuthTokenResponse requestAccessTokens;
        public OAuthTokens Tokens{ get; set;}

        private static TwitterController sharedController = null;

        private TwitterController()
        {

        }

        public static TwitterController GetSharedController()
        {
            if (sharedController == null) sharedController = new TwitterController();
            return sharedController;
        }

        public string GetAuthorizationURL(){
            //Step 1: Get a Request Token to ALLOW for a prompt the user for access to his/her profile, tweets, etc.
            requestAccessTokens = 
                OAuthUtility.GetRequestToken(Properties.Settings.Default.consumerKey, 
                                            Properties.Settings.Default.consumerSecret);

            //Step 2: Open a URL to visit to give permission to our application
            String permissionUri = OAuthUtility.BuildAuthorizationUri(requestAccessTokens.Token).AbsoluteUri;

            return permissionUri;
        }

        public int ConnectWithPIN(string pinNumber)
        {
            OAuthTokenResponse accessTokens;
            try
            {
                accessTokens = OAuthUtility.GetAccessToken(
                    Properties.Settings.Default.consumerKey, Properties.Settings.Default.consumerSecret, 
                    requestAccessTokens.Token, pinNumber);

                if (Tokens == null) Tokens = new OAuthTokens();

                Tokens.AccessToken = accessTokens.Token;
                Tokens.AccessTokenSecret = accessTokens.TokenSecret;
                Tokens.ConsumerKey = Properties.Settings.Default.consumerKey;
                Tokens.ConsumerSecret = Properties.Settings.Default.consumerSecret;

                return 0;

            }
            catch (TwitterizerException e)
            {
                return -1;
            }   
        }

        /*public int Connect()
        {
            OAuthTokens temp = Properties.Settings.Default.accessToken;
            if (temp == null)
            {
                return -1;
            }
            else
            {
                tokens = temp;
                return 0;
            }
        }*/

        public int updateStatus(String message)
        {
            TwitterStatus status = TwitterStatus.Update(Tokens, message);
            if (status == null) return -1;
            return 0;
        }
    }
}
