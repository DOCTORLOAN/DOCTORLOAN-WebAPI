using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Domain.Const.Validations;
using FluentValidation;

namespace DoctorLoan.Authen.Application.Features.Authenticates;

public class SignInCommandValidator : AbstractValidator<SignInCommand>
{
    public SignInCommandValidator(ICurrentTranslateService currentTranslateService)
    {
        #region field names
        var userName = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.UserName);
        var password = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.Password);
        #endregion

        #region message
        var required = currentTranslateService.TranslateByKey(MessageKeyValidation.Required);
        #endregion

        RuleFor(v => v.UserName).NotEmpty().WithMessage(required.TryFormat(userName));
        RuleFor(v => v.Password).NotEmpty().WithMessage(required.TryFormat(password));
    }
}
