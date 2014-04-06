ï»¿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoIOdotNET;
using VSTCoreDefsdotNET;

namespace GoTweet
{
    class GoLibrary: ILibrary
    {
        /// <summary>
        /// Calls init()
        /// </summary>
        public GoLibrary()
        {
        }

        #region Library Methods
        /// <summary>
        /// Initializes the Go!Library
        /// </summary>
        public static void Initialize()
        {
            IntPtr initResult = GoIO.Init();
            if (initResult.ToInt32() != 0) throw new GoIOException("Unable to init library");
        }

        /// <summary>
        /// Gets the number of Go! devices of the specified type connected to the computer
        /// </summary>
        /// <remarks>
        /// Call this method before <see cref="GoLibrary.openNthDevice"/>
        /// </remarks>
        /// <param name="productId">The type of device to look for</param>
        /// <returns>The number of devices</returns>
        public int getNumberOfDevices(int productId)
        {
            if(isGoDevice(productId)) 
            {
                return GoIO.UpdateListOfAvailableDevices(
                    VST_USB_defs.VENDOR_ID, productId);
            }
            else throw new GoIOException("Unknown product id");
        }

        /// <summary>
        /// Opens the 
        /// </summary>
        /// <remarks>
        /// Call <see cref="GoLibrary.getNumberOfDevices"/> before calling this method
        /// </remarks>
        /// <param name="productID">The type of device you want to open</param>
        /// <param name="n">A number between 0 and getNumberOfDevices()-1</param>
        /// <returns></returns>
        public IntPtr openNthDevice(int productID, int n)
        {
            StringBuilder name = new StringBuilder(GoIO.MAX_SIZE_SENSOR_NAME);
            if (isGoDevice(productID))
            {
                int result = GoIO.GetNthAvailableDeviceName(name, name.Capacity,
                    VST_USB_defs.VENDOR_ID, productID, n);
                if (result != 0) throw new GoIOException("Unable to get device name. "+
                                        "Did you forget to call getNumberOfDevices()?");

                IntPtr deviceHandle = GoIO.Sensor_Open(name.ToString(),
                    VST_USB_defs.VENDOR_ID, productID, 0);

                if (deviceHandle.ToInt32() == 0) throw new GoIOException("Unable to open device");

                GoIOSetLedStateParams ledParams = new GoIOSetLedStateParams();
                ledParams.brightness = GoIOSetLedStateParams.LED_BRIGHTNESS_MAX;
                ledParams.color = GoIOSetLedStateParams.LED_COLOR_GREEN;

                int ledResult = GoIO.Sensor_SendCmdAndGetResponse2(deviceHandle,
                    GoIO_ParmBlk.CMD_ID_SET_LED_STATE, ledParams, GoIO.TIMEOUT_MS_DEFAULT);
                if (ledResult != 0) throw new NGIOException("Unable to set LED state"); 


                return deviceHandle;

            }
            else throw new GoIOException("Unknown product id");
        }

        /// <summary>
        /// Close a device
        /// </summary>
        /// <param name="device">Handle to the Go!Device you want to close</param>
        public void closeDevice(IntPtr device)
        {
            GoIOSetLedStateParams ledParams = new GoIOSetLedStateParams();
            ledParams.brightness = GoIOSetLedStateParams.LED_BRIGHTNESS_ORANGE;
            ledParams.color = GoIOSetLedStateParams.LED_COLOR_ORANGE;

            GoIO.Sensor_SendCmdAndGetResponse2(device,
                GoIO_ParmBlk.CMD_ID_SET_LED_STATE, ledParams, GoIO.TIMEOUT_MS_DEFAULT);

            int result = GoIO.Sensor_Close(device);
            if (result != 0) throw new GoIOException("Unable to close device");
        }
        /// <summary>
        /// Returns the sensor part of the Go!Device
        /// </summary>
        /// <remarks>
        /// The returned array will always be length 1 because a Go! device can only have one sensor
        /// </remarks>
        /// <param name="device">Handle to the device you want to check for sensors on</param>
        /// <returns>An array of all the sensors connected</returns>
        public Sensor[] getConnectedSensors(IntPtr device)
        {
            Sensor[] sensors = new Sensor[1]; //Go! only supports one sensor at once
            try
            {
                sensors[0] = buildGoSensor(device);
                if (sensors[0].id == 0) sensors[0] = null;
            }
            catch (GoIOException e)
            {
                sensors[0] = null;
            }
            return sensors;  
        }

