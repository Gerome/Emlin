namespace Emlin
{
    public static class HelperFunctions
    {
        private static int NUMBER_OF_INPUTS = ConstantValues.NUMBER_OF_INPUTS;

        public static int GetCombinationId(char firstChar, char secondChar)
        {
            int firstId = ((int)firstChar);
            int secondId = ((int)secondChar);

            return 6000 * (firstId * NUMBER_OF_INPUTS + secondId);
        }
    }
}
