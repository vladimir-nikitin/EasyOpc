namespace EasyOpc.Common.Extensions
{
    /// <summary>
    /// String extensions
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts a string to bool
        /// </summary>
        /// <param name="value">String</param>
        /// <returns>Bool</returns>
        public static bool ConvertToBool(this string value) =>
            bool.Parse(value ?? "False");

        /// <summary>
        /// Converts a string to Int32
        /// </summary>
        /// <param name="value">String</param>
        /// <returns>Int32</returns>
        public static int ConvertToInt32(this string value)
        {
            int.TryParse(value, out int result);
            return result;
        }
    }
}
