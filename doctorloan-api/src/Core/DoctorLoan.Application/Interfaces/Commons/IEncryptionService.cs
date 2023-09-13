namespace DoctorLoan.Application.Interfaces.Commons;

public interface IEncryptionService
{
    string Encrypt(string value);
    string Decrypt(string value);
}
