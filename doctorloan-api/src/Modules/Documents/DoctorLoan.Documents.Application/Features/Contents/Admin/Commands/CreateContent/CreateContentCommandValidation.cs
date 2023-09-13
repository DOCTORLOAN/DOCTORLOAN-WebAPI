using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Domain.Const.Validations;
using FluentValidation;

namespace DoctorLoan.Contents.Application.Features.Contents.Admin.Commands;
public class CreateContentCommandValidation : AbstractValidator<CreateContentCommand>
{
    public CreateContentCommandValidation(ICurrentTranslateService currentTranslateService)
    {
        #region field names
        var documentCode = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.DocumentCode);
        var documentName = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.DocumentName);
        var documentType = currentTranslateService.TranslateFieldNameByKey(FieldNameValidation.DocumentType);
        #endregion

        #region message
        var required = currentTranslateService.TranslateByKey(MessageKeyValidation.Required);
        var maxLength = currentTranslateService.TranslateByKey(MessageKeyValidation.MaximumLength);
        #endregion

        RuleFor(v => v.Code).NotEmpty().WithMessage(required.TryFormat(documentCode))
                            .MaximumLength(50).WithMessage(maxLength.TryFormat(documentCode));
        RuleFor(v => v.Name).NotEmpty().WithMessage(required.TryFormat(documentName));
        RuleFor(v => v.Type).NotEmpty().WithMessage(required.TryFormat(documentType));
    }
}
