using System.Drawing;

namespace TimeCollapse.Models
{
    public class Explorer : Character
    {
        public Explorer(Game game, Point startLocation, Size colliderSize, bool gravityEffect) : base(game, startLocation, colliderSize, gravityEffect)
        {
        }

        public bool Jump { get; set; }
        public bool RightRun { get; set; }
        public bool LeftRun { get; set; }

        public override void Move(int tick)
        {
            var move = (Jump && OnFloor? new Vector(0, -20) : Vector.Zero) +
                       (RightRun ? new Vector(4, 0) : Vector.Zero) +
                       (LeftRun ? new Vector(-4, 0) : Vector.Zero);
            Translate(move);
        }
    }
}