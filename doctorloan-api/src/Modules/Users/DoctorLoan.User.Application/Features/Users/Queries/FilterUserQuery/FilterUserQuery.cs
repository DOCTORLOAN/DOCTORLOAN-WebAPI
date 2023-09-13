using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Models.Commons;
using DoctorLoan.Domain.Enums.Users;
using DoctorLoan.User.Application.Features.Users.Dtos;
using MediatR;

namespace DoctorLoan.User.Application.Features.Users;

public class FilterUserQuery : QueryParam, IRequest<Result<PaginatedList<UserDto>>>
{
    public string? Search { get; set; }
    public RequestFilter Filter { get; set; } = new RequestFilter();

}

public class RequestFilter : IFilter
{
    public UserStatus? Status { get; set; }
    public int? Role { get; set; }
    public long? LastLogin { get; set; }
}