using System.Drawing;

namespace TimeCollapse.Models
{
    public readonly struct Stage
    {
        public Stage(Point spawn, Rectangle target)
        {
            Spawn = spawn;
            Target = target;
        }

        public Point Spawn { get; }
        public Rectangle Target { get; }
    }
}