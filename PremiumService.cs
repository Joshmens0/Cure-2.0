using System;
using System.Threading.Tasks;
using Windows.Services.Store;

namespace Cure_2._0
{
    public class PremiumService
    {
        private const string ProductId = "CureAI_Premium_Unlock"; // Replace with your actual Product ID
        private StoreContext storeContext = null;

        public bool IsPremium { get; private set; }

        public PremiumService()
        {
            Initialize();
        }

        private async void Initialize()
        {
            storeContext = StoreContext.GetDefault();
            await CheckPremiumStatus();
        }

        private async Task CheckPremiumStatus()
        {
            if (storeContext == null)
            {
                IsPremium = false;
                return;
            }

            StoreAppLicense appLicense = await storeContext.GetAppLicenseAsync();
            if (appLicense == null)
            {
                IsPremium = false;
                return;
            }

            foreach (var addonLicense in appLicense.AddOnLicenses)
            {
                if (addonLicense.Value.SkuStoreId.StartsWith(ProductId) && addonLicense.Value.IsActive)
                {
                    IsPremium = true;
                    return;
                }
            }

            IsPremium = false;
        }

        public async Task<bool> PurchasePremium()
        {
            if (storeContext == null)
            {
                return false;
            }

            StorePurchaseResult result = await storeContext.RequestPurchaseAsync(ProductId);

            if (result.ExtendedError != null)
            {
                return false;
            }

            switch (result.Status)
            {
                case StorePurchaseStatus.Succeeded:
                    IsPremium = true;
                    return true;

                case StorePurchaseStatus.AlreadyPurchased:
                    IsPremium = true;
                    return true;

                default:
                    return false;
            }
        }
    }
}
