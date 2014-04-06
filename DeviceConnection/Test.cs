ï»¿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NGIOdotNET;


namespace DeviceConnection
{
    class Test
    {
        public static void Main()
        {
            NGIOLibrary lib = null;
            Device device = null;
            Sensor sensor = null;
            try
            {
                lib = new NGIOLibrary();
                uint deviceType = NGIO.DEVTYPE_LABQUEST_MINI;
                string[] deviceNames = lib.getAllConnectedDeviceNames(deviceType);
                Console.ReadLine();
                device = lib.openDeviceByName(deviceNames[0], deviceType);
                sensor = device.getSensor((sbyte)NGIO_ParmBlk.CHANNEL_ID_ANALOG1);
                Console.WriteLine(sensor.getLongName());
                Console.Write("Hit enter to start...");
                Console.ReadLine();
                device.startMeasuring();

                for (int i = 0; i < 30; i++)
                {
                    System.Threading.Thread.Sleep(500); //pause half a second
                    Console.WriteLine(device.getDataFromSensor(sensor, 1)[0] + "\n");
                }

                device.stopMeasuring();
                /*float[] data = device.getDataFromSensor(sensor, 10);
                foreach(float f in data){
                    Console.WriteLine(f);
                }*/
                Console.ReadLine();
                device.closeDevice();
                lib.uninit();
            }
            catch (NGIOException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                if (device != null) device.closeDevice();
                if (lib != null) lib.uninit(); //call this statement after the previous one
                return;
            }
            


        }
    }
}
