using CreativeGame.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreativeGame
{
    

    public class Life : AnimatedSprite, ITempObject
    {
        //private Point position;
        //private Game1 game;
        //private Texture2D life;
        //Camera.LookAt(_position);
        private bool rotating = false;
        public int lifeCount = 3;
        private Texture2D lifeImg;
        public Rectangle rectLife;

        public bool IsDead() => _currentTexture == 0 && rotating;

        public Life(Game game/*, Vector2 position*/) : base("life", /*position*/new Vector2(0f, 5f), Enumerable.Range(0, 1).Select(n => game.Content.Load<Texture2D>($"Life/heart")).ToArray())
        {
            _fps = 20;
        }

        /*public Lifes(Game1 game1, int x, int y) //recebe posicao do pai natal e da + x em altura e largura, colocando assim as vidas no canto superior esquerdo smp
        {
            position = new Point(x, y);
            game = game1;
        }*/

        /*public void LoadContents()
        {
            life = game.Content.Load<Texture2D>($"Life/heart");
        }*/

        public void contentLoad(Game game)
        {
            lifeImg = game.Content.Load<Texture2D>($"Life/heart");
        }

        public void Update(GameTime gameTime)
        {
            rectLife.X++;

          
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
          
        
            base.Draw(spriteBatch, gameTime);
        }
    }
}
