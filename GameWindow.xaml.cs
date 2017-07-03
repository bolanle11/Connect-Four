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
using System.Windows.Threading;
using System.Windows.Media.Effects;
namespace Connect4
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        string player1name, // the name of the first layer
            player2name;// the name of the second player
        int player1score, player2score; //the player scores
        const int circleSize = 80; //the size of the circle drawn
        const int dropRate = 20; //the rate at which the cirle should animate

        private GameBoard board; // the game board
        private DispatcherTimer animationTimer; //timer for animation
        private bool inputLock; // lock all the input
        private bool computer_playing; //true is it is humanvs computer
        private Space computer = Space.Player2; //the computer space

        private Space currentPlayer; //the current player / the player whose urn it is
        private Image currentCircle; // the circle being animnated
        private int currentColumn; // the column that was click

        private Rectangle[] InsertColumns;// the 
        Random randNumber; //Random number generator

        BitmapImage discRedBitmap = new BitmapImage(new Uri("pack://application:,,,/images/disc_red.png", UriKind.Absolute));//the location of red disc image
        BitmapImage discYellowBitmap = new BitmapImage(new Uri("pack://application:,,,/images/disc_yellow.png", UriKind.Absolute)); // the location of the yellow disc image
        BitmapImage boardBitmap = new BitmapImage(new Uri("pack://application:,,,/images/board.png", UriKind.Absolute)); // the location of the board background

        public GameWindow(string player1n,string player2n,bool computer = false)
        {
            InitializeComponent();

            //set the playernames
            player1name = player1n;
            player2name = player2n;

            //init th eplayer score
            player1score = 0;
            player2score = 0;

           
            player1.Text = player1n;
            player2.Text = player2n;

            computer_playing = computer; //ture if Humnan vs Human || false if Computer vs Human
            
            NewGame(); //init the game 
            //set the player turn
            turn.Text = GetPlayerName(currentPlayer)+"'s Turn";
        }

        //NewGame() - creates all the necessary componenets need to being a new game
        private void NewGame()
        {
            //hide the new match button
            this.NewMatch.Visibility = Visibility.Hidden;

            inputLock = true;//disable the user from inputing a circle

            //init the board
            board = new GameBoard(6, 7);

            randNumber = new Random();//seed random number generator

            //select the first player
            if (randNumber.Next(0, 1) == 0)
            {
                currentPlayer = Space.Player1;
            }
            else
            {
                currentPlayer = Space.Player2;
            }
            

            

            //begin the animnatin timer
            animationTimer = new DispatcherTimer();
            animationTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            animationTimer.Start();
            
            //clear the game canvas
            GameCanvas.Children.Clear();
            
            //draw the game back graground
            DrawBackground();
            CreateInsertColumns();
            inputLock = false;

        }//NewGame

        //CreateInsertColumns - create invinsible rectangle for the user to click
        private void CreateInsertColumns()
        {
            InsertColumns = new Rectangle[board.Connect4Board.GetLength(1)];
            for (int column = 0; column < board.Connect4Board.GetLength(1); column++)
            {                
                InsertColumns[column] = new Rectangle();
                InsertColumns[column].Height = circleSize * board.Connect4Board.GetLength(0);
                InsertColumns[column].Width = circleSize;
                InsertColumns[column].Fill = (column % 2 == 0) ? Brushes.LightGray : Brushes.Gray;
                InsertColumns[column].Opacity = 0;
                InsertColumns[column].Name = "InsertColumn_" + column;
                InsertColumns[column].MouseLeftButtonDown += OnInsertColumnMouseLeftButtonDown;
                Canvas.SetZIndex(InsertColumns[column], 5);

                Canvas.SetLeft(InsertColumns[column], circleSize * column);
                GameCanvas.Children.Add(InsertColumns[column]);

            }
        }//CreateInsertColumns

        //DrawBackground add the back ground image for the game
        private void DrawBackground()
        {
            Image Gameboard = new Image();
            Gameboard.Width = 560;
            Gameboard.Source = boardBitmap;
            Canvas.SetTop(Gameboard, 0);
            Canvas.SetLeft(Gameboard, 0);
            Canvas.SetZIndex(Gameboard, 0);
            GameCanvas.Children.Add(Gameboard);
           
        }//drawBackground

        //GetPlayerName - return's player name based on the space
        private string GetPlayerName(Space player)
        {
            if(player == Space.Player1)
            {
                return player1name;
            }
            if(player == Space.Player2)
            {
                return player2name;
            }
            return "";
        }// GetPlayerName

        //draw the circel to be animated
        //col - the column to draw the circle
        private void DrawCircle(Space side, int col)
        {
            inputLock = true;

            Image disc = new Image();
            disc.Height = circleSize;
            disc.Width = circleSize;
            disc.Source = (side == Space.Player1) ? discRedBitmap : discYellowBitmap;
            Canvas.SetTop(disc, 0);
            Canvas.SetLeft(disc, col * circleSize);
            GameCanvas.Children.Add(disc);
            currentCircle = disc;
            animationTimer.Tick += DropCircleAnimation;

        }

        //the function that runs 
        private void DropCircleAnimation(object sender, EventArgs e)
        {
            int dropLength = (circleSize) * (board.Connect4Board.GetLength(1) - 1 - board.CirlcesInColumn(currentColumn));
            if (Canvas.GetTop(currentCircle) < dropLength)
            {
                Canvas.SetTop(currentCircle, Canvas.GetTop(currentCircle) + dropRate);
            }
            else
            {
                animationTimer.Tick -= DropCircleAnimation;
                inputLock = false;
                if(currentPlayer == computer && computer_playing ) this.MakeComputerMove();

            }

        }//DropCircleAnimation

        private void AfterTurn()
        {
            //check to see if a winner has being determine
            Space winner = board.Winner();

            if (winner != Space.Empty)
            {
                if(winner == Space.Player1)
                {
                    player1score++;
                    Player1Score.Text = player1score.ToString();
                }else
                {
                    player2score++;
                    Player2Score.Text = player2score.ToString();
                }

                turn.Text = String.Format("{0}  Wins!", GetPlayerName(currentPlayer));
                RemoveClickAction();
                this.BlurGameBoard();
                NewMatch.Visibility = Visibility.Visible;
            }
            else if (board.Tied())
            {
                turn.Text = "Tied game!";
                RemoveClickAction();
                this.BlurGameBoard();
                NewMatch.Visibility = Visibility.Visible;
            }
            else
            {
                currentPlayer = (currentPlayer == Space.Player1) ? Space.Player2 : Space.Player1;
                turn.Text = String.Format("{0}'s Turn", GetPlayerName(currentPlayer));
            }
            

        }

        private void MakeComputerMove()
        {
            if (board.Winner() != Space.Empty)
            {

            }
            else if (board.Tied())
            {

            }
            else
            {
                //generate random number
                bool sure;
                int column = randNumber.Next(0, 6);
                sure = board.CheckColumn(column);

                while (!sure)
                {
                    column = randNumber.Next(0, 6);
                    sure = board.CheckColumn(column);
                }
                //make computer move
                this.MakePlayerMover(column);
            }
        }

        //MakePlayerMover - player the players turn
        //column - the column the player chose to play
        private void MakePlayerMover(int column)
        {
            //prevent the player from taking their turn if a circle being animinated
            if (inputLock == false)
            {
                //insert the player disc in the column
                bool success = board.InsertCircle(currentPlayer, column);
                if (success)
                {
                    currentColumn = column;
                    DrawCircle(currentPlayer, column);
                    AfterTurn();

                }//if
                
            }//if

        }//MakePlayerMover

        private void OnInsertColumnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle el = (Rectangle)e.OriginalSource;
            string name = el.Name;
            // check whick column was clicked
            int column = Convert.ToInt32(name.Replace("InsertColumn_",""));

            //make the necessary steps to make the player's move
            this.MakePlayerMover(column);
            
        }

        //disable the added of new circles
        private void RemoveClickAction()
        {
            for (int column = 0; column < board.Connect4Board.GetLength(1); column++)
            {
                InsertColumns[column].MouseLeftButtonDown -= OnInsertColumnMouseLeftButtonDown;
            }
        }//RemoveClickAction

        //begin a new game
        private void NewMatch_Click(object sender, RoutedEventArgs e)
        {       
            animationTimer.Tick -= DropCircleAnimation;
            this.BlurGameBoard(false);
            NewGame();
        }

        //end the game 
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }// ExitButton_Click

        //HelpButton_Click- opens the help window
        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            //hide the window and open the help window when the help buton is clicked
            this.Hide();
            HelpWindow HWindow = new HelpWindow();
            HWindow.Closed += HelpWindowClosed;
            HWindow.Show();

        }//HelpButton_Click

        //HelpWindowClosed - runs when the help window has being closed
        private void HelpWindowClosed(object sender, EventArgs e)
        {
            //reopen the window 
            ((Window)sender).Closed -= HelpWindowClosed;
            this.Show();
        }//HelpWindowClosed

        //BlurGameBoard - this blurs the backgound of the game
        private void BlurGameBoard(bool sure = true)
        {
            // Initialize a new BlurBitmapEffect that will be applied
            // to the Button.
            BlurEffect myBlurEffect = new BlurEffect();

            // Set the Radius property of the blur. This determines how 
            // blurry the effect will be. The larger the radius, the more
            // blurring. 
            myBlurEffect.Radius = 5;
            if(!sure) myBlurEffect.Radius = 0;

            // Set the KernelType property of the blur. A KernalType of "Box"
            // creates less blur than the Gaussian kernal type.
            myBlurEffect.KernelType = KernelType.Gaussian;

            // Apply the bitmap effect to the Button.
            GameCanvas.Effect = myBlurEffect;
        }//BlurGameBoard

        //open the help window when the help button is clicked
        private void button_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow SWindow = new SettingsWindow();
            SWindow.Show();
            this.Close();

        }
    }
}
