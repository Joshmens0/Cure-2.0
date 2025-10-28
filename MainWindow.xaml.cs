using Cure_WPF;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        private List<ChatMessage> chatHistory = new List<ChatMessage>();
        private const string ChatHistoryFile = "chathistory.json";
        private UsageTracker usageTracker = new UsageTracker();
        private PremiumService premiumService = new PremiumService();

        public MainWindow()
        {
            InitializeComponent();
            UserRequest.SendButtonClick += UserRequest_SendButtonClick;
            LoadChatHistory();
        }

        async Task OpenAiLogic()
        {
            var userMessage = new ChatMessage { Sender = "User", Content = UserRequest.input.Text };
            chatHistory.Add(userMessage);

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
                    var aiMessage = new ChatMessage { Sender = "AI", Content = responseText };
                    chatHistory.Add(aiMessage);
                    SaveChatHistory();

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

            if (!premiumService.IsPremium && usageTracker.IsChatLimitReached())
            {
                if (UpgradePrompt.Show())
                {
                    if (await premiumService.PurchasePremium())
                    {
                        MessageBox.Show("Congratulations! You've unlocked unlimited access. Please restart the application to apply the changes.", "Upgrade Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("The purchase could not be completed. Please try again later.", "Purchase Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                return;
            }

            if (!premiumService.IsPremium)
            {
                usageTracker.IncrementChatMessageCount();
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

        private void SaveChatHistory()
        {
            string json = JsonConvert.SerializeObject(chatHistory, Formatting.Indented);
            File.WriteAllText(ChatHistoryFile, json);
        }

        private void LoadChatHistory()
        {
            if (File.Exists(ChatHistoryFile))
            {
                string json = File.ReadAllText(ChatHistoryFile);
                chatHistory = JsonConvert.DeserializeObject<List<ChatMessage>>(json);

                foreach (var message in chatHistory)
                {
                    if (message.Sender == "User")
                    {
                        InputDisplayTemplate inputDisplay = new InputDisplayTemplate();
                        inputDisplay.DisplayUserInput.Text = message.Content;
                        this.dock.Children.Add(inputDisplay);
                    }
                    else if (message.Sender == "AI")
                    {
                        OutputDisplayTemplate outputDisplay = new OutputDisplayTemplate();
                        outputDisplay.DisplayResponse.Text = message.Content;
                        this.dock.Children.Add(outputDisplay);
                    }
                }
            }
        }
    }
}
