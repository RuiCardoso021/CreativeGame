using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CreativeGame
{
    // Father class for all game-objects, being them static or dynamic
    public class GameObject
    {
        protected Vector2 _position;
        protected string _name;

        public GameObject(string name) : this(name, Vector2.Zero)
        {
        }

        public GameObject(string name, Vector2 position)
        {
            _name = name;
            _position = position;
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
        }
    }
}