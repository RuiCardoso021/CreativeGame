using System;
using System.Collections.Generic;
using System.Linq;
using Genbox.VelcroPhysics.Collision.RayCast;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;
using CreativeGame.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CreativeGame
{
    public class Gift : AnimatedSprite, ITempObject
    {
        private HashSet<Fixture> _collisions;
        private Game1 _game;
        private bool rotating = false;
        
        public bool IsDead() => _currentTexture == 0 && rotating;

        public Gift(Game game/*, Vector2 position*/) : base("gift", /*position*/new Vector2(2f,2f), Enumerable.Range(0, 6).Select(n => game.Content.Load<Texture2D>($"assets/orig/images/gift.9")).ToArray())
        {
            _fps = 20;
            _collisions = new HashSet<Fixture>();

            _game = (Game1)game;

            //AddRectangleBody(_game.Services.GetService<World>(), width: _size.X / 2f); // kinematic is false by default

            /*Fixture sensor = FixtureFactory.AttachRectangle(_size.X / 3f, _size.Y * 0.05f, 4, new Vector2(0, -_size.Y / 2f), Body);
            sensor.IsSensor = true;

            sensor.OnCollision = (a, b, contact) =>
            {
                _collisions.Add(b);  // FIXME FOR BULLETS
            };
            sensor.OnSeparation = (a, b, contact) =>
            {
                _collisions.Remove(b);
            };*/
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
