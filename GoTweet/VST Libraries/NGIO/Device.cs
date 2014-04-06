ï»¿#define TRACE

using System;
using System.Collections.Generic;
using System.Text;
using NGIOdotNET;
using VSTCoreDefsdotNET;
using System.Diagnostics;

namespace GoTweet
{
    /// <summary>
    /// The Device class represents a single LabQuest or LabQuest Device
    /// uniquely identified by its name
    /// </summary>
    /// <remarks>
    /// The best way to get an opened device object is through the <see cref="NGIOLibrary"/> class.
    /// </remarks>
    class Device
    {
        /// <summary>
        /// Handle to the physical device
        /// </summary>
        IntPtr device;
        /// <summary>
        /// Labquest or LabQuest mini
        /// </summary>
        uint type;
        /// <summary>
        /// Unique name of the device from <see cref="NGIOLibrary.getAllConnectedDeviceNames"/>
        /// </summary>
        string name;
        /// <summary>
        /// Whether or not the device is open
        /// </summary>
        public bool open { get; set; }

        /// <summary>
        /// Device constructor. Best not to use it directly but through <see cref="NGIOLibrary"/>
        /// </summary>
        /// <param name="libraryHandle">The library the device belongs to</param>
        /// <param name="name">The unique name of the device (get this from <see cref="NGIOLibrary"/></param>
        /// <param name="type"><see cref="NGIO.DEVTYPE_LABQUEST"/>
        /// or <see cref="NGIO.DEVTYPE_LABQUEST_MINI"/></param>
        public Device(IntPtr libraryHandle, string name, uint type)
        {
            this.type = type;
            this.name = name;
            this.open = false;
        }

       
         
        /// <summary>
        /// Don't call this fuction directly. Instead, open a device by calling <see cref="NGIOLibrary.openDevice"/>.
        ///  libraryHandle is a private member in <see cref="NGIOLibrary"/>
        /// </summary>
        /// <param name="libraryHandle">The library you want to open the device in</param>
        internal void openDevice(IntPtr libraryHandle){
            trace("Attempting to open device");
            device = NGIO.Device_Open(libraryHandle, name, 0);
            if (device == IntPtr.Zero) throw new NGIOException("Unable to open device");
            //Need to take control from LaqQuest's display
            if (this.type == NGIO.DEVTYPE_LABQUEST)
            {
                int status = NGIO.Device_AcquireExclusiveOwnership(device, NGIO.GRAB_DAQ_TIMEOUT_MS);
                if (status != 0) throw new NGIOException("Unable to acquire exclusive ownership");
            }
            openChannels();
            if (this.type == NGIO.DEVTYPE_LABQUEST_MINI)
            {
                NGIOSetLedStateParams param = new NGIOSetLedStateParams();
                param.brightness = NGIOSetLedStateParams.LED_BRIGHTNESS_MAX;
                param.color = NGIOSetLedStateParams.LED_COLOR_GREEN;

                NGIO.Device_SendCmdAndGetResponse2(device, NGIO_ParmBlk.CMD_ID_SET_LED_STATE,
                    param, NGIO.TIMEOUT_MS_DEFAULT);
            }

            this.open = true;
        }

        /// <summary>
        /// Enable all analog channels on the LabQuest and LabQuest mini
        /// </summary>
        private void openChannels()
        {
            NGIOSetSensorChannelEnableMaskParams param = new NGIOSetSensorChannelEnableMaskParams();
            param.lsbyteLsword_EnableSensorChannels =
                NGIO_ParmBlk.CHANNEL_MASK_ANALOG1 + NGIO_ParmBlk.CHANNEL_MASK_ANALOG2 +
                NGIO_ParmBlk.CHANNEL_MASK_ANALOG3 + NGIO_ParmBlk.CHANNEL_MASK_ANALOG4;

            param.lsbyteMsword_EnableSensorChannels = 0;
            param.msbyteLsword_EnableSensorChannels = 0;
            param.msbyteMsword_EnableSensorChannels = 0;

            NGIO.Device_SendCmdAndGetResponse2(device, NGIO_ParmBlk.CMD_ID_SET_SENSOR_CHANNEL_ENABLE_MASK,
                param, NGIO.TIMEOUT_MS_DEFAULT);
        }

        /// <summary>
        /// Closes the device if open
        /// </summary>
        public void closeDevice()
        {
            if(this.open) {
                NGIO.Device_Close(device);
                this.open = false;
            }
        }

        /// <summary>
        /// Queries analog channels on the NGIO device for connected sensors
        /// </summary>
        /// <returns>An array of preconfigured Sensor objects</returns>
        public Sensor[] getConnectedSensors()
        {
            int index = 0;
            Sensor[] sensors = new Sensor[5];
            for (sbyte channel = (sbyte) NGIO_ParmBlk.CHANNEL_ID_ANALOG1; channel <= NGIO_ParmBlk.CHANNEL_ID_ANALOG4; channel++)
            {
                try
                {
                    sensors[index] = getSensor(channel);
                }
                catch (NGIOException e)
                {
                    sensors[index] = null;
                }
                index++;
                
            }
            try
            {
                if(type==NGIO.DEVTYPE_LABQUEST) sensors[index] = getSensor((sbyte)NGIO_ParmBlk.CHANNEL_ID_BUILT_IN_TEMP);
            }
            catch (NGIOException e)
            {
                sensors[index] = null;
            }
            return sensors;
        }

