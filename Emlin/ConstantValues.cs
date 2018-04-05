using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Emlin
{
    public static class ConstantValues
    {
        public const int NUMBER_OF_INPUTS = 128;
        public const int NUMBER_OF_COOMBINATIONS = NUMBER_OF_INPUTS * NUMBER_OF_INPUTS;
        public const int LENGTH_OF_SESSION_IN_MILLIS = 1500;
        public static string KEYBOARD_DATA_FILEPATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) , Assembly.GetCallingAssembly().GetName().Name);
    }
}
