using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TimeCollapse.Models
{
    public class Game
    {
        public static readonly Game TestGame = new(new List<Map> {Map.TestMap}, new Size(16, 32));
        private Explorer DefaultExplorer() => new Explorer(this, ActualMap.ActualSpawn, PresentExplorer.Collider.Size);

        public Game(List<Map> maps, Size explorerColliderSize)
        {
            if (!maps.Any()) throw new InvalidOperationException();
            Maps = maps;
            ActualMap = Maps.First();
            PresentExplorer = new Explorer(this, ActualMap.ActualSpawn, explorerColliderSize);
            ExplorersFromPast = new List<Explorer>();
            PreviousAttemptWin = false;
        }

        public Map ActualMap { get; }
        public List<Map> Maps { get; }
        public Explorer PresentExplorer { get; private set; }
        public List<Explorer> ExplorersFromPast { get; }
        public bool PreviousAttemptWin { get; private set; }

        private void PortalControl()
        {
            if (PresentExplorer.Collider.IntersectsWith(ActualMap.ActualTarget))
            {
                if (ActualMap.TrySwitchStage())
                {
                    ExplorersFromPast.Add(PresentExplorer);
                    foreach (var explorer in ExplorersFromPast)
                        explorer.ToPast();
                    PresentExplorer = DefaultExplorer();
                }
                else
                {
                    ActualMap.ResetStages();
                    ExplorersFromPast.Clear();
                    PresentExplorer = DefaultExplorer();
                    PreviousAttemptWin = true;
                }
            }

            if (ExplorersFromPast.Any(e => e.Collider.IntersectsWith(ActualMap.ActualTarget)))
            {
                ActualMap.ResetStages();
                ExplorersFromPast.Clear();
                PresentExplorer = DefaultExplorer();
                PreviousAttemptWin = false;
            }
        }

        public void Update(int tick)
        {
            PortalControl();
            PresentExplorer.Move(tick);
            foreach (var explorer in ExplorersFromPast)
                explorer.Move(tick);
        }
    }
}