using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emlin
{
    public static class HelperFunctions
    {
        private static int NUMBER_OF_INPUTS = ConstantValues.NUMBER_OF_INPUTS;

        public static int GetCombinationId(char firstChar, char secondChar)
        {
            int firstId = ((int)firstChar);
            int secondId = ((int)secondChar);

            return (firstId * NUMBER_OF_INPUTS + secondId);
        }

        
    }
}
