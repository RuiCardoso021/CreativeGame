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
        public int nrGifts = 0;

        public bool Catched => _collided;
        public bool IsDead() => Catched;

        public Gift(Game game, World world, Vector2 position) : base("gift", position, Enumerable.Range(0, 1).Select(n => game.Content.Load<Texture2D>($"assets/orig/images/gift.9")).ToArray())
        {
            _fps = 20;

            _game = (Game1) game;

            Body = BodyFactory.CreateCircle(world, .25f, 1f, _position, BodyType.Static, this);
            Body.IsSensor = true;

            Body.OnCollision = (thisGift, collider, contact) =>
            {
                if (!_collided && collider.GameObject().Name == "player")
                {
                    _collided = true;
                    world.RemoveBody(Body);
                    nrGifts++;
                    if (!_game.isSoundActive)
                        _game._catchGift.Play();
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
