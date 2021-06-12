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
        public NPC _npc;
        private World _world;
        private Coin _coin;
        private Enemy2 _enemy2;
        private Gift _gift;
        private Life _life;
        private SnowHouse _snowHouse;
        private SnowBall _snowBall;
        private SoundEffect _soundB;
        private SoundEffectInstance _soundBackground;
        private float _volume = 0.1f;
        private State _currentState;
        private State _nextState;

        public SoundEffect _soundJ, _soundT;
        public SoundEffectInstance _soundJump;
        public SoundEffectInstance _soundThrowSnowball;
        public bool activeMenu = false, activeCredits = false, activeBackButton = false;
        private SpriteFont _buttonFont;
        Rectangle screenRectangle;

        public Player Player => _player;
        public Coin Coin => _coin;
        public Enemy2 Enemy2 => _enemy2;
        public Gift Gift => _gift;
        public Life Life => _life;
        public SnowHouse SnowHouse => _snowHouse;
        public SnowBall SnowBall => _snowBall;

        public void ChangeState(State state) { _nextState = state; }

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
            screenRectangle = new Rectangle(0, 0, 1024, 768);
            //this.Life.rectLife.X = (screenRectangle.Width / 2)/* - (this.Life.rectLife.Width / 2)*/;
            //this.Life.rectLife.Y = (screenRectangle.Height / 2) /*- (this.Life.rectLife.Height / 2)*/;

            _graphics.PreferredBackBufferHeight = screenRectangle.Height;
            _graphics.PreferredBackBufferWidth = screenRectangle.Width;

            _graphics.ApplyChanges();

            Debug.SetGraphicsDevice(GraphicsDevice);

            new Camera(GraphicsDevice, height: 10f);
            Camera.LookAt(Camera.WorldSize / 2f);

            _player = new Player(this);
            _coin = new Coin(this);
            _enemy2 = new Enemy2(this);
            _snowHouse = new SnowHouse(this);
            _snowBall = new SnowBall(this);
            _gift = new Gift(this);
            _life = new Life(this);
            _npc = new NPC(this);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _soundB = Content.Load<SoundEffect>("background_sound");
            _soundBackground = _soundB.CreateInstance();
            _soundBackground.Volume = _volume - 0.05f;
            _soundJ = Content.Load<SoundEffect>("jump_sound");
            _soundT = Content.Load<SoundEffect>("throwSnowballSound");
            _soundJump = _soundJ.CreateInstance();
            _soundJump.Volume = _volume;
            _soundThrowSnowball = _soundT.CreateInstance();
            _soundThrowSnowball.Volume = _volume;
            _scene = new Scene(this, "MainScene");
            _currentState = new MenuState(this, _graphics.GraphicsDevice, Content);
            _buttonFont = Content.Load<SpriteFont>("Fonts/File");
        }

        protected override void Update(GameTime gameTime)
        {
            if (!IsVictory())
            {
                if (activeMenu && activeCredits == false)
                {
                    _world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
                    _player.Update(gameTime);
                    _coin.Update(gameTime);
                    _enemy2.Update(gameTime);
                    _snowHouse.Update(gameTime);
                    _snowBall.Update(gameTime);
                    _gift.Update(gameTime);
                    _life.Update(gameTime);
                    _npc.Update(gameTime);
                    _soundBackground.Play();

                }
                /*else if(activeMenu == false && activeCredits == true)
                {
                
             
                }*/
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
            }
            else
            {

            }

            base.Update(gameTime);
        }
        public bool IsVictory()
        {
            if (this.Coin.nrMoedas == 3 && this.Player.Position == this.SnowHouse.Position || this.SnowHouse.ImpactPos == this.Player.Position)
                return true;
            else
                return false;
        }

        protected override void Draw(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                foreach (Body b in _world.BodyList)
                    _world.RemoveBody(b);

                _scene = new Scene(this, "MainScene");
                _player = new Player(this);
                _coin = new Coin(this);
                _enemy2 = new Enemy2(this);
                _snowHouse = new SnowHouse(this);
                _snowBall = new SnowBall(this);
                _gift = new Gift(this);
                _life = new Life(this);
                _npc = new NPC(this);
                this.Life.lifeCount--;

                //verifica se jogador perdeu todas vidas
                if (this.Life.lifeCount == 0)
                {

                }
            }

            if (activeMenu && activeCredits == false)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                _spriteBatch.Begin();
                _scene.Draw(_spriteBatch, gameTime);
                _npc.Draw(_spriteBatch, gameTime);
                _snowHouse.Draw(_spriteBatch, gameTime);
                _snowBall.Draw(_spriteBatch, gameTime);
                _player.Draw(_spriteBatch, gameTime);
                _coin.Draw(_spriteBatch, gameTime);
                _enemy2.Draw(_spriteBatch, gameTime);
                _gift.Draw(_spriteBatch, gameTime);
                _life.Draw(_spriteBatch, gameTime);
                _spriteBatch.End();
            }
            else if (activeMenu == false && activeCredits == true)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                _spriteBatch.Begin();
                _spriteBatch.DrawString(_buttonFont, $"Developed by:\nPaulo Macedo 16614\nBruno Carvalho 16614\nRui Cardoso 16624\nEDJD - 2020/2021", new Vector2(5, 25), Color.White);
                MenuState._components["back"].Draw(gameTime, _spriteBatch);
                _spriteBatch.End();
            }
            else
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                _currentState.Draw(gameTime, _spriteBatch);
            }


            base.Draw(gameTime);
        }
    }
}
