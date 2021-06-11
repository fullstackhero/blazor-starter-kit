using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes.Queries.GetAllByEntityId;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Contracts;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using LazyCache;
using Microsoft.EntityFrameworkCore;

namespace BlazorHero.CleanArchitecture.Infrastructure.Repositories
{
    public class ExtendedAttributeRepository<TId, TEntityId, TEntity, TExtendedAttribute> : IExtendedAttributeRepository<TId, TEntityId>
        where TExtendedAttribute : class, IEntityExtendedAttribute<TId, TEntityId, TEntity>, IEntity<TId>
        where TEntity : IEntity<TEntityId>
    {
        private readonly IRepositoryAsync<TExtendedAttribute, TId> _repository;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public ExtendedAttributeRepository(IRepositoryAsync<TExtendedAttribute, TId> repository, IMapper mapper, IAppCache cache)
        {
            _repository = repository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>>>> GetAllByEntityIdAsync(TEntityId entityId)
        {
            Func<Task<List<TExtendedAttribute>>> getAllExtendedAttributesByEntityId = () => _repository.Entities.Where(x => x.EntityId.Equals(entityId)).ToListAsync();
            var extendedAttributes = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllEntityExtendedAttributesByEntityIdCacheKey(typeof(TEntity).Name, entityId), getAllExtendedAttributesByEntityId);
            var mappedExtendedAttributes = _mapper.Map<List<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>>>(extendedAttributes);
            return await Result<List<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>>>.SuccessAsync(mappedExtendedAttributes);
        }
    }
}