        /// <summary>
        /// Uninitializes the Go!Library
        /// </summary>
        public static void Uninitialize()
        {
            int uninitResult = GoIO.Uninit();
            if (uninitResult != 0) throw new GoIOException("Unable to uninit library");
        }

        /// <summary>
        /// Sends the Go!Device the command to start measuring
        /// </summary>
        /// <param name="device">Handle to the Go!Device you wish to start measuring</param>
        public void startMeasuring(IntPtr device)
        {
            GoIO.Sensor_ClearIO(device);
            int result = GoIO.Sensor_SendCmdAndGetResponse4(device, GoIO_ParmBlk.CMD_ID_START_MEASUREMENTS,
                GoIO.TIMEOUT_MS_DEFAULT);
            if (result != 0) throw new NGIOException("Unable to start measuring");
        }

        /// <summary>
        /// Sends the Go!Device the command to stop measuring
        /// </summary>
        /// <param name="device">Handle to the Go!Device you wish to stop measuring</param>
        public void stopMeasuring(IntPtr device)
        {
            GoIO.Sensor_SendCmdAndGetResponse4(device, GoIO_ParmBlk.CMD_ID_STOP_MEASUREMENTS,
                GoIO.TIMEOUT_MS_DEFAULT);
        }


        /// <summary>
        /// Returns an array of measurements, may be smaller in size then the maximum number of points 
        /// requested
        /// </summary>
        /// <param name="device">The device you want to get data from</param>
        /// <param name="sensor">The sensor youw want to get data from</param>
        /// <param name="maxNumMeasurements">The maximum number of points you want</param>
        /// <returns>An array of measurments</returns>
        public float[] getMeasurementsFromSensor(IntPtr device, Sensor sensor, int maxNumMeasurements)
        {
            int[] raw = new int[maxNumMeasurements];
            int numMeasurements = GoIO.Sensor_ReadRawMeasurements(device, raw, (uint)raw.Length);

            float[] results = new float[numMeasurements];
            for (int i = 0; i < numMeasurements; i++)
            {
                results[i] = (float) GoIO.Sensor_CalibrateData(device, GoIO.Sensor_ConvertToVoltage(device, raw[i]));
            }
            return results;
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
            int numMeasurments = GoIO.Sensor_GetNumMeasurementsAvailable(device);
            if (numMeasurments > 1) return getMeasurementsFromSensor(device, sensor, 100)[numMeasurments - 1]; //in an array of n-elements, n-1 is the last one
            return float.MaxValue;
        }

        #endregion Library Methods

        #region Utility Methods
        /// <summary>
        /// Checks if the given product ID is a Go!Device product id
        /// </summary>
        /// <param name="productId">The ID to check</param>
        /// <returns>true if the id is part of the Go! family, otherwise false</returns>
        private bool isGoDevice(int productId)
        {
            return (productId == VST_USB_defs.PRODUCT_ID_GO_LINK ||
                productId == VST_USB_defs.PRODUCT_ID_GO_MOTION ||
                productId == VST_USB_defs.PRODUCT_ID_GO_TEMP);
        }
        #endregion Utility Methods

        #region Sensor Builder Methods

