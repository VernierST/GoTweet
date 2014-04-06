ï»¿#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NGIOdotNET;
using VSTCoreDefsdotNET;
using System.Diagnostics;

namespace GoTweet
{
    /// <summary>
    /// A new and improved class
    /// </summary>
    class NGIOLib: ILibrary
    {
        public static IntPtr library;
        IntPtr deviceList;

        public NGIOLib()
        {
            //library = IntPtr.Zero;
            deviceList = IntPtr.Zero;
        }

        #region Library Methods
        /// <summary>
        /// Initialize the NGIO library
        /// </summary>
        public static void Initialize()
        {
            library = NGIO.Init();
            Debug.WriteLine("GoTweet:Turning on NGIO debugging");
            NGIO.Diags_SetDebugTraceThreshold(library, NGIO.TRACE_SEVERITY_LOWEST);
            if (library == IntPtr.Zero) throw new NGIOException("Unable to initialize library");
        }

        /// <summary>
        /// Get the number of NGIO devices of the specified type
        /// </summary>
        /// <param name="productId">Product id of the type of device you want to find</param>
        /// <returns>The number of devices of the product id connected</returns>
        /// <remarks>
        /// Call this function before openNthDevice!!!!!
        /// </remarks>
        public int getNumberOfDevices(int productId)
        {
            if (isNGIODevice(productId))
            {
                uint deviceType = getDeviceType(productId);
                uint s; //the signature - only used to satisfy the parameter list
                NGIO.SearchForDevices(library, deviceType, NGIO.COMM_TRANSPORT_USB, IntPtr.Zero, out s);

                uint numberOfDevices = 0; //the number of devices found of the specifed type
                deviceList = NGIO.OpenDeviceListSnapshot(library, deviceType, out numberOfDevices, out s);
                if (deviceList == IntPtr.Zero) throw new NGIOException("Unable to open device list snapshot");

                return (int)numberOfDevices;
            }
            else throw new NGIOException("Unknown product id");
        }

        /// <summary>
        /// Opens the nth device of the specified product id
        /// </summary>
        /// <param name="productID">Product id of the device you want to open</param>
        /// <param name="n">A number between 0 and getNumberOfDevices()-1, including</param>
        /// <returns>A handle to the opened device</returns>
        /// <remarks>
        /// Call this function after getNumberOfDevices()!!!!!!!!!!!!!!!!
        /// </remarks>
        public IntPtr openNthDevice(int productID, int n)
        {
            Debug.WriteLine("GoTweet: Opening Nth Device");
            if (deviceList == IntPtr.Zero) throw new NGIOException("Device list never opened. " +
                "Did you forget to call getNumberOfDevices()?");

            Debug.WriteLine("GoTweet: Getting device name");
            StringBuilder deviceName = new StringBuilder((int)NGIO.MAX_SIZE_DEVICE_NAME);
            uint sm;
            int status = NGIO.DeviceListSnapshot_GetNthEntry(deviceList, (uint)n,
                deviceName, (uint)deviceName.Capacity, out sm);

            Debug.WriteLine("GoTweet: Opening device");
            IntPtr device = NGIO.Device_Open(library, deviceName.ToString(), 0);

            if (productID == VST_USB_defs.PRODUCT_ID_LABQUEST)
            {
                Debug.WriteLine("GoTweet: Acquiring exclusive ownership of LabQuest");
                int result = NGIO.Device_AcquireExclusiveOwnership(device, NGIO.GRAB_DAQ_TIMEOUT_MS);
                if (result != 0) throw new NGIOException("Unable to acquire exclusive ownership");
            }
            if (productID == VST_USB_defs.PRODUCT_ID_LABQUEST_MINI)
            {
                Debug.WriteLine("GoTweet: Changing LabQuesrt Mini LED lights");
                NGIOSetLedStateParams param = new NGIOSetLedStateParams();
                param.brightness = NGIOSetLedStateParams.LED_BRIGHTNESS_MAX;
                param.color = NGIOSetLedStateParams.LED_COLOR_GREEN;

                NGIO.Device_SendCmdAndGetResponse2(device, NGIO_ParmBlk.CMD_ID_SET_LED_STATE,
                    param, NGIO.TIMEOUT_MS_DEFAULT);
            }

            if (device == IntPtr.Zero) throw new NGIOException("Unable to open device");

            return device;
        }

