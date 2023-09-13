using DoctorLoan.Application.Common.Extentions;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Domain.Const.Validations;

namespace DoctorLoan.Application.Models.Commons;
/// <summary>
/// All errors contained in ServiceResult objects must return an error of this type
/// Error codes allow the caller to easily identify the received error and take action.
/// Error messages allow the caller to easily show error messages to the end user.
/// </summary>
[Serializable]
public partial class ServiceError
{
    #region contructor & property 
    /// <summary>
    /// CTORv 
    /// </summary>
    public ServiceError(string message, int code)
    {
        this.Message = message;
        this.Code = code;
    }

    public ServiceError() { }

    /// <summary>
    /// Human readable error message
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Machine readable error code
    /// </summary>
    public int Code { get; }
    #endregion

    #region Override Equals Operator
    /// <summary>
    /// Use this to compare if two errors are equal
    /// Ref: https://msdn.microsoft.com/ru-ru/library/ms173147(v=vs.80).aspx
    /// </summary>
    public override bool Equals(object obj)
    {
        // If parameter cannot be cast to ServiceError or is null return false.
        var error = obj as ServiceError;

        // Return true if the error codes match. False if the object we're comparing to is nul
        // or if it has a different code.
        return Code == error?.Code;
    }

    public bool Equals(ServiceError error)
    {
        // Return true if the error codes match. False if the object we're comparing to is nul
        // or if it has a different code.
        return Code == error?.Code;
    }

    public override int GetHashCode()
    {
        return Code;
    }

    public static bool operator ==(ServiceError a, ServiceError b)
    {
        // If both are null, or both are same instance, return true.
        if (ReferenceEquals(a, b))
        {
            return true;
        }

        // If one is null, but not both, return false.
        if (a is null || b is null)
        {
            return false;
        }

        // Return true if the fields match:
        return a.Equals(b);
    }

    public static bool operator !=(ServiceError a, ServiceError b)
    {
        return !(a == b);
    }
    #endregion

    #region system
    /// <summary>
    /// Default error for when we receive an exception
    /// </summary>
    /// 
    public static ServiceError WithCustomMessage(string message)
   => new ServiceError(message, 900);
    public static ServiceError DefaultError => new("An exception occured.", 999);

    /// <summary>
    /// Default validation error. Use this for invalid parameters in controller actions and service methods.
    /// </summary>
    public static ServiceError ModelStateError(string validationError)
    {
        return new ServiceError(validationError, 996);
    }

