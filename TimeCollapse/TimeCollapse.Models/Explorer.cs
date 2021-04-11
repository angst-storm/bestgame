﻿using System;
using System.Collections.Generic;
using System.Drawing;

namespace TimeCollapse.Models
{
    public class Explorer: MovingObject
    {
        private bool _fromPast;
        private readonly Queue<(Action, int)> _actionsSequence = new ();
        private (Action, int) _nextAction;

        public Explorer(Point startLocation, Size colliderSize, float mass, float dt) : base(startLocation, colliderSize, mass, dt)
        {
        }

        public void Move(int tick)
        {
            if (!_fromPast)
            {
                _actionsSequence.Enqueue((() => UpdateLocation(VectorF.Zero), tick));
            }
            else
            {
                if (tick == _nextAction.Item2)
                {
                    _nextAction.Item1();
                    _nextAction = _actionsSequence.Dequeue();
                }
            }
        }

        public void ToCopy()
        {
            _fromPast = true;
            _nextAction = _actionsSequence.Dequeue();
        }
    }
}