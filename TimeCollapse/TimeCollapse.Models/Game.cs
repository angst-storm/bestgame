using System.Collections.Generic;
using System.Drawing;

namespace TimeCollapse.Models
{
    public class Game
    {
        public Map ActualMap { get; }
        public List<Map> Maps { get; }
        public Explorer PresentExplorer { get; }

        public Game()
        {
            Maps = new List<Map> {Map.TestMap};
            ActualMap = Maps[0];
            PresentExplorer = new Explorer(this, ActualMap.PlayerStartPosition, new Size(10, 20));
        }

        public void Update(int tick)
        {
            PresentExplorer.Move(tick);
        }
    }
}