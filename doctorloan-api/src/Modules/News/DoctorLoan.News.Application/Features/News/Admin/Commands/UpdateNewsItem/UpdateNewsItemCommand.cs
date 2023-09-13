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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DoctorLoan.News.Application.Features.News.Admin.Commands;
public class UpdateNewsItemCommand : NewsItemDto, IRequest<Result<int>>
{
}
public class UpdateNewsItemCommandHandler : ApplicationBaseService<UpdateNewsItemCommandHandler>, IRequestHandler<UpdateNewsItemCommand, Result<int>>
{
    private readonly  StorageConfiguration _storageConfiguration;
    private readonly IMediaService _mediaService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UpdateNewsItemCommandHandler(IHttpContextAccessor httpContextAccessor,IOptions<StorageConfiguration> storageConfigurationOption, IMediaService mediaService,ILogger<UpdateNewsItemCommandHandler> logger, IApplicationDbContext context, ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService, IDateTime dateTime) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
        _storageConfiguration = storageConfigurationOption.Value;
        _mediaService = mediaService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<int>> Handle(UpdateNewsItemCommand request, CancellationToken cancellationToken)
    {
        var newsItem = await _context.NewsItems
              .Include(x => x.NewsItemDetails)
                .Include(x => x.NewsCategories)
                .Include(x => x.NewsMedias).ThenInclude(x=>x.Media)
                .Include(x => x.NewsTags)
            .FirstOrDefaultAsync(x => x.Id==request.Id);
        if (newsItem == null)
            return Result.Failed<int>(ServiceError.NotFound(_currentTranslateService));
        request.MapperTo(newsItem);
           
         UpdateNewsDetail(newsItem, request);
        UpdateTags(request, newsItem);
        UpdateNewsItemCategories(newsItem, request);
        await _context.SaveChangesAsync(cancellationToken);
        await UpdateNewsImages(request,newsItem,cancellationToken);
        return Result.Success(newsItem.Id);
    }
    private void UpdateNewsItemCategories(NewsItem newsItem, UpdateNewsItemCommand updateNewsItemComman)
    {
        var deleteItems = newsItem.NewsCategories.Where(x => !updateNewsItemComman.CategoryIds.Contains(x.NewsCategoryId));
        _context.NewsCategoryMappings.RemoveRange(deleteItems);
        foreach (var item in updateNewsItemComman.CategoryIds)
        {
            if (!newsItem.NewsCategories.Any(x => x.NewsCategoryId == item))
            {
                newsItem.NewsCategories.Add(new NewsCategoryMapping {NewsItemId=newsItem.Id, NewsCategoryId = item });
            }
           

        }
    }      
   
    private void UpdateNewsDetail(NewsItem newsItem, UpdateNewsItemCommand updateNewsItemCommand)
    {
        
        foreach (var item in updateNewsItemCommand.NewsItemDetails)
        {
           
            var detail = newsItem.NewsItemDetails.FirstOrDefault(x => x.LanguageId == item.LanguageId);
            if (detail!=null)
            {               
                item.MapperTo(detail);
                if (detail.Title == null)
                    detail.Title = newsItem.Title;
            }
        }
    }
    private async Task UpdateNewsImages(UpdateNewsItemCommand req, NewsItem newsItem, CancellationToken cancellationToken)
    {
        var listFileSize = new List<int> {0};
        var listDeleted = newsItem.NewsMedias.Where(x => !req.NewsMedias.Where(x=>x.MediaId!=0).Any(m => m.MediaId == x.MediaId));
        foreach(var deleteItem in listDeleted)
        {
            var isDeleted = await _mediaService.DeleteMediaAsync(deleteItem.Media);
            if (isDeleted)
            {
                newsItem.NewsMedias.Remove(deleteItem);
               await _context.SaveChangesAsync(cancellationToken);
            }
          
        }
        foreach (var item in req.NewsMedias.Where(x=>x.MediaId>0))
        {
            var newsMedia = newsItem.NewsMedias.FirstOrDefault(x => x.MediaId == item.MediaId);
            if (newsMedia == null)
                continue;
            item.MapperTo(newsMedia);            
            await _context.SaveChangesAsync(cancellationToken);           
        }
        int i = 0;
        foreach (var item in req.NewsMedias.Where(x => x.MediaId == 0))
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
            }
           
            await _context.SaveChangesAsync(cancellationToken);

           


        }
    }
    private void UpdateTags(UpdateNewsItemCommand req, NewsItem newsItem)
    {
        var tagIds = req.Tags.Where(x => x.Id > 0).Select(x => x.Id).ToList();
        var deleteItems = newsItem.NewsTags.Where(x => !tagIds.Contains(x.NewsTagId));
        _context.NewsTagsMappings.RemoveRange(deleteItems);
        foreach (var item in req.Tags)
        {
            if (tagIds.Contains(item.Id))
                continue;
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
