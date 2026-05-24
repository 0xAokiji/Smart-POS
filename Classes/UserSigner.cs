using System;
using System.Security.Cryptography;
using System.Text;
using pos.Classes; // تأكد من namespace الصحيح لـ KeyManager

public static class UserSigner
{
    // ✅ بناء التوقيع من الحقول الثابتة (من غير Timestamp)
    public static string ComputeSignature(int userId, string username, string passwordHash, int staffId)
    {
        var payload = $"{userId}|{username ?? ""}|{passwordHash ?? ""}|{staffId}";

        byte[] key = KeyManager.GetOrCreateKey();

        using var hmac = new HMACSHA256(key);
        byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
        return Convert.ToBase64String(hash);
    }

    // ✅ نسخة للـ INSERT (userId لسه مش موجود)
    public static string ComputeSignatureForNew(string username, string passwordHash, int staffId)
    {
        var payload = $"0|{username ?? ""}|{passwordHash ?? ""}|{staffId}";
        byte[] key = KeyManager.GetOrCreateKey();
        using var hmac = new HMACSHA256(key);
        byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
        return Convert.ToBase64String(hash);
    }

    // ✅ تحقق التوقيع من غير Timest
    public static bool VerifySignature(int userId, string username, string passwordHash, int staffId, string signature)
    {
        if (string.IsNullOrEmpty(signature)) return false;
        string expected = ComputeSignature(userId, username, passwordHash, staffId);
        return SecureEquals(signature, expected);
    }

    private static bool SecureEquals(string aB64, string bB64)
    {
        try
        {
            byte[] a = Convert.FromBase64String(aB64);
            byte[] b = Convert.FromBase64String(bB64);
            if (a.Length != b.Length) return false;
            int diff = 0;
            for (int i = 0; i < a.Length; i++) diff |= a[i] ^ b[i];
            return diff == 0;
        }
        catch
        {
            return false;
        }
    }
}
