using EasyOpc.Common.Types;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Core.Repository.Base
{
    /// <summary>
    /// Base repository contract
    /// </summary>
    /// <typeparam name="TDto">Stored data type</typeparam>
    public interface IBaseRepository<TDto> where TDto : IIdentifiable
    {
        /// <summary>
        /// Returns all objects
        /// </summary>
        /// <returns>List of objects</returns>
        Task<IEnumerable<TDto>> GetAllAsync();

        /// <summary>
        /// Returns an object by its id
        /// </summary>
        /// <param name="id">Object identifier</param>
        /// <returns>Object</returns>
        Task<TDto> GetByIdAsync(Guid id);

        /// <summary>
        /// Adds an object
        /// </summary>
        /// <param name="item">Object</param>
        /// <returns>Object</returns>
        Task<TDto> AddAsync(TDto item);

        /// <summary>
        /// Adds a list of objects
        /// </summary>
        /// <param name="items">List of objects</param>
        /// <returns>List of objects</returns>
        Task<IEnumerable<TDto>> AddRangeAsync(IEnumerable<TDto> items);

        /// <summary>
        /// Updates the object
        /// </summary>
        /// <param name="item">Object</param>
        /// <returns>Object</returns>
        Task<TDto> UpdateAsync(TDto item);

        /// <summary>
        /// Refreshes the list of objects
        /// </summary>
        /// <param name="items">List of objects</param>
        /// <returns>List of objects</returns>
        Task<IEnumerable<TDto>> UpdateRangeAsync(IEnumerable<TDto> items);

        /// <summary>
        /// Removes an object
        /// </summary>
        /// <param name="id">Object identifier</param>
        /// <returns>Object</returns>
        Task<TDto> RemoveByIdAsync(Guid id);

        /// <summary>
        /// Removes an object
        /// </summary>
        /// <param name="item">Object</param>
        /// <returns>Object</returns>
        Task<TDto> RemoveAsync(TDto item);

        /// <summary>
        /// Removes the list object
        /// </summary>
        /// <param name="items">List of objects</param>
        /// <returns>List of objects</returns>
        Task<IEnumerable<TDto>> RemoveRangeAsync(IEnumerable<TDto> items);
    }
}
