ï»¿#define TRACE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace GoTweet.Controllers
{
    class PachubeController
    {
        public static void updatePachube(string value)
        {
            string url = "http://www.pachube.com/api/feeds/9098/datastreams/0.csv"; //vernier feed
            byte[] data = System.Text.Encoding.UTF8.GetBytes(value);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "PUT";
            request.ContentType = "text/csv";
            request.ContentLength = data.Length;
            request.Headers.Add("X-PachubeApiKey: 3d9e1df5f7ff5580a231f9c21a4ea54a3b65189a84d99bd6b9b5250469454950");

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(data, 0, data.Length);
            dataStream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            System.Diagnostics.Trace.WriteLine(response.StatusCode.ToString());
        }
    }
}
