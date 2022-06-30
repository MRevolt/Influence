using System.Security.Cryptography;
using System.Text;

namespace Influence.Service.Helper
{
    public static class Cryptography
    {

        public static string Encrypt(string password, string key, string IV)
        {
            byte[] textBytes = ASCIIEncoding.ASCII.GetBytes(password);
#pragma warning disable SYSLIB0021 // Type or member is obsolete
            using (AesCryptoServiceProvider encdec = new()
#pragma warning restore SYSLIB0021 // Type or member is obsolete
            {
                BlockSize = 128,
                KeySize = 256,
                Key = ASCIIEncoding.ASCII.GetBytes(key),
                IV = ASCIIEncoding.ASCII.GetBytes(IV),
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.CBC
            })
            {
                ICryptoTransform icrypt = encdec.CreateEncryptor(encdec.Key, encdec.IV);

                byte[] enc = icrypt.TransformFinalBlock(textBytes, 0, textBytes.Length);
                icrypt.Dispose();
                return Convert.ToBase64String(enc);
            }

        }
        public static string Decrypt(string encrypted, string key, string IV)
        {
            byte[] encbytes = Convert.FromBase64String(encrypted);
#pragma warning disable SYSLIB0021 // Type or member is obsolete
            using (AesCryptoServiceProvider encdec = new())
            {
                encdec.BlockSize = 128;
                encdec.KeySize = 256;
                encdec.Key = ASCIIEncoding.ASCII.GetBytes(key);
                encdec.IV = ASCIIEncoding.ASCII.GetBytes(IV);
                encdec.Padding = PaddingMode.PKCS7;
                encdec.Mode = CipherMode.CBC;
                ICryptoTransform icrypt = encdec.CreateDecryptor(encdec.Key, encdec.IV);

                byte[] dec = icrypt.TransformFinalBlock(encbytes, 0, encbytes.Length);
                icrypt.Dispose();
                return ASCIIEncoding.ASCII.GetString(dec);
            }

        }



    }
}
