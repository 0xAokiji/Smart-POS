using System;
using System.Security.Cryptography;
using System.Text;
using pos.Classes; // علشان توصل لـ KeyManager

public static class PermissionHelper
{
    // ✅ إنشاء توكن مشفّر/موقّع من قيمة true/false
    public static string GeneratePermissionToken(bool value)
    {
        string plainText = value ? "ALLOW" : "DENY";

        // استخدم المفتاح من KeyManager
        byte[] key = KeyManager.GetOrCreateKey();

        string signature = ComputeHmac(plainText, key);
        return $"{plainText}:{signature}";
    }

    // ✅ التحقق من التوكن اللي جاي من الداتا بيز
    public static bool VerifyPermission(string token)
    {
        if (string.IsNullOrEmpty(token)) return false;

        var parts = token.Split(':');
        if (parts.Length != 2) return false;

        string plainText = parts[0];
        string signature = parts[1];

        byte[] key = KeyManager.GetOrCreateKey();
        string expectedSignature = ComputeHmac(plainText, key);

        if (signature != expectedSignature)
            return false; // حد لعب في القيمة

        return plainText == "ALLOW";
    }

    // ✅ دالة HMAC للتوقيع باستخدام المفتاح
    private static string ComputeHmac(string data, byte[] key)
    {
        using (var hmac = new HMACSHA256(key))
        {
            byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return Convert.ToBase64String(hash);
        }
    }
}
