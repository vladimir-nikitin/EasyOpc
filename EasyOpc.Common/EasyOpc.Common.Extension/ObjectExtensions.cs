using System.Linq;

namespace EasyOpc.Common.Extension
{
    /// <summary>
    /// Object extensions
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Returns the value of the property
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="TResult">Returned data type</typeparam>
        /// <param name="item">Object</param>
        /// <param name="propertyName">Property name</param>
        /// <returns>Value of the property</returns>
        public static TResult GetPropertyValue<T, TResult>(this T item, string propertyName)
        {
            return (TResult)typeof(T).GetProperty(propertyName).GetValue(item);
        }

        /// <summary>
        /// Returns the value of the property
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="item">Object</param>
        /// <param name="propertyName">Property name</param>
        /// <returns>Property value as an object</returns>
        public static object GetPropertyValue<T>(this T item, string propertyName)
        {
            return typeof(T).GetProperty(propertyName).GetValue(item);
        }

        /// <summary>
        /// Updates the value of all properties
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="item">Refreshable object</param>
        /// <param name="newItem">New object</param>
        public static void UpdateAllProperties<T>(this T item, T newItem)
        {
            typeof(T).GetProperties().ToList().ForEach(p => p.SetValue(item, newItem.GetPropertyValue<T>(p.Name)));
        }
    }
}