using DoctorLoan.Application;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Enums.Commons;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Products.Application.Features.Products.Admin.Commands;
public class UpdateProductStatusCommand:IRequest<Result<bool>>
{
    public List<int> Ids { get; set; }=new List<int>();
    public StatusEnum Status { get; set; }
}
public class UpdateProductStatusCommandHandle : ApplicationBaseService<UpdateProductStatusCommandHandle>, IRequestHandler<UpdateProductStatusCommand, Result<bool>>
{
    public UpdateProductStatusCommandHandle(ILogger<UpdateProductStatusCommandHandle> logger, IApplicationDbContext context, ICurrentRequestInfoService currentRequestInfoService, ICurrentTranslateService currentTranslateService, IDateTime dateTime) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
    }

    public async Task<Result<bool>> Handle(UpdateProductStatusCommand request, CancellationToken cancellationToken)
    {
        var listProduct = await _context.Products.Where(x => request.Ids.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach(var product in listProduct) {
            product.Status = request.Status;
        }
        await _context.SaveChangesAsync(cancellationToken);
        return new Result<bool>(true);
    }
}
