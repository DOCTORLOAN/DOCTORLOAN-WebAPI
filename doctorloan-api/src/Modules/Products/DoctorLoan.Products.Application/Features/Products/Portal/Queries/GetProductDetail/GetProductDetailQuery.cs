using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Application;
using DoctorLoan.Domain.Entities.Products;
using DoctorLoan.Products.Application.Features.Products.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Products.Application.Features.Products.Portal.Dtos;

namespace DoctorLoan.Products.Application.Features.Products.Portal.Queries;

public record GetProductDetailQuery(int id,string slug) : IRequest<Result<GetProductDetailDto>>;
public class GetProductDetailQueryHandle : ApplicationBaseService<GetProductDetailQueryHandle>, IRequestHandler<GetProductDetailQuery, Result<GetProductDetailDto>>
{
    public GetProductDetailQueryHandle(ILogger<GetProductDetailQueryHandle> logger, IApplicationDbContext context, ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService, IDateTime dateTime) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
    }

    public async Task<Result<GetProductDetailDto>> Handle(GetProductDetailQuery request, CancellationToken cancellationToken)
    {
        var product = await Mapper.ProjectTo< GetProductDetailDto >( _context.Products
            
            .Include(x => x.ProductItems).ThenInclude(x => x.ProductOptions)
            .Include(x => x.ProductMedias).ThenInclude(x => x.Media)
            .Include(x => x.ProductDetails)
            .Include(x => x.ProductAttributes)
            .Where(x=>x.Status==Domain.Enums.Commons.StatusEnum.Publish&&x.Id==request.id&&x.Slug==request.slug)
            )
            .FirstOrDefaultAsync(cancellationToken);
        if (product == null)
            return Result.Failed<GetProductDetailDto>(ServiceError.NotFound(_currentTranslateService));
        return Result.Success(product);
    }
}
