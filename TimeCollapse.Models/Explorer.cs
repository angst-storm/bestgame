using System.Collections.Generic;
using System.Drawing;

namespace TimeCollapse.Models
{
    public class Explorer : Character
    {
        private const int RunSpeed = 5;
        private const int JumpSpeed = 15;
        private readonly Point spawn;
        private readonly List<(int, CharacterState)> states = new();
        private int repeatIndexer;
        private bool repeatMode;

        public Explorer(Game game, Stage stage) : base(game, new Rectangle(stage.Spawn, DefaultColliderSize))
        {
            spawn = stage.Spawn;
            Target = stage.Target;
        }

        public static Size DefaultColliderSize => new(32, 64);
        public Rectangle Target { get; }
        public bool Jump { get; set; }
        public bool RightRun { get; set; }
        public bool LeftRun { get; set; }

        public override void Move(int tick)
        {
            if (!repeatMode)
            {
                var move = (Jump && OnFloor ? new Vector(0, -JumpSpeed) : Vector.Zero) +
                           (RightRun ? new Vector(RunSpeed, 0) : Vector.Zero) +
                           (LeftRun ? new Vector(-RunSpeed, 0) : Vector.Zero);
                states.Add((tick, Translate(move)));
            }
            else if (repeatIndexer < states.Count && states[repeatIndexer].Item1 == tick)
            {
                AcceptTheState(states[repeatIndexer].Item2);
                repeatIndexer++;
            }
        }

        public void Repeat()
        {
            ChangePosition(spawn);
            repeatMode = true;
            repeatIndexer = 0;
        }
    }
}