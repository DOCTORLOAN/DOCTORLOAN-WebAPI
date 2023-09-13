using System.Security.Cryptography;

namespace DoctorLoan.Application.Common.Helpers;
public enum HashAlgorithmType
{
    MD5, SHA1, SHA256, SHA384, SHA512,
}
public class HashHelper
{

    public static string CreateHash(byte[] data, HashAlgorithmType hashAlgorithm, int trimByteCount = 0)
    {


        var algorithm = (HashAlgorithm)CryptoConfig.CreateFromName(hashAlgorithm.ToString());
        if (algorithm == null)
            throw new ArgumentException("Unrecognized hash name");

        if (trimByteCount > 0 && data.Length > trimByteCount)
        {
            var newData = new byte[trimByteCount];
            Array.Copy(data, newData, trimByteCount);

            return BitConverter.ToString(algorithm.ComputeHash(newData)).Replace("-", string.Empty);
        }

        return BitConverter.ToString(algorithm.ComputeHash(data)).Replace("-", string.Empty);
    }
}
