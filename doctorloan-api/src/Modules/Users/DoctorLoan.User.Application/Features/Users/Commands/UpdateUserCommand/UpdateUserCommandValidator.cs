using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Domain.Const.Validations;
using DoctorLoan.Domain.Enums.Validators;
using FluentValidation;

namespace DoctorLoan.User.Application.Features.Users.Commands;
public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator(ICurrentTranslateService currentTranslateService)
    {
        #region field names
        var firstName = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.FirstName);
        var lastName = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.LastName);
        var phone = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.Phone);
        var phoneCode = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.PhoneCode);
        var email = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.Email);
        #endregion

        #region message
        var required = currentTranslateService.TranslateByKey(MessageKeyValidation.Required);
        var notValid = currentTranslateService.TranslateByKey(MessageKeyValidation.NotValid);
        var betweenCharacter = currentTranslateService.TranslateByKey(MessageKeyValidation.BetweenCharacter);
        #endregion

        RuleFor(v => v.FirstName).NotEmpty().WithMessage(required.TryFormat(firstName));
        RuleFor(v => v.LastName).NotEmpty().WithMessage(required.TryFormat(lastName));

        RuleFor(v => v.Phone).NotEmpty().WithMessage(required.TryFormat(phone))
                 .Length((int)PhoneValidator.MinLength, (int)PhoneValidator.MaxForeignLength)
                 .WithMessage(betweenCharacter.TryFormat(phone, (int)PhoneMessageValidator.MinValue, (int)PhoneMessageValidator.MinValue))
                 .When(v => !string.IsNullOrEmpty(v.Phone));

        RuleFor(v => v.Email).NotEmpty().WithMessage(required.TryFormat(email))
                            .EmailAddress().WithMessage(string.Format(notValid, email));
    }
}
