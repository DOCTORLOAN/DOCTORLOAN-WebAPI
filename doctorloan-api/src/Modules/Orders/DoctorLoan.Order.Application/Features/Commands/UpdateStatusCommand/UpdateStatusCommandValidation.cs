using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Domain.Const.Validations;
using FluentValidation;

namespace DoctorLoan.Order.Application.Features.Commands;

public class UpdateStatusCommandValidator : AbstractValidator<UpdateStatusCommand>
{
    public UpdateStatusCommandValidator(ICurrentTranslateService currentTranslateService)
    {
        #region field names
        var status = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.Status);

        #endregion

        #region message
        var required = currentTranslateService.TranslateByKey(MessageKeyValidation.Required);
        var notValid = currentTranslateService.TranslateByKey(MessageKeyValidation.NotValid);
        #endregion


        RuleFor(v => v.Status).NotEmpty().WithMessage(required.TryFormat(status))
                    .NotNull().WithMessage(notValid.TryFormat(status));

        RuleFor(v => v.Id).NotEmpty().WithMessage(required.TryFormat("Mã đơn hàng"))
                    .NotNull().WithMessage(notValid.TryFormat("Mã đơn hàng"));

        RuleFor(v => v.Remarks).MaximumLength(500).WithMessage(notValid.TryFormat("Ghi chú"));
    }
}