using System.Windows;

namespace Cure_2._0
{
    public static class UpgradePrompt
    {
        public static bool Show()
        {
            string message = "You have reached your daily limit for the free version. Would you like to upgrade to the premium version for unlimited access?";
            string caption = "Upgrade to Premium";
            MessageBoxResult result = MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Information);
            return result == MessageBoxResult.Yes;
        }
    }
}
