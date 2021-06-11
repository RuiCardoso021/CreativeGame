using CreativeGame.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreativeGame
{
    public class Gift : AnimatedSprite, ITempObject
    {
        private bool rotating = false;

        public bool IsDead() => _currentTexture == 0 && rotating;

        public Gift(Game game/*, Vector2 position*/) : base("gift", /*position*/new Vector2(5f, 0f), Enumerable.Range(0, 6).Select(n => game.Content.Load<Texture2D>($"assets/orig/images/gift.9")).ToArray())
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
