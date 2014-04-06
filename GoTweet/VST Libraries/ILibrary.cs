ï»¿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoTweet
{
    interface ILibrary
    {
       

        /// <summary>
        /// Find how many devices are connected
        /// </summary>
        /// <param name="productId">The type of device you are trying to find</param>
        /// <returns>Number of devices of the type specified</returns>
        int getNumberOfDevices(int productId);
        
        /// <summary>
        /// Opens the nth device found of the type specified. Call <see cref="ILibrary.getNumberOfDevices"/>
        /// </summary>
        /// <param name="productID">The type of device to open</param>
        /// <param name="n">A number between 0 and getNumberOfDevices()</param>
        /// <returns>A handle to the opened device</returns>
        IntPtr openNthDevice(int productID, int n);



        /// <summary>
        /// Get all the sensors connected to the device
        /// </summary>
        /// <param name="device">A handle to the device to look for sensors on</param>
        /// <returns>An array of <see cref="Sensor"/> objects representing probes on the device</returns>
        Sensor[] getConnectedSensors(IntPtr device);



        /// <summary>
        /// Closes a device
        /// </summary>
        /// <param name="device">A handle to the device to close</param>
        void closeDevice(IntPtr device);

        /// <summary>
        /// Send the device the command to start measuring
        /// </summary>
        /// <param name="device">The device desired to start measuring</param>
        void startMeasuring(IntPtr device);

        /// <summary>
        /// Send the device the command to stop measuring
        /// </summary>
        /// <param name="device">The device desired to stop measuring</param>
        void stopMeasuring(IntPtr device);

        /// <summary>
        /// Get the latest measurements from a sensor on a device
        /// </summary>
        /// <param name="device">A handle to the device to get data from</param>
        /// <param name="sensor">The <see cref="Sensor"/> to get data from</param>
        /// <param name="maxNumMeasurements">The maximum number of measurements desired.</param>
        /// <returns>An array of measurements. May not be as long as maxNumMeasurements</returns>
        float[] getMeasurementsFromSensor(IntPtr device, Sensor sensor, int maxNumMeasurements);

        /// <summary>
        /// Convenience method to get the last data point from a sensor
        /// </summary>
        /// <param name="device">Handle to the device </param>
        /// <param name="sensor">The <see cref="Sensor"/> to get data from</param>
        /// <returns>The last measurement</returns>
        float getLastMeasurement(IntPtr device, Sensor sensor);
        
    }
}