        /// <summary>
        /// Returns a preconfigured <see cref="Sensor"/> in the specified channel. Reads the DDS record if it exists
        /// </summary>
        /// <param name="channel">The desired channel</param>
        /// <returns>The <see cref="Sensor"/> in the channel or null if it does not exist</returns>
        public Sensor getSensor(sbyte channel)
        {
            byte sensorId = getSensorId(channel);
            bool smart = false;
            
            if (sensorId == 0) return null;
            if (sensorId >= VSTSensorDDSMemRec.kSensorIdNumber_FirstSmartSensor)
            {
                smart = true;
                NGIO.Device_DDSMem_ReadRecord(device, channel, 0, NGIO.READ_DDSMEMBLOCK_TIMEOUT_MS);
                VSTSensorDDSMemRec info;
                NGIO.Device_DDSMem_GetRecord(device, channel, out info); 
                return new Sensor(sensorId, getSensorLongName(channel), getSensorShortName(channel), 
                    getSensorUnits(channel), smart, channel, getSensorDecimalPlaces(channel));
            }
            return new Sensor(sensorId, "","", "volts", smart, channel, 3);
        }

        /// <summary>
        /// Gets the number of the sensor in the specified channel
        /// </summary>
        /// <param name="channel">The channel to check for a sensor</param>
        /// <returns>The ID of the sensor in the channel</returns>
        /// <exception cref="NGIOException">No sensor was found</exception>
        private byte getSensorId(sbyte channel)
        {
            byte sensorId, flag=1;
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
        private string getSensorLongName(sbyte channel)  //Assumes sensor is smart and DDS is on computer
        {
            StringBuilder longName = new StringBuilder(30);
            NGIO.Device_DDSMem_GetLongName(device, channel, longName, (ushort) longName.Capacity);
            return longName.ToString();
        }

        /// <summary>
        /// Gets the short name of the sensor from the DDS, assuming it exists, of the specified channel
        /// </summary>
        /// <param name="channel">Channel to query</param>
        /// <returns>The short name of the sensor in the channel</returns>
        private string getSensorShortName(sbyte channel)
        {
            StringBuilder shortName = new StringBuilder(30);
            NGIO.Device_DDSMem_GetShortName(device, channel, shortName, (ushort) shortName.Capacity);
            return shortName.ToString();
        }

        /// <summary>
        /// Gets the units of the sensor from the DDS, assuming it exists, of the specified channel
        /// </summary>
        /// <param name="channel">The channel to query</param>
        /// <returns></returns>
        private string getSensorUnits(sbyte channel) //Assumes sensor is smart and DDS is on computer
        {
            sbyte equation;
            StringBuilder units = new StringBuilder(20);
            byte activeCalPage = 0;
            NGIO.Device_DDSMem_GetCalibrationEquation(device, channel, out equation);
            if (equation == VSTSensorDDSMemRec.kEquationType_Linear)
            {
                float a, b, c;
                NGIO.Device_DDSMem_GetCalPage(device, channel, activeCalPage, 
                    out a, out b, out c, units, (ushort)units.Capacity);
                return units.ToString();
            }
            else return "volts"; //might add support to look in sensor map later
        }

        /// <summary>
        /// Gets the <see cref="VSTCoreDefsdotNET.VSTSensorDDSMemRec.SignificantFigures"/> of the sensor
        /// from the DDS, assuming it exists, of the specified channel
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private byte getSensorSigFigs(VSTSensorDDSMemRec info)
        {
            return info.SignificantFigures;
        }

        private int getSensorDecimalPlaces(sbyte channel)
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
                return (int) Math.Ceiling(Math.Abs(Math.Log10(num)));
            }
            return 3;
        }

        /// <summary>
        /// Reads a number of measurments from the measurement buffer
        /// </summary>
        /// <param name="sensor">The sensor from which to get the data</param>
        /// <param name="desiredNumberOfMeasurements">Max size of the array of results - may be smaller</param>
        /// <returns>An array of converted values</returns>
        public float[] getDataFromSensor(Sensor sensor, int desiredNumberOfMeasurements)
        {
            int[] data = new int[desiredNumberOfMeasurements];
            int actualNumberOfMeasurments = NGIO.Device_ReadRawMeasurements(device, sensor.getChannel(),
                data, null, (uint) desiredNumberOfMeasurements);
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
        /// Convenience method that gets the most recent measurment from the measurement buffer of the specified sensor
        /// </summary>
        /// <param name="sensor">The desired sensor to get the data from</param>
        /// <returns>The most recent value or <see cref="float.MaxValue"/> in case of errors</returns>
        public float getLastMeasurment(Sensor sensor)
        {
            if (sensor == null) return float.MaxValue;
            int numMeasurments = NGIO.Device_GetNumMeasurementsAvailable(device, sensor.getChannel());
            if(numMeasurments > 1) return getDataFromSensor(sensor, 100)[numMeasurments - 1]; //in an array of n-elements, n-1 is the last one
            return float.MaxValue;
        }

        /// <summary>
        /// Sends the device the command to start aquiring data
        /// </summary>
        public void startMeasuring()
        {
            NGIO.Device_SendCmdAndGetResponse4(device, NGIO_ParmBlk.CMD_ID_START_MEASUREMENTS,
                NGIO.TIMEOUT_MS_DEFAULT);
        }

        /// <summary>
        /// Sends the device the command to stop acquiring data
        /// </summary>
        public void stopMeasuring()
        {
            NGIO.Device_SendCmdAndGetResponse4(device, NGIO_ParmBlk.CMD_ID_STOP_MEASUREMENTS,
                NGIO.TIMEOUT_MS_DEFAULT);

        }

        /// <summary>
        /// Traces a message to the output window and/or text file
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
