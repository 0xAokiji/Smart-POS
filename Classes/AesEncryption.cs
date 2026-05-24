using pos.Classes;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class AesEncryption
{
    // 🔹 توليد مفتاح عشوائي (لمرة واحدة)
    public static byte[] GenerateRandomKey(int sizeInBytes = 32) // 256-bit
    {
        var key = new byte[sizeInBytes];
        using (var rng = RandomNumberGenerator.Create())
            rng.GetBytes(key);
        return key;
    }

    // 🔹 التشفير => يرجع Base64 من (IV + CipherText)
    public static string Encrypt(string plainText, byte[] key)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            aes.GenerateIV();
            byte[] iv = aes.IV;

            using (var ms = new MemoryStream())
            {
                ms.Write(iv, 0, iv.Length); // حفظ IV في البداية
                using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                using (var sw = new StreamWriter(cs, Encoding.UTF8))
                {
                    sw.Write(plainText);
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    // 🔹 فك التشفير => يأخذ Base64 (IV + CipherText)
    public static string Decrypt(string cipherTextBase64, byte[] key)
    {
        if (string.IsNullOrEmpty(cipherTextBase64))
            throw new ArgumentException("Cipher text is empty");

        var full = Convert.FromBase64String(cipherTextBase64);

        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            int ivLen = aes.BlockSize / 8;
            if (full.Length < ivLen)
                throw new ArgumentException("Cipher text too short.");

            byte[] iv = new byte[ivLen];
            Array.Copy(full, 0, iv, 0, ivLen);

            byte[] cipher = new byte[full.Length - ivLen];
            Array.Copy(full, ivLen, cipher, 0, cipher.Length);

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, aes.CreateDecryptor(key, iv), CryptoStreamMode.Write))
                {
                    cs.Write(cipher, 0, cipher.Length);
                    cs.FlushFinalBlock();
                }
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }

}
