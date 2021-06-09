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
        private SoundEffect _soundB;
        private SoundEffectInstance _soundBackground;
        private float _volume = 0.1f;
        private State _currentState;
        private State _nextState;

        public SoundEffect _soundJ;
        public SoundEffectInstance _soundJump;
        public bool activeMenu = false;

        public Player Player => _player;
        public void ChangeState(State state){ _nextState = state; }

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
            IsMouseVisible = true;

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
            _soundB = Content.Load<SoundEffect>("background_sound");
            _soundBackground = _soundB.CreateInstance();
            _soundBackground.Volume = _volume-0.05f;
            _soundJ = Content.Load<SoundEffect>("jump_sound");
            _soundJump = _soundJ.CreateInstance();
            _soundJump.Volume = _volume;
            _scene = new Scene(this, "MainScene");
            _currentState = new MenuState(this, _graphics.GraphicsDevice, Content);
        }

        protected override void Update(GameTime gameTime)
        {

            if (activeMenu)
            {
                _world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
                _player.Update(gameTime);
                _npc.Update(gameTime);
                _soundBackground.Play();
            }
            else
            {
                if (_nextState != null)
                {
                    _currentState = _nextState;

                    _nextState = null;
                }
                _currentState.PostUpdate(gameTime);
                _currentState.Update(gameTime);
            }

            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            

            if (activeMenu)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                _spriteBatch.Begin();
                _scene.Draw(_spriteBatch, gameTime);
                _npc.Draw(_spriteBatch, gameTime);
                _player.Draw(_spriteBatch, gameTime);

                _spriteBatch.End();
            }
            else
            {
                GraphicsDevice.Clear(Color.Blue);
                _currentState.Draw(gameTime, _spriteBatch);
            }


            base.Draw(gameTime);
        }
    }
}
