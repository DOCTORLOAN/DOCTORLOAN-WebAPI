using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Contents.Application.Dtos;
using MediatR;

namespace DoctorLoan.Contents.Application.Features.Contents.Admin.Commands;
public class CreateContentCommand : ContentDtos, IRequest<Result<bool>>
{
}
