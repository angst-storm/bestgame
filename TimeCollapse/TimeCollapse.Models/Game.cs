using System.Collections.Generic;
using System.Linq;

namespace TimeCollapse.Models
{
    public class Game
    {
        private readonly List<Explorer> explorersFromPast = new();
        private readonly List<Explorer> removedExplorers = new();
        private int tickDiff;

        public Game(Map map)
        {
            Map = map;
            PresentExplorer = new Explorer(this, Map.ActualStage);
        }

        public Map Map { get; }
        public Explorer PresentExplorer { get; private set; }

        public bool GameOver { get; private set; }

        public IEnumerable<Explorer> AllExplorers =>
            explorersFromPast.Except(removedExplorers).Concat(new[] {PresentExplorer});

        private void PortalControl(int tick)
        {
            if (PresentExplorer.Collider.IntersectsWith(PresentExplorer.Target))
            {
                if (Map.TrySwitchStage())
                {
                    explorersFromPast.Add(PresentExplorer);
                    removedExplorers.Clear();
                    foreach (var explorer in explorersFromPast)
                        explorer.Repeat();
                    PresentExplorer = new Explorer(this, Map.ActualStage);
                }
                else
                {
                    GameOver = true;
                }
            }

            foreach (var explorer in explorersFromPast.Where(e => e.Collider.IntersectsWith(e.Target)))
                removedExplorers.Add(explorer);

            tickDiff = tick + 1;
        }

        public void Update(int tick)
        {
            foreach (var explorer in AllExplorers)
                explorer.Move(tick - tickDiff);
            PortalControl(tick);
            if (Map.TimeAnomalies.Any(a => a.IntersectsWith(PresentExplorer.Collider)))
            {
                removedExplorers.Clear();
                foreach (var explorer in explorersFromPast)
                    explorer.Repeat();
                PresentExplorer = new Explorer(this, Map.ActualStage);
            }
        }
    }
}