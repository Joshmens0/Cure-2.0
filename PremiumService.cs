using Newtonsoft.Json;
using System.IO;

namespace Cure_2._0
{
    public class PremiumService
    {
        private const string PremiumFile = "premium.json";

        public bool IsPremium { get; private set; }

        public PremiumService()
        {
            LoadPremiumStatus();
        }

        public void UnlockPremium()
        {
            IsPremium = true;
            SavePremiumStatus();
        }

        private void LoadPremiumStatus()
        {
            if (File.Exists(PremiumFile))
            {
                var json = File.ReadAllText(PremiumFile);
                var data = JsonConvert.DeserializeObject<PremiumData>(json);
                IsPremium = data.IsPremium;
            }
        }

        private void SavePremiumStatus()
        {
            var data = new PremiumData { IsPremium = this.IsPremium };
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(PremiumFile, json);
        }

        private class PremiumData
        {
            public bool IsPremium { get; set; }
        }
    }
}
