using System;
using System.Collections.Generic;
using System.Text;

namespace DbManagerDark1.Exceptions
{
    public class DarkExceptionSystem : Exception
    {
        public DarkExceptionSystem()
        {

        }

        public DarkExceptionSystem(string mensaje)
            : base(mensaje)
        {

        }
    }
}
