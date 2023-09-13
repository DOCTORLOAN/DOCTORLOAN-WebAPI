using AutoMapper;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Domain.Entities.Products;
using DoctorLoan.Domain.Enums.Commons;
using DoctorLoan.Products.Application.Features.Categories.Dtos;
using DoctorLoan.Products.Application.Features.Products.Dtos;
using DoctorLoan.Products.Application.Features.Products.Portal.Dtos;

namespace DoctorLoan.Products.Application.Mapping;

public class MappingProfile : Profile, IOrderedMapperProfile
{
    public MappingProfile()
    {
        #region Products
        CreateMap<Product, ProductDto>()
            .ForMember(x=>x.CategoryIds,x=>x.MapFrom(c=>c.ProductCategories.Select(x=>x.CategoryId)))
            ;
        CreateMap<ProductItem,ProductItemDto>();
        CreateMap<ProductDetail,ProductDetailDto>();
        CreateMap<ProductAttribute,ProductAttributeDto>();
        CreateMap<ProductMedia, ProductMediaDto>()
            .ForMember(x=>x.MediaUrl,x=>x.MapFrom(x=>x.Media.GetMediaUrl(true, 0)))
            .ForMember(x=>x.ItemCode,x=>x.MapFrom(x=>x.Product.ProductItems.FirstOrDefault(p=>p.Id==x.ProductItemId).Sku))
            ;
           
        CreateMap<ProductOption,ProductOptionDto>();
        CreateMap<ProductCategory, ProductCategoryDto>()
            .ForMember(x=>x.Name,x=>x.MapFrom(c=>c.Category.Name));
        CreateMap<ProductDto, Product>()
            .ForMember(x=>x.ProductItems,x=>x.Condition(x=>x.Id==0))
            .ForMember(x => x.ProductDetails, x => x.Condition(x => x.Id == 0))
            .ForMember(x => x.ProductAttributes, x => x.Condition(x => x.Id == 0))
            .ForMember(x => x.ProductMedias, x => x.Ignore())
            .ForMember(x=>x.ProductCategories,x=>x.Ignore())
            ;

        CreateMap<ProductItemDto, ProductItem>();
        CreateMap<ProductDetailDto, ProductDetail>();
        CreateMap<ProductAttributeDto, ProductAttribute>();
        CreateMap<ProductMediaDto, ProductMedia>();
           
        CreateMap<ProductOptionDto, ProductOption>();
        CreateMap<Product, ProductFilterResultDto>()
           .ForMember(x => x.BranchName, x => x.MapFrom(c => c.Brand.Name))
           .ForMember(x => x.CategoryNames, x => x.MapFrom(c => c.ProductCategories.Select(v => v.Category.Name)))
           .ForMember(x => x.ImageUrl, x =>
           {
             
               x.MapFrom(c => c.ProductMedias.FirstOrDefault().Media.GetMediaUrl(true, 0));
              
           })
           .ForMember(x => x.Stock, x => x.MapFrom(c => c.ProductItems.Sum(i => i.Quantity)))
            ;
        #region Portal
        CreateMap<Product, SearchProductResultDto>()
            .ForMember(x=>x.ImageUrl,x=>x.MapFrom(c=>c.ProductMedias.FirstOrDefault().Media.GetMediaPortalUrl(0)))
            .ForMember(x=>x.BrandName,x=>x.MapFrom(c=>c.Brand.Name))
            .ForMember(x=>x.Price,x=>x.MapFrom(c=>c.ProductItems.FirstOrDefault().Price))
               .ForMember(x => x.PriceDiscount, x => x.MapFrom(c => c.ProductItems.FirstOrDefault().PriceDiscount))
               .ForMember(x=>x.Short,x=>x.MapFrom(c=>c.ProductDetails.FirstOrDefault(x=>x.LanguageId==(int)LanguageEnum.VN).Summary))
            ;
        CreateMap<Product, GetProductDetailDto>() 
            .ForMember(x=>x.ProductDetail,x=>x.MapFrom(c=>c.ProductDetails.First(x=>x.LanguageId==(int)LanguageEnum.VN)))
            .ForMember(x=>x.CategoryIds,x=>x.MapFrom(c=>c.ProductCategories.Select(x=>x.CategoryId)))
            ;
        CreateMap<ProductItem, GetProductDetailProductItemDto>()
            
            ;
        CreateMap<ProductOption, GetProductDetailProductOptionDto>()
            .ForMember(x => x.GroupName, x => x.MapFrom(c => c.OptionGroup.Name));
        CreateMap<ProductMedia, GetProductDetaiMediaDto>()
            .ForMember(x=>x.MediaUrl,x=>x.MapFrom(c=>c.Media.GetMediaPortalUrl(0)))
            ;
        #endregion
        #endregion
        #region Category
        CreateMap<CategoryDto, Category>()
            .ForMember(x => x.Slug, x => x.Ignore());
        CreateMap<Category, CategoryDto>();

        #endregion

  

    }

    public int Order => 0;

}

