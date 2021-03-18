using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services.Identity;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Dashboard.GetData
{
    public class GetDashboardDataQuery : IRequest<Result<DashboardDataResponse>>
    {
        public class GetDashboardDataQueryHandler : IRequestHandler<GetDashboardDataQuery, Result<DashboardDataResponse>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUserService _userService;
            private readonly IRoleService _roleService;

            public GetDashboardDataQueryHandler(IUnitOfWork unitOfWork, IUserService userService, IRoleService roleService)
            {
                _unitOfWork = unitOfWork;
                _userService = userService;
                _roleService = roleService;
            }

            public async Task<Result<DashboardDataResponse>> Handle(GetDashboardDataQuery query, CancellationToken cancellationToken)
            {
                var response = new DashboardDataResponse();
                response.ProductCount = await _unitOfWork.Repository<Product>().Entities.CountAsync();
                response.BrandCount = await _unitOfWork.Repository<Brand>().Entities.CountAsync();
                response.UserCount = await _userService.GetCountAsync();
                response.RoleCount = await _roleService.GetCountAsync();
                return Result<DashboardDataResponse>.Success(response);
            }
        }
    }
}