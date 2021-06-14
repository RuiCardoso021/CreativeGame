using System;
using System.Collections.Generic;
using System.Linq;
using Genbox.VelcroPhysics.Collision.RayCast;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;
using CreativeGame.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CreativeGame
{
    public class NPC : AnimatedSprite
    {
        enum Status
        {
            Flying, Patroling, Chasing, Die
        }
        private Status _status = Status.Patroling;

        private Game1 _game;
        private bool _onGround;

        public bool IsDead() => _status == Status.Die;

        private List<Texture2D> _idleFrames;
        private List<Texture2D> _walkFrames;
        private Vector2 _startingPoint;

        public NPC(Game1 game, World world) : base("enemy", new Vector2(5.5f, 4f), Enumerable.Range(0, 27).Select(n => game.Content.Load<Texture2D>($"Inimigo/Screenshot_{n + 1}")).ToArray())
        {
            _idleFrames = _textures; // loaded by the base construtor
            _direction = Direction.Left;

            // _walkFrames = Enumerable.Range(1, 10)
            //     .Select(
            //         n => game.Content.Load<Texture2D>($"Walk_{n}")
            //     )
            //     .ToList();

            _game = game;

            AddRectangleBody(_game.Services.GetService<World>(), width: _size.X / 2f); // kinematic is false by default

            // Events on body
            Body.Friction = 0f;
            Body.OnCollision = (a, b, contact) =>
            {
                if (_status == Status.Die) return;

                if (b.GameObject().Name == "bullet")
                {
                    System.Diagnostics.Debug.WriteLine("NPC foi atingido.");
                    _status = Status.Die;
                    world.RemoveBody(Body);
                }
                else if (b.GameObject().Name == "player")
                {
                    System.Diagnostics.Debug.WriteLine("player perde 1 vida");
                    world.RemoveBody(Body);
                    _game.restart();
                }
            };

            // Events on foot
            Fixture sensor = FixtureFactory.AttachRectangle(_size.X / 3f, _size.Y * 0.05f, 4, new Vector2(0, -_size.Y / 2f), Body);
            sensor.IsSensor = true;
            sensor.OnCollision = (a, b, contact) =>
            {
                if (_status == Status.Die) return;

                _onGround = b.GameObject().Name.StartsWith("assets/orig/images/");

                if (_status == Status.Flying)
                {
                    _status = Status.Patroling;
                    _startingPoint = _position;
                }
            };

            sensor.OnSeparation = (a, b, contact) =>
            {
                if (_status == Status.Die) return;

                _onGround = b.GameObject().Name.StartsWith("assets/orig/images/");
            };
        }


        public override void Update(GameTime gameTime)
        {
            if (_status == Status.Die) return;

            if (_status != Status.Flying && !_onGround)
            {
                Body.LinearVelocity = Vector2.Zero;
                _status = Status.Flying;
            }

            // Chasing
            if (_status == Status.Chasing)
            {
                // Player ran away
                if ((_position - _game.Player.Position).Length() > 1.5f)
                    _status = Status.Patroling;
                // We are near the player
                else if ((_position - _game.Player.Position).Length() < 0.6f)
                {
                    // FIXME: Do Damage!!! Lots of it.
                    Body.LinearVelocity = Vector2.Zero;
                }
                else
                {
                    _direction = _position.X > _game.Player.Position.X ? Direction.Left : Direction.Right;
                    Body.LinearVelocity = new Vector2(_game.Player.Position.X - _position.X, 0);
                    Body.LinearVelocity.Normalize();
                }
            }

            // Patrolling
            if (_status == Status.Patroling)
            {
                float _patrolDistance = 2f;

                if ((_position - _game.Player.Position).Length() < 1.5f)
                {
                    _status = Status.Chasing;
                }
                else if (_direction == Direction.Left) // Leaving Starting Point
                {
                    if (_position.X < _startingPoint.X - _patrolDistance)
                        _direction = Direction.Right;
                    else
                        Body.LinearVelocity = -Vector2.UnitX;  //<<
                }
                else  // Going to starting Point
                {
                    if (_position.X > _startingPoint.X)  //<<
                        _direction = Direction.Left;
                    else
                        Body.LinearVelocity = Vector2.UnitX;  //<<
                }
            }

            base.Update(gameTime);
        }
    }
}