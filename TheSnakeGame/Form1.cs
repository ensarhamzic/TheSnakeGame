﻿
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TheSnakeGame.Properties;

namespace TheSnakeGame
{
    public partial class Form1 : Form
    {
        private Snake snake;
        private bool madeMove = false;
        private int width = 37;
        private int height = 27;
        private Point food;
        private int score = 0;
        private int highScore = 0;
        private bool isGameRunning = false;
        private int time = 0;

        public Form1()
        {
            InitializeComponent();
            snake = new Snake();
        }

        private void PaintGame(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            DrawBackground(g);

            Rectangle bodyPart = new Rectangle(0, 0, snake.size, snake.size);

            foreach (Point p in snake.body)
            {
                g.TranslateTransform(p.x * snake.size, p.y * snake.size);
                g.FillRectangle(Brushes.Green, bodyPart);
                g.ResetTransform();
            }
               

            if (!isGameRunning) return;

            if (food == null)
            {
                Random random = new Random();
                int randomNumber;
                int randomNumber2;
                while (true)
                {
                    randomNumber = random.Next(0, width-1);
                    randomNumber2 = random.Next(0, height-1);

                    bool isOnSnake = false;

                    foreach (Point p in snake.body)
                    {
                        if (p.x == randomNumber && p.y == randomNumber2)
                        {
                            isOnSnake = true;
                            break;
                        }
                    }

                    if (!isOnSnake)
                        break;
                }
                food = new Point(randomNumber, randomNumber2);
            }

            DrawCircle(g, snake.size / 2, food.x * snake.size + snake.size / 2, food.y * snake.size + snake.size / 2);
        }

        private void DrawBackground(Graphics g)
        {
            int radius = 100;
            PointF center = new PointF(150, 150);

            PointF[] starPoints = new PointF[10];
            double angle = Math.PI / 2;
            double angleStep = Math.PI / 5;

            for (int i = 0; i < 10; i++)
            {
                double r = radius * (i % 2 + 1) / 2;
                starPoints[i] = new PointF((int)(center.X + r * Math.Cos(angle)), (int)(center.Y - r * Math.Sin(angle)));
                angle += angleStep;
            }
            g.FillPolygon(Brushes.LightGray, starPoints);

            Matrix matrix = new Matrix();
            matrix.Scale(0.5f, 0.5f);
            matrix.Translate(700, 300);
            matrix.Rotate(45);
            g.Transform = matrix;
            g.FillPolygon(Brushes.LightGray, starPoints);

            g.ResetTransform();
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            Point snakeHead = snake.body[0];

            if(food != null)
            {
                if (snakeHead.x == food.x && snakeHead.y == food.y)
                {
                    snake.body.Add(new Point(snake.body[snake.body.Count - 1].x, snake.body[snake.body.Count - 1].y));
                    food = null;
                    score++;
                    scoreText.Text = score.ToString();
                }
            }
           

            int newX = 0;
            int newY = 0;
            switch (snake.direction)
            {
                case Direction.Up:
                    newX = snakeHead.x;
                    if (snakeHead.y - 1 < 0) newY = height - 1;
                    else newY = snakeHead.y - 1;
                    break;
                case Direction.Down:
                    newX = snakeHead.x;
                    if (snakeHead.y + 1 >= height) newY = 0;
                    else newY = snakeHead.y + 1;
                    break;
                case Direction.Left:
                    newY = snakeHead.y;
                    if (snakeHead.x - 1 < 0) newX = width - 1;
                    else newX = snakeHead.x - 1;
                    break;
                case Direction.Right:
                    newY = snakeHead.y;
                    if (snakeHead.x + 1 >= width) newX = 0;
                    else newX = snakeHead.x + 1;
                    break;
            }

            foreach (Point p in snake.body)
            {
                if (p.x == newX && p.y == newY)
                {
                    timer1.Stop();
                    gameTimer.Stop();

                    if (score > highScore)
                    {
                        highScore = score;
                        highScoreText.Text = highScore.ToString();
                    }

                    score = 0;
                    time = 0;
                    madeMove = false;
                    food = null;
                    snake = new Snake();
                    scoreText.Text = score.ToString();
                    timerText.Text = time.ToString() + "s";
                    isGameRunning = false;
                    GetPictureBox().Invalidate();
                    return;
                }
            }

            snake.body.Insert(0, new Point(newX, newY));
            snake.body.RemoveAt(snake.body.Count - 1);
            madeMove = true;

          

            GetPictureBox().Invalidate();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!madeMove)
                return base.ProcessCmdKey(ref msg, keyData);
            madeMove = false;
            if (keyData == Keys.Left && snake.direction != Direction.Right)
            {
                snake.direction = Direction.Left;
            }
            if (keyData == Keys.Right && snake.direction != Direction.Left)
            {
                snake.direction = Direction.Right;
            }
            if (keyData == Keys.Up && snake.direction != Direction.Down)
            {
                snake.direction = Direction.Up;
            }
            if (keyData == Keys.Down && snake.direction != Direction.Up)
            {
                snake.direction = Direction.Down;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (isGameRunning) return;
            timer1.Start();
            gameTimer.Start();
            isGameRunning = true;
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            time++;
            timerText.Text = time.ToString() + "s";
        }



        private void DrawCircle(Graphics g, int radius, int h, int k)
        {
            int x = radius;
            int y = 0;
            int decisionOver2 = 1 - x;

            while (y <= x)
            {
                DrawPixel(g, x + h, y + k);
                DrawPixel(g, y + h, x + k);
                DrawPixel(g, -x + h, y + k);
                DrawPixel(g, -y + h, x + k);
                DrawPixel(g, -x + h, -y + k);
                DrawPixel(g, -y + h, -x + k);
                DrawPixel(g, x + h, -y + k);
                DrawPixel(g, y + h, -x + k);
                y++;
                if (decisionOver2 <= 0)
                    decisionOver2 += 2 * y + 1;
                else { x--; decisionOver2 += 2 * (y - x) + 1; }
            }
        }


        private void DrawPixel(Graphics g, int x, int y)
        {
             g.FillRectangle(Brushes.Black, x, y, 1, 1);
        }

    }
}
