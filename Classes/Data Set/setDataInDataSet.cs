using Humanizer;
using Humanizer.Localisation;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;


namespace pos.Classes
{
    internal class setDataInDataSet
    {
        public static DataSet GetBillData
            (int mainID, string billStatments = "فاتورة مبيعات", double totalAfterDis = 0,
            double change = 0, double current = 0, double Previous = 0, string Parties_From = "", 
            string Parties_To = "", int partiesiD = 0, bool breakable = false, string date = "", string time = "")
        {
            // إنشاء DataSet مرة واحدة فقط
            DataSet ds = new DAL.DataSet_A5();

            using (SqlConnection con = MainClass.GetConnection())
            {
                con.Open();

                // ===============================
                // 1️⃣ ملء جدول Bill_Info
                // ===============================
                string qryInfo = @"
                SELECT  m.InvoiceCode,
                        ISNULL(p.pName, N'غير محدد') AS pName,
                        p.pAdderss,
                        p.pPhone,
                        m.aDate,
                        m.aTime,
                        m.total,
                        m.TotalWithInterest,
                        m.descountValue,
                        m.latePayTax,
                        m.PaymentMethod,
                        m.change,
                        m.previousDebitBalance,
                        m.currentDebitBalance,
                        m.PaidAmount
                FROM tblMain1 AS m
                LEFT JOIN Parties AS p ON m.partiesID = p.pID
                LEFT JOIN shifts sh1 ON m.shiftID = sh1.ID
                LEFT JOIN staff s1 ON sh1.staffID = s1.staffID
                WHERE m.MainID = @mainID";

                using (SqlCommand cmd = new SqlCommand(qryInfo, con))
                {
                    cmd.Parameters.AddWithValue("@mainID", mainID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) // صف واحد فقط
                        {
                            DataRow drInfo = ds.Tables["Bill_Info"].NewRow();

                            drInfo["InvoiceCode"] = reader["InvoiceCode"] != DBNull.Value ? reader["InvoiceCode"].ToString() : "";
                            drInfo["Paries_Name"] = reader["pName"] != DBNull.Value ? reader["pName"].ToString() : "";
                            drInfo["Partes_Addresess"] = reader["pAdderss"] != DBNull.Value ? reader["pAdderss"].ToString() : "";
                            drInfo["Partes_Phone"] = reader["pPhone"] != DBNull.Value ? reader["pPhone"].ToString() : "";


                            drInfo["Bill_Date"] = reader["aDate"] != DBNull.Value ? Convert.ToDateTime(reader["aDate"]).ToString("yyyy-MM-dd") : DateTime.MinValue;

                            drInfo["Bill_Time"] = reader["aTime"] != DBNull.Value ? reader["aTime"].ToString() : "";
                            drInfo["Total_Befor_Dis"] = reader["total"] != DBNull.Value ? Convert.ToDecimal(reader["total"]) : 0m;

                            if(totalAfterDis == 0)
                                totalAfterDis = reader["TotalWithInterest"] != DBNull.Value ? Convert.ToDouble(reader["TotalWithInterest"]): 0;
                            drInfo["Total_After_Dis"] = totalAfterDis;

                            drInfo["Discount_Value"] = reader["descountValue"] != DBNull.Value ? Convert.ToDecimal(reader["descountValue"]) : 0m;
                            drInfo["Amount_Paid"] = reader["PaidAmount"] != DBNull.Value ? Convert.ToDecimal(reader["PaidAmount"]) : 0m;
                            drInfo["Tax"] = reader["latePayTax"] != DBNull.Value ? Convert.ToDecimal(reader["latePayTax"]) : 0m;
                            drInfo["Paymert_Method"] = reader["PaymentMethod"] != DBNull.Value ? reader["PaymentMethod"].ToString() : "";

                            drInfo["Change"] = reader["change"] != DBNull.Value ? Convert.ToDecimal(reader["change"]) : 0m;
                            drInfo["Previous_Debit_Balance"] = reader["previousDebitBalance"] != DBNull.Value ? Convert.ToDecimal(reader["previousDebitBalance"]) : 0m;
                            decimal currentDebit = reader["currentDebitBalance"] != DBNull.Value ? Convert.ToDecimal(reader["currentDebitBalance"]) : 0m;
                            drInfo["Current_Debit_Balance"] = currentDebit;
                            int amount = (currentDebit == 0) ? (int)totalAfterDis : (int)currentDebit;

                            CultureInfo culture = new CultureInfo("ar");
                            string amountInWords = amount.ToWords(culture);

                            // النص النهائي
                            drInfo["Price_Text"] = $"المبلغ المستحق سداده هو {amountInWords} جنيهًا مصريًا لا غير.";
                            drInfo["Price_Statments"] = billStatments;
                            drInfo["Breakable"] = breakable;

                            ds.Tables["Bill_Info"].Rows.Add(drInfo);

                        }
                        else
                        {
                            paidAmount(partiesiD, ds, Parties_To, Parties_From, change, current, Previous, breakable, date,time);
                        }
                    }
                }

                // ===============================
                // 2️⃣ ملء جدول Bill_Details
                // ===============================
                string qryDetails = @"
                SELECT ROW_NUMBER() OVER(ORDER BY d.DetailID) AS RowNum,
                       CASE WHEN d.isUsed = 1 THEN N'مستعمل' ELSE N'جديد' END AS isUsedStatus,
                       d.unite,
                       d.qty,
                       d.price,
                       d.vDescount,
                       d.priceAfterDes,                                 
                       CASE WHEN d.proID = 0 THEN d.proName ELSE p.pName END AS pName
                FROM tblMain1 m
                INNER JOIN tblDetails d ON m.MainID = d.MainID
                LEFT JOIN products p ON p.pID = d.proID
                LEFT JOIN category c ON c.catID = p.categoryID
                WHERE m.MainID = @mainID 
                    AND (d.DeleteFlag IS NULL OR d.DeleteFlag = 0);";

                using (SqlCommand cmd = new SqlCommand(qryDetails, con))
                {
                    cmd.Parameters.AddWithValue("@mainID", mainID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DataRow drDetail = ds.Tables["Bill_Details"].NewRow();

                            drDetail["RowNum"] = Convert.ToInt32(reader["RowNum"]);
                            drDetail["Status"] = reader["isUsedStatus"].ToString();
                            drDetail["Unite"] = reader["unite"] != DBNull.Value ? reader["unite"].ToString() : "";
                            drDetail["Qty"] = reader["qty"] != DBNull.Value ? Convert.ToDecimal(reader["qty"]) : 0m;
                            drDetail["Unite_Price"] = reader["price"] != DBNull.Value ? Convert.ToDecimal(reader["price"]) : 0m;
                            drDetail["Discout_Value"] = reader["vDescount"] != DBNull.Value ? Convert.ToDecimal(reader["vDescount"]) : 0m;
                            drDetail["Total"] = reader["priceAfterDes"] != DBNull.Value ? Convert.ToDecimal(reader["priceAfterDes"]) : 0m;
                            drDetail["Product_Name"] = reader["pName"] != DBNull.Value ? reader["pName"].ToString() : "";

                            ds.Tables["Bill_Details"].Rows.Add(drDetail);
                        }
                    }
                }
            }

