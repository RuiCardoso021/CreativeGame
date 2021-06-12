using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreativeGame.Classes.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CreativeGame.Classes
{
    public class MenuState : State
    {
        public static Dictionary<String, Component> _components;

        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            var buttonTexture = _content.Load<Texture2D>("Button/Button");
            var buttonFont = _content.Load<SpriteFont>("Fonts/File");

            var newGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(420, 200),
                Text = "New Game",
            };

            newGameButton.Click += NewGameButton_Click;

            var loadGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(420, 250),
                Text = "Load Game",
            };

            loadGameButton.Click += LoadGameButton_Click;

            var creditsGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(420, 300),
                Text = "Credits",
            };

            creditsGameButton.Click += CreditsGameButton_Click;

            var highScoreGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(420, 350),
                Text = "HighScore",
            };

            highScoreGameButton.Click += HighScoreGameButton_Click;

            var backGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(420, 400),
                Text = "Back",
            };

            backGameButton.Click += BackGameButton_Click;

            var quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(420, 450),
                Text = "Quit Game",
            };

            quitGameButton.Click += QuitGameButton_Click;

            _components = new Dictionary<String, Component>()
            {
                {"newGame",newGameButton},
                {"loadGame",loadGameButton},
                {"credits", creditsGameButton},
                {"highScore", highScoreGameButton},
                {"back", backGameButton},
                {"quit",quitGameButton},
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var component in _components)
            {
                if (component.Key == "back" && !_game.activeMenu)
                {
                    continue;
                }

                component.Value.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            _game.activeMenu = true;
        }

        private void LoadGameButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Load Game");
        }
        private void CreditsGameButton_Click(object sender, EventArgs e)
        {
            _game.activeCredits = true;
        }
        private void HighScoreGameButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Score");
        }
        public void BackGameButton_Click(object sender, EventArgs e)
        {
            _game.activeMenu = false;
            _game.activeCredits = false;
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // remove sprites if they're not needed
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Value.Update(gameTime);
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }
    }
}