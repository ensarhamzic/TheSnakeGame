
using System;
using System.Drawing;
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

        public Form1()
        {
            InitializeComponent();
            snake = new Snake();
        }

        private void PaintGame(object sender, PaintEventArgs e)
        {
            foreach (Point p in snake.body)   
                e.Graphics.FillRectangle(Brushes.Green, p.x * snake.size, p.y * snake.size, snake.size, snake.size);
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            Point point = null;
            Point snakeHead = snake.body[0];
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

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}
