using DoctorLoan.Application.Interfaces.Commons;
using Microsoft.Extensions.Localization;

namespace DoctorLoan.WebAPI.Frameworks.Services;

public class CurrentTranslateService : ICurrentTranslateService
{
    private readonly IStringLocalizer<Validation.Resource> _stringLocalizer;
    private readonly IStringLocalizer<FieldName.Resource> _fieldNameLocalizer;
    private readonly IStringLocalizer<SystemMessage.Resource> _systemtMessageLocalizer;

    public CurrentTranslateService(IStringLocalizer<Validation.Resource> stringLocalizer,
        IStringLocalizer<FieldName.Resource> fieldNameLocalizer,
        IStringLocalizer<SystemMessage.Resource> systemtMessageLocalizer)
    {
        _stringLocalizer = stringLocalizer;
        _fieldNameLocalizer = fieldNameLocalizer;
        _systemtMessageLocalizer = systemtMessageLocalizer;
    }

    public string TranslateByKey(string key) => _stringLocalizer.GetString(key)?.Value ?? string.Empty;

    public string TranslateFieldNameByKey(string key) => _fieldNameLocalizer.GetString(key)?.Value ?? string.Empty;

    public string TranslateSystemMessageByKey(string key) => _systemtMessageLocalizer.GetString(key)?.Value ?? string.Empty;
}