        /// <summary>
        /// Get all sensors connected on the analog channel on a device
        /// </summary>
        /// <param name="device">Handle to the device you want to check</param>
        /// <returns>An array of preconfigured <see cref="Sensor"/> objects</returns>
        public Sensor[] getConnectedSensors(IntPtr device)
        {
            Sensor[] sensors = new Sensor[4];
            int index = 0;

            for (sbyte channel = NGIO_ParmBlk.CHANNEL_ID_ANALOG1; channel <= NGIO_ParmBlk.CHANNEL_ID_ANALOG4; channel++)
            {
                try
                {
                    sensors[index] = buildNGIOSensor(device, channel);
                }
                catch (NGIOException e)
                {
                    sensors[index] = null;
                }
               
                index++;
            }

            return sensors;

        }

        /// <summary>
        /// Close a device
        /// </summary>
        /// <param name="device">Handle to the device to close</param>
        public void closeDevice(IntPtr device)
        {
            Debug.WriteLine("Closing...");
            stopMeasuring(device); //doesn't matter if it is stopped twice
            NGIO.Device_Close(device);
        }

        /// <summary>
        /// Uninitialize the NGIO library
        /// </summary>
        public static void Uninitialize()
        {
            NGIO.Uninit(library); 
        }
        public void uninit()
        {
            NGIOLib.Uninitialize();
        }

        /// <summary>
        /// Send a device the command to start taking measurements
        /// </summary>
        /// <param name="device">Handle to the device to send the command</param>
        public void startMeasuring(IntPtr device)
        {
            NGIO.Device_SendCmdAndGetResponse4(device, NGIO_ParmBlk.CMD_ID_START_MEASUREMENTS,
                NGIO.TIMEOUT_MS_DEFAULT);
        }
        
        /// <summary>
        /// Send a device the command to stop taking measurements
        /// </summary>
        /// <param name="device">Handle to the device to send the command</param>
        public void stopMeasuring(IntPtr device)
        {
            NGIO.Device_SendCmdAndGetResponse4(device, NGIO_ParmBlk.CMD_ID_STOP_MEASUREMENTS,
                NGIO.TIMEOUT_MS_DEFAULT);
        }

        /// <summary>
        /// Returns an array of measurements, may be smaller in size then the maximum number of points 
        /// requested
        /// </summary>
        /// <param name="device">The device you want to get data from</param>
        /// <param name="sensor">The sensor youw want to get data from</param>
        /// <param name="maxNumMeasurements">The maximum number of points you want</param>
        /// <returns>An array of measurements</returns>
        public float[] getMeasurementsFromSensor(IntPtr device, Sensor sensor, int maxNumMeasurements)
        {
            int[] data = new int[maxNumMeasurements];
            int actualNumberOfMeasurments = NGIO.Device_ReadRawMeasurements(device, sensor.getChannel(),
                data, null, (uint)maxNumMeasurements);
            if (actualNumberOfMeasurments > 0)
            {
                float[] result = new float[actualNumberOfMeasurments];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = NGIO.Device_CalibrateData2(device, sensor.getChannel(), data[i]);
                }
                return result;
            }
            return null;
        }

