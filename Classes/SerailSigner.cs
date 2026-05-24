using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using pos.Classes;

public static class SerailSigner
{
    // إنشاء توقيع لأي نص (serial + isFirstTime)
    public static string ComputeSignature(string serial, bool isFirstTime)
    {
        string payload = $"{serial}|{(isFirstTime ? "1" : "0")}";

        byte[] key = KeyManagerForSerialNumber.GetOrCreateKey();
        using var hmac = new HMACSHA256(key);
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));

        return Convert.ToBase64String(hash);
    }

    // تحقق من السجل في جدول serialNumber
    public static bool VerifyFromDatabase(SqlConnection con)
    {
        string query = "SELECT TOP 1 serial, isFirstTime, Signature FROM serialNumber";
        using (SqlCommand cmd = new SqlCommand(query, con))
        using (SqlDataReader reader = cmd.ExecuteReader())
        {
            if (reader.Read())
            {
                string serial = reader.GetString(0);
                bool isFirstTime = reader.GetBoolean(1);
                string dbSignature = reader.GetString(2);

                string expectedSignature = ComputeSignature(serial, isFirstTime);
                //MessageBox.Show($"DB Signature: {dbSignature}\nExpected Signature: {expectedSignature}");
                return dbSignature == expectedSignature;
            }
        }
        return false;
    }

    // تحديث التوقيع في الجدول (مثلاً بعد أول إدخال أو تعديل)
    public static void UpdateSignature(SqlConnection con, string serial, bool isFirstTime)
    {
        string signature = ComputeSignature(serial, isFirstTime);

        string query = @"
        UPDATE serialNumber 
        SET serial = @serial, 
            isFirstTime = @isFirstTime, 
            Signature = @Signature
        WHERE serialID = 1";

        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            cmd.Parameters.AddWithValue("@serial", serial);
            cmd.Parameters.AddWithValue("@isFirstTime", isFirstTime);
            cmd.Parameters.AddWithValue("@Signature", signature);
            cmd.ExecuteNonQuery();
        }
    }

}
