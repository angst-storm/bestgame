using System.Drawing;

namespace TimeCollapse.Models
{
    public class CharacterState
    {
        public CharacterState(Rectangle collider, Vector speed, bool onFloor)
        {
            Collider = collider;
            Speed = speed;
            OnFloor = onFloor;
        }

        public Rectangle Collider { get; }
        public Vector Speed { get; }
        public bool OnFloor { get; }
    }
}