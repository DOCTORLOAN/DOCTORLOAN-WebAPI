namespace DoctorLoan.Application.Interfaces.Commons;
public interface IWebHelper
{
    bool IsCurrentConnectionSecured();
    string GetApiHost();
    string MapPath(string path);

}