        /// <summary>
        /// Convenience method that gets the last point
        /// </summary>
        /// <param name="device">Handle to the device you want to get data from</param>
        /// <param name="sensor">Handle to the sensor you want to get data from</param>
        /// <returns>The measurement, or <see cref="float.MaxValue"/> in case of error</returns>
        public float getLastMeasurement(IntPtr device, Sensor sensor)
        {
            if (sensor == null) return float.MaxValue;
            int numMeasurments = NGIO.Device_GetNumMeasurementsAvailable(device, sensor.getChannel());
            if (numMeasurments > 0) return getMeasurementsFromSensor(device, sensor, 100)[numMeasurments - 1]; //in an array of n-elements, n-1 is the last one
            return float.MaxValue;
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Convert an integer productId from VSTCoreDefs to a uint device type in NGIO
        /// </summary>
        /// <param name="productId">Product ID to get the device type for</param>
        /// <returns>The device type of the product id</returns>
        public uint getDeviceType(int productId)
        {
            if (productId == VST_USB_defs.PRODUCT_ID_LABQUEST) return NGIO.DEVTYPE_LABQUEST;
            else if (productId == VST_USB_defs.PRODUCT_ID_LABQUEST_MINI) return NGIO.DEVTYPE_LABQUEST_MINI;
            else throw new NGIOException("Unknown product id"); 
        }
        /// <summary>
        /// Checks if the given product ID is an NGIO product id
        /// </summary>
        /// <param name="productId">The ID to check</param>
        /// <returns>true if the id is a LabQuest or LabQuest Mini, otherwise false</returns>
        public bool isNGIODevice(int productId)
        {
            return (productId == VST_USB_defs.PRODUCT_ID_LABQUEST || productId == VST_USB_defs.PRODUCT_ID_LABQUEST_MINI);
        }

        #endregion Utility Methods

        #region Sensor Builder Methods
        public Sensor buildNGIOSensor(IntPtr device, sbyte channel)
        {
            byte sensorId = getSensorId(device, channel);
            bool smart = false;

            if (sensorId == 0) return null;
            if (sensorId >= VSTSensorDDSMemRec.kSensorIdNumber_FirstSmartSensor)
            {
                smart = true;
                NGIO.Device_DDSMem_ReadRecord(device, channel, 0, NGIO.READ_DDSMEMBLOCK_TIMEOUT_MS);
                return new Sensor(sensorId, getSensorLongName(device, channel),
                    getSensorShortName(device, channel), getSensorUnits(device, channel),
                    smart, channel, getSensorDecimalPlaces(device, channel));
            }
            return new Sensor(sensorId, "", "", "volts", smart, channel, 3);

            //Uncomment the following lines when new NGIO is available

            //return new Sensor(sensorId, getSensorLongName(device, channel), getSensorShortName(device, channel),
                //getSensorUnits(device, channel), false, channel, getSensorDecimalPlaces(device, channel));
        }

        /// <summary>
        /// Gets the number of the sensor in the specified channel
        /// </summary>
        /// <param name="channel">The channel to check for a sensor</param>
        /// <returns>The ID of the sensor in the channel</returns>
        /// <exception cref="NGIOException">No sensor was found</exception>
        private byte getSensorId(IntPtr device, sbyte channel)
        {
            byte sensorId, flag = 1;
            uint channelSig;

            NGIO.Device_DDSMem_GetSensorNumber(device, channel,
                out sensorId, flag, out channelSig, NGIO.TIMEOUT_MS_DEFAULT);

            if (sensorId == 0) throw new NGIOException("Unable to retreive sensor id");

            return sensorId;
        }

        /// <summary>
        /// Reads the long name of the sensor from the DDS, assuming it exists, of the specified channel 
        /// </summary>
        /// <param name="channel">Channel to query</param>
        /// <returns>The long name of the sensor in the channel</returns>
        private string getSensorLongName(IntPtr device, sbyte channel)  //Assumes sensor is smart and DDS is on computer
        {
            StringBuilder longName = new StringBuilder(30);
            NGIO.Device_DDSMem_GetLongName(device, channel, longName, (ushort)longName.Capacity);
            return longName.ToString();
        }

        /// <summary>
        /// Gets the short name of the sensor from the DDS, assuming it exists, of the specified channel
        /// </summary>
        /// <param name="channel">Channel to query</param>
        /// <returns>The short name of the sensor in the channel</returns>
        private string getSensorShortName(IntPtr device, sbyte channel)
        {
            StringBuilder shortName = new StringBuilder(30);
            NGIO.Device_DDSMem_GetShortName(device, channel, shortName, (ushort)shortName.Capacity);
            return shortName.ToString();
        }

        /// <summary>
        /// Gets the units of the sensor from the DDS, assuming it exists, of the specified channel
        /// </summary>
        /// <param name="channel">The channel to query</param>
        /// <returns></returns>
        private string getSensorUnits(IntPtr device, sbyte channel) //Assumes sensor is smart and DDS is on computer
        {
            sbyte equation;
            StringBuilder units = new StringBuilder(20);
            byte activeCalPage = 0;
            NGIO.Device_DDSMem_GetCalibrationEquation(device, channel, out equation);
            if (equation == VSTSensorDDSMemRec.kEquationType_Linear){
                float a, b, c;
                NGIO.Device_DDSMem_GetCalPage(device, channel, activeCalPage,
                    out a, out b, out c, units, (ushort)units.Capacity);
                return units.ToString();
            }
            else return "volts";
        }

        /// <summary>
        /// Gets of number of decimal places of a sensor connected to a channel
        /// </summary>
        /// <param name="device">Handle to device to check</param>
        /// <param name="channel">Channel to check</param>
        /// <returns>Number of decimal places</returns>
        private int getSensorDecimalPlaces(IntPtr device, sbyte channel)
        {
            sbyte equation;
            NGIO.Device_DDSMem_GetCalibrationEquation(device, channel, out equation);
            if (equation == VSTSensorDDSMemRec.kEquationType_Linear)
            {
                int maxVoltage = 5;
                int minVolate = 0;

                float calibratedMax = NGIO.Device_CalibrateData(device, channel, maxVoltage);
                float calibratedMin = NGIO.Device_CalibrateData(device, channel, minVolate);

                float num = Math.Abs(calibratedMax - calibratedMin) / 4096;
                return (int)Math.Ceiling(Math.Abs(Math.Log10(num)));
            }
            return 3;
        }

        #endregion
    }
}
