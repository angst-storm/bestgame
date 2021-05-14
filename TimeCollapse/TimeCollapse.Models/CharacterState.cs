using System.Drawing;

namespace TimeCollapse.Models
{
    public class CharacterState
    {
        public CharacterState(Rectangle collider, Vector speed, bool onFloor, bool turnedRight)
        {
            Collider = collider;
            Speed = speed;
            OnFloor = onFloor;
            TurnedRight = turnedRight;
        }

        public Rectangle Collider { get; }
        public Vector Speed { get; }
        public bool OnFloor { get; }
        public bool TurnedRight { get; }
    }
}