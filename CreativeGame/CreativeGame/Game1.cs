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
        public Texture2D lifeImg;
        private Scene _scene;
        private Player _player;
        public NPC _npc;
        private World _world;
        private Coin _coin;
        private Enemy2 _enemy2;
        private Gift _gift;
        private SnowHouse _snowHouse;
        private SnowBall _snowBall;
        private int lifeCount = 3;
        private float _volume = 0.1f;
        private State _currentState, _nextState;
        public bool isWin = false, isLose = false, isRDown = false, isPause = false, isPDown = false, isVDown = false, isSoundActive = true;

        public SoundEffect _soundJ, _soundT, _soundB, _soundCoin, _soundWin, _soundGift, _soundDie, _soundGO, _soundWG;
        public SoundEffectInstance _soundBackground, _soundJump, _soundThrowSnowball, _catchCoin, _catchGift, _soundFinishLevel, _soundDying, _soundGameOver, _soundWinGame;
        public bool activeMenu = false, activeCredits = false, activeBackButton = false;
        private SpriteFont _buttonFont;
        Rectangle screenRectangle;

        public Player Player => _player;
        public Coin Coin => _coin;
        public Enemy2 Enemy2 => _enemy2;
        public Gift Gift => _gift;
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

            _player = new Player(this, _world);
            _npc = new NPC(this, _world);
            _enemy2 = new Enemy2(this);
            _snowHouse = new SnowHouse(this, _world);
            _snowBall = new SnowBall(this);
            _coin = new Coin(this, _world);
            _gift = new Gift(this, _world);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            lifeImg = Content.Load<Texture2D>($"Life/heart");

            _soundB = Content.Load<SoundEffect>("background_sound");
            _soundJ = Content.Load<SoundEffect>("jump_sound");
            _soundT = Content.Load<SoundEffect>("throwSnowballSound");
            _soundCoin = Content.Load<SoundEffect>("catchCoin");
            _soundDie = Content.Load<SoundEffect>("dying");
            _soundGift = Content.Load<SoundEffect>("catchG");
            _soundWin = Content.Load<SoundEffect>("clap");
            _soundGO = Content.Load<SoundEffect>("gameOver");
            _soundWG = Content.Load<SoundEffect>("triumph");

            _soundBackground = _soundB.CreateInstance();
            _soundJump = _soundJ.CreateInstance();
            _soundThrowSnowball = _soundT.CreateInstance();
            _catchCoin = _soundCoin.CreateInstance();
            _soundDying = _soundDie.CreateInstance();
            _catchGift = _soundGift.CreateInstance();
            _soundFinishLevel = _soundWin.CreateInstance();
            _soundGameOver = _soundGO.CreateInstance();
            _soundWinGame = _soundWG.CreateInstance();

            _soundBackground.Volume = _volume - 0.05f;
            _soundJump.Volume = _volume;
            _soundThrowSnowball.Volume = _volume;
            _catchCoin.Volume = _volume;
            _soundDying.Volume = _volume;
            _catchGift.Volume = _volume;
            _soundFinishLevel.Volume = _volume;
            _soundGameOver.Volume = _volume;
            _soundWinGame.Volume = _volume;


            _scene = new Scene(this, "MainScene");
            _currentState = new MenuState(this, _graphics.GraphicsDevice, Content);
            _buttonFont = Content.Load<SpriteFont>("Fonts/File");
        }

        public void restart()
        {
            foreach (Body b in _world.BodyList)
                _world.RemoveBody(b);

            _scene = new Scene(this, "MainScene");
            _player = new Player(this, _world);
            _npc = new NPC(this, _world);
            _enemy2 = new Enemy2(this);
            _snowHouse = new SnowHouse(this, _world);
            _snowBall = new SnowBall(this);
            _coin = new Coin(this, _world);
            _gift = new Gift(this, _world);

            if (!isSoundActive) 
                _soundDying.Play();
            lifeCount--;

            //verifica se jogador perdeu todas vidas
            if (lifeCount == 0)
            {
                isLose = true;
                if (!isSoundActive)
                    _soundGO.Play();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                if (!isPDown)
                {
                    isPDown = true;
                    if (isPause == false)
                    {
                        isPause = true;
                        _soundBackground.Pause();
                    }
                    else
                    {
                        isPause = false;
                        _soundBackground.Play();
                    }
                        
                }

            }
            else
                isPDown = false;

            if (!isPause)
            { 
                if(!isWin && !isLose)
                { 
                    if (activeMenu && activeCredits == false)
                    {
                        _world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
                        _player.Update(gameTime);
                        if (!_npc.IsDead()) _npc.Update(gameTime);
                        //_enemy2.Update(gameTime);
                        _snowHouse.Update(gameTime);
                        //_snowBall.Update(gameTime);
                        if (!_coin.IsDead()) _coin.Update(gameTime);
                        if (!_gift.IsDead()) _gift.Update(gameTime);

                        if (Keyboard.GetState().IsKeyUp(Keys.V))
                        {
                            if (!isVDown)
                            {
                                isVDown = true;
                                if (isSoundActive)
                                {
                                    _soundBackground.Play();
                                    isSoundActive = false;
                                }
                                else
                                {
                                    _soundBackground.Pause();
                                    isSoundActive = true;
                                }
                            }
                        }
                        else
                            isVDown = false;
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
                if (!isRDown)
                {
                    restart();
                    isRDown = true;
                } 
            }
            else
                isRDown = false;


            if (activeMenu && activeCredits == false)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                _spriteBatch.Begin();

                if(lifeCount == 3)
                {
                    _spriteBatch.Draw(lifeImg, new Vector2(0f, 0f), Color.White);
                    _spriteBatch.Draw(lifeImg, new Vector2(50f, 0f), Color.White);
                    _spriteBatch.Draw(lifeImg, new Vector2(100f, 0f), Color.White);
                }
                else if (lifeCount == 2)
                {
                    _spriteBatch.Draw(lifeImg, new Vector2(0f, 0f), Color.White);
                    _spriteBatch.Draw(lifeImg, new Vector2(50f, 0f), Color.White);
                }
                else if (lifeCount == 1)
                    _spriteBatch.Draw(lifeImg, new Vector2(0f, 0f), Color.White);
                /*Life.lifeImg.Draw(_spriteBatch, gameTime);
                string lives = $"Lives: {Life.lifeCount}";
                Point measure = _buttonFont.MeasureString(lives).ToPoint();
                int posX = 100 - measure.X - 5;
                _spriteBatch.DrawString(_buttonFont, lives, new Vector2(posX, 100 + 30), Color.White);*/

                _scene.Draw(_spriteBatch, gameTime);
                if (!_npc.IsDead()) _npc.Draw(_spriteBatch, gameTime);
                _snowHouse.Draw(_spriteBatch, gameTime);
                //_snowBall.Draw(_spriteBatch, gameTime);
                _player.Draw(_spriteBatch, gameTime);
                //_enemy2.Draw(_spriteBatch, gameTime);
                if (!_coin.IsDead()) _coin.Draw(_spriteBatch, gameTime);
                if (!_gift.IsDead()) _gift.Draw(_spriteBatch, gameTime);
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
