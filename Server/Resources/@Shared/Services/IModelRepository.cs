#region Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Server.Resources.Shared.DTOs;
using Server.Resources.Shared.Exceptions;
using Server.Resources.Shared.Helpers;
using Server.Resources.Shared.Models;
#endregion // Imports

namespace Server.Resources.Shared.Services
{
    public interface IModelRepository<TDomain> where TDomain : BaseModel<TDomain>, new()
    {
        Task<TResponse> ReadModel<TResponse>(string id) where TResponse : BaseResponseDTO<TResponse>, new();
        Task<PagedList<TResponse>> ReadModels<TResponse>(FilterParams filters) where TResponse : BaseResponseDTO<TResponse>, new();
        Task<TResponse> CreateModel<TRequest, TResponse>(TRequest model) where TRequest : BaseDTO where TResponse : BaseResponseDTO<TResponse>, new();
        Task<PagedList<TResponse>> CreateModels<TRequest, TResponse>(List<TRequest> models) where TRequest : BaseDTO where TResponse : BaseResponseDTO<TResponse>, new();
        Task<TResponse> UpdateModel<TRequest, TResponse>(TRequest model) where TRequest : BaseDTO where TResponse : BaseResponseDTO<TResponse>, new();
        Task<PagedList<TResponse>> UpdateModels<TRequest, TResponse>(List<TRequest> models) where TRequest : BaseDTO where TResponse : BaseResponseDTO<TResponse>, new();
        Task<TResponse> DeleteModel<TResponse>(string id) where TResponse : BaseResponseDTO<TResponse>, new();
        Task<PagedList<TResponse>> DeleteModels<TResponse>(List<string> models) where TResponse : BaseResponseDTO<TResponse>, new();
    }

