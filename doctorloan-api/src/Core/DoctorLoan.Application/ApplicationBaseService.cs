using AutoMapper;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Interfaces.Data;
using Microsoft.Extensions.Logging;

namespace DoctorLoan.Application;
public abstract class ApplicationBaseService<TService> where TService : class
{
    protected readonly ILogger<TService> _logger;
    protected readonly IApplicationDbContext _context;
    protected ICurrentRequestInfoService _currentRequestInfoService;
    protected ICurrentTranslateService _currentTranslateService;
    protected readonly IDateTime _dateTime;
    protected IMapper Mapper => EngineContext.GetService<IMapper>();
    public ApplicationBaseService(
        ILogger<TService> logger,
        IApplicationDbContext context,
        ICurrentRequestInfoService currentRequestInfoService,
        ICurrentTranslateService currentTranslateService,
        IDateTime dateTime)
    {
        _logger = logger;
        _context = context;
        _currentRequestInfoService = currentRequestInfoService;
        _currentTranslateService = currentTranslateService;
        _dateTime = dateTime;
    }
}
