using AutoMapper;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Domain.Entities.News;
using DoctorLoan.News.Application.Features.News.Admin.Dtos;
using DoctorLoan.News.Application.Features.News.Dtos;
using DoctorLoan.News.Application.Features.NewsCategories.Dtos;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.News.Application.Features.News.Portal.Dtos;
using DoctorLoan.Domain.Enums.Commons;

namespace DoctorLoan.News.Application.Mappings;
public class MappingProfile : Profile, IOrderedMapperProfile
{
    public MappingProfile()
    {
        #region News Catetory
        CreateMap<NewsCategoryDto, NewsCategory>();
        CreateMap<NewsCategory, NewsCategoryDto>();
        #endregion
        #region News Item
        CreateMap<NewsItem, NewsItemDto>()
            .ForMember(x => x.CategoryIds, x => x.MapFrom(c => c.NewsCategories.Select(x => x.NewsCategoryId)))
                .ForMember(x => x.Tags, x => x.MapFrom(c => c.NewsTags.Select(x => new TagDto { Id=x.NewsTagId,Name=x.NewsTag.Name})))
            ;
      
        CreateMap<NewsItemDetail, NewsItemDetailDto>();
        CreateMap<NewsMedia, NewsMediaDto>()
            .ForMember(x => x.MediaUrl, x => x.MapFrom(x => x.Media.GetMediaUrl(true, 0)));


        CreateMap<NewsItemDto, NewsItem>()
            .ForMember(x => x.NewsItemDetails, x => x.Condition(x => x.Id == 0))
            .ForMember(x => x.NewsMedias, x => x.Ignore());

        CreateMap<NewsItemDetailDto, NewsItemDetail>();
        CreateMap<NewsMediaDto, NewsMedia>();
      
        CreateMap<NewsItem, NewsItemFilterResultDto>()
           .ForMember(x => x.Categories, x => x.MapFrom(c => c.NewsCategories.Select(v => v.NewsCategory.Name)))
           .ForMember(x => x.ImageUrl, x =>
           {

               x.MapFrom(c => c.NewsMedias.FirstOrDefault().Media.GetMediaUrl(true, 0));

           })
          
            ;
        #region Portal
        CreateMap<NewsItem, NewsSearchResultDto>()
            .ForMember(x=>x.Short,x=>x.MapFrom(c=>c.NewsItemDetails.FirstOrDefault(x=>x.LanguageId==(int)LanguageEnum.VN).Short))
         .ForMember(x => x.ImageUrl, x =>
         {
             
             x.MapFrom(c => c.NewsMedias.FirstOrDefault().Media.GetMediaPortalUrl(0));

         });

        #endregion
        #endregion
    }
    public int Order => 1;
}
