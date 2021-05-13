using System;
using System.Collections.Generic;
using System.Linq;

namespace TimeCollapse.Models
{
    public class Game
    {
        public static readonly Game TestGame = new(new List<Map> {Map.TestMap});

        private readonly List<Explorer> removedExplorers = new();
        private int tickDiff = 0;

        private Game(List<Map> maps)
        {
            if (!maps.Any()) throw new InvalidOperationException();
            Maps = maps;
            ActualMap = Maps.First();
            PresentExplorer = new Explorer(this, ActualMap.ActualStage);
            ExplorersFromPast = new List<Explorer>();
        }

        public Map ActualMap { get; }
        public List<Map> Maps { get; }
        public Explorer PresentExplorer { get; private set; }
        public List<Explorer> ExplorersFromPast { get; }

        private void PortalControl(int tick)
        {
            if (PresentExplorer.Collider.IntersectsWith(PresentExplorer.Target))
            {
                if (ActualMap.TrySwitchStage())
                {
                    ExplorersFromPast.Add(PresentExplorer);
                    foreach (var explorer in ExplorersFromPast)
                        explorer.Repeat();
                    PresentExplorer = new Explorer(this, ActualMap.ActualStage);
                }
                else
                {
                    ActualMap.ResetStages();
                    ExplorersFromPast.Clear();
                    PresentExplorer = new Explorer(this, ActualMap.ActualStage);
                }
            }

            if (ExplorersFromPast.Any(e => e.Collider.IntersectsWith(e.Target)))
            {
                ActualMap.ResetStages();
                ExplorersFromPast.Clear();
                PresentExplorer = new Explorer(this, ActualMap.ActualStage);
            }

            tickDiff = tick + 1;
        }

        public void Update(int tick)
        {
            PresentExplorer.Move(tick - tickDiff);
            foreach (var explorer in ExplorersFromPast)
                explorer.Move(tick - tickDiff);
            PortalControl(tick);
        }
    }
}