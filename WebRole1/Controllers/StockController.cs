using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebRole1.Helper;

namespace WebRole1.Controllers
{
    public class StockController : ApiController
    {
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
        private class Stock
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
        // | GET api/stock                                                   |
        // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =
        public dynamic Get()
        {
            var getDB = new GetDatebaseHelper();
            string sql = $"SELECT * FROM dbo.stock ORDER BY Score DESC";

            return getDB.GetStock(sql);
        }
        // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =
        // | GET api/stock/fast                                              |
        // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =
        [HttpGet]
        [Route("api/stockfast")]
        public dynamic GetFast()
        {
            var getDB = new GetDatebaseHelper();
            string sql = $"SELECT TOP (10) * FROM dbo.stock ORDER BY Score DESC";

            return getDB.GetStock(sql);
        }
        // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =
        // | GET api/stock/fast                                              |
        // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =
        [HttpGet]
        [Route("api/stock/{symbol}")]
        public dynamic GetBySymbol(string symbol)
        {
            var getDB = new GetDatebaseHelper();
            string sql = $"SELECT * FROM dbo.stock WHERE Symbol = '{symbol}'";

            return getDB.GetStock(sql, true);
        }
        // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =
        // | POST api/visitor                                              |
        // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =
        [HttpPost]
        [Route("api/visitor")]
        public dynamic PostFundamental([FromBody]Visitor value)
        {
            var insertDB = new GetDatebaseHelper();

            return insertDB.VisitorDatabase(value, "visitor");
        }
        // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =
        // | GET api/sector                                                  |
        // = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =
        [HttpGet]
        [Route("api/sector/{sector}")]
        public dynamic GetStocksBySector(string sector)
        {
            var getDB = new GetDatebaseHelper();
            string sql = $"SELECT stock.Symbol, " +
            "stock.Industry, " +
            "stock.Sector, " +
            "finance_stat_daily.Lastprice, " +
            "finance_stat_daily.PE, " +
            "finance_stat_daily.PBV, " +
            "finance_stat_daily.Dvd_Yield, " +
            "stock.Market_cap, " +
            "stock.LastUpdate " +
            "FROM stock INNER JOIN finance_stat_daily ON stock.Symbol = finance_stat_daily.Symbol " +
            $"WHERE stock.Sector = '{sector}' AND finance_stat_daily.LastUpdate = (SELECT MAX(LastUpdate) FROM finance_stat_daily) ";

            return getDB.GetDatabase<StocksSector>(sql);
        }
    }
}
