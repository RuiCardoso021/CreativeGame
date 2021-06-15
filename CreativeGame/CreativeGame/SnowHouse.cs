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
    public class SnowHouse : AnimatedSprite
    {
        private Game1 _game;

        public SnowHouse(Game game, World world, Vector2 position) : base("snowhouse", position, new Texture2D[] { game.Content.Load<Texture2D>($"assets/orig/images/Igloo") })
        {
            _fps = 20;

            _game = (Game1) game;

            Body = BodyFactory.CreateCircle(world, .75f, 1f, _position, BodyType.Static, this);
            Body.IsSensor = true;
            Body.OnCollision = (thisSnowHouse, collider, contact) =>
            {
                if (collider.GameObject().Name == "player")
                {
                    if(_game.nrCoins == 3)
                    {
                        if (_game.level == 1)
                            _game.isWin = true;
                        else
                        {
                            _game._soundFinishLevel.Play();
                            _game.level++;
                            _game.lifeCount = 3;
                            _game.nrCoins = 0;
                            _game.SaveGame();
                            _game.restart();
                        } 
                    }

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
