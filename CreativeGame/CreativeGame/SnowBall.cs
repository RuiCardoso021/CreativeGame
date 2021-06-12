using CreativeGame.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreativeGame
{
    public class SnowBall : AnimatedSprite, ITempObject
    {
        private bool rotating = false;

        public bool IsDead() => _currentTexture == 0 && rotating;

        public SnowBall(Game game/*, Vector2 position*/) : base("gift", /*position*/new Vector2(2f, 2f), Enumerable.Range(0, 9).Select(n => game.Content.Load<Texture2D>($"SnowBall/bola{n}")).ToArray())
        {
            _fps = 20;
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
