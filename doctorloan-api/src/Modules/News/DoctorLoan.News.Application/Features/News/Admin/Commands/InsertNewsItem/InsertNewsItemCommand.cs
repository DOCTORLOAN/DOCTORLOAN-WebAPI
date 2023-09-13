using DoctorLoan.Application;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Interfaces.Medias;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Application.Models.Settings;
using DoctorLoan.Domain.Entities.News;
using DoctorLoan.News.Application.Features.News.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Asn1.Ocsp;

namespace DoctorLoan.News.Application.Features.News.Admin.Commands;
public class InsertNewsItemCommand : NewsItemDto, IRequest<Result<int>>
{
}
public class InsertNewsItemCommandHandler : ApplicationBaseService<InsertNewsItemCommandHandler>, IRequestHandler<InsertNewsItemCommand, Result<int>>
{
    private readonly IMediaService _mediaService;
    private readonly StorageConfiguration _storageConfiguration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public InsertNewsItemCommandHandler(IHttpContextAccessor httpContextAccessor, 
        IOptions<StorageConfiguration> storageConfigurationOption,IMediaService mediaService,ILogger<InsertNewsItemCommandHandler> logger,
        IApplicationDbContext context, ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService, IDateTime dateTime) 
        : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
        _mediaService = mediaService;
        _storageConfiguration = storageConfigurationOption.Value;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<int>> Handle(InsertNewsItemCommand request, CancellationToken cancellationToken)
    {
        request.NewsItemDetails.ForEach(x => x.Title = request.Title);
        var newsItem = request.MapperTo<InsertNewsItemCommand, NewsItem>();
        
        newsItem.NewsCategories.AddRange(request.CategoryIds.Select(x => new NewsCategoryMapping
        {
             NewsCategoryId= x,
        }));

        InsertTags(request, newsItem);
        newsItem.Status = Domain.Enums.Commons.StatusEnum.Draft;
        newsItem.Slug = request.Title.ToSlug();
        await _context.NewsItems.AddAsync(newsItem);
    
        await _context.SaveChangesAsync(cancellationToken);
        await InserNewsImages(request, newsItem, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success(newsItem.Id);
    }
    private async Task InserNewsImages(InsertNewsItemCommand req,NewsItem newsItem,CancellationToken  cancellationToken)
    {
        var listFileSize = new List<int> { 0 };
        int i = 0;
        foreach (var item in req.NewsMedias)
        {
            var newsMedia = new NewsMedia();
            var file = _httpContextAccessor?.HttpContext?.Request.Form.Files.ElementAtOrDefault(i++);
            if (file == null)
                continue;
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms, cancellationToken);              
                var media = await _mediaService.UploadMediaAsync(ms.ToArray(), file.FileName, Domain.Enums.Medias.MediaType.News, listFileSize, newsItem.Id.ToString("0000"));
                if (media.Id == 0)
                    continue;
                newsItem.NewsMedias.Add(new NewsMedia { MediaId = media.Id, OrderBy = item.OrderBy });
                item.MapperTo(newsMedia);
            }
                
            




        }
    }
    private void InsertTags(InsertNewsItemCommand req, NewsItem newsItem)
    {
        foreach (var item in req.Tags)
        {
            if (item.Id == 0)
            {
                newsItem.NewsTags.Add(new NewsTagsMapping
                {
                    NewsTag = new NewsTag
                    {
                        Name = item.Name,
                    }
                });
            }
            else
            {
                newsItem.NewsTags.Add(new NewsTagsMapping
                {
                    NewsTagId = item.Id,
                });
            }
        }
    }
}
