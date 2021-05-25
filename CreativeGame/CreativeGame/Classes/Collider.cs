using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CreativeGame.Classes
{
    public abstract class Collider
    {
        protected GameObject _gameObject;
        protected bool _debug;
        protected Debug _draw;

        private static List<Collider> _colliders = new List<Collider>();
        public static List<Collider> Colliders => _colliders;  // Temporary stuff???

        public Collider(GameObject go)
        {
            _gameObject = go;
            _draw = new Debug();
            _colliders.Add(this);
        }

        public void EnableDebug(bool debug)
        {
            _debug = debug;
        }

        public abstract bool CollidesWith(Collider other);
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
