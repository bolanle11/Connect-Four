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

namespace Connect4
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

        //open setting window when new game is clicked
        private void button_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow SWindow = new SettingsWindow();
            SWindow.Show();
            this.Close();
        }

        //open Help window when hep button is click
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow HWindow = new HelpWindow();
            this.Hide();

            //reopen this window when the help window is closed
            HWindow.Closed += HelpWindowClosed;
            HWindow.Show();

        }

        private void HelpWindowClosed(object sender, EventArgs e)
        {
            //this function will run when help window is closed
            ((Window)sender).Closed -= HelpWindowClosed;
            this.Show();
        }
    }
}
