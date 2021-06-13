using CreativeGame.Classes;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreativeGame
{
    public class Coin : AnimatedSprite, ITempObject
    {
        private Game1 _game;
        private bool _collided = false;
        public int nrCoins = 0;
        public bool Catched => _collided;
        public bool IsDead() => Catched;

        public Coin(Game game, World world) : base("coin", /*position*/new Vector2(8f, 1f), Enumerable.Range(0, 6).Select(n => game.Content.Load<Texture2D>($"Coin/coin{n + 1}")).ToArray())
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
                    nrCoins++;
                    if(!_game.isSoundActive) 
                        _game._catchCoin.Play();
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
