using DoctorLoan.Application;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Interfaces.Medias;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Application.Models.Settings;
using DoctorLoan.Domain.Entities.Products;
using DoctorLoan.Products.Application.Features.Products.Admin.Commands.InsertProduct;
using DoctorLoan.Products.Application.Features.Products.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DoctorLoan.Products.Application.Features.Products.Admin.Commands;
public class UpdateProductCommand : ProductDto, IRequest<Result<int>>
{
}
public class UpdateProductCommandHandler : ApplicationBaseService<UpdateProductCommandHandler>, IRequestHandler<UpdateProductCommand, Result<int>>
{
    private readonly  StorageConfiguration _storageConfiguration;
    private readonly IMediaService _mediaService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UpdateProductCommandHandler(IHttpContextAccessor httpContextAccessor,IOptions<StorageConfiguration> storageConfigurationOption, IMediaService mediaService,ILogger<UpdateProductCommandHandler> logger, IApplicationDbContext context, ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService, IDateTime dateTime) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
        _storageConfiguration = storageConfigurationOption.Value;
        _mediaService = mediaService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<int>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Include(x => x.ProductItems).ThenInclude(x => x.ProductOptions)
            .Include(x=>x.ProductCategories)
                .Include(x => x.ProductMedias).ThenInclude(x=>x.Media)
                .Include(x => x.ProductDetails)
                .Include(x => x.ProductAttributes)
            .FirstOrDefaultAsync(x => x.Id==request.Id);
        if (product == null)
            return Result.Failed<int>(ServiceError.NotFound(_currentTranslateService));
        request.MapperTo(product);
           
         UpdateProductItems(product, request);
         UpdateProductDetail(product, request);
         UpdateProductAttribute(product, request);
       
         UpdateProductCategories(product, request);
        await _context.SaveChangesAsync(cancellationToken);
        await UpdateProductImages(request,product,cancellationToken);
        return Result.Success(product.Id);
    }
    private void UpdateProductCategories(Product product, UpdateProductCommand updateProductCommand)
    {
        var deleteItems = product.ProductCategories.Where(x => !updateProductCommand.CategoryIds.Contains(x.CategoryId));
        _context.ProductCategories.RemoveRange(deleteItems);
        foreach (var item in updateProductCommand.CategoryIds)
        {
            if (!product.ProductCategories.Any(x => x.CategoryId == item))
            {
                product.ProductCategories.Add(new ProductCategory {ProductId=product.Id, CategoryId = item });
            }
           

        }
    }

    private void UpdateProductAttribute(Product product, UpdateProductCommand updateProductCommand)
    {
        
        product.ProductAttributes.Clear();
        foreach (var item in updateProductCommand.ProductAttributes)
        {
            var attribute = item.MapperTo<ProductAttributeDto, ProductAttribute>();
            product.ProductAttributes.Add(attribute);

        }
    }
    private void UpdateProductItems(Product product,UpdateProductCommand updateProductCommand)
    {
        var listIds = updateProductCommand.ProductItems.Where(x => x.Id > 0).Select(x => x.Id);
        product.ProductItems.RemoveWhen(x=> !listIds.Contains(x.Id));
        foreach(var item in updateProductCommand.ProductItems)
        {
            ProductItem productItem  = new ProductItem();
            if (item.Id > 0)
            {
                productItem = product.ProductItems.FirstOrDefault(x => x.Id == item.Id);
                if (productItem == null)
                    continue;
                if(productItem.ProductOptions!=null)
                     productItem.ProductOptions.Clear();
            }
            item.MapperTo(productItem);
            if (productItem.Id == 0)
                product.ProductItems.Add(productItem);

        }
    }
    private void UpdateProductDetail(Product product, UpdateProductCommand updateProductCommand)
    {
        
        foreach (var item in updateProductCommand.ProductDetails)
        {
            var detail = product.ProductDetails.FirstOrDefault(x => x.LanguageId == item.LanguageId);
            if (detail!=null)
            {
                item.MapperTo(detail);
            }
        }
    }
    private async Task UpdateProductImages(UpdateProductCommand req, Product product, CancellationToken cancellationToken)
    {
        var listFileSize = new List<int> {0};
        var listDeleted = product.ProductMedias.Where(x => !req.ProductMedias.Where(x=>x.MediaId!=0).Any(m => m.MediaId == x.MediaId));
        foreach(var deleteItem in listDeleted)
        {
            var isDeleted = await _mediaService.DeleteMediaAsync(deleteItem.Media);
            if (isDeleted)
            {
                product.ProductMedias.Remove(deleteItem);
               await _context.SaveChangesAsync(cancellationToken);
            }
          
        }
        foreach (var item in req.ProductMedias.Where(x=>x.MediaId>0))
        {
            var productMedia = product.ProductMedias.FirstOrDefault(x => x.MediaId == item.MediaId);
            if (productMedia == null)
                continue;
            item.MapperTo(productMedia);
            if (!string.IsNullOrEmpty(item.ItemCode))
                productMedia.ProductItemId = product.ProductItems.First(x => x.Sku == item.ItemCode).Id;
            await _context.SaveChangesAsync(cancellationToken);           
        }
        int i = 0;
        foreach (var item in req.ProductMedias.Where(x => x.MediaId == 0))
        {
            var productItem = product.ProductItems.FirstOrDefault(x => x.Sku == item.ItemCode);
            var productMedia = new ProductMedia();
            var file = _httpContextAccessor.HttpContext.Request.Form.Files.ElementAtOrDefault(i++);
            if (file == null)
                continue;
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms, cancellationToken);
                var media = await _mediaService.UploadMediaAsync(ms.ToArray(), file.FileName, Domain.Enums.Medias.MediaType.Product, listFileSize, product.Id.ToString("0000"));
                if (media.Id == 0)
                    continue;
                product.ProductMedias.Add(new ProductMedia {ProductItemId= productItem?.Id, MediaId = media.Id, OrderBy = item.OrderBy, Status = Domain.Enums.Commons.StatusEnum.Publish });
            }
           
            await _context.SaveChangesAsync(cancellationToken);

           


        }
    }
}
