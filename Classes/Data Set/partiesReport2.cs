using System;
using System.Data;
using System.Data.SqlClient;

namespace pos.Classes.Data_Set
{
    internal class partiesReport2
    {
        public static DataSet GetBillData(DateTime? startDate, DateTime? endDate, int partyID, string partyName, bool isSupplier, bool showAll = false)
        {
            DataSet ds = new DAL.DataSetPartesReport();

            (int previousDebitBalance, int currentDebitBalance) = new partiesReport2().GetTotalChangeForActiveInvoices(partyID);

            using (SqlConnection con = MainClass.GetConnection())
            {
                con.Open();

                // ✅ لو showAll = true → تجاهل التاريخ
                string dateCondition = "";
                if (!showAll && startDate.HasValue && endDate.HasValue)
                    dateCondition = "AND pt.aDate BETWEEN @StartDate AND @EndDate";

                string qryInfo = $@"
                SELECT 
                    pt.tID,
                    pt.partiesID,
                    pt.shiftID,
                    pt.mainID,
                    pt.transactionsInfo,
                    pt.transactionsType,
                    pt.previousDebitBalance,
                    pt.currentDebitBalance,
                    pt.aDate,
                    pt.aTime,
                    s.sName AS StaffName,
                    m.InvoiceCode,

                    -- ✅ حساب TotalWithInterest حسب نوع العملية
                    CASE 
                        WHEN pt.transactionsType = N'فاتورة اجل' THEN ISNULL(m.TotalWithInterest, 0)
                        WHEN pt.transactionsType IN (N'مرتجعات', N'ايداع',N'سحب') THEN 
                            ISNULL(pt.previousDebitBalance, 0) - ISNULL(pt.currentDebitBalance, 0)
                        ELSE 0
                    END AS TotalWithInterest,

                    -- ✅ المبلغ المدفوع فقط في حالة الفاتورة الآجلة
                    CASE 
                        WHEN pt.transactionsType = N'فاتورة اجل' THEN ISNULL(m.PaidAmount, 0)
                        WHEN pt.transactionsType = N'سداد من الاجل' THEN 
                            ISNULL(pt.previousDebitBalance, 0) - ISNULL(pt.currentDebitBalance, 0)
                        ELSE 0
                    END AS PaidAmount

                FROM PartiesTransactions pt
                LEFT JOIN shifts sh ON pt.shiftID = sh.ID
                LEFT JOIN staff s ON sh.staffID = s.staffID
                LEFT JOIN tblMain1 m ON pt.mainID = m.MainID
                WHERE pt.partiesID = @PartyID
                {dateCondition}
                ORDER BY pt.tID";

                using (SqlDataAdapter da = new SqlDataAdapter(qryInfo, con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@PartyID", partyID);

                    if (!showAll && startDate.HasValue && endDate.HasValue)
                    {
                        da.SelectCommand.Parameters.AddWithValue("@StartDate", startDate.Value);
                        da.SelectCommand.Parameters.AddWithValue("@EndDate", endDate.Value);
                    }

                    da.Fill(ds, "parteisRepert");
                }

                // ✅ تعبئة الأعمدة الإضافية داخل DataSet
                DataTable dt = ds.Tables["parteisRepert"];
                if (dt == null) return ds;

                int rowNum = 1;

                foreach (DataRow dr in dt.Rows)
                {
                    // رقم الصف
                    SetColumnValue(dr, "rowNum", rowNum++);
                    SetColumnValue(dr, "partyName", partyName);

                    // فترة التقرير
                    SetColumnValue(dr, "dateFrom", showAll ? "الكل" : (startDate?.ToString("yyyy-MM-dd") ?? "الكل"));
                    SetColumnValue(dr, "dateTo", showAll ? "الكل" : (endDate?.ToString("yyyy-MM-dd") ?? "الكل"));

                    // نوع المعاملة
                    SetColumnValue(dr, "transfareType", dr["transactionsType"]?.ToString() ?? "");

                    SetColumnValue(dr, "transfareValue",
                        dr["TotalWithInterest"] != DBNull.Value ? Convert.ToDecimal(dr["TotalWithInterest"]).ToString("N0") : "0");
                    SetColumnValue(dr, "paied",
                        dr["PaidAmount"] != DBNull.Value ? Convert.ToDecimal(dr["PaidAmount"]).ToString("N0") : "0");

                    // الأرصدة القديمة والجديدة
                    SetColumnValue(dr, "transfarePrevious_Debit_Balance",
                        dr["previousDebitBalance"] != DBNull.Value ? Convert.ToDecimal(dr["previousDebitBalance"]).ToString("N0") : "0");

                    SetColumnValue(dr, "transfareCurrent_Debit_Balance",
                        dr["currentDebitBalance"] != DBNull.Value ? Convert.ToDecimal(dr["currentDebitBalance"]).ToString("N0") : "0");

                    // التاريخ والوقت
                    SetColumnValue(dr, "transfareDate",
                        dr["aDate"] != DBNull.Value ? Convert.ToDateTime(dr["aDate"]).ToString("yyyy-MM-dd") : "");

                    SetColumnValue(dr, "transfareTime", dr["aTime"]?.ToString() ?? "");

                    // الرصيد النهائي من residualTable
                    SetColumnValue(dr, "previous_Debit_Balance", previousDebitBalance.ToString("N0"));
                    SetColumnValue(dr, "current_Debit_Balance", currentDebitBalance.ToString("N0"));
                }

                return ds;
            }
        }

        // ✅ دالة آمنة لتعيين القيم فقط لو العمود موجود
        private static void SetColumnValue(DataRow dr, string columnName, object value)
        {
            if (dr.Table.Columns.Contains(columnName))
                dr[columnName] = value;
        }

        // ✅ تجميع أرصدة جميع السجلات لنفس partyID
        private (int previousDebitBalance, int currentDebitBalance) GetTotalChangeForActiveInvoices(int partyID)
        {
            string qry = @"
                SELECT 
                    SUM(previousDebitBalance) AS previousDebitBalance,
                    SUM(currentDebitBalance) AS currentDebitBalance
                FROM residualTable 
                WHERE PartiesID = @PartiesID";

            using (SqlConnection con = MainClass.GetConnection())
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.Parameters.AddWithValue("@PartiesID", partyID);

                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int previous = reader["previousDebitBalance"] != DBNull.Value ? Convert.ToInt32(reader["previousDebitBalance"]) : 0;
                        int current = reader["currentDebitBalance"] != DBNull.Value ? Convert.ToInt32(reader["currentDebitBalance"]) : 0;
                        return (previous, current);
                    }
                }

                return (0, 0);
            }
        }
    }
}
