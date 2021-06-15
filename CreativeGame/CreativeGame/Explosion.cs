using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CreativeGame.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CreativeGame
{
    public class Explosion : AnimatedSprite, ITempObject
    {
        private bool cycled = false;
        public bool IsDead() => _currentTexture == 0 && cycled;

        public Explosion(Game game, Vector2 position) : base("explosion", position, Enumerable.Range(1, 20).Select(n => game.Content.Load<Texture2D>($"Explosion/tile_{n}")).ToArray())
        {
            _fps = 20;
        }

        public override void Update(GameTime gameTime)
        {
            if (_currentTexture > 0) cycled = true;
            base.Update(gameTime);
        }
    }
}