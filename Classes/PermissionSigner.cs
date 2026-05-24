using System;
using System.Security.Cryptography;
using System.Text;
using pos.Classes; // علشان تقدر تستخدم KeyManager

public static class PermissionSigner
{
    // ✅ الدالة لإنشاء توقيع بناءً على staffID + مجموعة الصلاحيات
    public static string ComputeSignature(int staffID, params bool[] flags)
    {
        // حول البوليان لـ 0 أو 1
        var sb = new StringBuilder();
        sb.Append(staffID);
        foreach (var f in flags)
            sb.Append("|").Append(f ? "1" : "0");

        var payload = sb.ToString();

        // ✅ المفتاح يجي من KeyManager (بدون ما يبقى مكشوف في الكود)
        byte[] key = KeyManager.GetOrCreateKey();

        using var hmac = new HMACSHA256(key);
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
        return Convert.ToBase64String(hash);
    }

    // ✅ دالة للتحقق من التوقيع (Validation)
    public static bool VerifySignature(int staffID, string signature, params bool[] flags)
    {
        string expected = ComputeSignature(staffID, flags);
        return signature == expected;
    }
}
