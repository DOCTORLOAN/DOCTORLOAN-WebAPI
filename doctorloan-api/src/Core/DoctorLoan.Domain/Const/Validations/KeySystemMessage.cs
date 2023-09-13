namespace DoctorLoan.Domain.Const.Validations;

public static class KeySystemMessage
{
    #region System
    public const string DefaultError = "DefaultError";
    public const string Forbidden = "Forbidden";
    public const string ForbiddenError = "ForbiddenError";
    public const string DateTimeFormatError = "DateTimeFormatError";
    public const string ServiceProvider = "ServiceProvider";
    public const string ServiceProviderNotFound = "ServiceProviderNotFound";
    public const string SearchAtLeastOneCharacter = "SearchAtLeastOneCharacter";
    public const string Validation = "Validation";
    public const string ValidationFormat = "ValidationFormat";
    public const string NotFound = "NotFound";
    public const string Canceled = "Canceled";
    public const string InternalServerError = "InternalServerError";
    public const string TokenExpired = "TokenExpired";
    #endregion

    #region User
    public const string User_Not_Found = "User_Not_Found";
    public const string User_Phone_Existed = "User_Phone_Existed";
    public const string User_Registered = "User_Registered";
    public const string User_Not_Found_With_PhoneNo = "User_Not_Found_With_PhoneNo";
    public const string Invalid_OTP = "Invalid_OTP";
    public const string User_Email_Existed = "User_Email_Existed";
    public const string OTP_Waiting = "OTP_Waiting";
    public const string Parent_Not_Exist = "Parent_Not_Exist";
    public const string Password_Invalid = "Password_Invalid";
    public const string System_Temporarily_Interrupted = "System_Temporarily_Interrupted";
    public const string The_User_Infor_Incorrect = "The_User_Infor_Incorrect";
    public const string The_Invalid_Code_In_Team = "The_Invalid_Code_In_Team";
    public const string UserName_Or_Password_Invalid = "UserName_Or_Password_Invalid";
    public const string The_User_Code_Invalid = "The_User_Code_Invalid";
    public const string TheUserInforIncorrect = "The_User_Infor_Incorrect";
    public const string OTP_Expired = "OTP_Expired";
    public const string UserBankExisted = "UserBankExisted";
    public const string UserBlocked = "UserBlocked";
    public const string UserIdentityPending = "UserIdentityPending";
    public const string UserIdentityVerified = "UserIdentityVerified";
    public const string UserIdentityNotFound = "UserIdentityNotFound";
    public const string UserExitedPinCode = "UserExitedPinCode";
    public const string UserNotHavePinCode = "UserNotHavePinCode";
    public const string ReferalCodeInvalidType = "ReferalCodeInvalidType";
    public const string Update_User_Profile_Failure = "Update_User_Profile_Failure";
    public const string Invalid = "Invalid";

    #endregion

    #region SMS
    public const string CodeOTP = "CodeOTP";
    public const string CodeOTP2 = "CodeOTP2";
    #endregion

    #region Email
    public const string SubjectOTP = "SubjectOTP";
    public const string ContentOTP = "ContentOTP";
    #endregion

    #region Character
    public const string Minute = "Minute";
    public const string Second = "Second";
    #endregion

    #region OTP Status
    public const string OTPWaiting = "OTPWaiting";
    #endregion

    #region Common
    public const string BankNotFound = "BankNotFound";
    public const string BranchNotExistedInBank = "BranchNotExistedInBank";
    public const string Desc = "_Desc";
    public const string Buy = "Buy";
    public const string Sell = "Sell";
    public const string Company = "Company";
    public const string InventoryCompany = "InventoryCompany";
    public const string O5_Warehouse = "O5_Warehouse";
    #endregion

    #region Unit
    public const string UnitExsited = "UnitExsited";
    public const string CodeExsited = "CodeExsited";
    #endregion
}
