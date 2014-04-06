ï»¿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoIOdotNET;
using VSTCoreDefsdotNET;

namespace GoDeviceConnect
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initialzing library...");
            IntPtr initResult = GoIO.Init();
            if (initResult.ToInt32() != 0)
            {
                Console.Write("Unable to initialize library");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Updating list of available Go!Temps");
            int numGoTempsFound = GoIO.UpdateListOfAvailableDevices(
                VST_USB_defs.VENDOR_ID, VST_USB_defs.PRODUCT_ID_GO_TEMP);

            StringBuilder deviceName = new StringBuilder(GoIO.MAX_SIZE_SENSOR_NAME);

            Console.WriteLine("Getting first device name");
            int status = GoIO.GetNthAvailableDeviceName(deviceName, deviceName.Capacity,
                VST_USB_defs.VENDOR_ID, VST_USB_defs.PRODUCT_ID_GO_TEMP, 0);

            if (status != 0)
            {
                Console.WriteLine("Unable to get the first device name");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Opening device");
            IntPtr sensorHandle = GoIO.Sensor_Open(deviceName.ToString(), VST_USB_defs.VENDOR_ID, 
                VST_USB_defs.PRODUCT_ID_GO_TEMP, 0);
            if (sensorHandle == IntPtr.Zero)
            {
                Console.WriteLine("Unable to open sensor.");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("Starting measurements");
            GoIO.Sensor_SendCmdAndGetResponse4(sensorHandle, GoIO_ParmBlk.CMD_ID_START_MEASUREMENTS,
                GoIO.TIMEOUT_MS_DEFAULT);
            System.Threading.Thread.Sleep(10000);
            Console.WriteLine("Stoping measurments");
            GoIO.Sensor_SendCmdAndGetResponse4(sensorHandle, GoIO_ParmBlk.CMD_ID_STOP_MEASUREMENTS, 
                GoIO.TIMEOUT_MS_DEFAULT);

            int MAX = 10;
            int[] raw = new int[MAX];
            int numMeasurements = GoIO.Sensor_ReadRawMeasurements(sensorHandle, raw, (uint) raw.Length);

            double[] results = new double[numMeasurements];
            for (int i = 0; i < numMeasurements; i++)
            {
                results[i] = GoIO.Sensor_CalibrateData(sensorHandle, GoIO.Sensor_ConvertToVoltage(sensorHandle, raw[i]));
            }

            foreach (double d in results)
            {
                Console.WriteLine(d);
            }

            Console.WriteLine("Closing sensor handle");
            GoIO.Sensor_Close(sensorHandle);

            Console.WriteLine("Uninitializing library");
            GoIO.Uninit();

            Console.Write("Press <Enter> to exit");
            Console.ReadLine();

        }
    }
}
