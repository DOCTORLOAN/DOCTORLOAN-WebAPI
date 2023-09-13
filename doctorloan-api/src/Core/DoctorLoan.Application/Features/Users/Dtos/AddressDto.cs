using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Domain.Entities.Users;

namespace DoctorLoan.Application.Features.Users.Dtos;
public class AddressDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PhoneNo { get; set; }
    public string Address { get; set; }

    public AddressDto(UserAddress data)
    {
        if (data == null)
            return;
        Id = data.Id;
        Name = data.FullName;
        PhoneNo = data.PhoneCode.DisplayPhoneNo(data.Phone);
        Address = data.Address.DisplayAddress();
    }
}