        /// <summary>
        /// Builds a <see cref="Sensor"/> object representing the single sensor on the Go!Device
        /// </summary>
        /// <param name="GoDevice">Handle to device you want to build a <see cref="Sensor"/> for</param>
        /// <returns></returns>
        private Sensor buildGoSensor(IntPtr GoDevice)
        {
            byte sensorNumber = getSensorId(GoDevice);
            bool smart = false;
            if (sensorNumber >= VSTSensorDDSMemRec.kSensorIdNumber_FirstSmartSensor)
            {
                GoIO.Sensor_DDSMem_ReadRecord(GoDevice, 0, GoIO.READ_DDSMEMBLOCK_TIMEOUT_MS);
                return new Sensor(sensorNumber, getLongName(GoDevice), getShortName(GoDevice), 
                    getUnits(GoDevice), true, -1, getDecimalPlaces(GoDevice));

            }
            return new Sensor(sensorNumber, getLongName(GoDevice), getShortName(GoDevice),
                getUnits(GoDevice), false, -1, getDecimalPlaces(GoDevice));
        }

        /// <summary>
        /// Gets the number of the sensor on the device
        /// </summary>
        /// <param name="device">Device to get sensor number from</param>
        /// <returns>The sensor number</returns>
        private byte getSensorId(IntPtr device)
        {
            byte num;
            int result = GoIO.Sensor_DDSMem_GetSensorNumber(device, out num, 1, GoIO.TIMEOUT_MS_DEFAULT);

            if (result == 0) return num;
            else throw new GoIOException("Unable to get sensor number"); 
        }
        /// <summary>
        /// Gets the long name of the sensor on the given device
        /// </summary>
        /// <param name="device">The name of the device</param>
        /// <returns>The long name</returns>
        private string getLongName(IntPtr device)
        {
            StringBuilder name = new StringBuilder(GoIO.MAX_SIZE_SENSOR_NAME);
            int result = GoIO.Sensor_DDSMem_GetLongName(device, name, (ushort)name.Capacity).ToInt32();
            if (result != 0) throw new GoIOException("Unable to get sensor long name");
            return name.ToString();
        }
        /// <summary>
        /// Gets the short name of the sensor on the given device
        /// </summary>
        /// <param name="device">Handle to the device to get the name from</param>
        /// <returns>The short name of the device</returns>
        private string getShortName(IntPtr device)
        {
            StringBuilder name = new StringBuilder(GoIO.MAX_SIZE_SENSOR_NAME);
            int result = GoIO.Sensor_DDSMem_GetShortName(device, name, (ushort)name.Capacity).ToInt32();
            if (result != 0) throw new GoIOException("Unable to get sensor long name");
            return name.ToString();
        }
        /// <summary>
        /// Gets the units of the sensor on the given device
        /// </summary>
        /// <param name="device">handle to the device to get the units from</param>
        /// <returns>The units of the device</returns>
        private string getUnits(IntPtr device)
        {
            sbyte equation;
            StringBuilder units = new StringBuilder(20);
            byte activeCalPage = 0;
            GoIO.Sensor_DDSMem_GetCalibrationEquation(device, out equation);
            float a, b, c;
            int result = GoIO.Sensor_DDSMem_GetCalPage(device, activeCalPage, 
                out a, out b, out c, units, (ushort)units.Capacity);
            if (result != 0) throw new GoIOException("Unable to get sensor units");
            return units.ToString();
        }

        /// <summary>
        /// Gets the decimal places of the sensor on the given device
        /// </summary>
        /// <param name="device">handle to the device to get decimal places from</param>
        /// <returns>The number of decimal places</returns>
        private int getDecimalPlaces(IntPtr device)
        {
            int maxVoltage = 5;
            int minVolate = 0;

            double calibratedMax = GoIO.Sensor_CalibrateData(device, maxVoltage);
            double calibratedMin = GoIO.Sensor_CalibrateData(device, minVolate);

            double num = Math.Abs(calibratedMax - calibratedMin) / 4096; //12-bit ADC
            return (int)Math.Ceiling(Math.Abs(Math.Log10(num)));
        }

        #endregion Sensor Builder Methods


    }
}
