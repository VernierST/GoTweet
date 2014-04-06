ï»¿#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSTCoreDefsdotNET;
using System.Diagnostics;
using System.Windows.Forms;

namespace GoTweet
{
    class VSTController
    {
        IntPtr vstDevice;
        Sensor sensor;
        ILibrary vstLibrary;
        bool measuring;

        /// <summary>
        /// Whether or not the device is open
        /// </summary>
        public bool DeviceOpen
        {
            get
            {
                return (vstDevice != IntPtr.Zero);
            }
        }
        /// <summary>
        /// Whether or not a sensor is connected
        /// </summary>
        public bool SensorConnected
        {
            get
            {
                return (sensor != null);
            }
        }

        public VSTController()
        {
            measuring = false;
            vstDevice = IntPtr.Zero;
            sensor = null;
            NGIOLib.Initialize();
            try
            {
                GoLibrary.Initialize();
            }
            catch (GoIOException ge)
            {
                MessageBox.Show("Please close any application using the Go! Library");
            }
        }

        /// <summary>
        /// Go through a list of devices and open the first one.
        /// </summary>
        /// <remarks>
        /// It is important to call this function in order to initialize not only a device, 
        /// but the library as well
        /// Priority in this order:
        /// <list type="number">
        /// <item>LabQuest</item>
        /// <item>LabQuest Mini</item>
        /// <item>Go!Link</item>
        /// <item>Go!Temp</item>
        /// <item>Go!Motion</item>
        /// </list>
        /// </remarks>
        /// <returns>0 if successfull, -1 otherwise</returns>
        public int searchForAndOpenDevice()
        {
            sensor = null;
            measuring = false;

            Debug.WriteLine("GoTweet: Initializing NGIO library");
            vstLibrary = new NGIOLib();
            //Top priority to LabQuests
            Debug.WriteLine("GoTweet: Trying to open LabQuests"); 
            if (openDevice(VST_USB_defs.PRODUCT_ID_LABQUEST) == 0) return 0;
 
            //LabQuest minis next
            Debug.WriteLine("GoTweet: Trying to open LabQuest Minis");
            if (openDevice(VST_USB_defs.PRODUCT_ID_LABQUEST_MINI) == 0) return 0;

            vstLibrary = null;

            try
            {
                vstLibrary = new GoLibrary();
            }
            catch (GoIOException e)
            {
                Debug.WriteLine("GoTweet: Unable to initalize GoLibrary. Checking processes");
                foreach (Process p in Process.GetProcesses())
                {
                    if (p.ProcessName.Contains("LoggerPro"))
                    {
                        MessageBox.Show("Please close Logger Pro and then search again for devices");
                        return -1;
                    }
                    if (p.ProcessName.Contains("LoggerLite"))
                    {
                        MessageBox.Show("Please close Logger Lite and then search again for devices");
                        return -1;
                    }
                }
                MessageBox.Show("Please close any application trying to access GoIO and then search again for devices");
                return -1;
            }

            //Let's first look for some Go!Links
            Debug.WriteLine("GoTweet: Trying to open Go!Link");
            if (openDevice(VST_USB_defs.PRODUCT_ID_GO_LINK) == 0) return 0;

            //If we can't find any of those, we can look for Go!Temps
            Debug.WriteLine("GoTweet: Trying to open Go!Temps");
            if (openDevice(VST_USB_defs.PRODUCT_ID_GO_TEMP) == 0) return 0;

            //Still can't find anything? Look for a Go!Motion
            Debug.WriteLine("GoTweet: Trying to open Go!Motion");
            if (openDevice(VST_USB_defs.PRODUCT_ID_GO_MOTION) == 0) return 0;

            vstLibrary = null;
            return -1;

        }

