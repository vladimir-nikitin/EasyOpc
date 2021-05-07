using AutoMapper;
using EasyOpc.Common.Types;
using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Core.Repository.Base;
using EasyOpc.WinService.Core.Repository.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Core.Service.Base
{
    /// <summary>
    /// Base service contract
    /// </summary>
    /// <typeparam name="TData">Input/output data type</typeparam>
    /// <typeparam name="TDto">Stored data type</typeparam>
    public class BaseService<TData, TDto> : IBaseService<TData> where TData : IIdentifiable
                                                                      where TDto : BaseDto
    {
        /// <summary>
        /// Repository
        /// </summary>
        protected IBaseRepository<TDto> Repository { get; }

        /// <summary>
        /// Mapper
        /// </summary>
        protected IMapper Mapper { get; }

        /// <summary>
        /// Logger
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repository">Repository</param>
        /// <param name="mapper">Mapper</param>
        /// <param name="logger">Logger</param>
        /// </summary>
        public BaseService(IBaseRepository<TDto> repository, IMapper mapper, ILogger logger)
        {
            Repository = repository;
            Mapper = mapper;
            Logger = logger;
        }

        /// <summary>
        /// <see cref="IBaseService.GetAllAsync"/>
        /// </summary>
        public virtual async Task<IEnumerable<TData>> GetAllAsync()
        {
            try
            {
                return Mapper.Map<IEnumerable<TData>>(await Repository.GetAllAsync());
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="IBaseService.GetByIdAsync(Guid)"/>
        /// </summary> 
        public virtual async Task<TData> GetByIdAsync(Guid id)
        {
            try
            {
                return Mapper.Map<TData>(await Repository.GetByIdAsync(id));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="IBaseService.AddAsync(TData)"/>
        /// </summary>
        public virtual async Task<TData> AddAsync(TData item)
        {
            try
            {
                return Mapper.Map<TData>(await Repository.AddAsync(Mapper.Map<TDto>(item)));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="IBaseService.AddRangeAsync(IEnumerable{TData})"/>
        /// </summary>
        public virtual async Task<IEnumerable<TData>> AddRangeAsync(IEnumerable<TData> items)
        {
            try
            {
                return Mapper.Map<IEnumerable<TData>>(await Repository.AddRangeAsync(Mapper.Map<IEnumerable<TDto>>(items)));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="IBaseService.UpdateAsync(TData)"/>
        /// </summary>
        public virtual async Task<TData> UpdateAsync(TData item)
        {
            try
            {
                return Mapper.Map<TData>(await Repository.UpdateAsync(Mapper.Map<TDto>(item)));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="IBaseService.UpdateRangeAsync(IEnumerable{TData})"/>
        /// </summary>
        public virtual async Task<IEnumerable<TData>> UpdateRangeAsync(IEnumerable<TData> items)
        {
            try
            {
                return Mapper.Map<IEnumerable<TData>>(await Repository.UpdateRangeAsync(Mapper.Map<IEnumerable<TDto>>(items)));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="IBaseService.RemoveByIdAsync(Guid)"/>
        /// </summary>
        public virtual async Task<TData> RemoveByIdAsync(Guid id)
        {
            try
            {
                return Mapper.Map<TData>(await Repository.RemoveByIdAsync(id));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="IBaseService.RemoveAsync(TData)"/>
        /// </summary>
        public virtual async Task<TData> RemoveAsync(TData item)
        {
            try
            {
                return Mapper.Map<TData>(await Repository.RemoveAsync(Mapper.Map<TDto>(item)));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="IBaseService.RemoveRangeAsync(IEnumerable{TData})"/>
        /// </summary>
        public virtual async Task<IEnumerable<TData>> RemoveRangeAsync(IEnumerable<TData> items)
        {
            try
            {
                return Mapper.Map<IEnumerable<TData>>(await Repository.RemoveRangeAsync(Mapper.Map<IEnumerable<TDto>>(items)));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
