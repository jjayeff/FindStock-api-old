using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebRole1.Helper
{
    public class GetDatebaseHelper
    {
        // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =
        // | Config                                                          |
        // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =
        private static string DatabaseServer = ConfigurationManager.AppSettings["DatabaseServer"];
        private static string Database = ConfigurationManager.AppSettings["Database"];
        private static string Username = ConfigurationManager.AppSettings["DatabaseUsername"];
        private static string Password = ConfigurationManager.AppSettings["DatabasePassword"];
        // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =
        // | Model                                                           |
        // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =
        public class Visitor
        {
            public string IP_Address { get; set; }
            public string Continent { get; set; }
            public string Country { get; set; }
            public string Region { get; set; }
            public string Org { get; set; }
            public string Latitude { get; set; }
            public string Longitude { get; set; }
            public string Path_To { get; set; }
            public string Path_From { get; set; }
        }
        public class FinanceInfo
        {

            public FinanceInfo() { }

            // Properties.
            public string Date { get; set; }
            public string Year { get; set; }
            public string Quarter { get; set; }
            public double Assets { get; set; }
            public double Liabilities { get; set; }
            public double Equity { get; set; }
            public double Paid_up_cap { get; set; }
            public double Revenue { get; set; }
            public double NetProfit { get; set; }
            public double EPS { get; set; }
            public double ROA { get; set; }
            public double ROE { get; set; }
            public double NetProfitMargin { get; set; }
            public string LastUpdate { get; set; }
        }
        public class FinanceStat
        {

            public FinanceStat() { }

            // Properties.
            public string Date { get; set; }
            public string Year { get; set; }
            public double Lastprice { get; set; }
            public double Market_cap { get; set; }
            public string FS_date { get; set; }
            public double PE { get; set; }
            public double PBV { get; set; }
            public double BookValue_Share { get; set; }
            public double Dvd_Yield { get; set; }
            public string LastUpdate { get; set; }
        }
        public class Finance
        {
            public List<FinanceInfo> FinanceInfoYearly { get; set; }
            public List<FinanceStat> FinanceStatYearly { get; set; }
            public FinanceInfo FinanceInfoQuarter { get; set; }
            public FinanceStat FinanceStatDaily { get; set; }
        }
        public class GrowthStock
        {

            public GrowthStock() { }

            // Properties.
            public double Net_rate { get; set; }
            public double Assets_rate { get; set; }
            public double Price_rate { get; set; }
        }
        public class StockDividend
        {

            public StockDividend() { }

            // Properties.
            public double DIv_rate { get; set; }
            public double More_one_rate { get; set; }
            public double Double_rate { get; set; }
        }
        public class Stock
        {

            public Stock() { }

            // Properties.
            public string Symbol { get; set; }
            public string Name { get; set; }
            public string Market { get; set; }
            public string Industry { get; set; }
            public string Sector { get; set; }
            public string Website { get; set; }
            public bool SET50 { get; set; }
            public bool SET100 { get; set; }
            public double Market_cap { get; set; }
            public string First_trade_date { get; set; }
            public double Return_rate { get; set; }
            public double Price_rate { get; set; }
            public double IAA_rate { get; set; }
            public double Growth_stock_rate { get; set; }
            public double Stock_dividend_rate { get; set; }
            public double SET50_rate { get; set; }
            public double SET100_rate { get; set; }
            public double PE_rate { get; set; }
            public double PBV_rate { get; set; }
            public double ROE_rate { get; set; }
            public double ROA_rate { get; set; }
            public double Market_cap_rate { get; set; }
            public double Score { get; set; }
            public GrowthStock GrowthStock { get; set; }
            public StockDividend StockDividend { get; set; }
            public Finance Finance { get; set; }
            public List<FinanceStat> HistoryFinanceStat { get; set; }
            public string LastUpdate { get; set; }
        }
        public class StocksSector
        {

            public StocksSector() { }

            // Properties.
            public string Symbol { get; set; }
            public string Industry { get; set; }
            public string Sector { get; set; }
            public double Lastprice { get; set; }
            public double PE { get; set; }
            public double PBV { get; set; }
            public double Dvd_Yield { get; set; }
            public double Market_cap { get; set; }
            public string LastUpdate { get; set; }
        }
        // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =
        // | Main Function                                                   |
        // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =
        public dynamic GetStock(string sql, bool single = false)
        {
            string connetionString;
            SqlConnection cnn;
            connetionString = $@"Data Source={DatabaseServer};Initial Catalog={Database};User ID={Username};Password={Password}";
            cnn = new SqlConnection(connetionString);
            cnn.Open();

            SqlCommand command = new SqlCommand(sql, cnn);
            command.Parameters.AddWithValue("@zip", "india");

            List<Stock> item = new List<Stock>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var tmp = new Stock();
                    foreach (var propertyInfo in tmp.GetType().GetProperties())
                    {
                        var key = propertyInfo.Name;
                        var prop = tmp.GetType().GetProperty(key);
                        if (key != "StockDividend" && key != "GrowthStock" && key != "Finance" && key != "HistoryFinanceStat")
                        {
                            var data = String.Format("{0}", reader[key]) == "" ? null : String.Format("{0}", reader[key]);
                            if (propertyInfo.PropertyType == typeof(System.Boolean))
                            {
                                var value = Convert.ToBoolean(data);
                                prop.SetValue(tmp, value);
                            }
                            else if (propertyInfo.PropertyType == typeof(System.Double))
                            {
                                double value = 0;
                                if (data != null)
                                    value = Convert.ToDouble(data);
                                prop.SetValue(tmp, value);
                            }
                            else if (key == "LastUpdate" || key == "Date" || key == "FS_date")
                            {
                                string value = null;
                                if (data != null)
                                    value = ChangeDateFormat(data);
                                prop.SetValue(tmp, value);
                            }
                            else if (key == "First_trade_date")
                            {
                                string value = null;
                                if (data != null)
                                    value = ChangeDateFormat1(data);
                                prop.SetValue(tmp, value);
                            }
                            else if (key == "Year")
                            {
                                string value = null;
                                if (data != null)
                                    value = ChangeYearFormat(data);
                                prop.SetValue(tmp, value);
                            }
                            else
                            {
                                prop.SetValue(tmp, data);
                            }
                        }
                    }
                    //--- Get growth_stock table
                    sql = $"SELECT * FROM dbo.growth_stock WHERE Symbol = '{tmp.Symbol}'";
                    tmp.GrowthStock = SeleteDB<GrowthStock>(sql);
                    //--- Get stock_dividend table
                    sql = $"SELECT * FROM dbo.stock_dividend WHERE Symbol = '{tmp.Symbol}'";
                    tmp.StockDividend = SeleteDB<StockDividend>(sql);

                    DateTime dt = new DateTime(Convert.ToInt32(DateTime.Now.ToString("yyyy")) - 543 * 2, 1, 1);
                    DateTime newDt = dt.AddYears(-4);
                    var finance = new Finance();

                    //--- When Get all data
                    if (single)
                    {
                        //--- Get finance_info_yearly table
                        sql = $"SELECT * FROM dbo.finance_info_yearly WHERE Symbol = '{tmp.Symbol}' AND Date >= '{newDt.ToString("yyyy-MM-dd HH:mm:ss")}' ORDER BY Date";
                        finance.FinanceInfoYearly = SeleteArrayDB<FinanceInfo>(sql);
                        //--- Get finance_stat_yearly table
                        sql = $"SELECT * FROM dbo.finance_stat_yearly WHERE Symbol = '{tmp.Symbol}' AND Date >= '{newDt.ToString("yyyy-MM-dd HH:mm:ss")}' ORDER BY Date";
                        finance.FinanceStatYearly = SeleteArrayDB<FinanceStat>(sql);
                        //--- Get finance_info_quarter table
                        sql = $"SELECT * FROM dbo.finance_info_quarter WHERE Symbol = '{tmp.Symbol}' AND Date IN (SELECT max(Date) FROM dbo.finance_info_quarter WHERE Symbol = '{tmp.Symbol}')";
                        finance.FinanceInfoQuarter = SeleteDB<FinanceInfo>(sql);
                        //--- Get finance_stat_daily history table
                        sql = $"SELECT * FROM dbo.finance_stat_daily WHERE Symbol = '{tmp.Symbol}' AND Date >= '{dt.ToString("yyyy-MM-dd HH:mm:ss")}' ORDER BY Date";
                        tmp.HistoryFinanceStat = SeleteArrayDB<FinanceStat>(sql);
                    }
                    else
                    {
                        //--- Get finance_stat_daily history table
                        sql = $"SELECT TOP (2) * FROM dbo.finance_stat_daily WHERE Symbol = '{tmp.Symbol}' AND Date >= '{dt.ToString("yyyy-MM-dd HH:mm:ss")}' ORDER BY Date DESC";
                        tmp.HistoryFinanceStat = SeleteArrayDB<FinanceStat>(sql);
                    }

                    //--- Get finance_stat_daily table
                    sql = $"SELECT * FROM dbo.finance_stat_daily WHERE Symbol = '{tmp.Symbol}' AND Date IN (SELECT max(Date) FROM dbo.finance_stat_daily WHERE Symbol = '{tmp.Symbol}')";
                    finance.FinanceStatDaily = SeleteDB<FinanceStat>(sql);
                    tmp.Finance = finance;

                    item.Add(tmp);

                }
            }

            cnn.Close();

            return item;
        }
        public dynamic GetDatabase<T>(string sql) where T : class, new()
        {
            string connetionString;
            SqlConnection cnn;
            connetionString = $@"Data Source={DatabaseServer};Initial Catalog={Database};User ID={Username};Password={Password}";
            cnn = new SqlConnection(connetionString);
            cnn.Open();

            SqlCommand command = new SqlCommand(sql, cnn);
            command.Parameters.AddWithValue("@zip", "india");

            List<T> item = new List<T>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var tmp = new T();
                    foreach (var propertyInfo in tmp.GetType().GetProperties())
                    {
                        var key = propertyInfo.Name;
                        var prop = tmp.GetType().GetProperty(key);
                        if (key != "StockDividend" && key != "GrowthStock" && key != "Finance" && key != "HistoryFinanceStat")
                        {
                            var data = String.Format("{0}", reader[key]) == "" ? null : String.Format("{0}", reader[key]);
                            if (propertyInfo.PropertyType == typeof(System.Boolean))
                            {
                                var value = Convert.ToBoolean(data);
                                prop.SetValue(tmp, value);
                            }
                            else if (propertyInfo.PropertyType == typeof(System.Double))
                            {
                                double value = 0;
                                if (data != null)
                                    value = Convert.ToDouble(data);
                                prop.SetValue(tmp, value);
                            }
                            else if (key == "LastUpdate" || key == "Date" || key == "FS_date")
                            {
                                string value = null;
                                if (data != null)
                                    value = ChangeDateFormat(data);
                                prop.SetValue(tmp, value);
                            }
                            else if (key == "First_trade_date")
                            {
                                string value = null;
                                if (data != null)
                                    value = ChangeDateFormat1(data);
                                prop.SetValue(tmp, value);
                            }
                            else if (key == "Year")
                            {
                                string value = null;
                                if (data != null)
                                    value = ChangeYearFormat(data);
                                prop.SetValue(tmp, value);
                            }
                            else
                            {
                                prop.SetValue(tmp, data);
                            }
                        }
                    }
                    item.Add(tmp);
                }
            }

            cnn.Close();


            return item;
        }
        // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =
        // | Database                                                        |
        // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =
        private dynamic SeleteDB<T>(string sql) where T : class, new()
        {
            T item = new T();
            string connetionString;
            SqlConnection cnn;
            connetionString = $@"Data Source={DatabaseServer};Initial Catalog={Database};User ID={Username};Password={Password}";
            cnn = new SqlConnection(connetionString);
            cnn.Open();

            SqlCommand command = new SqlCommand(sql, cnn);
            command.Parameters.AddWithValue("@zip", "india");
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var tmp = new T();
                    foreach (var propertyInfo in tmp.GetType().GetProperties())
                    {
                        var key = propertyInfo.Name;
                        var prop = tmp.GetType().GetProperty(key);
                        if (key != "StockDividend" && key != "GrowthStock" && key != "Finance" && key != "HistoryFinanceStat")
                        {
                            Console.WriteLine($"{key} {String.Format("{0}", reader[key])}");
                            var data = String.Format("{0}", reader[key]) == "" ? null : String.Format("{0}", reader[key]);
                            if (propertyInfo.PropertyType == typeof(System.Boolean))
                            {
                                var value = Convert.ToBoolean(data);
                                prop.SetValue(tmp, value);
                            }
                            else if (propertyInfo.PropertyType == typeof(System.Double))
                            {
                                double value = 0;
                                if (data != null)
                                    value = Convert.ToDouble(data);
                                prop.SetValue(tmp, value);
                            }
                            else if (key == "LastUpdate" || key == "Date" || key == "FS_date")
                            {
                                string value = null;
                                if (data != null)
                                    value = ChangeDateFormat(data);
                                prop.SetValue(tmp, value);
                            }
                            else if (key == "First_trade_date")
                            {
                                string value = null;
                                if (data != null)
                                    value = ChangeDateFormat1(data);
                                prop.SetValue(tmp, value);
                            }
                            else if (key == "Year")
                            {
                                string value = null;
                                if (data != null)
                                    value = ChangeYearFormat(data);
                                prop.SetValue(tmp, value);
                            }
                            else
                            {
                                prop.SetValue(tmp, data);
                            }
                        }
                    }
                    item = tmp;
                }
            }


            cnn.Close();

            return item;
        }
        private dynamic SeleteArrayDB<T>(string sql) where T : class, new()
        {
            List<T> item = new List<T>();
            string connetionString;
            SqlConnection cnn;
            connetionString = $@"Data Source={DatabaseServer};Initial Catalog={Database};User ID={Username};Password={Password}";
            cnn = new SqlConnection(connetionString);
            cnn.Open();

            SqlCommand command = new SqlCommand(sql, cnn);
            command.Parameters.AddWithValue("@zip", "india");
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var tmp = new T();
                    foreach (var propertyInfo in tmp.GetType().GetProperties())
                    {
                        var key = propertyInfo.Name;
                        var prop = tmp.GetType().GetProperty(key);
                        if (key != "StockDividend" && key != "GrowthStock" && key != "Finance" && key != "HistoryFinanceStat")
                        {
                            Console.WriteLine($"{key} {String.Format("{0}", reader[key])}");
                            var data = String.Format("{0}", reader[key]) == "" ? null : String.Format("{0}", reader[key]);
                            if (propertyInfo.PropertyType == typeof(System.Boolean))
                            {
                                var value = Convert.ToBoolean(data);
                                prop.SetValue(tmp, value);
                            }
                            else if (propertyInfo.PropertyType == typeof(System.Double))
                            {
                                double value = 0;
                                if (data != null)
                                    value = Convert.ToDouble(data);
                                prop.SetValue(tmp, value);
                            }
                            else if (key == "LastUpdate" || key == "Date" || key == "FS_date")
                            {
                                string value = null;
                                if (data != null)
                                    value = ChangeDateFormat(data);
                                prop.SetValue(tmp, value);
                            }
                            else if (key == "First_trade_date")
                            {
                                string value = null;
                                if (data != null)
                                    value = ChangeDateFormat1(data);
                                prop.SetValue(tmp, value);
                            }
                            else if (key == "Year")
                            {
                                string value = null;
                                if (data != null)
                                    value = ChangeYearFormat(data);
                                prop.SetValue(tmp, value);
                            }
                            else
                            {
                                prop.SetValue(tmp, data);
                            }
                        }
                    }
                    item.Add(tmp);
                }
            }


            cnn.Close();

            return item;
        }
        private static void UpdateDatebase(string sql, SqlConnection cnn)
        {
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();

            try
            {
                command = new SqlCommand(sql, cnn);
                adapter.UpdateCommand = new SqlCommand(sql, cnn);
                adapter.UpdateCommand.ExecuteNonQuery();
                command.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {sql}");
            }
        }
        private static void InsertDatebase(string sql, SqlConnection cnn)
        {
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();

            try
            {
                command = new SqlCommand(sql, cnn);
                adapter.InsertCommand = new SqlCommand(sql, cnn);
                adapter.InsertCommand.ExecuteNonQuery();
                command.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {sql}");
            }
        }
        public dynamic VisitorDatabase(object item ,string db)
        {
            string connetionString;
            SqlConnection cnn;
            connetionString = $@"Data Source={DatabaseServer};Initial Catalog={Database};User ID={Username};Password={Password}";
            cnn = new SqlConnection(connetionString);
            cnn.Open();

            string sql = GetInsertSQL(item, db);

            InsertDatebase(sql, cnn);

            cnn.Close();

            return item;
        }
        private static string GetInsertSQL(object item, string db)
        {
            string sql = $"INSERT INTO dbo.{db} (:columns:) VALUES (:values:);";

            string[] columns = new string[item.GetType().GetProperties().Count()];
            string[] values = new string[item.GetType().GetProperties().Count()];
            int i = 0;
            foreach (var propertyInfo in item.GetType().GetProperties())
            {
                columns[i] = propertyInfo.Name;
                values[i++] = (string)(propertyInfo.GetValue(item, null));
            }

            //replacing the markers with the desired column names and values
            sql = FillColumnsAndValuesIntoInsertQuery(sql, columns, values);

            return sql;
        }
        private static string GetUpdateSQL(object item, string db, string whare)
        {
            string sql = $"UPDATE dbo.{db} SET :update: WHERE {whare} ;";

            string[] columns = new string[item.GetType().GetProperties().Count()];
            string[] values = new string[item.GetType().GetProperties().Count()];
            int i = 0;
            foreach (var propertyInfo in item.GetType().GetProperties())
            {
                columns[i] = propertyInfo.Name;
                values[i++] = (string)(propertyInfo.GetValue(item, null));
            }

            //replacing the markers with the desired column names and values
            sql = FillColumnsAndValuesIntoUpdateQuery(sql, columns, values);

            return sql;
        }
        private static string FillColumnsAndValuesIntoInsertQuery(string query, string[] columns, string[] values)
        {
            //joining the string arrays with a comma character
            string columnnames = string.Join(",", columns);
            //adding values with single quotation marks around them to handle errors related to string values
            string valuenames = ("'" + string.Join("','", values) + "'").Replace("''", "null");
            //replacing the markers with the desired column names and values
            return query.Replace(":columns:", columnnames).Replace(":values:", valuenames);
        }
        private static string FillColumnsAndValuesIntoUpdateQuery(string query, string[] columns, string[] values)
        {
            string result = "";
            for (int i = 0; i < columns.Length; i++)
                if (values[i] != null)
                    result += $"{columns[i]} = '{values[i]}'" + (i + 1 != columns.Length ? ", " : " ");
                else
                    result += $"{columns[i]} = null" + (i + 1 != columns.Length ? ", " : " ");
            //replacing the markers with the desired column names and values
            return query.Replace(":update:", result);
        }
        // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =
        // | Other    Function                                               |
        // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =
        private static string ChangeYearFormat(string year)
        {
            int new_year = Convert.ToInt32(year) + 543;
            return new_year.ToString().Substring(2);
        }
        private static string ChangeDateFormat(string date)
        {
            var part = date.Split(' ');
            var parts = part[0].Split('/');
            int mm = Convert.ToInt32(parts[0]); ;
            int dd = Convert.ToInt32(parts[1]);
            int yy = Convert.ToInt32(parts[2]) + 543;

            return $"{dd}/{mm}/{yy}";
        }
        private static string ChangeDateFormat1(string date)
        {
            var part = date.Split(' ');
            var parts = part[0].Split('/');
            int m = Convert.ToInt32(parts[0]);
            string mm = "";
            switch (m)
            {
                case 1:
                    mm = "ม.ค.";
                    break;
                case 2:
                    mm = "ก.พ.";
                    break;
                case 3:
                    mm = "มี.ค.";
                    break;
                case 4:
                    mm = "เม.ย.";
                    break;
                case 5:
                    mm = "พ.ค.";
                    break;
                case 6:
                    mm = "มิ.ย.";
                    break;
                case 7:
                    mm = "ก.ค.";
                    break;
                case 8:
                    mm = "ส.ค.";
                    break;
                case 9:
                    mm = "ก.ย.";
                    break;
                case 10:
                    mm = "ต.ค.";
                    break;
                case 11:
                    mm = "พ.ย.";
                    break;
                default:
                    mm = "ธ.ค";
                    break;
            }
            int dd = Convert.ToInt32(parts[1]);
            int yy = Convert.ToInt32(parts[2]) + 543;

            return $"{dd} {mm} {yy}";
        }
    }
}