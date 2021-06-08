using Genbox.VelcroPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CreativeGame.Classes;
using Microsoft.Xna.Framework.Audio;

namespace CreativeGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Scene _scene;
        private Player _player;
        private NPC _npc;
        private World _world;
        private SoundEffect _sound;
        private SoundEffectInstance _soundBackground;

        public Player Player => _player;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _world = new World(new Vector2(0, -9.82f));
            Services.AddService(_world);

            new KeyboardManager(this);
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = 768;
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.ApplyChanges();

            Debug.SetGraphicsDevice(GraphicsDevice);

            new Camera(GraphicsDevice, height: 5f);
            Camera.LookAt(Camera.WorldSize / 2f);

            _player = new Player(this);
            _npc = new NPC(this);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _sound = Content.Load<SoundEffect>("background_sound");
            _soundBackground = _sound.CreateInstance();
            _scene = new Scene(this, "MainScene");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
            _player.Update(gameTime);
            _npc.Update(gameTime);
            _soundBackground.Play();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _scene.Draw(_spriteBatch, gameTime);
            _npc.Draw(_spriteBatch, gameTime);
            _player.Draw(_spriteBatch, gameTime);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
