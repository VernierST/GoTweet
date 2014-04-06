ï»¿#define TRACE

using System;
using System.Collections.Generic;
using System.Text;
using NGIOdotNET;
using VSTCoreDefsdotNET;
using System.Diagnostics;
using System.Globalization;

namespace GoTweet
{
    /// <summary>
    /// A mediator for Go!Tweet that connects to the NGIO and passes its data
    /// </summary>
    class NGIOController
    {
        /// <summary>
        /// The controller's handle of the NGIO components
        /// </summary>
        private NGIOLibrary ngiolib;
        /// <summary>
        /// The device currently being tracked
        /// </summary>
        private Device device;
        /// <summary>
        /// The sensor currently being tracked
        /// </summary>
        private Sensor sensor;

        /// <summary>
        /// A queue of previous values to spit out in case of emergencies
        /// </summary>
        private Queue<float> dataCache;

        /// <summary>
        /// Whether or not <paramref name="device"/> is taking measurments
        /// </summary>
        private bool measuring = false;

        /// <summary>
        /// Initializes the NGIO library and pauses the program to allow for USB drivers to startup
        /// </summary>
        public NGIOController()
        {
            try
            {
                dataCache = new Queue<float>();
                dataCache.Enqueue(0);
                ngiolib = new NGIOLibrary();
                trace("NGIO Library opened");
                System.Threading.Thread.Sleep(1000);
            }
            catch (NGIOException e)
            {
                trace(e.Message);
            }
        }

        /// <summary>
        /// Gets the last measurment taken from the tracked sensor. In case of errors, 
        ///  previous, garbage values are returned until the program can recover
        /// </summary>
        /// <returns>The last measurement or a garbage value</returns>
        public float getData()
        {
            float f = device.getLastMeasurment(sensor);
            if (f != float.MaxValue) dataCache.Enqueue(f);
            else
            {
                f = dataCache.Dequeue(); //back to the end of the line!
                dataCache.Enqueue(f);
            }
            trace("Got measurement: " + f.ToString());
            return f;
        }

        /// <summary>
        /// Gets the units of the tracked sensor, sans parantheses
        /// </summary>
        /// <returns>Prettified Unit string</returns>
        public string getSensorUnits()
        {
            return sensor.getUnits().Replace("(","").Replace(")",""); //strip the units of parantheses
        }

        /// <summary>
        /// Closes the device and uninitializes the NGIO library. Very important to call this function
        ///  when you are done!
        /// </summary>
        public void dispose()
        {
            if (isDeviceOpen())
            {
                closeDevice();
                trace("Device closed");
            }
            ngiolib.uninit();
            trace("Library uninitialized");
        }

        /// <summary>
        /// Checks the status of the attached device
        /// </summary>
        /// <returns>If the device is not null and <see cref="Device.open"/></returns>
        public bool isDeviceOpen()
        {
            return (device != null && device.open);
        }
        /// <summary>
        /// Checks the status of the tracked sensor
        /// </summary>
        /// <returns>If the sensor is not null</returns>
        public bool isSensorConnected()
        {
            return (sensor != null);
        }
        /// <summary>
        /// Nullifies the sensor to prevent further measurments, tells the device to stop measuring,
        /// and tells it to close itself.
        /// </summary>
        public void closeDevice()
        {
            sensor = null;
            device.stopMeasuring();
            device.closeDevice();
            measuring = false;
        }

        /// <summary>
        /// Searches for LabQuests and LabQuest Minis, respectively, and connects to the first one it 
        ///  finds and it opens it.
        /// </summary>
        /// <returns>0 if successful, -1 if unable to find any device or errors out</returns>
        public int searchAndOpenDevice()
        {
            try{
                trace("Trying to find LabQuests");
                string[] names = ngiolib.getAllConnectedDeviceNames(NGIO.DEVTYPE_LABQUEST);
                if (names.Length != 0)
                {
                    if (isDeviceOpen()) this.closeDevice();
                    device = ngiolib.openDeviceByName(names[0],NGIO.DEVTYPE_LABQUEST);
                    return 0;
                }
                trace("No LabQuests found");
                trace("Looking for LabQuest Minis");
                names = ngiolib.getAllConnectedDeviceNames(NGIO.DEVTYPE_LABQUEST_MINI);
                if (names.Length != 0)
                {
                    if (isDeviceOpen()) this.closeDevice();
                    device = ngiolib.openDeviceByName(names[0],NGIO.DEVTYPE_LABQUEST_MINI);
                    return 0;
                }
                trace("No devices found");
                device = null;
                return -1;

            }
            catch (NGIOException e)
            {
                trace(e.Message);
                return -1;
            }
        }

        /// <summary>
        /// Queries the device for connected a <see cref="Sensor"/> on an analog channel 
        ///  on the NGIO <see cref="Device"/>, finds the first one, and begins measuring
        /// </summary>
        /// <returns></returns>
        public int connectToSensor()
        {
            Sensor[] sensors = null;
            try
            {
                trace("Looking for connected sensors");
                sensors = device.getConnectedSensors();
            }
            catch (NGIOException e)
            {
            }
            for (int i = 0; i < sensors.Length; i++)
            {
                if (sensors[i] != null)
                {
                    trace("Sensor found!");
                    sensor = sensors[i];
                    if (!measuring)
                    {
                        device.startMeasuring();
                        measuring = true;
                    }
                    return 0;
                }
            }
            this.sensor = null;
            return -1;
        }

        /// <summary>
        /// Returns the attached sensor's long name
        /// </summary>
        /// <returns>Sensor's long name</returns>
        public string getSensorLongName()
        {
            return sensor.getLongName();
        }
        /// <summary>
        /// Returns the number of decimal places
        /// </summary>
        /// <returns>The calibrated number of decimal places</returns>
        public int getPrecision()
        {
            return sensor.getDecimalPlaces();
        }

        /// <summary>
        /// Outputs debug information about NGIO connections and measurements
        /// </summary>
        /// <param name="message">The message to write</param>
        private void trace(string message)
        {
            //if (Properties.Settings.Default.logNGIO)
            {
                Trace.WriteLine(message);
            }
        }
    }
}
