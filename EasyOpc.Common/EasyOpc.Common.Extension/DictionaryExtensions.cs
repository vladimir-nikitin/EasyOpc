using System.Collections.Generic;

namespace EasyOpc.Common.Extension
{
    /// <summary>
    /// Dictionary extensions
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Tries to add data to the dictionary
        /// </summary>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="dic">Dictionary</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        public static void TryAdd<TKey, TValue>(this SortedDictionary<TKey, TValue> dic, TKey key, TValue value)
        {
            if (!dic.ContainsKey(key))
            {
                dic.Add(key, value);
            }
        }
    }
}
