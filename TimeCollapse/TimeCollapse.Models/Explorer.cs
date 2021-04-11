using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace TimeCollapse.Models
{
    public class Explorer : MovingObject
    {
        private readonly Queue<(Action, int)> _actionsSequence = new();
        private readonly Vector2 _jumpVector;
        private readonly Vector2 _leftRunVector;
        private readonly Vector2 _rightRunVector;
        private bool _fromPast;
        private (Action, int) _nextAction;
        public bool Jump = false;
        public bool LeftRun = false;
        public bool RightRun = false;

        public Explorer(Point startLocation, Size colliderSize, int runSpeed, int jumpHeight) : base(startLocation,
            colliderSize)
        {
            _jumpVector = new Vector2(0, -jumpHeight);
            _leftRunVector = new Vector2(-runSpeed, 0);
            _rightRunVector = new Vector2(runSpeed, 0);
        }

        public void Move(int tick)
        {
            if (!_fromPast)
            {
                var jump = Jump && OnFloor ? _jumpVector : Vector2.Zero;
                var run = (RightRun ? _rightRunVector : Vector2.Zero) + (LeftRun ? _leftRunVector : Vector2.Zero);
                _actionsSequence.Enqueue((() => UpdateLocation(jump, run), tick));
                UpdateLocation(run, jump);
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