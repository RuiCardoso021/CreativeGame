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
    public class SnowHouse : AnimatedSprite, ITempObject
    {
        private bool rotating = false;
        public Vector2 ImpactPos;
        private bool _collided = false;
        World _world = new World(new Vector2(0, -9.82f));
        private Vector2 _directon;

        public bool Collided => _collided;
        public bool IsDead() => _currentTexture == 0 && rotating;

        public SnowHouse(Game game/*, Vector2 position*/) : base("snowhouse", /*position*/new Vector2(44f, 0.4f), Enumerable.Range(0, 6).Select(n => game.Content.Load<Texture2D>($"assets/orig/images/Igloo")).ToArray())
        {
            _fps = 20;

            Body = BodyFactory.CreateCircle(_world, _size.Y / 2f, 1f, _position + (_size.X / 2f - _size.Y / 2f) * _directon, BodyType.Dynamic, this);
            Body.OnCollision = (a, b, contact) =>
            {
                string[] ignore = { "bullet", "explosion" };
                if (!ignore.Contains(b.GameObject().Name))
                {
                    _collided = true;
                    ImpactPos = _position + (b.GameObject().Position - _position) / 2f;

                }
            };
        }

        public override void Update(GameTime gameTime)
        {
            if (_currentTexture > 0) rotating = true;
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
        }
    }
}
