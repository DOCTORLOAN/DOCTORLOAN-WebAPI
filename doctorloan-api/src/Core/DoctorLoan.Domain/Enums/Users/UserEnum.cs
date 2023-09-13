using System.ComponentModel;

namespace DoctorLoan.Domain.Enums.Users;

public enum UserStatus
{
    [Description("Chưa kích hoạt")]
    UnActive = 1,
    [Description("Đã kích hoạt")]
    Active = 2,
    [Description("Khóa TK")]
    Block = 3,
    [Description("Xóa")]
    Removed = 4,
    [Description("Xóa vĩnh viễn")]
    Deleted = 20 // permanently delete
}

public enum UserBankBranchStatus
{
    [Description("Chờ xác nhận")]
    Pending = 1,
    [Description("Không xác nhận")]
    Rejected = 2,
    [Description("Đã xác nhận")]
    Verified = 3,
    [Description("Xóa")]
    Blocked = 4
}

public enum UserIdentityType
{
    CMND_CCCD = 1,
    Passport = 2,
    Driver_License = 3
}

public enum UserIdentityStatus
{
    [Description("Chờ xác nhận")]
    Pending = 1,
    [Description("Đã xác nhận")]
    Verified = 2,
    [Description("Đã hủy")]
    Rejected = 3
}

public enum UserMediaType
{
    Avatar = 1,
    Identity = 2
}

public enum UserAddressType
{
    Home = 1,
    Company = 2
}
