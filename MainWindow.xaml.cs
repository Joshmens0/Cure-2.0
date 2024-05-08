using Cure_WPF;
using Microsoft.VisualBasic;
using System.IO;
using System.Media;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
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
        public MainWindow()
        {
            InitializeComponent();
            
        }
       
        async Task OpenAiLogic()
        {
            Openai openai = new Openai() 
            {
             SystemMessage= "you are a highly renowned medical doctor who gives diagnosis and treament to all diseases",
             UserRequest=UserRequest.input.Text
            };
            UserRequest.input.Text = null;
            loading.Visibility = Visibility.Visible;
            OutputDisplayTemplate response = new OutputDisplayTemplate();
            response.DisplayResponse.Text = await openai.MakeRequest();
            this.dock.Children.Add(response);
            loading.Visibility= Visibility.Collapsed;
            var soundloction = new Uri(@"Sounds/MessageRecieved.wav", UriKind.Relative);
            StreamResourceInfo info = Application.GetResourceStream(soundloction);
            var sound = new SoundPlayer(info.Stream);
            sound.Play();
        }
        public async void UserRequest_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter) 
            {   
                
                InputDisplayTemplate inputDisplay = new InputDisplayTemplate();
                inputDisplay.DisplayUserInput.Text = UserRequest.input.Text;
                var soundlocation = new Uri(@"Sounds/MessageSent.wav",UriKind.Relative);
                StreamResourceInfo streamResourceInfo=Application.GetResourceStream(soundlocation);
                var sound=new SoundPlayer(streamResourceInfo.Stream);
                sound.Play();
                this.dock.Children.Add(inputDisplay);
                await OpenAiLogic();
            }
        }
    }
}