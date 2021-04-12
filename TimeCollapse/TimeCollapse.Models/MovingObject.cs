using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TimeCollapse.Models
{
    public class MovingObject
    {
        private static int _gravity = 10;
        private readonly Size _colliderSize;
        private int _verticalSpeed;
        public Point Location;

        public MovingObject(Point startLocation, Size colliderSize)
        {
            _colliderSize = colliderSize;
            Location = startLocation;
        }

        public bool OnFloor
        {
            get
            {
                var collider = Collider;
                return Game.ActualMap.Blocks.Any(r => TouchesLine(r, new Point(collider.Left, collider.Bottom),
                    new Point(Collider.Right, collider.Bottom)));
            }
        }

        private bool UnderRoof
        {
            get
            {
                var collider = Collider;
                return Game.ActualMap.Blocks.Any(r => TouchesLine(r, new Point(collider.Left, collider.Top),
                    new Point(Collider.Right, collider.Top)));
            }
        }

        public Rectangle Collider => new(Location, _colliderSize);

        private bool BesideWall(int x) //x - координата бока коллайдера, который нам нужен
        {
            var collider = Collider;
            return Game.ActualMap.Blocks.Any(r =>
                TouchesLine(r, new Point(x, collider.Top), new Point(x, collider.Bottom)));
        }

        public void UpdateLocation(int xMove, int yMove)
        {
            //игрок двигается на run или насколько возможно в сторону run
            //если jump не равен нулю, то скорость падения игрока равна jump, иначе, если он не на полу, скорость падения игрока равна скорость падения + гравитация
            //игрок двигается на скорость падения или насколько возможно в сторону скорости падения
        }

        private bool TouchesLine(Rectangle rect, Point dot1, Point dot2)
        {
            return AllLinesPoints(dot1, dot2).Any(rect.Contains);
        }

        private IEnumerable<Point> AllLinesPoints(Point startPoint, Point endPoint)
        {
            var n = new Point(startPoint.Y - endPoint.Y, startPoint.X - endPoint.X);
            for (var p = startPoint; p != endPoint; p = new Point(p.X + n.X, p.Y + n.Y))
                yield return p;
        }
    }
}