using Cure_WPF;
using Microsoft.VisualBasic;
using System;
using System.IO;
using System.Media;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;

namespace Cure_2._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SpeechSynthesizer synthesizer = new SpeechSynthesizer();

        public MainWindow()
        {
            InitializeComponent();
            UserRequest.SendButtonClick += UserRequest_SendButtonClick;
        }

        async Task OpenAiLogic()
        {
            Openai openai = new Openai()
            {
             SystemMessage= "you are a highly renowned medical doctor who gives diagnosis and treament to all diseases",
             UserRequest=UserRequest.input.Text
            };
            UserRequest.input.Text = "";
            loading.Visibility = Visibility.Visible;
            try
            {
                OutputDisplayTemplate response = new OutputDisplayTemplate();
                string responseText = await openai.MakeRequest();
                if (!string.IsNullOrEmpty(responseText))
                {
                    response.DisplayResponse.Text = responseText;
                    this.dock.Children.Add(response);
                    synthesizer.SpeakAsync(responseText);
                }
                else
                {
                    MessageBox.Show("The AI did not return a response.", "API Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while communicating with the AI. Please check your internet connection and API key. Details: " + ex.Message, "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                loading.Visibility = Visibility.Collapsed;
            }
        }

        private async void SendMessage()
        {
            if (string.IsNullOrWhiteSpace(UserRequest.input.Text) || UserRequest.input.Text == "Send A Message")
            {
                return;
            }

            InputDisplayTemplate inputDisplay = new InputDisplayTemplate();
            inputDisplay.DisplayUserInput.Text = UserRequest.input.Text;
            var soundlocation = new Uri(@"Sounds/MessageSent.wav",UriKind.Relative);
            StreamResourceInfo streamResourceInfo=Application.GetResourceStream(soundlocation);
            var sound=new SoundPlayer(streamResourceInfo.Stream);
            sound.Play();
            this.dock.Children.Add(inputDisplay);
            await OpenAiLogic();
        }

        public async void UserRequest_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                SendMessage();
            }
        }

        private void UserRequest_SendButtonClick(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }
    }
}
