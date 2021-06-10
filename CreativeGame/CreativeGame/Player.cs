using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;
using CreativeGame.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CreativeGame
{
    public class Player : AnimatedSprite
    {
        enum Status
        {
            Idle, Walk,
        }
        private Status _status = Status.Idle;

        private Game1 _game;
        private bool _isGrounded = false;
        private Texture2D _fireBall;
        private Vector2 dir = new Vector2(30, 0);

        private List<ITempObject> _objects;

        private List<Texture2D> _idleFrames;
        private List<Texture2D> _walkFrames;

        public Player(Game1 game) :
            base("idle",
                new Vector2(0f, 4f),
                Enumerable.Range(1, 16)
                    .Select(
                        n => game.Content.Load<Texture2D>($"Idle ({n})")
                        )
                    .ToArray())
        {
            _idleFrames = _textures; // loaded by the base construtor

            _walkFrames = Enumerable.Range(1, 13)
                .Select(
                    n => game.Content.Load<Texture2D>($"Walk({n})")
                )
                .ToList();

            _game = game;

            
            _fireBall = _game.Content.Load<Texture2D>("fireball");
            _objects = new List<ITempObject>();

            AddRectangleBody(
                _game.Services.GetService<World>(),
                width: _size.X / 2.2f
                //height: _size.Y / 1.5f
            ); // kinematic is false by default

            Fixture sensor = FixtureFactory.AttachRectangle(
                _size.X / 3f, _size.Y * 0.05f,
                4, new Vector2(0, -_size.Y / 2f),
                Body);
            sensor.IsSensor = true;

            sensor.OnCollision = (a, b, contact) =>
            {
                if (b.GameObject().Name != "bullet")
                    _isGrounded = true;
            };
            sensor.OnSeparation = (a, b, contact) => _isGrounded = false;

            KeyboardManager.Register(
                Keys.Space,
                KeysState.GoingDown,
                () =>
                {
                    if (_isGrounded) { Body.ApplyForce(new Vector2(0, 300f)); _game._soundJump.Play(); }
                });
            KeyboardManager.Register(
                Keys.A,
                KeysState.Down,
                () => { Body.ApplyForce(new Vector2(-10, 0)); dir = new Vector2(-10, 0); }); //Body.LinearVelocity = new Vector2(-5, 0); });
            KeyboardManager.Register(
                Keys.D,
                KeysState.Down,
                () => { Body.ApplyForce(new Vector2(10, 0)); dir = new Vector2(10, 0); }); //Body.LinearVelocity = new Vector2(5, 0); });

            KeyboardManager.Register(
                Keys.Enter, 
                KeysState.GoingDown,
                () => { Bullet bullet = new Bullet(_fireBall, _position, dir, game.Services.GetService<World>()); _objects.Add(bullet); } );

        }

        public override void Update(GameTime gameTime)
        {
            foreach (ITempObject obj in _objects)
                obj.Update(gameTime);

            if (_status == Status.Idle && Body.LinearVelocity.LengthSquared() > 0.001f)
            {
                _status = Status.Walk;
                _textures = _walkFrames;
                _currentTexture = 0;
            }

            if (_status == Status.Walk && Body.LinearVelocity.LengthSquared() <= 0.001f)
            {
                _status = Status.Idle;
                _textures = _idleFrames;
                _currentTexture = 0;
            }

            if (Body.LinearVelocity.X < 0f) _direction = Direction.Left;
            else if (Body.LinearVelocity.X > 0f) _direction = Direction.Right;

            base.Update(gameTime);
            Camera.LookAt(_position);

            _objects.AddRange(_objects
                .Where(obj => obj is Bullet)
                .Cast<Bullet>()
                .Where(b => b.Collided)
                .Select(b => new Explosion(_game, b.ImpactPos))
                .ToArray()
            );
            _objects = _objects.Where(b => !b.IsDead()).ToList();
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
            foreach (ITempObject obj in _objects)
                obj.Draw(spriteBatch, gameTime);
        }
    }
}
