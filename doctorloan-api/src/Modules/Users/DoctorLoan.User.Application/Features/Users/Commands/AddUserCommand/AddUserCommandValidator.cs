using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Domain.Const.Validations;
using DoctorLoan.Domain.Conts.Regex;
using DoctorLoan.Domain.Enums.Validators;
using FluentValidation;

namespace DoctorLoan.User.Application.Features.Users.Commands;
public class AddUserCommandValidator : AbstractValidator<AddUserCommand>
{
    public AddUserCommandValidator(ICurrentTranslateService currentTranslateService)
    {
        #region field names
        var role = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.Role);
        var firstName = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.FirstName);
        var lastName = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.LastName);
        var phone = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.Phone);
        var phoneCode = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.PhoneCode);
        var email = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.Email);
        var password = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.Password);
        #endregion

        #region message
        var required = currentTranslateService.TranslateByKey(MessageKeyValidation.Required);
        var greater = currentTranslateService.TranslateByKey(MessageKeyValidation.GreaterThan);
        var minimumLength = currentTranslateService.TranslateByKey(MessageKeyValidation.MinimumLength);
        var notValid = currentTranslateService.TranslateByKey(MessageKeyValidation.NotValid);
        var capitalLetters = currentTranslateService.TranslateByKey(MessageKeyValidation.CapitalLetters);
        var lowercaseLetters = currentTranslateService.TranslateByKey(MessageKeyValidation.LowercaseLetters);
        var containDigits = currentTranslateService.TranslateByKey(MessageKeyValidation.ContainDigits);
        var noSpace = currentTranslateService.TranslateByKey(MessageKeyValidation.NoSpace);
        var messagePassword = currentTranslateService.TranslateByKey(MessageKeyValidation.Password);
        var betweenCharacter = currentTranslateService.TranslateByKey(MessageKeyValidation.BetweenCharacter);
        #endregion

        RuleFor(v => v.FirstName).NotEmpty().WithMessage(required.TryFormat(firstName));
        RuleFor(v => v.LastName).NotEmpty().WithMessage(required.TryFormat(lastName));

        RuleFor(request => request.Password).NotEmpty().WithMessage(required.TryFormat(password))
            .MinimumLength(8).WithMessage(string.Format(minimumLength, password, 8))
            .Matches(CommonRegex.CapitalLetters).WithMessage(capitalLetters.TryFormat(password))
            .Matches(CommonRegex.LowercaseLetters).WithMessage(lowercaseLetters.TryFormat(password))
            .Matches(CommonRegex.ContainDigits).WithMessage(containDigits.TryFormat(password))
            .Matches(CommonRegex.NoSpace).WithMessage(noSpace.TryFormat(password))
        .WithMessage(messagePassword.TryFormat(password));

        RuleFor(v => v.Phone).NotEmpty().WithMessage(required.TryFormat(phone))
                 .Length((int)PhoneValidator.MinLength, (int)PhoneValidator.MaxForeignLength)
                 .WithMessage(betweenCharacter.TryFormat(phone, (int)PhoneMessageValidator.MinValue, (int)PhoneMessageValidator.MinValue))
                 .When(v => !string.IsNullOrEmpty(v.Phone));

        RuleFor(v => v.Email).NotEmpty().WithMessage(required.TryFormat(email))
                            .EmailAddress().WithMessage(string.Format(notValid, email));
    }
}