        /// <summary>
        /// Connects to the first <see cref="Sensor"/> it finds on the device
        /// </summary>
        /// <returns>0 if succesful, -1 otherwise </returns>
        public int connectToSensor()
        {
            Sensor[] sensors = null;
            try
            {
                Debug.WriteLine("GoTweet: Trying to get connected sensors to device");
                sensors = vstLibrary.getConnectedSensors(vstDevice);
            }
            catch (Exception e)
            {
            }
            for (int i = 0; i < sensors.Length; i++)
            {
                if (sensors[i] != null)
                {
                    sensor = sensors[i];
                    if (!measuring)
                    {
                        try { vstLibrary.startMeasuring(vstDevice); }
                        catch (Exception vste) { }
                        measuring = true;
                    }
                    return 0;
                }
            }
            sensor = null;
            try { vstLibrary.stopMeasuring(vstDevice); }
            catch (Exception vste) { }
            measuring = false;
            return -1;

           

        }

        /// <summary>
        /// Gets the last measurement from the device being tracked
        /// </summary>
        /// <returns>The last measurement</returns>
        /// <remarks>A Float.MaxValue may appear in case of an error</remarks>
        public float getData()
        {
            return vstLibrary.getLastMeasurement(vstDevice, sensor);
        }

        /// <summary>
        /// Cleans up all handles and uninitializes the library. Very important to call this!
        /// </summary>
        public void uninit()
        {
            measuring = false;
            sensor = null;
            if (vstDevice != null)
            {
                if (vstLibrary != null)
                {
                    try
                    {
                        vstLibrary.closeDevice(vstDevice);
                    }
                    catch (GoIOException ge)
                    {
                    }
                }


            }
            NGIOLib.Uninitialize();
            GoLibrary.Uninitialize();
        }

        /// <summary>
        /// Closes the device being tracked
        /// </summary>
        public void closeDevice()
        {
            if (vstDevice != IntPtr.Zero)
            {
                vstLibrary.stopMeasuring(vstDevice);
                vstLibrary.closeDevice(vstDevice);
                vstDevice = IntPtr.Zero;
                sensor = null;
            }
        }

        #region Utility Methods
        /// <summary>
        /// Opens a device of the given product id
        /// </summary>
        /// <param name="productId">Type of device to open</param>
        /// <returns>0 if successful, -1 otherwise</returns>
        private int openDevice(int productId)
        {
            Debug.WriteLine("GoTweet: Getting number of devices of id " + productId);
            int numDevices = vstLibrary.getNumberOfDevices(productId);
            if (numDevices > 0)
            {
                try
                {
                    Debug.WriteLine("GoTweet: Opening first device found");
                    vstDevice = vstLibrary.openNthDevice(productId, 0); //open the first one
                    return 0;
                }
                catch (Exception e)
                {
                    //this would fail in the this.closeDevice() error checking
                    Debug.WriteLine("GoTweet: Unable to open device. Closing handle.");
                    try
                    {
                        vstLibrary.closeDevice(vstDevice);
                    }
                    catch (GoIOException ge)
                    {
                        return -1;
                    }
                    return -1;
                }
            }
            return -1;
        }
        #endregion

        #region Sensor Properties

        /// <summary>
        /// Long name of the <see cref="Sensor"/> being tracked
        /// </summary>
        public string LongName
        {
            get
            {
                return sensor.getLongName();
            }
        }

        /// <summary>
        /// Short name of the <see cref="Sensor"/> being tracked
        /// </summary>
        public string ShortName
        {
            get
            {
                return sensor.getShortName();
            }
        }

        /// <summary>
        /// Units of the <see cref="Sensor"/> being tracked
        /// </summary>
        public string Units
        {
            get
            {
                return sensor.getUnits().Replace("(","").Replace(")","");
            }
        }

        /// <summary>
        /// Number of decimal places of the <see cref="Sensor"/> being tracked
        /// </summary>
        public int DecimalPlaces
        {
            get
            {
                return sensor.getDecimalPlaces();
            }
        }

        #endregion


    }
}
