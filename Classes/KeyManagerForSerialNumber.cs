using System;
using System.Security.Cryptography;
using System.Text;

namespace pos.Classes
{
    internal class KeyManagerForSerialNumber
    {
        // مفتاح ثابت (يدوي) مكتوب Base64
        private static readonly string manualKeyBase64 = "7yH2FJgkPq9bR1sYvXe9tGZc3Lp4d8Mq2j5Tk6Uw1z0=";

        public static byte[] GetOrCreateKey()
        {
            // رجع المفتاح مباشرة كمصفوفة بايت
            return Convert.FromBase64String(manualKeyBase64);
        }
    }
}
