using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TimeCollapse.Models
{
    public class Game
    {
        public Map ActualMap { get; private set; }
        public List<Map> Maps { get; }
        public Explorer PresentExplorer { get; private set; }

        public Game()
        {
            Maps = new List<Map> {Map.TestMap};
            ActualMap = Maps[0];
            PresentExplorer = new Explorer(this, ActualMap.PlayerStartPosition, new Size(10, 20), true);
        }

        public Game(List<Map> maps, Size explorerColliderSize, bool explorerGravityEffect)
        {
            if (!maps.Any()) throw new InvalidOperationException();
            Maps = maps;
            ActualMap = Maps[0];
            PresentExplorer = new Explorer(this, ActualMap.PlayerStartPosition, explorerColliderSize, explorerGravityEffect);
        }

        public void SwitchMap(int mapIndex)
        {
            if (mapIndex >= Maps.Count) throw new InvalidOperationException();
            ActualMap = Maps[mapIndex];
            PresentExplorer = new Explorer(this, ActualMap.PlayerStartPosition, PresentExplorer.Collider.Size, true);
        }

        public void Update(int tick)
        {
            PresentExplorer.Move(tick);
        }
    }
}