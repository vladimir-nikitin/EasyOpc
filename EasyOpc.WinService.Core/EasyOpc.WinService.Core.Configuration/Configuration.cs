using EasyOpc.WinService.Core.Configuration.Contract;
using System.Collections.Generic;

namespace EasyOpc.WinService.Core.Configuration
{
    /// <summary>
    /// Configuration module contract
    /// </summary>
    public class Configuration : IConfiguration
    {
        /// <summary>
        /// Settings dictionary
        /// </summary>
        private IDictionary<string, string> Dictionary { get; }

        /// <summary>
        /// Returns the value of the setting by key 
        /// </summary>
        /// <param name="key">Setting key</param>
        /// <returns>Value</returns>
        public string this[string key] => Dictionary[key];

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dictionary">Settings dictionary</param>
        public Configuration(IDictionary<string, string> dictionary)
        {
            Dictionary = dictionary;
        }
    }
}
