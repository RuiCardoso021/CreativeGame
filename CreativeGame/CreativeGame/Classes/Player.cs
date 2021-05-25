using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CreativeGame.Classes
{
    public class Player : Sprite
    {
        private Game1 _game;
        private bool _grounded = false;
        public Player(Game1 game) :
            base(game, "idle", new Vector2(0f, 4f))
        {
            _game = game;
        }

        public override void Update(GameTime gameTime)
        {
            if (!_grounded)
            {
                Translate(0f, -.15f);
                Collider.Update(gameTime); // update collider position accordingly with previous translation

                bool colliding = false;
                Collider collidedWith = null;
                foreach (Collider c in Collider.Colliders)
                {
                    if (c != Collider && Collider.CollidesWith(c))
                    {
                        colliding = true;
                        collidedWith = c;
                        break;
                    }
                }

                if (colliding)
                {
                    while (Collider.CollidesWith(collidedWith))
                    {
                        Translate(0f, .01f);
                        Collider.Update(gameTime);
                    }
                    _grounded = true;
                }

            }

            base.Update(gameTime);
        }
    }
}
