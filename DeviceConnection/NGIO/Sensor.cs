ï»¿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeviceConnection
{
    class Sensor
    {
        byte id;
        string longName;
        string shortName;
        string type; //currently unimplemented
        string units;
        bool smart;
        public float[] reading { set; get; }
        sbyte channel; //the channel it is connected to

        public Sensor(byte i, string l, string sh, string u, string t, Boolean s, sbyte c)
        {
            this.longName  =  l;
            this.shortName = sh;
            this.units =  u;
            this.type  =  t;
            this.smart =  s;
            this.id = i;
            this.channel = c;
        }

        public string getLongName()    { return longName; }
        public string getType()    { return type; }
        public string getUnits()   { return units; }
        public sbyte  getChannel() { return channel; }
        public string getShortName() { return shortName; }



    }
}
