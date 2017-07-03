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
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public string player1,player2; //the player names
        bool computer_playing = false;

        public SettingsWindow()
        {
            InitializeComponent();
        }
        

        private void radioButtonHH_Checked(object sender, RoutedEventArgs e)
        {
            //enable the Yellow player textbox 
            player2name.IsEnabled = true;
            player2name.Text = "Player Two";
           computer_playing = false;

        }

        //
        private void radioButtonHC_Checked(object sender, RoutedEventArgs e)
        {
            //disable the Yellow player textbox 
            player2name.IsEnabled = false;
            player2name.Text = "Computer";
           computer_playing = true;

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //verify user input and set default values as the player names if the use didi not entr anything
            if(player1name.Text != String.Empty)
            {
                player1 = player1name.Text;
            }else
            {
                player1 = "Player One";
            }

            if (player2name.Text != String.Empty)
            {
                player2 = player2name.Text;
            }else
            {
                player2 = "Player Two";
            }

            //open game window so the user can play the game
            GameWindow Gwindow = new GameWindow(player1, player2,computer_playing);
            this.Close();
            Gwindow.Show();
        }
    }
}
