using FluentValidation;

namespace DoctorLoan.News.Application.Features.News.Admin.Commands;
public class InsertNewsItemCommandValidator : AbstractValidator<InsertNewsItemCommand>
{
    public InsertNewsItemCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
       
    }
}
