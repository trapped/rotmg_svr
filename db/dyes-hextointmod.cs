using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace db
{
    static public class dyes_hextointmod
    {
        public static int hextoint(String hex, bool cloth)
        {
            int converted = 0;
            if (hex.StartsWith("0x"))
            {
                hex = hex.Substring(2);
                if (cloth == true)
                    hex = "0" + hex;
                converted = Convert.ToInt32(hex, 16);
            }
            else
                if (cloth == true)
                    hex = "0" + hex;
                converted = Convert.ToInt32(hex, 16);
            return converted;
        }
    }
}
