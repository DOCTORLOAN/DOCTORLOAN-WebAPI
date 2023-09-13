using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Domain.Const.Validations;
using DoctorLoan.Domain.Conts.Regex;
using DoctorLoan.Domain.Enums.Validators;
using FluentValidation;

namespace DoctorLoan.Customer.Application.Features.Customers.Commands;

public class AddCustomerCommandValidator : AbstractValidator<AddCustomerCommand>
{
    public AddCustomerCommandValidator(ICurrentTranslateService currentTranslateService)
    {
        #region field names
        var firstName = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.FirstName);
        var lastName = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.LastName);
        var phone = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.Phone);
        var password = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.Password);

        #endregion

        #region message
        var required = currentTranslateService.TranslateByKey(MessageKeyValidation.Required);
        var betweenCharacter = currentTranslateService.TranslateByKey(MessageKeyValidation.BetweenCharacter);

        var minimumLength = currentTranslateService.TranslateByKey(MessageKeyValidation.MinimumLength);
        var noSpace = currentTranslateService.TranslateByKey(MessageKeyValidation.NoSpace);
        var messagePassword = currentTranslateService.TranslateByKey(MessageKeyValidation.Password);
        var notValid = currentTranslateService.TranslateByKey(MessageKeyValidation.NotValid);
        #endregion


        RuleFor(v => v.FirstName).NotEmpty().WithMessage(required.TryFormat(firstName));
        RuleFor(v => v.LastName).NotEmpty().WithMessage(required.TryFormat(lastName));
        RuleFor(v => v.Email).NotEmpty().WithMessage(required.TryFormat("Email"))
                           .EmailAddress().WithMessage(string.Format(notValid, "Email"));

        RuleFor(v => v.Phone).NotEmpty().WithMessage(required.TryFormat(phone))
                 .Length((int)PhoneValidator.MinLength, (int)PhoneValidator.MaxForeignLength)
                 .WithMessage(betweenCharacter.TryFormat(phone, (int)PhoneMessageValidator.MinValue, (int)PhoneMessageValidator.MinValue));

        RuleFor(request => request.Password).NotEmpty().WithMessage(required.TryFormat(password))
                .MinimumLength(8).WithMessage(string.Format(minimumLength, password, 8))
                .Matches(CommonRegex.NoSpace).WithMessage(noSpace.TryFormat(password))
                .WithMessage(messagePassword.TryFormat(password));

    }
}


