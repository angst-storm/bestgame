﻿using System.Drawing;
using System.Linq;

namespace TimeCollapse.Models
{
    public abstract class Character
    {
        private static readonly Vector Gravity = new(0, 1);
        private static readonly double maxSpeed = 10;
        private static readonly double dt = 1;
        private readonly Game game;
        private Vector speed;
        private readonly bool gravityEffect;

        public Character(Game game, Point startLocation, Size colliderSize, bool gravityEffect)
        {
            this.game = game;
            Collider = new Rectangle(startLocation, colliderSize);
            speed = Vector.Zero;
            this.gravityEffect = gravityEffect;
        }

        public Rectangle Collider { get; private set; }

        public Point Location => Collider.Location;

        public bool OnFloor { get; private set; }

        public void Translate(Vector force)
        {
            speed = new Vector(force.X * dt, speed.Y + (force.Y + Gravity.Y * (gravityEffect ? 1 : 0)) * dt);
            if (speed.Length > maxSpeed) speed = speed.Normalize() * maxSpeed;
            var offset = new Vector(Location) + speed * dt;
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
                                                         (Collider.Left >= block.Left && Collider.Left <= block.Right ||
                                                          Collider.Right >= block.Left &&
                                                          Collider.Right <= block.Right));
        }

        public abstract void Move(int tick);
    }
}