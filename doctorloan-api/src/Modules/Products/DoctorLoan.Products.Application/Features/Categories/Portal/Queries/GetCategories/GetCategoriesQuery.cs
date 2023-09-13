using DoctorLoan.Application;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Entities.Products;
using DoctorLoan.Products.Application.Features.Categories.Portal.Dtos;
using DoctorLoan.Products.Application.Features.Products.Portal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;

namespace DoctorLoan.Products.Application.Features.Categories.Portal.Queries.GetCategories;
public class GetCategoriesQuery:IRequest<Result<List<GetCategoriesResultDto>>>
{
    public bool IncludeProduct { get; set; }
    public class GetCategoriesQueryHandle : ApplicationBaseService<GetCategoriesQueryHandle>, IRequestHandler<GetCategoriesQuery, Result<List<GetCategoriesResultDto>>>
    {
        public GetCategoriesQueryHandle(ILogger<GetCategoriesQueryHandle> logger, IApplicationDbContext context, ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService, IDateTime dateTime) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
        {
        }

        public async Task<Result<List<GetCategoriesResultDto>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {          

                var categories = await _context.Categories
                .Include(x => x.ProductCategories
                .Where(x => x.Product.Status == Domain.Enums.Commons.StatusEnum.Publish))
                .Where(x => x.Status == Domain.Enums.Commons.StatusEnum.Publish).OrderBy(x => x.Sort).ToListAsync(cancellationToken);           
            var result = categories.Where(x=>x.ParentId==0).Select(x=> PrepairingCategory(x,categories,request.IncludeProduct)).ToList();
           
            return Result.Success(result);
        }

        private GetCategoriesResultDto PrepairingCategory(Category category, List<Category> categories,bool includeProduct)
        {
            var model = new GetCategoriesResultDto
            {
                Id= category.Id,
                Name= category.Name,
                Slug= category.Slug,
                
            };
            if (includeProduct)
            {
                var products = _context.Products.Where(x => category.ProductCategories.Select(c => c.ProductId).Contains(x.Id))
                         .Select(x => new SearchProductResultDto { Sku = x.Sku, Id = x.Id, Name = x.Name, Slug = x.Slug }).ToList();
                model.Products = products;
                
            }
            var childs = categories.Where(x => x.ParentId == category.Id);
            if (childs.Any())
                return model;
            model.Childs=childs.Select(x=>PrepairingCategory(x,categories, includeProduct)).ToList();
            return model;
        }
       
    }
}

