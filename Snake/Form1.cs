using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();// W Liscie bedzie pprzetrzymywany waz
        private Circle food = new Circle(); // pojedynczym kolkiem bedzie jedzenie weza
        
        public Form1()
        {
            InitializeComponent();
            new Settings();
            gameTimer.Interval = 1000 / Settings.Speed;//,,klatki na sekund,,
            gameTimer.Tick += updateScreen;
            gameTimer.Start();
            startGame();
        }
        private void startGame()
        {
            label3.Visible = false;
            new Settings();
            Snake.Clear();
            Circle glowa = new Circle { X = 10, Y = 2 };
            Snake.Add(glowa);
            label2.Text = Settings.Score.ToString();
            generatefood();
        }
        private void generatefood()
        {
            int MaksymalnaPozycjaX = pictureBox1.Size.Width / Settings.Width;
            int MaksymalnaPozycjaY = pictureBox1.Size.Height / Settings.Height;
            Random los = new Random();
            food = new Circle { X = los.Next(0, MaksymalnaPozycjaX), Y = los.Next(0, MaksymalnaPozycjaY) };
        }
        private void updateScreen(object sender, EventArgs e)
        {
            if (Settings.GameOver == true)
            {
                if (Input.KeyPress(Keys.Enter))
                {
                    startGame();
                }
            }
            else
            {
                if (Input.KeyPress(Keys.Right) && Settings.direction != Directions.Left)
                {
                    Settings.direction = Directions.Right;
                }
                else if (Input.KeyPress(Keys.Left) && Settings.direction != Directions.Right)
                {
                    Settings.direction = Directions.Left;
                }
                else if (Input.KeyPress(Keys.Up) && Settings.direction != Directions.Down)
                {
                    Settings.direction = Directions.Up;
                }
                else if (Input.KeyPress(Keys.Down) && Settings.direction != Directions.Up)
                {
                    Settings.direction = Directions.Down;
                }
                movePlayer();
            }
            pictureBox1.Invalidate();//odswiez picturebox i wgraj grafiki
        }
        private void movePlayer()
        {
            for (int i= Snake.Count-1;i>=0;i--)
            {
                if (i == 0)
                {//jesli glowa sie porusza ... to reszta ciala za nim
                    switch (Settings.direction)
                    {
                        case Directions.Right:
                            Snake[i].X++;
                            break;
                            case Directions.Up:
                            Snake[i].Y--;
                            break;

                        case Directions.Left:
                            Snake[i].X--;
                            break;
                        case Directions.Down:
                            Snake[i].Y++;
                            break;
                    }
                    //Zakres,zeby waz nie wyszedl poza obszar
                    int MaksymalnaPozycjaX = pictureBox1.Size.Width / Settings.Width;
                    int MaksymalnaPozycjaY = pictureBox1.Size.Height / Settings.Height;
                    if (Snake[i].X < 0 || Snake[i].Y < 0 || Snake[i].X > MaksymalnaPozycjaX || Snake[i].Y > MaksymalnaPozycjaY)
                    {
                        die();//zakoncz gre,jesli waz zderzy sie ze sciana

                    }
                    if(Snake[0].X==food.X && Snake[0].Y == food.Y)//jak glowa dotknie jedzenia
                    {
                        eat();
                    }
                }
                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }
        private void die()
        {
            Settings.GameOver = true;
        }
        private void eat()
        {//dodaj ,,cialo,, wezowi
            Circle cialo = new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };
            Snake.Add(cialo);
            Settings.Score += Settings.Points;
            label2.Text = Settings.Score.ToString();
            generatefood();
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void keyisdown(object sender, KeyEventArgs e)
        {
            Input.changeState(e.KeyCode, true);
        }
        private void keyisup(object sender, KeyEventArgs e)
        {
            Input.changeState(e.KeyCode, false);
        }
        private void updateGra(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            if (Settings.GameOver==false)
            {
                Brush KolorWeza;
                for (int i =0;i<Snake.Count;i++)
                {
                    if (i==0)
                    {//Glowa weza zolta
                        KolorWeza = Brushes.Yellow;
                    }
                    else
                    {//reszta ciala czarna
                        KolorWeza = Brushes.Black;
                    }
                    graphics.FillEllipse(KolorWeza, new Rectangle(Snake[i].X * Settings.Width, Snake[i].Y * Settings.Height, Settings.Width, Settings.Height));
                    graphics.FillEllipse(Brushes.Blue, new Rectangle(food.X * Settings.Width, food.Y * Settings.Height, Settings.Width, Settings.Height));
                }
            }
            else
            {
                //Jak koniec gry to komunikat
                string gameOver = "GAME OVER\n" + "\n Enter - Nowa Gra\n";
                label3.Text = gameOver;
                label3.Visible = true;
            }
        }
    }
}
