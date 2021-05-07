using System;
using System.Text;

namespace EasyOpc.Common.Extension
{
    /// <summary>
    /// Array extensions
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Converts an array to a string
        /// </summary>
        /// <param name="array">Array</param>
        /// <param name="separator">Separator</param>
        /// <returns>String</returns>
        public static string ConvertToString(this Array array, string separator)
        {
            var result = new StringBuilder();

            try
            {
                foreach (var item in array)
                    result.Append($"{item}{separator}");
            }
            catch { }

            return result.ToString();
        }
    }
}
