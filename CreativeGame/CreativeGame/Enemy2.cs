using CreativeGame.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace CreativeGame
{
    public class Enemy2 : AnimatedSprite, ITempObject
    {
        private bool rotating = false;

        public bool IsDead() => _currentTexture == 0 && rotating;

        public Enemy2(Game game/*, Vector2 position*/) : base("enemy", /*position*/new Vector2(1f, 3f), Enumerable.Range(0, 27).Select(n => game.Content.Load<Texture2D>($"Inimigo/Screenshot_{n + 1}")).ToArray())
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
