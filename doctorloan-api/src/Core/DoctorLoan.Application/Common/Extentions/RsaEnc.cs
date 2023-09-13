using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace DoctorLoan.Application.Common.Extentions;

public class RsaEnc
{
    private static RSACryptoServiceProvider csp = new RSACryptoServiceProvider(2048);
    private RSAParameters _privateKey;
    private RSAParameters _publicKey;

    public RsaEnc(string privateKey, string publicKey, string passwordPrivateKey = null)
    {
        string public_pem = publicKey;
        string private_pem = privateKey;

        var pub = RsaEnc.GetPublicKeyFromPemFile(public_pem);
        var pri = RsaEnc.GetPrivateKeyFromPemFile(private_pem, passwordPrivateKey);

        _publicKey = pub.ExportParameters(false);
        _privateKey = pri.ExportParameters(true);
    }

    public static RSACryptoServiceProvider GetPrivateKeyFromPemFile(string filePath, string password)
    {
        using (TextReader privateKeyTextReader = new StringReader(filePath))
        {
            RSAParameters rsa;
            if (string.IsNullOrEmpty(password))
            {
                var KeyPair = (RsaPrivateCrtKeyParameters)new PemReader(privateKeyTextReader).ReadObject();
                rsa = DotNetUtilities.ToRSAParameters(KeyPair);
            }
            else
            {
                PemReader pr = new PemReader(privateKeyTextReader, new PasswordFinder(password));
                AsymmetricCipherKeyPair KeyPair = (AsymmetricCipherKeyPair)pr.ReadObject();
                rsa = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)KeyPair.Private);
            }

            RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
            csp.ImportParameters(rsa);
            return csp;
        }
    }

    public static RSACryptoServiceProvider GetPublicKeyFromPemFile(String filePath)
    {
        using (TextReader publicKeyTextReader = new StringReader(filePath))
        {
            PemReader pemReader = new PemReader(publicKeyTextReader, new PasswordFinder("220718"));
            RsaKeyParameters publicKeyParam = (RsaKeyParameters)new PemReader(publicKeyTextReader).ReadObject();

            RSAParameters rsaParams = DotNetUtilities.ToRSAParameters((RsaKeyParameters)publicKeyParam);

            RSACryptoServiceProvider csp = new RSACryptoServiceProvider();// cspParams);
            csp.ImportParameters(rsaParams);
            return csp;
        }
    }

    public string PublicKeyString()
    {
        var sw = new StringWriter();
        var xs = new XmlSerializer(typeof(RSAParameters));
        xs.Serialize(sw, _publicKey);
        return sw.ToString();
    }

    public string PrivateKeyString()
    {
        var sw = new StringWriter();
        var xs = new XmlSerializer(typeof(RSAParameters));
        xs.Serialize(sw, _privateKey);
        return sw.ToString();
    }

    public byte[] SignData(byte[] hashOfDataToSign)
    {
        using (var rsa = new RSACryptoServiceProvider(2048))
        {
            rsa.ImportParameters(_privateKey);
            var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
            rsaFormatter.SetHashAlgorithm("SHA256");
            return rsaFormatter.CreateSignature(hashOfDataToSign);
        }
    }

    public byte[] GetHash(string plaintext)
    {
        HashAlgorithm algorithm = SHA256.Create();
        return algorithm.ComputeHash(Encoding.UTF8.GetBytes(plaintext));
    }

    public bool VerifySignature(byte[] hashOfDataToSign, byte[] signature)
    {
        using (var rsa = new RSACryptoServiceProvider(2048))
        {
            rsa.ImportParameters(_publicKey);
            var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
            rsaDeformatter.SetHashAlgorithm("SHA256");
            return rsaDeformatter.VerifySignature(hashOfDataToSign, signature);
        }
    }

    private class PasswordFinder : IPasswordFinder
    {
        private readonly string password;

        public PasswordFinder(string password)
        {
            this.password = password;
        }

        public char[] GetPassword()
        {
            return password.ToCharArray();
        }
    }
}
