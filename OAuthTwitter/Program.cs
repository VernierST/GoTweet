ï»¿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twitterizer;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace OAuthTwitter
{
    class Program
    {
        /*
         * Simple test program to become more comfortable with using OAuth with Twitter 
         * and the Twitterizer library 
         */

        //The "consumer" is our application. The following values were given by Twitter
        //They will be stored safely somewhere else
        static string consumerKey = "xS4OhCpfHLfHm03UqFGlQ";
        static string consumerSecret = "wxOvXmZga3hAqkTvgljsOq5SUdk4e8r7ZdWJFV7m0";

        static void Main(string[] args)
        {
            Console.WriteLine("Hello! Using OAuth we will allow you to update your status from here!");
            Console.WriteLine("In just a moment, your web browser will open with a page showing some numbers. Enter" +
                   "those numbers when prompted");
            OAuthTokens userInfo = connectAndGetUser();
            if (userInfo != null)
            {
                Console.WriteLine("What would you like your status to be? ");
                string status = Console.ReadLine();
                updateStatus(userInfo, status);
                Console.WriteLine("Twitter status updated!");
                Console.ReadLine();
            }
            else return;


            
        }

        static OAuthTokens connectAndGetUser()
        {
            //Convenience class the stores all the necessary information for making authenticated calls to twitter
            OAuthTokens result = new OAuthTokens();

            //If we already have permission
            if (File.Exists("Test.tst")) return (OAuthTokens)open("Test.tst");

            OAuthTokenResponse access = getAccessTokens();

            if (access != null)
            {
                result.AccessToken = access.Token;
                result.AccessTokenSecret = access.TokenSecret;
                result.ConsumerKey = consumerKey;
                result.ConsumerSecret = consumerSecret;
                saveData(result,"Test.tst");
                return result;
            }
            return null;

        }

        static void saveData(Object data, string fname)
        {
            //Might decide to store these in App Data instead of in a binary file
            Stream stream = File.Open(fname, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(stream, data);
            stream.Close();
        }

        static OAuthTokenResponse getAccessTokens(){
                //Step 1: Get a Request Token to ALLOW for a prompt the user for access to his/her profile, tweets, etc.
                OAuthTokenResponse requestAccessTokens = OAuthUtility.GetRequestToken(consumerKey, consumerSecret);

                //Step 2: Open a URL to visit to give permission to our application
                String permissionUri = OAuthUtility.BuildAuthorizationUri(requestAccessTokens.Token).AbsoluteUri;

                System.Diagnostics.Process.Start(permissionUri); //Quick and dirty way of opening it

                System.Threading.Thread.Sleep(2000); //pause for 2 seconds to get input

                //Step 3: Finally, we can get the access token which can be saved for later use to skip steps 1 and 2
                Console.Write("Enter pin number: ");
                string pinNumber = Console.ReadLine(); //They'll get this number from Twitter
                OAuthTokenResponse accessTokens;
                try
                {
                    accessTokens = OAuthUtility.GetAccessToken(
                        consumerKey, consumerSecret, requestAccessTokens.Token, pinNumber);
                    
                    return accessTokens;
                }
                catch (TwitterizerException e)
                {
                    Console.WriteLine("Unable to authenticate");
                    Console.ReadLine();
                    return null;
                }    
        }

        static TwitterUser getTwitterUserFromAccessToken(OAuthTokenResponse access)
        {
            decimal userid = access.UserId;
            return TwitterUser.Show(userid);
        }

        static void updateStatus(OAuthTokens tokens, String message){
            TwitterStatus ts = TwitterStatus.Update(tokens, message);
        }

        static Object open(string fname)
        {
            Stream stream = File.Open(fname, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            Object o = bf.Deserialize(stream);
            stream.Close();
            return o;
        }
    }
}
