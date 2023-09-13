using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoctorLoan.Application;
using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Interfaces.Medias;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Application.Models.Settings;
using DoctorLoan.Domain.Entities.Products;
using DoctorLoan.Products.Application.Features.Products.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Math.EC.Rfc7748;
using Org.BouncyCastle.Utilities;

namespace DoctorLoan.Products.Application.Features.Products.Admin.Commands.InsertProduct;
public class InsertProductCommand : ProductDto, IRequest<Result<int>>
{
}
public class InsertProductCommandHandler : ApplicationBaseService<InsertProductCommandHandler>, IRequestHandler<InsertProductCommand, Result<int>>
{
    private readonly IMediaService _mediaService;
    private readonly StorageConfiguration _storageConfiguration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public InsertProductCommandHandler(IHttpContextAccessor httpContextAccessor, IOptions<StorageConfiguration> storageConfigurationOption,IMediaService mediaService,ILogger<InsertProductCommandHandler> logger, IApplicationDbContext context, ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService, IDateTime dateTime) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
        _mediaService = mediaService;
        _storageConfiguration = storageConfigurationOption.Value;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<int>> Handle(InsertProductCommand request, CancellationToken cancellationToken)
    {
        var exists = await _context.Products.FirstOrDefaultAsync(x => x.Sku ==request.Sku);
        if (exists != null)
            return Result.Failed<int>(ServiceError.CustomMessage("Mã sản phẩm đã tồn tại!"));
        var product = request.MapperTo<InsertProductCommand, Product>();
        product.ProductCategories.AddRange(request.CategoryIds.Select(x => new ProductCategory
        {
            CategoryId = x,
        }));
        product.Status = Domain.Enums.Commons.StatusEnum.Draft;
        product.Slug = product.Name.ToSlug();
        await _context.Products.AddAsync(product);       
        await _context.SaveChangesAsync(cancellationToken);
        await InsertProductImages(request, product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success(product.Id);
    }
    private async Task InsertProductImages(InsertProductCommand req,Product product,CancellationToken  cancellationToken)
    {
        var listFileSize = new List<int> { 0};
        int i = 0;
        foreach (var item in req.ProductMedias)
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
                
            




        }
    }
}
