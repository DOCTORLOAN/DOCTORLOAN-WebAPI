using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Domain.Const.Validations;
using FluentValidation;

namespace DoctorLoan.MedicalRecord.Application.Features.Commands;
public class UpdateMedicalRecordCommandValidator : AbstractValidator<UpdateMedicalRecordCommand>
{
    public UpdateMedicalRecordCommandValidator(ICurrentTranslateService currentTranslateService)
    {
        var status = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.Status);
        var required = currentTranslateService.TranslateByKey(MessageKeyValidation.Required);


        RuleFor(v => v.Status).NotEmpty().WithMessage(required.TryFormat(status));
    }
}