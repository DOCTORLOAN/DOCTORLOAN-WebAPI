using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Domain.Const.Validations;
using DoctorLoan.Domain.Enums.Bookings;
using DoctorLoan.Domain.Enums.Validators;
using FluentValidation;

namespace DoctorLoan.Booking.Application.Features.Commands;

public class AddBookingCommandValidator : AbstractValidator<AddBookingCommand>
{
    public AddBookingCommandValidator(ICurrentTranslateService currentTranslateService)
    {
        #region field names
        var firstName = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.FirstName);
        var lastName = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.LastName);
        var phone = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.Phone);

        var bookingDate = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.BookingDate);
        var startTime = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.BookingStartTime);
        var endTime = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.BookingEndTime);
        #endregion

        #region message
        var required = currentTranslateService.TranslateByKey(MessageKeyValidation.Required);
        var betweenCharacter = currentTranslateService.TranslateByKey(MessageKeyValidation.BetweenCharacter);
        #endregion


        RuleFor(v => v.FirstName).NotEmpty().WithMessage(required.TryFormat(firstName));
        RuleFor(v => v.LastName).NotEmpty().WithMessage(required.TryFormat(lastName));

        RuleFor(v => v.Phone).NotEmpty().WithMessage(required.TryFormat(phone))
                 .Length((int)PhoneValidator.MinLength, (int)PhoneValidator.MaxForeignLength)
                 .WithMessage(betweenCharacter.TryFormat(phone, (int)PhoneMessageValidator.MinValue, (int)PhoneMessageValidator.MinValue));

        RuleFor(v => v.BookingDate).NotEmpty().WithMessage(required.TryFormat(bookingDate))
                .When(v => v.Type == BookingType.Schedule_An_Appointment);

        RuleFor(v => v.BookingStartTime).NotEmpty().WithMessage(required.TryFormat(startTime))
                .When(v => v.Type == BookingType.Schedule_An_Appointment);

        RuleFor(v => v.BookingEndTime).NotEmpty().WithMessage(required.TryFormat(endTime))
                .When(v => v.Type == BookingType.Schedule_An_Appointment);
    }
}