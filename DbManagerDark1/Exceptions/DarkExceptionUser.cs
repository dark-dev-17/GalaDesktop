using System;
using System.Collections.Generic;
using System.Text;

namespace DbManagerDark1.Exceptions
{
    public class DarkExceptionUser : Exception
    {
        public DarkExceptionUser()
        {

        }

        public DarkExceptionUser(string mensaje)
            : base(mensaje)
        {

        }
    }
}
