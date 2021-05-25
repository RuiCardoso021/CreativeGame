using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CreativeGame.Classes
{
    public class AABBCollider : Collider
    {
        private RectangleF _collider;

        private Vector2 PositionFromCenter()  // <==== 
        {
            return _gameObject.Position + _gameObject.Size * new Vector2(-0.5f, 0.5f);
        }

        public AABBCollider(GameObject go) : base(go)
        {
            Vector2 location = PositionFromCenter();  // <====
            _collider = new RectangleF(location, go.Size); // <====
        }

        public override bool CollidesWith(Collider other)
        {
            switch (other)
            {
                case AABBCollider aabb:
                    return _collider.Intersects(aabb._collider);
                default:
                    throw new Exception("AABBCollider CollidesWith with unknown type of collider");
            }
        }

        public override void Update(GameTime gameTime)
        {
            _collider.Location = PositionFromCenter();   // <====
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (_debug)
            {
                _draw.DrawRectangle(spriteBatch, Camera.Rectangle2Pixels(_collider), Color.Yellow);
            }
        }
    }
}
