using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komaxApp.Utility.Extensions
{
    public static class Int32Extensions
    {
        public static int ToInt(this string s)
        {
            int returnInt = 0;

            try
            {
                returnInt = Int32.Parse(s);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return returnInt;
        }
    }

}
