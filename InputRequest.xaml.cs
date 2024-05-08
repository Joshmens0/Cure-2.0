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
        public InputRequest()
        {
            InitializeComponent();
        }

        private void input_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed && this.input.Text!=null) 
            {
                this.input.Text = null;
                this.input.Focusable = true;
                this.input.Focus(); 
                this.input.FontStyle=  FontStyles.Normal;
                this.input.FontWeight = FontWeights.Bold;
            }
            
        }

    }
}
