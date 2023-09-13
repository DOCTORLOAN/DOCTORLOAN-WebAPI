using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Domain.Const.Validations;
using DoctorLoan.Domain.Enums.Validators;
using FluentValidation;

namespace DoctorLoan.Customer.Application.Features.CustomerAddresses;

public class AddCustomerAddressCommandValidator : AbstractValidator<AddCustomerAddressCommand>
{
    public AddCustomerAddressCommandValidator(ICurrentTranslateService currentTranslateService)
    {
        #region field names
        var phone = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.Phone);
        var addressNo = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.AddressNo);
        var provinceId = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.ProvinceId);
        var districtId = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.DistrictId);
        var wardId = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.WardId);
        #endregion

        #region message
        var required = currentTranslateService.TranslateByKey(MessageKeyValidation.Required);
        var betweenCharacter = currentTranslateService.TranslateByKey(MessageKeyValidation.BetweenCharacter);
        #endregion


        RuleFor(v => v.CustomerId).NotEmpty().NotNull().GreaterThan(0);
        RuleFor(v => v.Type).NotEmpty();
        RuleFor(v => v.Phone).NotEmpty().WithMessage(required.TryFormat(phone))
                 .Length((int)PhoneValidator.MinLength, (int)PhoneValidator.MaxForeignLength)
                 .WithMessage(betweenCharacter.TryFormat(phone, (int)PhoneMessageValidator.MinValue, (int)PhoneMessageValidator.MinValue));


        RuleFor(v => v.AddressLine).NotEmpty().WithMessage(required.TryFormat(addressNo));

    }
}