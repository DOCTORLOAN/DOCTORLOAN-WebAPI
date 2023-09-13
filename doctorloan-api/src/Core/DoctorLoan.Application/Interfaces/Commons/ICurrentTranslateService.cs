namespace DoctorLoan.Application.Interfaces.Commons;

public interface ICurrentTranslateService
{
    string TranslateByKey(string key);
    string TranslateFieldNameByKey(string key);
    string TranslateSystemMessageByKey(string key);
}
