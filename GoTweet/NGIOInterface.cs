ï»¿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NGIOdotNET;
using VSTCoreDefsdotNET;

namespace GoTweet
{
    class NGIOInterface
    {

        private NGIOLibrary lib;
        private Device device;
        private Sensor sensor;

        private float data;

        public NGIOInterface()
        {
            try
            {
                lib = new NGIOLibrary();
                string[] names = lib.getAllConnectedDeviceNames(NGIO.DEVTYPE_LABQUEST_MINI);
                device = lib.openDeviceByName(names[0], NGIO.DEVTYPE_LABQUEST_MINI);
                
                sensor = device.getSensor((sbyte)NGIO_ParmBlk.CHANNEL_ID_ANALOG1);
                device.startMeasuring();
            }
            catch (NGIOException e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public float getData()
        {

            return device.getLastMeasurment(sensor);
        }

        public string getUnits()
        {
            return sensor.getUnits();
        }

        public void dispose()
        {
            sensor = null;
            device.stopMeasuring();
            device.closeDevice();
            lib.uninit();
        }


        internal string getShortName()
        {
            return sensor.getShortName();
        }
    }
}
