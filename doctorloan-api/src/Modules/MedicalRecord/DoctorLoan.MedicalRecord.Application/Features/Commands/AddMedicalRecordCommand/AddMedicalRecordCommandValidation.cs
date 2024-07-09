using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Domain.Const.Validations;
using DoctorLoan.Domain.Enums.Commons;
using DoctorLoan.Domain.Enums.Validators;
using FluentValidation;

namespace DoctorLoan.MedicalRecord.Application.Features.Commands;
public class AddMedicalRecordCommandValidator : AbstractValidator<AddMedicalRecordCommand>
{
    public AddMedicalRecordCommandValidator(ICurrentTranslateService currentTranslateService)
    {
        #region field names
        var firstName = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.FirstName);
        var lastName = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.LastName);
        var phone = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.Phone);

        var dateCreated = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.DateCreated);
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

        RuleFor(v => v.DateCreated).NotEmpty().WithMessage(required.TryFormat(dateCreated))
                .When(v => v.Status == StatusEnum.Publish);
    }
}
