using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;
using Genbox.VelcroPhysics.Shared.Optimization;
using CreativeGame.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CreativeGame
{
    public class Bullet : AnimatedSprite, ITempObject
    {
        private Vector2 _directon;
        private float _speed;
        private float _maxDistance = 10f; // tempo de vida
        private Vector2 _origin; // starting Point, to compute current distance
        private Vector2 _anchor;
        private bool _collided = false;
        public bool Collided => _collided;
        public bool IsDead() => Collided || (_origin - _position).LengthSquared() > _maxDistance * _maxDistance;

        public Vector2 ImpactPos;

        public Bullet(Game game, World world, Vector2 startingPos, Vector2 direction) : base("bullet", startingPos, Enumerable.Range(0, 8).Select(n => game.Content.Load<Texture2D>($"SnowBall/bola{n}")).ToArray())
        {
            _origin = startingPos;
            _anchor = _texture.Bounds.Size.ToVector2() / 2f;
            // Speed
            _speed = direction.Length();
            // Normalized direction
            _directon = direction;
            _directon.Normalize();
            // Rotation
            _rotation = MathF.Atan2(-_directon.Y, _directon.X);
            _size = _texture.Bounds.Size.ToVector2() / 128f; // FIXME!!!!
            Body = BodyFactory.CreateCircle(world, _size.Y/4.5f, 1f, _position, BodyType.Dynamic, this);
            Body.LinearVelocity = _directon * _speed;
            Body.IsBullet = true;
            Body.IgnoreGravity = true;
            Body.IsSensor = true;

            Body.OnCollision = (a, b, contact) =>
            {
                string[] ignore = { "player", "bullet", "explosion", "coin" };
                if (!ignore.Contains(b.GameObject().Name))
                {
                    _collided = true;
                    ImpactPos = _position + (b.GameObject().Position - _position) / 2f;
                }
            };
        }

        public override void Update(GameTime gameTime)
        {
            _position = Body.Position + (_size.Y / 2f - _size.X / 2f) * _directon;
            base.Update(gameTime);
        }

    }
}