// Copyright(c) Daniel Veintimilla 2016.

namespace CSharpAssignment.Model
{
    /// <summary>Holds a stock model and its data. Price will be auto generated each request.</summary>
    public class StockModel : BaseModel
    {

        public string StockCode { get; set; }
        public string StockName { get; set; }
        public double Price { get; set; }

        public StockModel()
        {
            Price = -1;
            StockCode = string.Empty;
            StockName = string.Empty;
        }

        public StockModel(string stockCode, string stockName, double price)
        {
            Price = price;
            StockCode = stockCode;
            StockName = stockName;
        }
    }
}