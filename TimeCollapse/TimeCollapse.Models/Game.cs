using System.Collections.Generic;
using System.Data;

namespace TimeCollapse.Models
{
    public static class Game
    {
        public static Map ActualMap;
        public static readonly List<Map> Maps;
        public static Explorer PresentExplorer;
        public static List<Explorer> Explorers;

        public static void Update(int tick)
        {
            foreach (var explorer in Explorers)
            {
                explorer.Move(tick);
            }
        }
    }
}