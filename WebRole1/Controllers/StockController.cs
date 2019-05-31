using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebRole1.Controllers
{
    public class StockController : ApiController
    {
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
            public string SET50 { get; set; }
            public string SET100 { get; set; }
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
        // GET api/values
        public dynamic Get()
        {
            string sql = "";
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Data Source=ekkawitl.database.windows.net;Initial Catalog=Application;User ID=ekkawitl;Password=Ekk68864";
            cnn = new SqlConnection(connetionString);
            cnn.Open();

            sql = $"SELECT * FROM dbo.stock";

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
