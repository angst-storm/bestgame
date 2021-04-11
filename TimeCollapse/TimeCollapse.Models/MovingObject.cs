using System.Drawing;

namespace TimeCollapse.Models
{
    public class MovingObject
    {
        private readonly SizeF _colliderSize;
        private readonly float _dt;
        private readonly float _mass;
        private VectorF _velocity;
        public PointF Location;
        public static readonly VectorF Gravity = new(0, -9.8f);

        public MovingObject(Point startLocation, Size colliderSize, float mass, float dt)
        {
            _mass = mass;
            _velocity = VectorF.Zero;
            Location = startLocation;
            _colliderSize = colliderSize;
            _dt = dt;
        }

        public RectangleF Collider => new(Location, _colliderSize);

        public void UpdateLocation(VectorF offSetForce = null)
        {
            var newVelocity = _velocity + Gravity + offSetForce ?? VectorF.Zero * _dt / _mass;
            var newLocation = Location + newVelocity * _dt;
            // нужна проверка на столкновения
            _velocity = newVelocity;
            Location = newLocation;
        }
    }
}