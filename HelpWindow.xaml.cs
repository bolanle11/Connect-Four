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
using System.Windows.Shapes;

namespace Connect4
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        private Window PreviousWindow;

        public HelpWindow(Window back)
        {
            InitializeComponent();
            this.PreviousWindow = back;
        }

        public HelpWindow()
        {
            InitializeComponent();
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            //reopen the previous window 
            this.Close();
            if(PreviousWindow != null) PreviousWindow.Show();
            

        }
    }
}
