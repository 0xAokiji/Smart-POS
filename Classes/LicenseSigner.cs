using System;
using System.Security.Cryptography;
using System.Text;
using pos.Classes; // تأكد من namespace الصحيح لـ KeyManager

public static class LicenseSigner
{
    public static string ComputeHashActivationKey(string activationKey)
    {
        if (string.IsNullOrEmpty(activationKey)) return "";
        using var sha = SHA256.Create();
        byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(activationKey));
        return Convert.ToBase64String(hash);
    }

    public static bool VerifyHashActivationKey(string inputKey, string storedHash)
    {
        string computedHash = ComputeHashActivationKey(inputKey);
        return computedHash == storedHash;
    }
    /// <summary>
    /// ✅ إنشاء توقيع (Signature) آمن للصف بالكامل
    /// </summary>
    public static string ComputeSignature(string keyCurrent, string activationKey, string hashActivationKey, string publicKey, bool isActivated)
    {
        string payload = $"{keyCurrent ?? ""}|{activationKey ?? ""}|{hashActivationKey ?? ""}|{publicKey ?? ""}|{isActivated}";
        byte[] key = KeyManager.GetOrCreateKey(); // مفتاح سري ثابت في البرنامج

        using var hmac = new HMACSHA256(key);
        byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
        return Convert.ToBase64String(hash);
    }

    /// <summary>
    /// ✅ تحقق من أن التوقيع المخزن يطابق القيم الحالية في الجدول
    /// </summary>
    public static bool VerifySignature(string keyCurrent, string activationKey, string hashActivationKey, string publicKey, bool isActivated, string storedHash)
    {
        if (string.IsNullOrEmpty(storedHash)) return false;
        string expected = ComputeSignature(keyCurrent, activationKey, hashActivationKey, publicKey, isActivated);
        return SecureEquals(expected, storedHash);
    }

    /// <summary>
    /// 🧱 مقارنة ثابتة لمنع timing attacks
    /// </summary>
    private static bool SecureEquals(string aB64, string bB64)
    {
        try
        {
            byte[] a = Convert.FromBase64String(aB64);
            byte[] b = Convert.FromBase64String(bB64);
            if (a.Length != b.Length) return false;

            int diff = 0;
            for (int i = 0; i < a.Length; i++)
                diff |= a[i] ^ b[i];

            return diff == 0;
        }
        catch
        {
            return false;
        }
    }
}
