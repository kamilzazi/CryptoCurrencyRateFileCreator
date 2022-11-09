using System.Collections.Generic;

namespace CryptoCurrencyRateFileCreator.Models
{
    public class AppSettingsModel
    {
        public string ApiKey { get; set; }
        public List<MachineSetting> MachineSettings { get; set; }

        public AppSettingsModel() { }
    }

    public class MachineSetting
    {
        public string MachineName { get; set; }
        public string Path { get; set; }

        public MachineSetting() { }
    }
}
