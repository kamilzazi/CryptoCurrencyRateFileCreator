namespace CryptoCurrencyRateFileCreator.Models
{
    public class CryptoCurrencyResponseModel
    {
        public string asset_id { get; set; }
        public string name { get; set; }
        public decimal price_usd { get; set; }

        public CryptoCurrencyResponseModel() { }
    }
}
