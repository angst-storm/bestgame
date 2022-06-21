using System.Drawing;

namespace TimeCollapse.View
{
    public class ConstructorStage
    {
        public ConstructorStage(int number)
        {
            Number = number;
        }

        public int Number { get; }
        public Rectangle Spawn { get; set; }
        public Rectangle Target { get; set; }
    }
}