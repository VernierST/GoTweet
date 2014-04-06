ï»¿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NGIOdotNET;
using VSTCoreDefsdotNET;

namespace DeviceConnection
{
    class Device
    {
        IntPtr device;
        uint type;
        string name;
        bool open;
        Sensor[] sensors;

        public Device(IntPtr libraryHandle, string name, uint type)
        {
            this.type = type;
            this.name = name;
            this.open = false;
        }

        /*
         * Don't call this fuction directly. Instead, open a device by calling NGIOLibrary.openDevice(string name)
         * libraryHandle is a private member in NGIOLibrary
         */ 
        internal void openDevice(IntPtr libraryHandle){
            device = NGIO.Device_Open(libraryHandle, name, 0);
            if (device == IntPtr.Zero) throw new NGIOException("Unable to open device");

            //Need to take control from LaqQuest's display
            if (this.type == NGIO.DEVTYPE_LABQUEST)
            {
                int status = NGIO.Device_AcquireExclusiveOwnership(device, NGIO.GRAB_DAQ_TIMEOUT_MS);
                if (status == 0) throw new NGIOException("Unable to acquire exclusive ownership");
                //TODO: Thrown an exception if unable to get exclusive ownership
            }

            this.open = true;
        }

        /*
         * <summary>Closes the device if open</summary>
         */
        public void closeDevice()
        {
            if(this.open) {
                NGIO.Device_Close(device);
                this.open = false;
            }
        }

        public void getConnectedSensors()
        {
            int index = 0;
            this.sensors = new Sensor[4];
            for (sbyte channel = (sbyte) NGIO_ParmBlk.CHANNEL_ID_ANALOG1; channel <= NGIO_ParmBlk.CHANNEL_ID_ANALOG4; channel++)
            {
                sensors[index] = getSensor(channel);
                index++;
            }
        }

        public Sensor getSensor(sbyte channel)
        {
            byte sensorId = getSensorId(channel);
            bool smart = false;
            if (sensorId >= VSTSensorDDSMemRec.kSensorIdNumber_FirstSmartSensor)
            {
                smart = true;
                NGIO.Device_DDSMem_ReadRecord(device, channel, 0, NGIO.READ_DDSMEMBLOCK_TIMEOUT_MS);
                return new Sensor(sensorId, getSensorLongName(channel), getSensorShortName(channel), getSensorUnits(channel), "", smart, channel); //TOD0: GET PROBE TYPE
            }
            return new Sensor(sensorId, "","", "volts", "", smart, channel);
        }

        private byte getSensorId(sbyte channel)
        {
            byte sensorId, flag=1;
            uint channelSig;

            NGIO.Device_DDSMem_GetSensorNumber(device, channel,
                out sensorId, flag, out channelSig, NGIO.DEFAULT_CMD_RESPONSE_TIMEOUT_MS);

            if (sensorId == 0) throw new NGIOException("Unable to retreive sensor id");

            return sensorId;
        }

        private string getSensorLongName(sbyte channel)  //Assumes sensor is smart and DDS is on computer
        {
            StringBuilder longName = new StringBuilder(30);
            NGIO.Device_DDSMem_GetLongName(device, channel, longName, (ushort) longName.Capacity);
            return longName.ToString();
        }
        private string getSensorShortName(sbyte channel)
        {
            StringBuilder shortName = new StringBuilder(30);
            NGIO.Device_DDSMem_GetShortName(device, channel, shortName, (ushort) shortName.Capacity);
            return shortName.ToString();
        }
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

        public float[] getDataFromSensor(Sensor s, int desiredNumberOfMeasurements)
        {
            int[] data = new int[desiredNumberOfMeasurements];
            int actualNumberOfMeasurments = NGIO.Device_ReadRawMeasurements(device, s.getChannel(),
                data, null, (uint) desiredNumberOfMeasurements);
            if (actualNumberOfMeasurments > 0)
            {
                float[] result = new float[actualNumberOfMeasurments];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = NGIO.Device_CalibrateData2(device, s.getChannel(), data[i]);
                }
                return result;
            }
            return null;
        }

        public void startMeasuring()
        {
            NGIO.Device_SendCmdAndGetResponse4(device, NGIO_ParmBlk.CMD_ID_START_MEASUREMENTS,
                NGIO.DEFAULT_CMD_RESPONSE_TIMEOUT_MS);
        }
        public void stopMeasuring()
        {
            NGIO.Device_SendCmdAndGetResponse4(device, NGIO_ParmBlk.CMD_ID_STOP_MEASUREMENTS,
                NGIO.DEFAULT_CMD_RESPONSE_TIMEOUT_MS);
        }




    }
}
