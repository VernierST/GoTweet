ï»¿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NGIOdotNet;
using VSTCoreDefsdotNET;

namespace DeviceConnection
{
    class NGIOInterface
    {

        private NGIOLibrary lib;
        private Device device;
        private Sensor sensor;

        public NGIOInterface()
        {
            lib = NGIOLibrary();
        }

    }
}