            return ds;
        }
        public static void paidAmount(int partiesID, DataSet ds, string Parties_To, string Parties_From, double change, double current, double Previous ,bool breakable, string date , string time)
        {
            string qryInfo = @"
        SELECT  pName,
                pPhone,
                pAdderss                       
        FROM Parties
        WHERE pID = @partiesID";

            using (SqlConnection con = MainClass.GetConnection())
            {
                if (con.State == ConnectionState.Closed)
                    con.Open(); // 🔹 افتح الاتصال

                using (SqlCommand cmd = new SqlCommand(qryInfo, con))
                {
                    cmd.Parameters.AddWithValue("@partiesID", partiesID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) // صف واحد فقط
                        {
                            DataRow drInfo = ds.Tables["Bill_Info"].NewRow();

                            drInfo["Paries_Name"] = reader["pName"] != DBNull.Value ? reader["pName"].ToString() : "";
                            drInfo["Partes_Addresess"] = reader["pAdderss"] != DBNull.Value ? reader["pAdderss"].ToString() : "";
                            drInfo["Partes_Phone"] = reader["pPhone"] != DBNull.Value ? reader["pPhone"].ToString() : "";
                            drInfo["Parties_To"] = Parties_To;
                            drInfo["Parties_From"] = Parties_From;
                            drInfo["Change"] = Convert.ToDecimal(change);
                            drInfo["Current_Debit_Balance"] = Convert.ToDecimal(current);
                            drInfo["Previous_Debit_Balance"] = Convert.ToDecimal(Previous);
                            drInfo["Bill_Date"] = date;
                            drInfo["Bill_Time"] = time;
                            drInfo["Breakable"] = breakable;

                            ds.Tables["Bill_Info"].Rows.Add(drInfo);
                        }
                    }
                }
            }
        }


    }
         
 }
