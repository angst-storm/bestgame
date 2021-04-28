using System.Collections.Generic;
using System.Drawing;

namespace TimeCollapse.Models
{
    public class Explorer : Character
    {
        private readonly Point startLocation;
        private readonly Queue<Vector> translates = new();

        private bool present = true;

        public Explorer(Game game, Point startLocation, Size colliderSize) : base(game, startLocation, colliderSize)
        {
            this.startLocation = startLocation;
        }

        public bool Jump { get; set; }
        public bool RightRun { get; set; }
        public bool LeftRun { get; set; }

        public override void Move(int tick)
        {
            Vector move;
            if (present)
                move = (Jump && OnFloor ? new Vector(0, -15) : Vector.Zero) +
                       (RightRun ? new Vector(5, 0) : Vector.Zero) +
                       (LeftRun ? new Vector(-5, 0) : Vector.Zero);
            else
                move = translates.Dequeue();

            translates.Enqueue(move);
            Translate(move);
        }

        public void ToPast()
        {
            Teleport(startLocation);
            present = false;
        }
    }
}