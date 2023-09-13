using System.Linq.Expressions;
using DoctorLoan.Domain.Entities.Addresses;

namespace DoctorLoan.Application.Common.Expressions;

public static class AddressExpression
{
    public static Expression<Func<Address, bool>> IsDuplicated(string addressNo, int? provinceId, int? districtId, int? wardId) => x =>
            x.AddressLine.ToLower().Trim() == addressNo.ToLower().Trim()
                && (provinceId.HasValue || x.ProvinceId == provinceId)
                && (districtId.HasValue || x.DistrictId == districtId)
                && (wardId.HasValue || x.WardId == wardId);
    public static Expression<Func<Address, bool>> FilterAddress(string addressNo, int? provinceId, int? districtId, int? wardId) => x =>
          (addressNo == null || x.AddressLine.Contains(addressNo))
                || (provinceId.HasValue || x.ProvinceId == provinceId)
                || (districtId.HasValue || x.DistrictId == districtId)
                || (wardId.HasValue || x.WardId == wardId);

}
