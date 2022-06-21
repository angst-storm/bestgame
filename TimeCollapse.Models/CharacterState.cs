using System.Drawing;

namespace TimeCollapse.Models
{
    public class CharacterState
    {
        public CharacterState(Rectangle collider, Vector speed, bool onFloor, bool turnedRight, bool go)
        {
            Collider = collider;
            Speed = speed;
            OnFloor = onFloor;
            TurnedRight = turnedRight;
            Go = go;
        }

        public Rectangle Collider { get; }
        public Vector Speed { get; }
        public bool OnFloor { get; }
        public bool TurnedRight { get; }
        public bool Go { get; }
    }
}