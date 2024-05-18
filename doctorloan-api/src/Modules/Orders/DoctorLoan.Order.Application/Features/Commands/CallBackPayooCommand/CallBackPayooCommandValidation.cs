using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Domain.Const.Validations;
using FluentValidation;

namespace DoctorLoan.Order.Application.Features.Commands.CallBackPayooCommand;
public class CallBackPayooCommandValidator : AbstractValidator<CallBackPayooCommand>
{
    public CallBackPayooCommandValidator(ICurrentTranslateService currentTranslateService)
    {
        #region message
        var required = currentTranslateService.TranslateByKey(MessageKeyValidation.Required);
        var notValid = currentTranslateService.TranslateByKey(MessageKeyValidation.NotValid);
        #endregion


        RuleFor(v => v.ResponseData).NotEmpty().WithMessage(required.TryFormat("ResponseData"))
                    .NotNull().WithMessage(notValid.TryFormat("ResponseData"));

        RuleFor(v => v.Signature).NotEmpty().WithMessage(required.TryFormat("Signature"))
                    .NotNull().WithMessage(notValid.TryFormat("Signature"));

        RuleFor(v => v.SecureHash).MaximumLength(500).WithMessage(notValid.TryFormat("SecureHash"))
            .NotNull().WithMessage(notValid.TryFormat("SecureHash"));
    }
}
