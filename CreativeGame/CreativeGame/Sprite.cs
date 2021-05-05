using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CreativeGame
{
    // Game Object child
    // represents a game object that has a texture
    public class Sprite : GameObject
    {
        // TODO: we should not duplicate textures on each instance
        private Texture2D _texture;
        
        public Sprite(Game game, string textureName, Vector2 position) : base(textureName, position)
        {
            _texture = game.Content.Load<Texture2D>(textureName);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // TODO: 128 should not be hardcoded!!
            spriteBatch.Draw(_texture, _position * 128, Color.White);
        }
    }
}