using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.ExtendedAttributes;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using LazyCache;
using MediatR;

namespace BlazorHero.CleanArchitecture.Application.Features.DocumentExtendedAttributes.Queries.GetAll
{
    public class GetAllDocumentExtendedAttributesQuery : IRequest<Result<List<GetAllDocumentExtendedAttributesResponse>>>
    {
        public GetAllDocumentExtendedAttributesQuery()
        {
        }
    }

    internal class GetAllDocumentExtendedAttributesQueryHandler : IRequestHandler<GetAllDocumentExtendedAttributesQuery, Result<List<GetAllDocumentExtendedAttributesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllDocumentExtendedAttributesQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllDocumentExtendedAttributesResponse>>> Handle(GetAllDocumentExtendedAttributesQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<DocumentExtendedAttribute>>> getAllDocumentExtendedAttributes = () => _unitOfWork.Repository<DocumentExtendedAttribute>().GetAllAsync();
            var documentTypeList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllDocumentExtendedAttributesCacheKey, getAllDocumentExtendedAttributes);
            var mappedDocumentTypes = _mapper.Map<List<GetAllDocumentExtendedAttributesResponse>>(documentTypeList);
            return await Result<List<GetAllDocumentExtendedAttributesResponse>>.SuccessAsync(mappedDocumentTypes);
        }
    }
}