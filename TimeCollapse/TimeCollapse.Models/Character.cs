using System.Drawing;
using System.Linq;

namespace TimeCollapse.Models
{
    public abstract class Character
    {
        private readonly Game game;
        public Rectangle Collider { get; private set; }
        private Vector speed;


        public Character(Game game, Point startLocation, Size colliderSize)
        {
            this.game = game;
            Collider = new Rectangle(startLocation, colliderSize);
            speed = Vector.Zero;
        }

        public Point Location => Collider.Location;
        private static readonly Vector Gravity = new(0, 1);
        private static readonly double maxSpeed = 10;
        private static readonly double dt = 1;
        public void Translate(Vector force)
        {
            speed = new Vector(force.X * dt, speed.Y + (force.Y + Gravity.Y) * dt);
            if (speed.Length > maxSpeed) speed = speed.Normalize() * maxSpeed;
            var offset = new Vector(Location) + speed * dt; 
            var offsetCollider = new Rectangle(offset.ToPoint(), Collider.Size);
            foreach (var block in game.ActualMap.Blocks.Where(offsetCollider.IntersectsWith))
            {
                if (offsetCollider.Right > block.Left && Collider.Right <= block.Left)
                {
                    speed = new Vector(0, speed.Y);
                    offsetCollider.Offset(-(offsetCollider.Right - block.Left),0);
                }

                if (offsetCollider.Left < block.Right && Collider.Left >= block.Right)
                {
                    speed = new Vector(0, speed.Y);
                    offsetCollider.Offset(block.Right - offsetCollider.Left, 0);
                }
                
                if (offsetCollider.Bottom > block.Top && Collider.Bottom <= block.Top)
                {
                    speed = new Vector(speed.X, 0);
                    offsetCollider.Offset(0, -(offsetCollider.Bottom - block.Top));
                }

                if (offsetCollider.Top < block.Bottom && Collider.Top >= block.Bottom)
                {
                    speed = new Vector(speed.X, 0);
                    offsetCollider.Offset(0, block.Bottom - offsetCollider.Top);
                }
            }

            Collider = offsetCollider;
        }


        public abstract void Move(int tick);
    }
}