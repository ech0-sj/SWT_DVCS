using System;
using System.Collections.Generic;
using System.Text;

namespace Tools
{
    internal class Typeconverter
    {
        public static String ListByteToString(List<Byte> bytelist)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            return encoder.GetString(bytelist.ToArray());
        }
    }
}