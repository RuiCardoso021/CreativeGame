using Genbox.VelcroPhysics.Collision.Shapes;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CreativeGame.Classes
{
    public class Player : Sprite
    {
        private Game1 _game;
        private bool _isGrounded = false;
        public Player(Game1 game) :
            base(game, "idle", new Vector2(0f, 4f))
        {
            _game = game;

            AddRectangleBody(
                game.Services.GetService<World>(),
                width: _size.X / 2f
            ); // kinematic is false by default

            Fixture sensor = FixtureFactory.AttachRectangle(
                _size.X / 3f, _size.Y * 0.05f,
                4, new Vector2(0, -_size.Y / 2f),
                Body);
            sensor.IsSensor = true;

            sensor.OnCollision = (a, b, contact) => _isGrounded = true;
            sensor.OnSeparation = (a, b, contact) => _isGrounded = false;

            KeyboardManager.Register(
                Keys.Space,
                KeysState.GoingDown,
                () =>
                {
                    if (_isGrounded) Body.ApplyForce(new Vector2(0, 200f));
                });
            KeyboardManager.Register(
                Keys.A,
                KeysState.Down,
                () => { Body.ApplyForce(new Vector2(-5, 0)); });
            KeyboardManager.Register(
                Keys.D,
                KeysState.Down,
                () => { Body.ApplyForce(new Vector2(5, 0)); });

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
