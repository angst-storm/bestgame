using System.Drawing;

namespace TimeCollapse.Models
{
    public class MovingObject
    {
        private readonly Size _colliderSize;
        public Point Location;

        public MovingObject(Point startLocation, Size colliderSize)
        {
            _colliderSize = colliderSize;
            Location = startLocation;
        }

        public Rectangle Collider => new(Location, _colliderSize);

        public void UpdateLocation()
        {
        }
    }
}