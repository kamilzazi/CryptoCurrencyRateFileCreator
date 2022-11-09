using System.Collections.Generic;

namespace CryptoCurrencyRateFileCreator.Models
{
    public class CurrencyResponseModel
    {
        public List<CurrencyRatesResponseModel> rates { get; set; }
        public string table { get; set; }
        public string currency { get; set; }
        public string code { get; set; }

        public CurrencyResponseModel() { }
    }

    public class CurrencyRatesResponseModel
    {
        public string no { get; set; }
        public string effectiveDate { get; set; }
        public string mid { get; set; }

        public CurrencyRatesResponseModel() { }
    }
}
