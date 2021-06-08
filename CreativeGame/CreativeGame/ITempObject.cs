using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CreativeGame
{

        public interface ITempObject
        {
            bool IsDead();
            void Update(GameTime gameTime);
            void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
