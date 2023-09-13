using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Domain.Const.Validations;
using DoctorLoan.Domain.Enums.Validators;
using FluentValidation;

namespace DoctorLoan.Order.Application.Features.Commands;


public class AddOrderCommandValidator : AbstractValidator<AddOrderCommand>
{
    public AddOrderCommandValidator(ICurrentTranslateService currentTranslateService)
    {
        #region field names
        var name = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.Name);
        var phone = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.Phone);
        var address = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.AddressNo);

        #endregion

        #region message
        var required = currentTranslateService.TranslateByKey(MessageKeyValidation.Required);
        var betweenCharacter = currentTranslateService.TranslateByKey(MessageKeyValidation.BetweenCharacter);
        var notValid = currentTranslateService.TranslateByKey(MessageKeyValidation.NotValid);
        #endregion


        RuleFor(v => v.PaymentMethod).NotEmpty().WithMessage(required.TryFormat("Phương thức thanh toán"));
        RuleFor(v => v.AddressLine).NotEmpty().WithMessage(required.TryFormat(address));
        RuleFor(v => v.FullName).NotEmpty().WithMessage(required.TryFormat(name));
        //RuleFor(v => v.Email).NotEmpty().WithMessage(required.TryFormat("Email"))
        //                   .EmailAddress().WithMessage(string.Format(notValid, "Email"));

        RuleFor(v => v.ListItem).NotEmpty().WithMessage(required.TryFormat("Sản phẩm"))
                                .NotNull().WithMessage(required.TryFormat("Sản phẩm"));

        RuleFor(v => v.Phone).NotEmpty().WithMessage(required.TryFormat(phone))
                 .Length((int)PhoneValidator.MinLength, (int)PhoneValidator.MaxForeignLength)
                 .WithMessage(betweenCharacter.TryFormat(phone, (int)PhoneMessageValidator.MinValue, (int)PhoneMessageValidator.MinValue));

        RuleForEach(p => p.ListItem).ChildRules(child =>
        {
            child.RuleFor(x => x.ProductItemId).NotEmpty().WithMessage(required.TryFormat("Sản phẩm"));
            child.RuleFor(x => x.Quantity).NotEmpty().WithMessage(required.TryFormat("Số lượng"));
        });
    }
}