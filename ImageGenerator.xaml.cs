using Cure_WPF;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Cure_2._0
{
    /// <summary>
    /// Interaction logic for ImageGenerator.xaml
    /// </summary>
    public partial class ImageGenerator : UserControl
    {
        private UsageTracker usageTracker = new UsageTracker();
        private PremiumService premiumService = new PremiumService();

        public ImageGenerator()
        {
            InitializeComponent();
        }

        private async void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ImagePrompt.Text) || ImagePrompt.Text == "Enter a prompt to generate an image")
            {
                MessageBox.Show("Please enter a prompt for the image.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!premiumService.IsPremium && usageTracker.IsImageLimitReached())
            {
                if (UpgradePrompt.Show())
                {
                    premiumService.UnlockPremium();
                    MessageBox.Show("Congratulations! You've unlocked unlimited access.", "Upgrade Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                return;
            }

            if (!premiumService.IsPremium)
            {
                usageTracker.IncrementImageGenerationCount();
            }

            loading.Visibility = Visibility.Visible;
            GeneratedImage.Source = null;

            try
            {
                Openai openai = new Openai()
                {
                    ImagePrompt = ImagePrompt.Text
                };

                string imageUrl = await openai.GetImage();

                if (!string.IsNullOrEmpty(imageUrl))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(imageUrl, UriKind.Absolute);
                    bitmap.EndInit();

                    GeneratedImage.Source = bitmap;
                }
                else
                {
                    MessageBox.Show("The AI did not return an image.", "API Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while generating the image. Please check your internet connection and API key. Details: " + ex.Message, "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                loading.Visibility = Visibility.Collapsed;
            }
        }
    }
}