    /// <typeparam name="TDomain">Represents the current domain model which inherits from the BaseModel class</typeparam>
    /// <typeparam name="TContext">Represents an instance of DbContext containing the current TDomain data set</typeparam>
    public class ModelRepository<TDomain, TContext> : IModelRepository<TDomain>
        where TDomain : BaseModel<TDomain>, new() where TContext : DbContext
        {
            private readonly TContext context;
            private readonly IMapper mapper;

            public ModelRepository(TContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            #region External Repository Methods
            public virtual async Task<TResponse> ReadModel<TResponse>(string id) where TResponse : BaseResponseDTO<TResponse>, new()
            {
                return await this.Process<TResponse>(
                    function: async() =>
                    {
                        TDomain dbResult = await this.context
                            .Set<TDomain>()
                            .FirstAsync(item => item.ID == Guid.Parse(id));
                        return this.mapper
                            .Map<TDomain, TResponse>(dbResult, new TResponse());
                    }, error : new NoContentException());
            }
            public virtual async Task<PagedList<TResponse>> ReadModels<TResponse>(FilterParams filters) where TResponse : BaseResponseDTO<TResponse>, new()
            {
                return await this.Process<PagedList<TResponse>>(
                    function: async() =>
                    {
                        IEnumerable<TDomain> dbResults = await this.context
                            .Set<TDomain>()
                            .Include(entry => entry.Parents.Take(filters.IncludeParents == true ? entry.Parents.Count : 0))
                            .Include(entry => entry.Children.Take(filters.IncludeChildren == true ? entry.Children.Count : 0))
                            .Where(entry => entry.GetType().GetProperties()
                                .ToList()
                                .Where(prop => filters.GetType().GetProperties()
                                    .ToList()
                                    .Any(item => item.Name == prop.Name &&
                                        typeof(FilterParams).GetProperty(prop.Name).GetValue(filters, null) != null &&
                                        typeof(FilterParams).GetProperty(prop.Name).GetValue(filters, null).ToString().ToLower()
                                        .Contains(entry.GetType().GetProperty(prop.Name).GetValue(entry, null).ToString().ToLower()))) != null)
                            .ToListAsync();

                        return await PagedList<TResponse>.CreateAsync(this.mapper
                            .Map<IEnumerable<TDomain>, IQueryable<TResponse>>(dbResults), filters.PageNumber, filters.PageSize);
                    }, error : new NoContentException());
            }
            public virtual async Task<TResponse> CreateModel<TRequest, TResponse>(TRequest model) where TRequest : BaseDTO where TResponse : BaseResponseDTO<TResponse>, new()
            {
                return await this.Process<TResponse>(
                    function: async() =>
                    {
                        EntityEntry<TDomain> dbResult = await this.context
                            .Set<TDomain>()
                            .AddAsync(this.mapper
                                .Map<TRequest, TDomain>(model, new TDomain()));
                        await this.context.SaveChangesAsync();
                        return this.mapper
                            .Map<TDomain, TResponse>(dbResult.Entity, new TResponse());
                    }, error : new ProcessFailedException());
            }
            public virtual async Task<PagedList<TResponse>> CreateModels<TRequest, TResponse>(List<TRequest> models) where TRequest : BaseDTO where TResponse : BaseResponseDTO<TResponse>, new()
            {
                return await this.Process<PagedList<TResponse>>(
                    function: async() =>
                    {
                        List<TDomain> dbResults = new List<TDomain>();
                        models.ForEach(async(TRequest model) =>
                        {
                            EntityEntry<TDomain> dbResult = await this.context
                                .Set<TDomain>()
                                .AddAsync(this.mapper
                                    .Map<TRequest, TDomain>(model, new TDomain()));
                            dbResults.Append(dbResult.Entity);
                        });
                        await this.context.SaveChangesAsync();
                        return await PagedList<TResponse>.CreateAsync(this.mapper
                            .Map<List<TDomain>, IQueryable<TResponse>>(dbResults));
                    }, error : new ProcessFailedException());
            }
            public virtual async Task<TResponse> UpdateModel<TRequest, TResponse>(TRequest model) where TRequest : BaseDTO where TResponse : BaseResponseDTO<TResponse>, new()
            {
                return await this.Process<TResponse>(
                    function: async() =>
                    {
                        TDomain dbResult = this.context
                            .Update<TDomain>(this.mapper
                                .Map<TRequest, TDomain>(model)).Entity;
                        await this.context.SaveChangesAsync();
                        return this.mapper
                            .Map<TDomain, TResponse>(dbResult, new TResponse());
                    }, error : new ProcessFailedException());
            }
            public virtual async Task<PagedList<TResponse>> UpdateModels<TRequest, TResponse>(List<TRequest> models) where TRequest : BaseDTO where TResponse : BaseResponseDTO<TResponse>, new()
            {
                return await this.Process<PagedList<TResponse>>(
                    function: async() =>
                    {
                        List<TDomain> dbResults = new List<TDomain>();
                        models.ForEach((TRequest model) =>
                        {
                            dbResults.Append(this.context
                                .Update(this.mapper
                                    .Map<TRequest, TDomain>(model, new TDomain())).Entity);
                        });
                        await this.context.SaveChangesAsync();
                        return await PagedList<TResponse>.CreateAsync(this.mapper
                            .Map<List<TDomain>, IQueryable<TResponse>>(dbResults));
                    }, error : new ProcessFailedException());
            }
            public virtual async Task<TResponse> DeleteModel<TResponse>(string id) where TResponse : BaseResponseDTO<TResponse>, new()
            {
                return await this.Process<TResponse>(
                    function: async() =>
                    {
                        TDomain dbResult = this.context
                            .Remove(await this.context
                                .Set<TDomain>()
                                .FirstAsync(x => x.ID == Guid.Parse(id))).Entity;
                        await this.context.SaveChangesAsync();
                        return this.mapper
                            .Map<TDomain, TResponse>(dbResult, new TResponse());
                    }, error : new ProcessFailedException());
            }
            public virtual async Task<PagedList<TResponse>> DeleteModels<TResponse>(List<string> models) where TResponse : BaseResponseDTO<TResponse>, new()
            {
                return await this.Process<PagedList<TResponse>>(
                    function: async() =>
                    {
                        List<TDomain> dbResults = new List<TDomain>();
                        models.ForEach(async(string id) =>
                        {
                            TDomain item = await this.context.Set<TDomain>().FirstAsync(x => x.ID == Guid.Parse(id));
                            EntityEntry<TDomain> result = this.context.Set<TDomain>().Remove(item);
                            dbResults.Append(result.Entity);
                        });
                        await this.context.SaveChangesAsync();
                        return await PagedList<TResponse>.CreateAsync(this.mapper
                            .Map<List<TDomain>, IQueryable<TResponse>>(dbResults));
                    }, error : new ProcessFailedException("Remove", typeof(TDomain).Name));
            }
            #endregion // Global Repository Methods
            #region Internal Repository Methods
            internal virtual async Task<T> Process<T>(Func<Task<T>> function, [Optional] Exception error)
            {
                try { return await function(); }
                catch (DbUpdateException ex) { throw new ProcessFailedException("Process failed: Failed to update the database", ex); }
                catch (Exception ex) { throw error != null ? error : ex; }
            }
            #endregion // Internal Repository
        }
}