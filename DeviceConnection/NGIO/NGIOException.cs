ï»¿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeviceConnection
{
    class NGIOException: System.ApplicationException
    {
        public NGIOException(string message):base(message)
        {
        }
    }
}
