ï»¿using System;
using System.Collections.Generic;
using System.Text;

namespace GoTweet
{
    class Sensor
    {
        /// <summary>
        /// (read-only) The sensor's id
        /// </summary>
        public byte id { private set; get; }

        /// <summary>
        /// (read-only) The sensor's long name
        /// </summary>
        string longName;

        /// <summary>
        /// (read-only) The sensor's short name
        /// </summary>
        string shortName;

        /// <summary>
        /// (read-only) The sensor's units
        /// </summary>
        string units;

        /// <summary>
        /// (read-only) If the sensor is smart 
        /// </summary>
        bool smart;
        
        /// <summary>
        /// (read-only) The channel the sensor is connected to
        /// </summary>
        sbyte channel; //the channel it is connected to

        /// <summary>
        /// (read-only) The number of decimal places
        /// </summary>
        int decimalPlaces;

        /// <summary>
        /// Sets all the instance variables
        /// </summary>
        /// <param name="i">id</param>
        /// <param name="l">long name</param>
        /// <param name="sh">short name</param>
        /// <param name="u">units</param>
        /// <param name="s">smartness</param>
        /// <param name="c">channel</param>
        /// <param name="sf">significant figures</param>
        public Sensor(byte i, string l, string sh, string u, Boolean s, sbyte c, int dp)
        {
            this.longName  =  l;
            this.shortName = sh;
            this.units =  u;
            this.smart =  s;
            this.id = i;
            this.channel = c;
            this.decimalPlaces = dp;
        }

        /// <summary>
        /// Returns long name
        /// </summary>
        /// <returns>long name</returns>
        public string getLongName()    { return longName; }

        /// <summary>
        /// Returns units
        /// </summary>
        /// <returns>units</returns>
        public string getUnits()   { return units; }
        /// <summary>
        /// returns channel
        /// </summary>
        /// <returns>channel</returns>
        public sbyte  getChannel() { return channel; }
        /// <summary>
        /// Returns the short name
        /// </summary>
        /// <returns>short name</returns>
        public string getShortName() { return shortName; }
        /// <summary>
        /// Returns the significant figures
        /// </summary>
        /// <returns>Significant Figures</returns>
        public int getDecimalPlaces() { return decimalPlaces; }
    }
}
