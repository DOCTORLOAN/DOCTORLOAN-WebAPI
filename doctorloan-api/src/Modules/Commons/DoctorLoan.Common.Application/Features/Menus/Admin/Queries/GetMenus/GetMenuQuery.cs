using DoctorLoan.Application;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Common.Application.Features.Menus.Dtos;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DoctorLoan.Common.Application.Features.Menus.Admin.Queries;

public record GetMenuQuery : IRequest<Result<List<MenuDto>>>;

public class GetMenuQueryHandler : ApplicationBaseService<GetMenuQueryHandler>, IRequestHandler<GetMenuQuery, Result<List<MenuDto>>>
{
    private readonly IHostingEnvironment _hostingEnvironment;

    public GetMenuQueryHandler(ILogger<GetMenuQueryHandler> logger,
        IApplicationDbContext context,
        ICurrentRequestInfoService currentRequestInfoService,
        ICurrentTranslateService currentTranslateService,
        IDateTime dateTime,
        IHostingEnvironment hostingEnvironment) : base(logger, context, currentRequestInfoService, currentTranslateService, dateTime)
    {
        _hostingEnvironment = hostingEnvironment;
    }

    public Task<Result<List<MenuDto>>> Handle(GetMenuQuery request, CancellationToken cancellationToken)
    {
        var result = new List<MenuDto>();
        var rootPath = _hostingEnvironment.ContentRootPath; //get the root path

        var fullPath = Path.Combine(rootPath, "Files/menus.json"); //combine the root path with that of our json file inside mydata directory

        var jsonData = System.IO.File.ReadAllText(fullPath); //read all the content inside the file

        if (string.IsNullOrWhiteSpace(jsonData)) return Task.FromResult(Result.Success(result)); //if no data is present then return null or error if you wish

        result = JsonConvert.DeserializeObject<List<MenuDto>>(jsonData);
        return Task.FromResult<Result<List<MenuDto>>>(Result.Success(result));
    }

}

