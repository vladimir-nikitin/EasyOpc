using EasyOpc.Common.Types;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Core.Service.Base
{
    /// <summary>
    /// Base service contract
    /// </summary>
    /// <typeparam name="TData">Input/output data type</typeparam>
    public interface IBaseService<TData> where TData : IIdentifiable
    {
        /// <summary>
        /// Returns all objects
        /// </summary>
        /// <returns>List of objects</returns>
        Task<IEnumerable<TData>> GetAllAsync();

        /// <summary>
        /// Returns an object by its id
        /// </summary>
        /// <param name="id">Object identifier</param>
        /// <returns>Object</returns>
        Task<TData> GetByIdAsync(Guid id);

        /// <summary>
        /// Adds an object
        /// </summary>
        /// <param name="item">Object</param>
        /// <returns>Object</returns>
        Task<TData> AddAsync(TData item);

        /// <summary>
        /// Adds a list of objects
        /// </summary>
        /// <param name="items">List of objects</param>
        /// <returns>List of objects</returns>
        Task<IEnumerable<TData>> AddRangeAsync(IEnumerable<TData> items);

        /// <summary>
        /// Updates the object
        /// </summary>
        /// <param name="item">Object</param>
        /// <returns>Object</returns>
        Task<TData> UpdateAsync(TData item);

        /// <summary>
        /// Refreshes the list of objects
        /// </summary>
        /// <param name="items">List of objects</param>
        /// <returns>List of objects</returns>
        Task<IEnumerable<TData>> UpdateRangeAsync(IEnumerable<TData> items);

        /// <summary>
        /// Removes an object
        /// </summary>
        /// <param name="id">Object identifier</param>
        /// <returns>Object</returns>
        Task<TData> RemoveByIdAsync(Guid id);

        /// <summary>
        /// Removes an object
        /// </summary>
        /// <param name="item">Object</param>
        /// <returns>Object</returns>
        Task<TData> RemoveAsync(TData item);

        /// <summary>
        /// Removes the list object
        /// </summary>
        /// <param name="items">List of objects</param>
        /// <returns>List of objects</returns>
        Task<IEnumerable<TData>> RemoveRangeAsync(IEnumerable<TData> items);
    }
}
