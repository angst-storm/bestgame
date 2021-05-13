﻿using System.Drawing;
using System.Linq;

namespace TimeCollapse.Models
{
    public abstract class Character
    {
        private const double MaxSpeed = 16;
        private readonly Game game;
        private Vector speed;

        protected Character(Game game, Rectangle collider)
        {
            this.game = game;
            Collider = collider;
            speed = Vector.Zero;
        }

        public Rectangle Collider { get; private set; }

        public Point Location => Collider.Location;

        public bool OnFloor { get; private set; }

        protected CharacterState Translate(Vector force)
        {
            speed = new Vector(force.X, speed.Y + (force.Y + 1));
            if (speed.Length > MaxSpeed) speed = speed.Normalize() * MaxSpeed;
            var offset = new Vector(Location) + speed;
            var offsetCollider = new Rectangle(offset.ToPoint(), Collider.Size);
            foreach (var block in game.ActualMap.Blocks.Where(offsetCollider.IntersectsWith))
            {
                if (offsetCollider.Right > block.Left && Collider.Right <= block.Left)
                {
                    speed = new Vector(0, speed.Y);
                    offsetCollider.Offset(-(offsetCollider.Right - block.Left), 0);
                }

                if (offsetCollider.Left < block.Right && Collider.Left >= block.Right)
                {
                    speed = new Vector(0, speed.Y);
                    offsetCollider.Offset(block.Right - offsetCollider.Left, 0);
                }

                if (offsetCollider.Bottom > block.Top && Collider.Bottom <= block.Top)
                {
                    speed = new Vector(speed.X, 0);
                    offsetCollider.Offset(0, -(offsetCollider.Bottom - block.Top));
                }

                if (offsetCollider.Top < block.Bottom && Collider.Top >= block.Bottom)
                {
                    speed = new Vector(speed.X, 0);
                    offsetCollider.Offset(0, block.Bottom - offsetCollider.Top);
                }
            }

            Collider = offsetCollider;

            OnFloor = game.ActualMap.Blocks.Any(block => Collider.Bottom == block.Top &&
                                                         (Collider.Left >= block.Left &&
                                                          Collider.Left <= block.Right ||
                                                          Collider.Right >= block.Left &&
                                                          Collider.Right <= block.Right));
            return new CharacterState(Collider, speed, OnFloor);
        }

        protected CharacterState ChangePosition(Point position)
        {
            Collider = new Rectangle(position, Collider.Size);
            speed = new Vector(0, 0);
            OnFloor = game.ActualMap.Blocks.Any(block => Collider.Bottom == block.Top &&
                                                         (Collider.Left >= block.Left &&
                                                          Collider.Left <= block.Right ||
                                                          Collider.Right >= block.Left &&
                                                          Collider.Right <= block.Right));
            return new CharacterState(Collider, speed, OnFloor);
        }

        protected void AcceptTheState(CharacterState state)
        {
            Collider = state.Collider;
            speed = state.Speed;
            OnFloor = state.OnFloor;
        }

        public abstract void Move(int tick);
    }
}