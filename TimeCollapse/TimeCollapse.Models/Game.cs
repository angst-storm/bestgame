using System;
using System.Collections.Generic;
using System.Linq;

namespace TimeCollapse.Models
{
    public class Game
    {
        public static readonly Game TestGame = new(new List<Map> {Map.TestMap});
        private readonly List<Explorer> removedExplorers = new();
        private int actualMapIndex;
        private int tickDiff;

        private Game(List<Map> maps)
        {
            if (!maps.Any()) throw new InvalidOperationException();
            Maps = maps;
            StartLevel(0);
        }

        public Map ActualMap { get; private set; }
        public List<Map> Maps { get; }
        public Explorer PresentExplorer { get; private set; }
        public List<Explorer> ExplorersFromPast { get; } = new();

        public List<Explorer> AllExplorers =>
            ExplorersFromPast.Except(removedExplorers).Concat(new[] {PresentExplorer}).ToList();

        private void StartLevel(int index)
        {
            if (index == Maps.Count)
                index = 0;
            ActualMap = Maps[index];
            actualMapIndex = index;
            PresentExplorer = new Explorer(this, ActualMap.ActualStage);
            ExplorersFromPast.Clear();
        }

        private void PortalControl(int tick)
        {
            if (PresentExplorer.Collider.IntersectsWith(PresentExplorer.Target))
            {
                if (ActualMap.TrySwitchStage())
                {
                    ExplorersFromPast.Add(PresentExplorer);
                    removedExplorers.Clear();
                    foreach (var explorer in ExplorersFromPast)
                        explorer.Repeat();
                    PresentExplorer = new Explorer(this, ActualMap.ActualStage);
                }
                else
                {
                    StartLevel(actualMapIndex + 1);
                }
            }

            foreach (var explorer in ExplorersFromPast.Where(e => e.Collider.IntersectsWith(e.Target)))
                removedExplorers.Add(explorer);

            tickDiff = tick + 1;
        }

        public void Update(int tick)
        {
            PresentExplorer.Move(tick - tickDiff);
            foreach (var explorer in ExplorersFromPast.Except(removedExplorers))
                explorer.Move(tick - tickDiff);
            PortalControl(tick);
        }
    }
}