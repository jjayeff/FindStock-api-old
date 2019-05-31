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
        public class FinanceInfo
        {

            public FinanceInfo() { }

            // Properties.
            public string Date { get; set; }
            public string Year { get; set; }
            public string Quarter { get; set; }
            public string Assets { get; set; }
            public string Liabilities { get; set; }
            public string Equity { get; set; }
            public string Paid_up_cap { get; set; }
            public string Revenue { get; set; }
            public string NetProfit { get; set; }
            public string EPS { get; set; }
            public string ROA { get; set; }
            public string ROE { get; set; }
            public string NetProfitMargin { get; set; }
            public string LastUpdate { get; set; }
        }
        public class FinanceStat
        {

            public FinanceStat() { }

            // Properties.
            public string Date { get; set; }
            public string Year { get; set; }
            public string Lastprice { get; set; }
            public string Market_cap { get; set; }
            public string FS_date { get; set; }
            public string PE { get; set; }
            public string PBV { get; set; }
            public string BookValue_Share { get; set; }
            public string Dvd_Yield { get; set; }
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
            public string Net_rate { get; set; }
            public string Assets_rate { get; set; }
            public string Price_rate { get; set; }
        }
        public class StockDividend
        {

            public StockDividend() { }

            // Properties.
            public string DIv_rate { get; set; }
            public string More_one_rate { get; set; }
            public string Double_rate { get; set; }
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
            public string Market_cap { get; set; }
            public string First_trade_date { get; set; }
            public string Return_rate { get; set; }
            public string Price_rate { get; set; }
            public string IAA_rate { get; set; }
            public string Growth_stock_rate { get; set; }
            public string Stock_dividend_rate { get; set; }
            public string SET50_rate { get; set; }
            public string SET100_rate { get; set; }
            public string PE_rate { get; set; }
            public string PBV_rate { get; set; }
            public string ROE_rate { get; set; }
            public string ROA_rate { get; set; }
            public string Market_cap_rate { get; set; }
            public string Score { get; set; }
            public GrowthStock GrowthStock { get; set; }
            public StockDividend StockDividend { get; set; }
            public Finance Finance { get; set; }
            public List<FinanceStat> HistoryFinanceStat { get; set; }
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
                            if (key == "SET50" || key == "SET100")
                            {
                                var value = Convert.ToBoolean(String.Format("{0}", reader[key]));
                                prop.SetValue(tmp, value);
                            }
                            else
                            {
                                var value = String.Format("{0}", reader[key]);
                                prop.SetValue(tmp, value == "" ? null : value);
                            }
                    }
                    //--- Get growth_stock table
                    sql = $"SELECT * FROM dbo.growth_stock WHERE Symbol = '{tmp.Symbol}'";
                    tmp.GrowthStock = SeleteDB<GrowthStock>(sql);
                    //--- Get stock_dividend table
                    sql = $"SELECT * FROM dbo.stock_dividend WHERE Symbol = '{tmp.Symbol}'";
                    tmp.StockDividend = SeleteDB<StockDividend>(sql);

                    DateTime dt = new DateTime(Convert.ToInt32(DateTime.Now.ToString("yyyy")), 1, 1);
                    DateTime newDt = dt.AddYears(-4);

                    //--- When Get all data
                    if (!single)
                    {
                        var finance = new Finance();
                        //--- Get finance_info_yearly table
                        sql = $"SELECT * FROM dbo.finance_info_yearly WHERE Symbol = '{tmp.Symbol}' AND Date >= '{newDt.ToString("yyyy-MM-dd HH:mm:ss")}'";
                        finance.FinanceInfoYearly = SeleteArrayDB<FinanceInfo>(sql);
                        //--- Get finance_stat_yearly table
                        sql = $"SELECT * FROM dbo.finance_stat_yearly WHERE Symbol = '{tmp.Symbol}' AND Date >= '{newDt.ToString("yyyy-MM-dd HH:mm:ss")}'";
                        finance.FinanceStatYearly = SeleteArrayDB<FinanceStat>(sql);
                        //--- Get finance_info_quarter table
                        sql = $"SELECT * FROM dbo.finance_info_quarter WHERE Symbol = '{tmp.Symbol}' AND Date IN (SELECT max(Date) FROM dbo.finance_info_quarter WHERE Symbol = '{tmp.Symbol}')";
                        finance.FinanceInfoQuarter = SeleteDB<FinanceInfo>(sql);
                        //--- Get finance_stat_daily table
                        sql = $"SELECT * FROM dbo.finance_stat_daily WHERE Symbol = '{tmp.Symbol}' AND Date IN (SELECT max(Date) FROM dbo.finance_stat_daily WHERE Symbol = '{tmp.Symbol}')";
                        finance.FinanceStatDaily = SeleteDB<FinanceStat>(sql);
                        tmp.Finance = finance;
                        //--- Get finance_stat_daily history table
                        sql = $"SELECT * FROM dbo.finance_stat_daily WHERE Symbol = '{tmp.Symbol}' AND Date >= '{dt.ToString("yyyy-MM-dd HH:mm:ss")}' ORDER BY Date DESC";
                        tmp.HistoryFinanceStat = SeleteArrayDB<FinanceStat>(sql);
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
                        prop.SetValue(tmp, String.Format("{0}", reader[key]), null);
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
                        prop.SetValue(tmp, String.Format("{0}", reader[key]), null);
                    }
                    item.Add(tmp);
                }
            }


            cnn.Close();

            return item;
        }
    }
}