using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CreativeGame.Classes
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
            _size = _texture.Bounds.Size.ToVector2() / 128f;  // TODO: HARDCODED!
            _position = position + new Vector2(_size.X, _size.Y) / 2f; // Anchor in the middle
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Vector2 pos = Camera.Position2Pixels(_position);
            // Vector2 anchor = _texture.Bounds.Size.ToVector2() / 2f;
            Vector2 anchor = new Vector2(_texture.Width, _texture.Height) / 2f;

            Vector2 scale = Camera.Length2Pixels(_size) / 128f; // TODO: HARDCODED!
            scale.Y = scale.X;  // FIXME! TODO: HACK HACK HACK

            spriteBatch.Draw(_texture, pos, null, Color.White,
                0f, anchor, scale, SpriteEffects.None, 0);

            base.Draw(spriteBatch, gameTime);
        }
    }
}