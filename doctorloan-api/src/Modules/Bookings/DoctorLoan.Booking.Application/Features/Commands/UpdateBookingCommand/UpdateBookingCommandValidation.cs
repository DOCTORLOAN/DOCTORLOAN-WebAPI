using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Domain.Const.Validations;
using FluentValidation;

namespace DoctorLoan.Booking.Application.Features.Commands;

public class UpdateBookingCommandValidator : AbstractValidator<UpdateBookingCommand>
{
    public UpdateBookingCommandValidator(ICurrentTranslateService currentTranslateService)
    {
        var status = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.Status);
        var required = currentTranslateService.TranslateByKey(MessageKeyValidation.Required);


        RuleFor(v => v.Status).NotEmpty().WithMessage(required.TryFormat(status));
    }
}
