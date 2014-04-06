ï»¿using System;
using System.Collections.Generic;
using System.Text;

namespace GoTweet
{
    /// <summary>
    /// Thrown when a command to the NGIO fails
    /// </summary>
    class NGIOException: System.ApplicationException
    {
        /// <summary>
        /// Initializes the Exception with a message
        /// </summary>
        /// <param name="message">The exceptio's message</param>
        public NGIOException(string message):base(message)
        {
        }
    }
}