    /// <summary>
    /// Use this for unauthorized responses.
    /// </summary>
    public static ServiceError ForbiddenError(ICurrentTranslateService currentTranslateService)
        => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.ForbiddenError), 998);

    /// <summary>
    /// Use this to send a custom error message
    /// </summary>
    public static ServiceError CustomMessage(string errorMessage)
    {
        return new ServiceError(errorMessage, 997);
    }

    public static ServiceError DateTimeFormatError(ICurrentTranslateService currentTranslateService)
        => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.DateTimeFormatError), 100);

    public static ServiceError ServiceProvider(ICurrentTranslateService currentTranslateService)
        => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.ServiceProvider), 101);

    /// <summary>
    /// Default error for when we receive an exception
    /// </summary>
    public static ServiceError ServiceProviderNotFound(ICurrentTranslateService currentTranslateService)
        => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.ServiceProviderNotFound), 102);

    public static ServiceError SearchAtLeastOneCharacter(ICurrentTranslateService currentTranslateService)
        => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.SearchAtLeastOneCharacter), 103);

    public static ServiceError Validation(ICurrentTranslateService currentTranslateService)
        => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.Validation), 104);

    public static ServiceError ValidationFormat(ICurrentTranslateService currentTranslateService)
        => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.ValidationFormat), 105);

    public static ServiceError NotFound(ICurrentTranslateService currentTranslateService)
        => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.NotFound), 106);

    public static ServiceError Canceled(ICurrentTranslateService currentTranslateService)
        => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.Canceled), 107);

    public static ServiceError Invalid(ICurrentTranslateService currentTranslateService)
        => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.Invalid), 108);
    #endregion

    #region User message errors
    public static ServiceError UserNotFound(ICurrentTranslateService currentTranslateService)
        => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.User_Not_Found), 120);

    public static ServiceError UserPhoneExisted(ICurrentTranslateService currentTranslateService)
        => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.User_Phone_Existed), 121);

    public static ServiceError UserRegistered(ICurrentTranslateService currentTranslateService)
        => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.User_Registered), 122);

    public static ServiceError User_Not_Found_With_PhoneNo(ICurrentTranslateService currentTranslateService)
        => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.User_Not_Found_With_PhoneNo), 123);

    public static ServiceError InvalidOTP(ICurrentTranslateService currentTranslateService)
        => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.Invalid_OTP), 123);

    public static ServiceError UserEmailExisted(ICurrentTranslateService currentTranslateService)
        => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.User_Email_Existed), 124);

    public static ServiceError OTPWaiting(ICurrentTranslateService currentTranslateService)
        => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.OTP_Waiting), 125);

    public static ServiceError ParentNotExist(ICurrentTranslateService currentTranslateService)
        => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.Parent_Not_Exist), 125);

    public static ServiceError PasswordInvalid(ICurrentTranslateService currentTranslateService)
        => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.Password_Invalid), 126);

    public static ServiceError SystemTemporarilyInterrupted(ICurrentTranslateService currentTranslateService)
        => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.System_Temporarily_Interrupted), 127);

    public static ServiceError TheUserInforIncorrect(ICurrentTranslateService currentTranslateService)
        => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.The_User_Infor_Incorrect), 127);

    public static ServiceError TheInvalidCodeInTeam(ICurrentTranslateService currentTranslateService)
        => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.The_Invalid_Code_In_Team), 129);

    public static ServiceError UserNameOrPasswordInvalid(ICurrentTranslateService currentTranslateService)
        => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.UserName_Or_Password_Invalid), 130);

    public static ServiceError OTPExpired(ICurrentTranslateService currentTranslateService)
       => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.OTP_Expired), 131);

    public static ServiceError UserBankExisted(ICurrentTranslateService currentTranslateService)
       => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.UserBankExisted), 132);

    public static ServiceError UserBlocked(ICurrentTranslateService currentTranslateService)
       => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.UserBlocked), 133);

    public static ServiceError UserIdentityPending(ICurrentTranslateService currentTranslateService)
       => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.UserIdentityPending), 134);

    public static ServiceError UserIdentityVerified(ICurrentTranslateService currentTranslateService)
       => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.UserIdentityVerified), 135);

    public static ServiceError UserIdentityNotFound(ICurrentTranslateService currentTranslateService)
       => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.UserIdentityNotFound), 136);

    public static ServiceError UserExitedPinCode(ICurrentTranslateService currentTranslateService)
       => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.UserExitedPinCode), 137);

    public static ServiceError UserNotHavePinCode(ICurrentTranslateService currentTranslateService)
       => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.UserNotHavePinCode), 138);

    public static ServiceError ReferalCodeInvalidType(ICurrentTranslateService currentTranslateService)
       => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.ReferalCodeInvalidType), 139);

    public static ServiceError UpdateUserProfileFailure(ICurrentTranslateService currentTranslateService)
      => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.ReferalCodeInvalidType), 139);
    #endregion

    #region common Exclude: 200 -> 208, 226
    public static ServiceError BankNotFound(ICurrentTranslateService currentTranslateService)
      => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.BankNotFound), 230);

    public static ServiceError BranchNotExistedInBank(ICurrentTranslateService currentTranslateService, string bankName)
      => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.BranchNotExistedInBank).ToString().TryFormat(bankName), 231);
    #endregion

    #region 300
    public static ServiceError UnitExisted(ICurrentTranslateService currentTranslateService)
       => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.UnitExsited), 300);
    public static ServiceError CodeExisted(ICurrentTranslateService currentTranslateService)
       => new(currentTranslateService.TranslateSystemMessageByKey(KeySystemMessage.CodeExsited), 301);

    #endregion  
}