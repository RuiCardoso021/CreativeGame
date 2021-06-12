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
        public bool isWin = false, isLose = false;

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
            _coin = new Coin(this, _world);
            _enemy2 = new Enemy2(this);
            _snowHouse = new SnowHouse(this);
            _snowBall = new SnowBall(this);
            _gift = new Gift(this, _world);
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

        public void restart()
        {
            foreach (Body b in _world.BodyList)
                _world.RemoveBody(b);

            _scene = new Scene(this, "MainScene");
            _player = new Player(this);
            _coin = new Coin(this, _world);
            _enemy2 = new Enemy2(this);
            _snowHouse = new SnowHouse(this);
            _snowBall = new SnowBall(this);
            _gift = new Gift(this, _world);

            _npc = new NPC(this);
            this.Life.lifeCount--;

            //verifica se jogador perdeu todas vidas
            if (this.Life.lifeCount == 0)
            {
                isLose = true;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if(!isWin && !isLose)
            { 
                if (activeMenu && activeCredits == false)
                {
                    _world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
                    _player.Update(gameTime);
                    if (!_coin.IsDead()) _coin.Update(gameTime);
                    //_enemy2.Update(gameTime);
                    _snowHouse.Update(gameTime);
                    //_snowBall.Update(gameTime);
                    if (!_gift.IsDead()) _gift.Update(gameTime);
                    _life.Update(gameTime);
                    if (!_npc.IsDead()) _npc.Update(gameTime);
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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //Verifica se e vitoria
            if (isWin)
            {
                _spriteBatch.Begin();
                Vector2 windowSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

                Texture2D pixel = new Texture2D(GraphicsDevice, 1, 1);
                pixel.SetData(new[] { Color.White });
                _spriteBatch.Draw(pixel, new Rectangle(Point.Zero, windowSize.ToPoint()), new Color(Color.Gold, 0.1f));

                //Desenha mensagem de vitoria
                string win = $"You Win!!!";
                Vector2 winMeasures = _buttonFont.MeasureString(win) / 2f;
                Vector2 windowCenter = windowSize / 2f;
                Vector2 pos = windowCenter - winMeasures;
                _spriteBatch.DrawString(_buttonFont, win, pos, Color.Red);
                _spriteBatch.End();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                restart();
            }

            if (activeMenu && activeCredits == false)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                _spriteBatch.Begin();
                _scene.Draw(_spriteBatch, gameTime);
                if (!_npc.IsDead()) _npc.Draw(_spriteBatch, gameTime);
                _snowHouse.Draw(_spriteBatch, gameTime);
                //_snowBall.Draw(_spriteBatch, gameTime);
                _player.Draw(_spriteBatch, gameTime);
                if (!_coin.IsDead()) _coin.Draw(_spriteBatch, gameTime);
                //_enemy2.Draw(_spriteBatch, gameTime);
                if (!_gift.IsDead()) _gift.Draw(_spriteBatch, gameTime);
                //_life.Draw(_spriteBatch, gameTime);
                _spriteBatch.End();
            }
            else if (activeMenu == false && activeCredits == true)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                _spriteBatch.Begin();
                Vector2 windowSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight/2);
                string credits = $"Developed by:\nPaulo Macedo 16544\nBruno Carvalho 16614\nRui Cardoso 16624\nEDJD - 2020/2021";
                Vector2 winMeasures = _buttonFont.MeasureString(credits) / 2f;
                Vector2 windowCenter = windowSize / 2f;
                Vector2 pos = windowCenter - winMeasures;
                _spriteBatch.DrawString(_buttonFont, credits, pos, Color.White);
                MenuState._components["back"].Draw(gameTime, _spriteBatch);
                _spriteBatch.End();
            }
            else
            {
                if(!isWin && !isLose)
                {
                    GraphicsDevice.Clear(Color.CornflowerBlue);
                    _currentState.Draw(gameTime, _spriteBatch);
                }
                
            }

            //Verifica se e derrota
            if (isLose)
            {
                _spriteBatch.Begin();
                Vector2 windowSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
                Texture2D pixel = new Texture2D(GraphicsDevice, 1, 1);
                pixel.SetData(new[] { Color.White });
                _spriteBatch.Draw(pixel, new Rectangle(Point.Zero, windowSize.ToPoint()), new Color(Color.Silver, 0.1f));

                //Desenha mensagem de derrota
                string lose = $"Soory! Now u don't have more gifts!";
                Vector2 winMeasures = _buttonFont.MeasureString(lose) / 2f;
                Vector2 windowCenter = windowSize / 2f;
                Vector2 pos = windowCenter - winMeasures;
                _spriteBatch.DrawString(_buttonFont, lose, pos, Color.Red);

                _spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
