using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace No7.Solution
{
    public struct TradeRecord
    { 
        public TradeRecord(string destinationCurrency, string sourceCurrency, float lots, decimal price)
        {
            DestinationCurrency = destinationCurrency;
            Lots = lots;
            Price = price;
            SourceCurrency = sourceCurrency;
        }

        public string DestinationCurrency { get; }

        public float Lots { get; }

        public decimal Price { get; }

        public string SourceCurrency { get; }
    }
}
