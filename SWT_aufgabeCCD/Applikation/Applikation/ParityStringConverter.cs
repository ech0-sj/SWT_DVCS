using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace Tools
{
    internal class ParityStringConverter
    {
        public static String ParityToString(Parity parity)
        {
            return parity.ToString();
        }

        public static Parity StringToParity(String paritystring)
        {
            foreach (Parity parity in (Parity[])Enum.GetValues(typeof(Parity)))
            {
                if (paritystring.Equals(parity.ToString()))
                {
                    return parity;
                }
            }
            throw new KeyNotFoundException(paritystring + " is not a parity type");
        }

        public static List<String> GetParityStringlist()
        {
            List<String> paritylist = new List<string>();

            foreach (Parity parity in (Parity[])Enum.GetValues(typeof(Parity)))
            {
                paritylist.Add(parity.ToString());
            }
            return paritylist;
        }
    }
}