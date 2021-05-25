using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CreativeGame.Classes
{
    // Father class for all game-objects, being them static or dynamic
    public class GameObject
    {
        protected Vector2 _position, _size;
        protected string _name;

        public Vector2 Position => _position;
        public Vector2 Size => _size;
        public string Name => _name;

        public GameObject(string name) : this(name, Vector2.Zero)
        {
        }

        public GameObject(string name, Vector2 position)
        {
            _size = Vector2.One;
            _name = name;
            _position = position;
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
        }

        public void Translate(float x, float y)
        {
            _position.X += x;
            _position.Y += y;
        }
    }
}