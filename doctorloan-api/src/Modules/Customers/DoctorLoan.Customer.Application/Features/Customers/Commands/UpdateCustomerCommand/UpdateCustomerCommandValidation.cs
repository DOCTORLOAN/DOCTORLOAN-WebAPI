using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Domain.Const.Validations;
using DoctorLoan.Domain.Enums.Validators;
using FluentValidation;

namespace DoctorLoan.Customer.Application.Features.Customers;


public class UpdateCustomerCommandValidation : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidation(ICurrentTranslateService currentTranslateService)
    {
        var firstName = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.FirstName);
        var lastName = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.LastName);
        var phone = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.Phone);
        var addressNo = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.AddressNo);
        var provinceId = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.ProvinceId);
        var districtId = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.DistrictId);
        var wardId = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.WardId);

        var required = currentTranslateService.TranslateByKey(MessageKeyValidation.Required);
        var betweenCharacter = currentTranslateService.TranslateByKey(MessageKeyValidation.BetweenCharacter);

        RuleFor(v => v.Id).NotEmpty().NotNull().GreaterThan(0);
        RuleFor(v => v.FirstName).NotEmpty().WithMessage(required.TryFormat(firstName));
        RuleFor(v => v.LastName).NotEmpty().WithMessage(required.TryFormat(lastName));

        RuleFor(v => v.Phone).NotEmpty().WithMessage(required.TryFormat(phone))
                 .Length((int)PhoneValidator.MinLength, (int)PhoneValidator.MaxForeignLength)
                 .WithMessage(betweenCharacter.TryFormat(phone, (int)PhoneMessageValidator.MinValue, (int)PhoneMessageValidator.MinValue));
    }
}
