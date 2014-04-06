ï»¿using System;
using System.Collections.Generic;
using System.Text;
using NGIOdotNET;
using VSTCoreDefsdotNET;

namespace GoTweet
{
    /// <summary>
    /// This class represents the many methods that access the NGIOLibrary
    /// </summary>
    /// <remarks>
    /// Remember to call uninit to prevent corruption and memory leaks
    /// </remarks>
    class NGIOLibrary
    {
        /// <summary>
        /// A handle to the NGIO library
        /// </summary>
        IntPtr library;

        /// <summary>
        /// Initilizes the library
        /// </summary>
        /// <exception cref="NGIOException">Thrown if the library can't be initialized</exception>
        public NGIOLibrary()
        {
            library = NGIO.Init();
            NGIO.Diags_SetDebugTraceThreshold(library, NGIO.TRACE_SEVERITY_LOWEST);
            if (library == IntPtr.Zero) throw new NGIOException("Unable to initialize library");
        }

        ///<summary>
        ///Opens a Device List snapshot and returns an array of the names of all devices connected
        ///A device can be opened by creating a new Device object with the name
        ///</summary>
        ///<param name="deviceType">
        ///The type of device you are looking for. Look in the NGIO class for a list of constants
        ///</param>
        ///<returns>An array of strings containing the names of all connected devices</returns>
        ///<exception cref="NGIOException">Check the message of the exception to see error</exception>
        public string[] getAllConnectedDeviceNames(uint deviceType){
            uint s; //the signature - only used to satisfy the parameter list
            NGIO.SearchForDevices(library, deviceType, NGIO.COMM_TRANSPORT_USB, IntPtr.Zero, out s);

            uint numberOfDevices = 0; //the number of devices found of the specifed type
            IntPtr deviceList = NGIO.OpenDeviceListSnapshot(library, deviceType, out numberOfDevices, out s);
            if (deviceList == IntPtr.Zero) throw new NGIOException("Unable to open device list snapshot");

            uint sm; //status mask - only used to satisfy the parameter list

            string[] devices = new string[numberOfDevices];
            for (uint N = 0; N < numberOfDevices; N++)
            {
                StringBuilder deviceName = new StringBuilder((int)(NGIO.MAX_SIZE_DEVICE_NAME));
                int status = NGIO.DeviceListSnapshot_GetNthEntry(deviceList, N, deviceName, 
                    NGIO.MAX_SIZE_DEVICE_NAME, out sm);
                if (status != 0) throw new NGIOException("Unable to get device name");

                devices[N] = deviceName.ToString();
            }
            return devices;
        }

        
        ///<summary>Opens the given <see cref="Device"/>. Remember to call close on the device when done!</summary>
        ///<param name="device">The device desired to be opened</param>
        public void openDevice(Device device)
        {
            device.openDevice(library);
        }

        
        ///<summary>Opens a <see cref="Device"/> with the given unique name. The type helps determine if exclusive
        ///ownership is necessary</summary>
        ///<param name="name">A unique name given from a value in the array from  <see cref="getAllConnectedDeviceNames"/>)</param>
        ///<param name="type">The type of device needed to be open</param>
        ///<returns>An open <see cref="Device"/> ready for taking measurements and retreiving sensor data</returns>
         
        public Device openDeviceByName(string name, uint type)
        {
            Device d = new Device(library, name, type);
            openDevice(d);
            return d;
        }

        /// <summary>
        /// Unintializes the library. Remember to call this before finishing the application
        /// </summary>
        public void uninit()
        {
            NGIO.Uninit(library);
        }

    }
}
