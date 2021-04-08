using System;
using System.Security.Cryptography;
using System.Text;

public static class Encryption
{
    private static readonly string hash = "242424242424242424242424";

    public static string Encrypt(string input)
    {
        byte[] data = UTF8Encoding.UTF8.GetBytes(input);
        using (SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider())
        {
            byte[] key = sha256.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            byte[] trimmedKey = new byte[24];
            Buffer.BlockCopy(key, 0, trimmedKey, 0, 24);
            using TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider() { Key = trimmedKey, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 };
            ICryptoTransform tr = trip.CreateEncryptor();
            byte[] result = tr.TransformFinalBlock(data, 0, data.Length);
            return Convert.ToBase64String(result, 0, result.Length);
        }
    }

    public static string Decrypt(string input)
    {
        byte[] data = Convert.FromBase64String(input);
        using (SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider())
        {
            byte[] key = sha256.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            byte[] trimmedKey = new byte[24];
            Buffer.BlockCopy(key, 0, trimmedKey, 0, 24);
            using TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider() { Key = trimmedKey, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 };
            ICryptoTransform tr = trip.CreateDecryptor();
            byte[] result = tr.TransformFinalBlock(data, 0, data.Length);
            return UTF8Encoding.UTF8.GetString(result);
        }
    }
}
