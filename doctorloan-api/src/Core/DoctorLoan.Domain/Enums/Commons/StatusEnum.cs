namespace DoctorLoan.Domain.Enums.Commons;

public enum StatusEnum
{
    Draft = 1,
    Publish = 2,
    Removed = 10
}

public enum BookingStatus
{
    Pending = 10,
    Confirm = 20,
    Rejected = 30,
    Processing  = 40,
    Completed = 50
}

public enum OrderStatus
{
    Pending = 10,
    Confirm = 20,
    Devivery = 30,
    Completed = 40,
    Rejected = 50,
    Return = 60,
    ReturnCompleted = 70
}