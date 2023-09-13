using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Application;
using DoctorLoan.Domain.Entities.Products;
using DoctorLoan.Products.Application.Features.Products.Portal.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;
using DoctorLoan.Domain.Extentions;
using Microsoft.EntityFrameworkCore;

namespace DoctorLoan.Products.Application.Features.Products.Portal.Queries.GetCarts;
public class GetCartsQueryDto
{
  
    public int TotalCartQuantity { get; set; }
    public decimal TotalSubPrice { get; set; }
    public decimal TotalDiscount { get; set; }
    public decimal ShippmentFee { get; set; }
    public decimal TotalPrice { get; set; }
    public List<ProductCartInfoDto> Products { get; set; } = new List<ProductCartInfoDto>();
    public class ProductCartInfoDto {
        public ProductCartInfoDto(ProductItem productItem)
        {
            this.ProductItemId = productItem.Id;
            this.ProductItemName = productItem.Name;
            var productMedia = productItem.Product.ProductMedias.FirstOrDefault(x => x.ProductItemId == productItem.Id);
            if (productMedia == null)
                productMedia = productItem.Product.ProductMedias.FirstOrDefault();
            this.ProductItemImage = productMedia.Media.GetMediaPortalUrl();
            this.Options=productItem.ProductOptions.Select(x => $"{x.OptionGroup.Name}-{x.Name}").ToList();
            this.Price=productItem.Price;
            this.PriceDiscount=productItem.PriceDiscount;
            
        }
        public int ProductItemId { get; set; }
        public  string ProductItemName { get; set; }
        public  string ProductItemImage { get; set; }
        public List<string> Options { get; set; } = new List<string>();
        public decimal Price { get; set; }
        public decimal PriceDiscount { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => PriceDiscount * Quantity;
    }
}
public class CartStoreInfo
{
    public int ProductItemId { get; set; }
    public int Quantity { get; set; }
}
public class GetCartsQuery:IRequest<Result<GetCartsQueryDto>>
{
    public List<CartStoreInfo> CartInfo { get; set; } = new List<CartStoreInfo>();
}

public class GetCartsQueryHandle : ApplicationBaseService<GetCartsQueryHandle>, IRequestHandler<GetCartsQuery, Result<GetCartsQueryDto>>
{
    public GetCartsQueryHandle(ILogger<GetCartsQueryHandle> logger, IApplicationDbContext context, ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService, IDateTime dateTime) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
    }

    public async Task<Result<GetCartsQueryDto>> Handle(GetCartsQuery request, CancellationToken cancellationToken)
    {
        var result = new GetCartsQueryDto();
        if (!request.CartInfo.Any())
            return Result.Success(result);

        var itemIds = request.CartInfo.Select(x => x.ProductItemId);

        var condition = PredicateBuilder.True<ProductItem>();
        condition = condition.And(x => itemIds.Contains(x.Id));
         var listProduct = _context.ProductItems.Include(x=>x.Product).ThenInclude(x=>x.ProductMedias).ThenInclude(x=>x.Media)
            .Include(x=>x.ProductOptions).ThenInclude(x=>x.OptionGroup)
            .Where(condition);
        var listQuantity = request.CartInfo.ToDictionary(x => x.ProductItemId, x => x.Quantity);
        result.Products =await listProduct.Select(x => new GetCartsQueryDto.ProductCartInfoDto(x)).ToListAsync();
        foreach(var item in result.Products)
        {
            var quantity = listQuantity[item.ProductItemId];
            item.Quantity= quantity;
            result.TotalSubPrice += item.Price* quantity;
            result.TotalDiscount += (item.Price-item.PriceDiscount) * quantity;
            result.TotalCartQuantity += quantity;
            
        }
        result.TotalPrice = result.TotalSubPrice - result.TotalDiscount;
        return Result.Success(result);
    }
}