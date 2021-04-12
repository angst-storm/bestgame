using System;
using System.Collections.Generic;
using System.Drawing;

namespace TimeCollapse.Models
{
    public class Explorer : MovingObject
    {
        private readonly Queue<(Action, int)> _actionsSequence = new();
        private readonly int _jumpHeight;
        private readonly int _leftRunSpeed;
        private readonly int _rightRunSpeed;
        private bool _fromPast;
        private (Action, int) _nextAction;
        public bool Jump = false;
        public bool LeftRun = false;
        public bool RightRun = false;

        public Explorer(Point startLocation, Size colliderSize, int runSpeed, int jumpHeight) : base(startLocation,
            colliderSize)
        {
            _jumpHeight = -jumpHeight;
            _leftRunSpeed = -runSpeed;
            _rightRunSpeed = runSpeed;
        }

        public void Move(int tick)
        {
            if (!_fromPast)
            {
                var jump = Jump && OnFloor ? _jumpHeight : 0;
                var run = (RightRun ? _rightRunSpeed : 0) + (LeftRun ? _leftRunSpeed : 0);
                _actionsSequence.Enqueue((() => UpdateLocation(run, jump), tick));
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