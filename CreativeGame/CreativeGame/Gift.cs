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

        private Game1 _game;
        private bool _collided = false;

        public bool Catched => _collided;
        public bool IsDead() => Catched;

        public Gift(Game game, World world) : base("gift", new Vector2(2f,0.2f), Enumerable.Range(0, 6).Select(n => game.Content.Load<Texture2D>($"assets/orig/images/gift.9")).ToArray())
        {
            _fps = 20;

            _game = (Game1)game;

            Body = BodyFactory.CreateCircle(world, .25f, 1f, _position, BodyType.Static, this);
            Body.IsSensor = true;

            Body.OnCollision = (thisGift, collider, contact) =>
            {
                if (!_collided && collider.GameObject().Name == "player")
                {
                    System.Diagnostics.Debug.WriteLine("Toquei na gift.");
                    _collided = true;
                }
            };

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
        }
    }
}
