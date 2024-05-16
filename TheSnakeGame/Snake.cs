using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TheSnakeGame
{
    public class Snake
    {
        public List<Point> body = new List<Point>();
        public Direction direction;
        public int size;

        public Snake()
        {
            body.Add(new Point(12, 10));
            body.Add(new Point(11, 10));
            body.Add(new Point(10, 10));
            direction = Direction.Right;
            size = 15;
        }
    }
}
