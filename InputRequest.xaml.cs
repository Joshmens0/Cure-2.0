using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Shapes;

namespace Cure_2._0
{
    /// <summary>
    /// Interaction logic for InputRequest.xaml
    /// </summary>
    public partial class InputRequest : UserControl
    {
        public static readonly RoutedEvent SendButtonClickEvent =
            EventManager.RegisterRoutedEvent("SendButtonClick", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(InputRequest));

        public event RoutedEventHandler SendButtonClick
        {
            add { AddHandler(SendButtonClickEvent, value); }
            remove { RemoveHandler(SendButtonClickEvent, value); }
        }

        public InputRequest()
        {
            InitializeComponent();
        }

        private void input_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && this.input.Text == "Send A Message")
            {
                this.input.Text = "";
                this.input.Focusable = true;
                this.input.Focus();
                this.input.FontStyle = FontStyles.Normal;
                this.input.FontWeight = FontWeights.Normal;
            }
        }

        private void input_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.input.Text == "Send A Message")
            {
                this.input.Text = "";
                this.input.FontStyle = FontStyles.Normal;
                this.input.FontWeight = FontWeights.Normal;
            }
        }

        private void input_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.input.Text))
            {
                this.input.Text = "Send A Message";
                this.input.FontStyle = FontStyles.Italic;
                this.input.FontWeight = FontWeights.Light;
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(SendButtonClickEvent));
        }
    }
}
