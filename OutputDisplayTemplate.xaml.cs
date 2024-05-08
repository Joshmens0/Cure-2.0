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
    /// Interaction logic for OutputDisplayTemplate.xaml
    /// </summary>
    public partial class OutputDisplayTemplate : UserControl
    {
        public OutputDisplayTemplate()
        {
            InitializeComponent();
        }

        private void Copy_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed) 
            {
                Clipboard.SetText(this.DisplayResponse.Text);
                this.Copy.Source = new BitmapImage(new Uri(@"/Images/Copied.png",UriKind.Relative));
            }
        }
    }
}
