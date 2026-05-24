using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace pos.Classes.Data_Set
{
    internal class partiesRport
    {
        public static DataSet GetBillData(DateTime? startDate, DateTime? endDate, int partyID, string partyName, bool isSupplier, bool showAll = false)
        {
            DataSet ds = new DAL.DataSetPartesReport();

            (int previousDebitBalance, int currentDebitBalance) = new partiesRport().GetTotalChangeForActiveInvoices(partyID);

            using (SqlConnection con = MainClass.GetConnection())
            {
                con.Open();
                string qryInfo;

                // ✅ إذا كان showAll = true ⇒ تجاهل التاريخ تمامًا
                string dateCondition = "";
                if (!showAll && startDate.HasValue && endDate.HasValue)
                    dateCondition = "AND [date] BETWEEN @StartDate AND @EndDate";

                if (!isSupplier)
                {
                    // ✅ عميل
                    qryInfo = $@"
            SELECT 
                N'فاتورة أجل' AS TransactionType,
                total AS Value,
                PaidAmount AS PaidAmount,
                previousDebitBalance,
                currentDebitBalance,
                aDate AS [Date],
                aTime AS [Time]
            FROM tblMain1
            WHERE partiesID = @PartyID
              {(showAll ? "" : (startDate.HasValue && endDate.HasValue ? "AND aDate BETWEEN @StartDate AND @EndDate" : ""))}

            UNION ALL

            SELECT 
                N'سداد من الأجل' AS TransactionType,
                NULL AS Value,
                recipt AS PaidAmount,
                [previousDebitBalance] AS previousDebitBalance,
                [change] AS currentDebitBalance,
                [date] AS [Date],
                [time] AS [Time]
            FROM chargeResidual
            WHERE partiesID = @PartyID
              {dateCondition}

            ORDER BY [Date], [Time];";
                }
                else
                {
                    // ✅ مورد
                    qryInfo = $@"
            SELECT 
                N'فاتورة أجل' AS TransactionType,
                total AS Value,
                PaidAmount AS PaidAmount,
                previousDebitBalance,
                currentDebitBalance,
                [date] AS [Date],
                [time] AS [Time]
            FROM billPrcheses
            WHERE supplierID = @PartyID
              {dateCondition}

            UNION ALL

            SELECT 
                N'سداد من الأجل' AS TransactionType,
                NULL AS Value,
                recipt AS PaidAmount,
                [previousDebitBalance] AS previousDebitBalance,
                [change] AS currentDebitBalance,
                [date] AS [Date],
                [time] AS [Time]
            FROM chargeResidualSuplieser
            WHERE partiesID = @PartyID
              {dateCondition}

            ORDER BY [Date], [Time];";
                }

                using (SqlDataAdapter da = new SqlDataAdapter(qryInfo, con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@PartyID", partyID);

                    // ✅ ما تضيفش براميترات التاريخ إلا لو showAll = false
                    if (!showAll && startDate.HasValue && endDate.HasValue)
                    {
                        da.SelectCommand.Parameters.AddWithValue("@StartDate", startDate.Value);
                        da.SelectCommand.Parameters.AddWithValue("@EndDate", endDate.Value);
                    }

                    da.Fill(ds, "parteisRepert");
                }

                // ✅ تعبئة الأعمدة المحسوبة
                int rowNum = 1;
                DataTable dt = ds.Tables["parteisRepert"];

                foreach (DataRow dr in dt.Rows)
                {
                    dr["rowNum"] = rowNum++;
                    dr["partyName"] = partyName;


                    dr["dateFrom"] = showAll ? "الكل" : (startDate.HasValue ? startDate.Value.ToString("yyyy-MM-dd") : "الكل");
                    dr["dateTo"] = showAll ? "الكل" : (endDate.HasValue ? endDate.Value.ToString("yyyy-MM-dd") : "الكل");

                    dr["transfareType"] = dr["TransactionType"]?.ToString() ?? "";

                    dr["transfareValue"] = dr["Value"] != DBNull.Value
                        ? Convert.ToDecimal(dr["Value"]).ToString("N0")
                        : "0";

                    dr["paied"] = dr["PaidAmount"] != DBNull.Value
                        ? Convert.ToDecimal(dr["PaidAmount"]).ToString("N0")
                        : "0";

                    dr["transfarePrevious_Debit_Balance"] = dr["previousDebitBalance"] != DBNull.Value
                        ? Convert.ToDecimal(dr["previousDebitBalance"]).ToString("N0")
                        : "0";

                    dr["transfareCurrent_Debit_Balance"] = dr["currentDebitBalance"] != DBNull.Value
                        ? Convert.ToDecimal(dr["currentDebitBalance"]).ToString("N0")
                        : "0";

                    dr["transfareDate"] = dr["Date"] != DBNull.Value
                        ? Convert.ToDateTime(dr["Date"]).ToString("yyyy-MM-dd")
                        : "";
                    dr["transfareTime"] = dr["Time"]?.ToString() ?? "";

                    dr["previous_Debit_Balance"] = previousDebitBalance.ToString("N0");
                    dr["current_Debit_Balance"] = currentDebitBalance.ToString("N0");
                }

                return ds;
            }
        }

        private (int previousDebitBalance, int currentDebitBalance) GetTotalChangeForActiveInvoices(int partyID)
        {
            string qry = @"
                SELECT previousDebitBalance, currentDebitBalance
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
                    else
                    {
                        return (0, 0);
                    }
                }
            }
        }
    }
}
