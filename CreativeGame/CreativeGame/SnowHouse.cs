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
        private HashSet<Fixture> _collisions;
        private Game1 _game;

        public bool Collided => _collided;
        public bool IsDead() => _currentTexture == 0 && rotating;

        public SnowHouse(Game game/*, Vector2 position*/) : base("snowhouse", /*position*/new Vector2(2f, 2f), Enumerable.Range(0, 6).Select(n => game.Content.Load<Texture2D>($"assets/orig/images/Igloo")).ToArray())
        {
            _fps = 20;

            /*Body = BodyFactory.CreateCircle(_world, _size.Y / 2f, 1f, _position + (_size.X / 2f - _size.Y / 2f) * _directon, BodyType.Dynamic, this);
            Body.OnCollision = (a, b, contact) =>
            {
                string[] ignore = { "bullet", "explosion" };
                if (!ignore.Contains(b.GameObject().Name))
                {
                    _collided = true;
                    ImpactPos = _position + (b.GameObject().Position - _position) / 2f;

                }
            };*/

            _collisions = new HashSet<Fixture>();

            _game = (Game1)game;

            AddRectangleBody(_game.Services.GetService<World>(), width: _size.X*2); // kinematic is false by default

            Fixture sensor = FixtureFactory.AttachRectangle(_size.X, _size.Y, 4, new Vector2(0, -_size.Y*2), Body);
            sensor.IsSensor = true;

            sensor.OnCollision = (a, b, contact) =>
            {
                _collisions.Add(b);  // FIXME FOR BULLETS
                _game.IsVictory();
            };
            sensor.OnSeparation = (a, b, contact) =>
            {
                _collisions.Remove(b);
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
