ï»¿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NGIOdotNET;
using VSTCoreDefsdotNET;

namespace DeviceConnection
{
    class Program
    {
        /*
        static void Main(string[] args)
        {
            Console.WriteLine("Initializing...");
            IntPtr ngiolib = NGIO.Init(); //handle to the NGIO device library

            //Not sure what to initiliaze to?
            uint signature;
            uint numDevices;
            
            const uint FIRST_DEVICE = 0;

            if (ngiolib == IntPtr.Zero)
            {
                Console.WriteLine("Couldn't connect to library!");
                NGIO.Uninit(ngiolib);
                Exit();
                return;
            }

            Console.WriteLine("Searching for LabQuest Mini devices...");
            NGIO.SearchForDevices(ngiolib, NGIO.DEVTYPE_LABQUEST_MINI, NGIO.COMM_TRANSPORT_USB,
                IntPtr.Zero, out signature);

            Console.WriteLine("Opening device list snapshot...");
            IntPtr deviceList = NGIO.OpenDeviceListSnapshot(ngiolib, NGIO.DEVTYPE_LABQUEST_MINI,
                out numDevices, out signature);

            if (deviceList == IntPtr.Zero)
            {
                Console.WriteLine("Couldn't open device list...");
                NGIO.Uninit(ngiolib);
                Exit();
                return;
            }

            //Connect to the first device found
            StringBuilder deviceName = new StringBuilder((int)(NGIO.MAX_SIZE_DEVICE_NAME)); //That should work
            uint status_mask;
            Console.WriteLine("Trying to get first entry from snapshot...");
            int status = NGIO.DeviceListSnapshot_GetNthEntry(deviceList, FIRST_DEVICE, deviceName,
                NGIO.MAX_SIZE_DEVICE_NAME, out status_mask);
            if (status != 0)
            {
                Console.WriteLine("Unable to get device name from snapshot...");
                NGIO.CloseDeviceListSnapshot(deviceList);
                NGIO.Uninit(ngiolib);
                return;
            }
            Console.WriteLine("Closing device list...");
            NGIO.CloseDeviceListSnapshot(deviceList);

            Console.WriteLine("Opening device "+deviceName.ToString()+"... ");
            IntPtr device = NGIO.Device_Open(ngiolib, deviceName.ToString(), 0); //Hopefully that ToString works!

            if (device == IntPtr.Zero)
            {
                Console.WriteLine("Unable to open device...");
                NGIO.Uninit(ngiolib);
                Exit();
                return;
            }

            //LabQuest Only

            /*Console.WriteLine("Attempting to get exclusive ownership of device...");
            status = NGIO.Device_AcquireExclusiveOwnership(device, NGIO.GRAB_DAQ_TIMEOUT_MS);
            if (status != 0)
            {
                Console.WriteLine("Unable to acquire exclusive ownership of device");
                NGIO.Device_Close(device);
                NGIO.Uninit(ngiolib);
                Exit();
                return;
            }*/
        /*
            Console.Write("Beginning measurement period. Press <ENTER> to begin...");
            Console.ReadLine();
            NGIO.Device_SendCmdAndGetResponse4(device, NGIO_ParmBlk.CMD_ID_START_MEASUREMENTS, 
                NGIO.DEFAULT_CMD_RESPONSE_TIMEOUT_MS);

            Console.Write("Press <Enter> again to stop the measurements....");
            Console.ReadLine();

            Console.WriteLine("Stopping measurements....");
            NGIO.Device_SendCmdAndGetResponse4(device, NGIO_ParmBlk.CMD_ID_STOP_MEASUREMENTS, 
                NGIO.DEFAULT_CMD_RESPONSE_TIMEOUT_MS);

            Console.WriteLine("Getting sensor number of sensor in Analog Channel 1...");
            byte sensorNumber, flag=1;
            uint channelSig;

            sbyte channel = (sbyte) NGIO_ParmBlk.CHANNEL_ID_ANALOG1; //I don't trust the cast

            NGIO.Device_DDSMem_GetSensorNumber(device, channel ,
                out sensorNumber, flag, out channelSig, NGIO.DEFAULT_CMD_RESPONSE_TIMEOUT_MS);

            if (sensorNumber == 0)
            {
                Console.WriteLine("Unable to get sensor number...");
                NGIO.Device_Close(device);
                NGIO.Uninit(ngiolib);
                Exit();
                return;
            }

            Console.WriteLine("Determining if sensor is smart....");
            bool smart = (sensorNumber >= VSTSensorDDSMemRec.kSensorIdNumber_FirstSmartSensor);

            if (smart)
            {
                Console.WriteLine("Sensor is smart! Retreiving DDS record");
                NGIO.Device_DDSMem_ReadRecord(device, channel, 0, NGIO.READ_DDSMEMBLOCK_TIMEOUT_MS);
            }

            Console.WriteLine("Getting probe type...");
            int probeType = NGIO.Device_GetProbeType(device, channel);

            Console.WriteLine("Averaging data...");

            int[] results = new int[50];
            int numMeasurements = NGIO.Device_ReadRawMeasurements(device, channel, results, null, 50);
            double average = 0.0;
            if (numMeasurements > 0)
            {
                
                for (int i = 0; i < numMeasurements; i++)
                {
                    //double volts = NGIO.Device_ConvertToVoltage(device, channel, results[i], probeType);
                    double calbMeas = NGIO.Device_CalibrateData2(device, channel, results[i]);
                    //double calibratedValue = NGIO.Device_Cal
                    average += calbMeas;
                }
                average /= numMeasurements;
            }

            Console.WriteLine("Getting units..");
            sbyte equation;
            StringBuilder units = new StringBuilder(20);
            byte activeCalPage = 0;
            NGIO.Device_DDSMem_GetCalibrationEquation(device, channel, out equation);
            if (equation == VSTSensorDDSMemRec.kEquationType_Linear)
            {
                float a, b, c;
                NGIO.Device_DDSMem_GetCalPage(device, channel, activeCalPage, out a, out b, out c, units, (ushort)units.Capacity);
            }
            else units.Append("volts");

            Console.WriteLine("The result is....");
            Console.WriteLine(average + " "+units.ToString());

            Console.WriteLine("Cleaning up library...");
            NGIO.Device_Close(device);
            NGIO.Uninit(ngiolib);

            //Exit();
            
            return;
        }

        static void Exit()
        {
            Console.Write("Press <Enter> to exit...");
            Console.ReadLine();
        }


    }*/
    }
}
