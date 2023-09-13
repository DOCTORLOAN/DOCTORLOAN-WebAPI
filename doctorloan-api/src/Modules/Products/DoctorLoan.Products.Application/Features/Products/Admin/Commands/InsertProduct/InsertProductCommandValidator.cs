using DoctorLoan.Products.Application.Features.Products.Admin.Commands.InsertProduct;
using FluentValidation;

namespace DoctorLoan.Products.Application.Features.Products.Admin.Commands;
public class InsertProductCommandValidator : AbstractValidator<InsertProductCommand>
{
    public InsertProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Sku).NotEmpty().MaximumLength(50);
        RuleFor(x => x.ProductItems).NotEmpty();
    }
}
