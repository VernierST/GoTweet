ï»¿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoTweet
{
    class GoIOException:Exception
    {
        public GoIOException(string message)
            : base(message)
        {
        }
    }
}
