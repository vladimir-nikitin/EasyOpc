using EasyOpc.Common.Extension;
using EasyOpc.Common.Types;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Core.Repository.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Core.Repository.Base
{
    /// <summary>
    /// Base repository
    /// </summary>
    /// <typeparam name="TContext">Database context type</typeparam>
    /// <typeparam name="TDto">Stored data type</typeparam>
    public abstract class BaseRepository<TContext, TDto> : IBaseRepository<TDto> where TContext : DbContext
                                                                                 where TDto : BaseDto
    {
        /// <summary>
        /// Logger
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// Database context
        /// </summary>
        protected TContext DbContext { get; }

        /// <summary>
        /// List of stored objects
        /// </summary>
        protected DbSet<TDto> Entities { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext">Database context</param>
        /// <param name="logger">Logger</param>
        /// </summary>
        public BaseRepository(TContext dbContext, ILogger logger)
        {
            DbContext = dbContext;
            Logger = logger;

            var tableAttribute = (TableAttribute)Attribute.GetCustomAttribute(typeof(TDto), typeof(TableAttribute));
            Entities = dbContext.GetPropertyValue<TContext, DbSet<TDto>>(tableAttribute.Name);
        }

        /// <summary>
        /// <see cref="IBaseRepository.GetAllAsync"/>
        /// </summary>
        public virtual async Task<IEnumerable<TDto>> GetAllAsync()
        {
            try
            {
                return await Entities.ToListAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="IBaseRepository.GetByIdAsync(Guid)"/>
        /// </summary>
        public virtual async Task<TDto> GetByIdAsync(Guid id)
        {
            try
            {
                return await Entities.FirstOrDefaultAsync(i => i.Id == id);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="IBaseRepository.AddAsync(TDto)"/>
        /// </summary>
        public virtual async Task<TDto> AddAsync(TDto entity)
        {
            TDto result;
            try
            {
                result = Entities.Add(entity);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }

            return result;
        }

        /// <summary>
        /// <see cref="IBaseRepository.AddRangeAsync(IEnumerable{TDto})"/>
        /// </summary>
        public virtual async Task<IEnumerable<TDto>> AddRangeAsync(IEnumerable<TDto> entities)
        {
            IEnumerable<TDto> result = new List<TDto>();

            try
            {
                Entities.AddRange(entities);
                await DbContext.SaveChangesAsync();
                result = entities;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }

            return result;
        }

        /// <summary>
        /// <see cref="IBaseRepository.UpdateAsync(TDto)"/>
        /// </summary>
        public virtual async Task<TDto> UpdateAsync(TDto entity)
        {
            TDto result = null;

            try
            {
                result = await GetByIdAsync(entity.GetPropertyValue<TDto, Guid>(nameof(IIdentifiable.Id)));
                result.UpdateAllProperties(entity);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }

            return result;
        }

        /// <summary>
        /// <see cref="IBaseRepository.UpdateRangeAsync(IEnumerable{TDto})"/>
        /// </summary>
        public virtual async Task<IEnumerable<TDto>> UpdateRangeAsync(IEnumerable<TDto> entities)
        {
            var result = new List<TDto>();

            TDto currentItem = null;
            foreach (var entity in entities)
            {
                try
                {
                    currentItem = await GetByIdAsync(entity.GetPropertyValue<TDto, Guid>(nameof(IIdentifiable.Id)));
                    currentItem.UpdateAllProperties(entity);
                    result.Add(currentItem);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    throw;
                }
            }

            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }

            return result;
        }

        /// <summary>
        /// <see cref="IBaseRepository.RemoveByIdAsync(Guid)(Guid)"/>
        /// </summary>
        public virtual async Task<TDto> RemoveByIdAsync(Guid id)
        {
            TDto result;
            try
            {
                result = Entities.Remove(await Entities.FirstOrDefaultAsync(p => p.Id == id));
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
            return result;
        }

        /// <summary>
        /// <see cref="IBaseRepository.RemoveAsync(TDto)"/>
        /// </summary>
        public virtual async Task<TDto> RemoveAsync(TDto entity) => await RemoveByIdAsync(entity?.Id ?? Guid.Empty);

        /// <summary>
        /// <see cref="IBaseRepository.RemoveRangeAsync(IEnumerable{TDto})"/>
        /// </summary>
        public virtual async Task<IEnumerable<TDto>> RemoveRangeAsync(IEnumerable<TDto> entities)
        {
            try
            {
                var ids = entities.Select(x => x.Id).ToList();
                var removeItems = Entities.Where(p => ids.Contains(p.Id)).ToList();
                Entities.RemoveRange(removeItems);

                await DbContext.SaveChangesAsync();
                return removeItems;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
