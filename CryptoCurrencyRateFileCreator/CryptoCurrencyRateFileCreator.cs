using CryptoCurrencyRateFileCreator.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GetCryptoCurrenciesRates
{
    class CryptoCurrencyRateFileCreator
    {
        static void Main(string[] args)
        {
            GetRateByAssetIdAndSaveToFile();
        }

        private static void GetRateByAssetIdAndSaveToFile()
        {
            Console.WriteLine(Environment.MachineName);

            AppSettingsModel appSettingsModel;
            var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\")) + "appSettings.json";

            using (StreamReader sr = File.OpenText(path))
            {
                var appSettingsString = sr.ReadToEnd();
                appSettingsModel = JsonConvert.DeserializeObject<AppSettingsModel>(appSettingsString);
            }

            GetCryptoCurrencyRateByAssetIdAndSaveToFile(appSettingsModel);
            GetCurrencyRateByAssetIdAndSaveToFile(appSettingsModel);
        }

        public static void GetCryptoCurrencyRateByAssetIdAndSaveToFile(AppSettingsModel appSettingsModel)
        {
            var firstMachineSettings = appSettingsModel.MachineSettings[0];
            var secondMachineSettings = appSettingsModel.MachineSettings[1];

            var cryptoCurrencyAssetIdcs = typeof(CryptoCurrencyAssetIdcs)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(field => field.IsLiteral)
                .Where(field => field.FieldType == typeof(String))
                .Select(field => field.GetValue(null) as String);

            foreach (var assetId in cryptoCurrencyAssetIdcs)
            {
                var client = new RestClient("https://rest.coinapi.io/v1/assets/" + assetId + "?apikey=" + appSettingsModel.ApiKey);
                var request = new RestRequest();
                var response = client.Get(request);
                if (response.IsSuccessful)
                {
                    var deserializedContent = JsonConvert.DeserializeObject<List<CryptoCurrencyResponseModel>>(response.Content);

                    Console.WriteLine(deserializedContent[0].asset_id + ": " + deserializedContent[0].price_usd);

                    if (Environment.MachineName == firstMachineSettings.MachineName)
                    {
                        var filePath = Path.GetFullPath(firstMachineSettings.Path + assetId + ".txt");
                        using (StreamWriter sw = File.CreateText(filePath))
                        {
                            sw.WriteLine(deserializedContent[0].price_usd);
                        }
                    }
                    else if (Environment.MachineName == secondMachineSettings.MachineName)
                    {
                        var filePath = Path.GetFullPath(secondMachineSettings.Path + assetId + ".txt");
                        using (StreamWriter sw = File.CreateText(filePath))
                        {
                            sw.WriteLine(deserializedContent[0].price_usd);
                        }
                    }
                }
                else
                {
                    GetCryptoCurrencyRateByAssetIdAndSaveToFile(appSettingsModel);
                }
            }
        }

        public static void GetCurrencyRateByAssetIdAndSaveToFile(AppSettingsModel appSettingsModel)
        {
            var firstMachineSettings = appSettingsModel.MachineSettings[0];
            var secondMachineSettings = appSettingsModel.MachineSettings[1];

            var currencyAssetIdcs = typeof(CurrencyAssetIdcs)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(field => field.IsLiteral)
                .Where(field => field.FieldType == typeof(String))
                .Select(field => field.GetValue(null) as String);

            foreach (var assetId in currencyAssetIdcs)
            {
                var client = new RestClient("https://api.nbp.pl/api/exchangerates/rates/a/" + assetId + "?format=json");
                var request = new RestRequest();
                var response = client.Get(request);
                if (response.IsSuccessful)
                {
                    var deserializedContent = JsonConvert.DeserializeObject<CurrencyResponseModel>(response.Content);

                    Console.WriteLine(deserializedContent.code + ": " + deserializedContent.rates[0].mid);

                    if (Environment.MachineName == firstMachineSettings.MachineName)
                    {
                        var filePath = Path.GetFullPath(firstMachineSettings.Path + assetId + ".txt");

                        using (StreamWriter sw = File.CreateText(filePath))
                        {
                            sw.WriteLine(deserializedContent.rates[0].mid);
                        }
                    }
                    else if (Environment.MachineName == secondMachineSettings.MachineName)
                    {
                        var filePath = Path.GetFullPath(secondMachineSettings.Path + assetId + ".txt");

                        using (StreamWriter sw = File.CreateText(filePath))
                        {
                            sw.WriteLine(deserializedContent.rates[0].mid);
                        }
                    }
                }
                else
                {
                    GetCurrencyRateByAssetIdAndSaveToFile(appSettingsModel);
                }
            }
        }
    }
}
