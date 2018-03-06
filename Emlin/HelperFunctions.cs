using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emlin
{
    public static class HelperFunctions
    {
        private static int DECIMAL_ASCII_OF_FIRST_CHAR = 32;
        private static int NUMBER_OF_INPUTS = 96;

        public static int GetCombinationId(char firstChar, char secondChar)
        {
            int firstId = ((int)firstChar) - DECIMAL_ASCII_OF_FIRST_CHAR;
            int secondId = ((int)secondChar) - DECIMAL_ASCII_OF_FIRST_CHAR;

            return (firstId * NUMBER_OF_INPUTS + secondId);
        }
    }
}
