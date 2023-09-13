using System.Security.Cryptography;
using System.Text;
using DoctorLoan.Application.Interfaces.Commons;
using DoctorLoan.Application.Models.Settings;
using Microsoft.Extensions.Options;

namespace DoctorLoan.Infrastructure.Services;
public class EncryptionService : IEncryptionService
{
    private readonly SystemConfiguration _systemConfig;

    public EncryptionService(IOptions<SystemConfiguration> systemConfig)
    {
        _systemConfig = systemConfig.Value;
    }

    public string Encrypt(string value)
    {
        string EncryptionKey = _systemConfig.EncryptionKey;
        byte[] clearBytes = Encoding.Unicode.GetBytes(value);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                value = Convert.ToBase64String(ms.ToArray());
            }
        }
        return value;
    }

    public string Decrypt(string value)
    {
        string EncryptionKey = _systemConfig.EncryptionKey;
        byte[] cipherBytes = Convert.FromBase64String(value);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                value = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return value;
    }
}
