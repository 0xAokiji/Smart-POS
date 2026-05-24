using System;
using System.IO;
using System.Security.Cryptography;

namespace pos.Classes
{
    internal class KeyManager
    {
        private static readonly string keyPath = "aes.key"; // ملف لتخزين المفتاح المحمي

        // 🔹 يرجع المفتاح فقط لو الملف موجود، ولو مش موجود ينشئه مرة واحدة
        public static byte[] GetOrCreateKey()
        {
            if (File.Exists(keyPath))
            {
                // ✅ الملف موجود → نقرأ المفتاح المحمي
                byte[] protectedKey = File.ReadAllBytes(keyPath);
                return ProtectedData.Unprotect(protectedKey, null, DataProtectionScope.CurrentUser);
            }
            else
            {
                // ⚙️ الملف غير موجود → نولّد مفتاح جديد ونخزّنه فقط
                byte[] key = AesEncryption.GenerateRandomKey();
                byte[] protectedKey = ProtectedData.Protect(key, null, DataProtectionScope.CurrentUser);
                File.WriteAllBytes(keyPath, protectedKey);

                // لا نرجّع المفتاح (تم الإنشاء فقط)
                return null;
            }
        }
    }
}
