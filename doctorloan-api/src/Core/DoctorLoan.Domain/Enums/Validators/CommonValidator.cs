namespace DoctorLoan.Domain.Enums.Validators;

public enum PhoneValidator
{
    MinLength = 9,
    MinForeignLength = 5,
    MaxForeignLength = 13
}

public enum PhoneMessageValidator
{
    MinValue = 10,
    MinForeignValue = 6,
    MaxForeignValue = 14
}

public enum IdentityValidator
{
    MinLength = 9,
    MaxLength = 12
}