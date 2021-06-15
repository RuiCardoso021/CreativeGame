using Genbox.VelcroPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CreativeGame.Classes;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using System;

namespace CreativeGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public Texture2D lifeImg, coinImg, imgPlay, imgPause, imgMute, imgUnmuted;
        private Scene _scene;
        private Player _player;
        public NPC _npc;
        private World _world;
        private Coin _coin, _coin2, _coin3;
        private Gift _gift;
        private SnowHouse _snowHouse;
        private SnowBall _snowBall;
        private int contMinLvlTime = 0;
        private double lvlTime = 0f;
        public int nrCoins = 0, lifeCount = 3;
        private string[] levelNames = new[] { "MainScene", "MainScene2"};
        private Texture2D _background, _background2;
        private float _volume = 0.1f;
        private State _currentState, _nextState;
        public int level = 0;
        public bool isWin = false, isLose = false, isRDown = false, isPause = false, isPDown = false, isVDown = false, isSoundActive = true, activeCommands = false;

        public SoundEffect _soundJ, _soundT, _soundB, _soundCoin, _soundWin, _soundGift, _soundDie, _soundGO, _soundWG;
        public SoundEffectInstance _soundBackground, _soundJump, _soundThrowSnowball, _catchCoin, _catchGift, _soundFinishLevel, _soundDying, _soundGameOver, _soundWinGame;
        public bool activeMenu = false, activeCredits = false, activeBackButton = false;
        private SpriteFont _buttonFont;
        Rectangle screenRectangle;

        public Player Player => _player;
        public Coin Coin => _coin;
        public Coin Coin2 => _coin2;
        public Coin Coin3 => _coin3;
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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            lifeImg = Content.Load<Texture2D>($"Life/heart");
            coinImg = Content.Load<Texture2D>($"Coin/coin0");
            imgPlay = Content.Load<Texture2D>($"tile000");
            imgPause = Content.Load<Texture2D>($"tile001");
            imgMute = Content.Load<Texture2D>($"Sound/v0");
            imgUnmuted = Content.Load<Texture2D>($"Sound/v1");

            _soundB = Content.Load<SoundEffect>("background_sound");
            _soundJ = Content.Load<SoundEffect>("jump_sound");
            _soundT = Content.Load<SoundEffect>("throwSnowballSound");
            _soundCoin = Content.Load<SoundEffect>("catchCoin");
            _soundDie = Content.Load<SoundEffect>("dying");
            _soundGift = Content.Load<SoundEffect>("catchG");
            _soundWin = Content.Load<SoundEffect>("clap");
            _soundGO = Content.Load<SoundEffect>("gameOver");
            _soundWG = Content.Load<SoundEffect>("triumph");
            _background = this.Content.Load<Texture2D>("Background/BG");
            _background2 = this.Content.Load<Texture2D>("Background/BG1");

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
            _soundGameOver.Volume = _volume - 0.09f;
            _soundWinGame.Volume = _volume;

            _currentState = new MenuState(this, _graphics.GraphicsDevice, Content);
            _buttonFont = Content.Load<SpriteFont>("Fonts/File");
        }

        public void loadLevel_1()
        {
            _scene = new Scene(this, levelNames[level]);
            _player = new Player(this, _world, new Vector2(5f, 5f));
            _npc = new NPC(this, _world, new Vector2(18f, 4f));
            _snowHouse = new SnowHouse(this, _world, new Vector2(51f, 3.70f));
            _snowBall = new SnowBall(this);
            _coin = new Coin(this, _world, new Vector2(54f, 8f));
            _coin2 = new Coin(this, _world, new Vector2(16f, 5f));
            _coin3 = new Coin(this, _world, new Vector2(39.85f, 13.85f));
            _gift = new Gift(this, _world, new Vector2(10f, 3.23f));      
        }

        public void loadLevel_2()
        {
            _scene = new Scene(this, levelNames[level]);
            _player = new Player(this, _world, new Vector2(5f, 10f));
            _npc = new NPC(this, _world, new Vector2(9f, 10f));
            _snowHouse = new SnowHouse(this, _world, new Vector2(45f, .5f));
            _snowBall = new SnowBall(this);
            _coin = new Coin(this, _world, new Vector2(8f, 1f));
            _coin2 = new Coin(this, _world, new Vector2(16f, 2f));
            _coin3 = new Coin(this, _world, new Vector2(40f, 5.5f));
            _gift = new Gift(this, _world, new Vector2(2f, 0.2f));
        }

        public void iniciarJogo()
        {
            foreach (Body b in _world.BodyList)
                _world.RemoveBody(b);

            if (level == 0)
                loadLevel_1();
            else loadLevel_2();
        }

        public void restart()
        {
            iniciarJogo();

            lvlTime = 0f;
            contMinLvlTime = 0;
            nrCoins = 0;

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
                        lvlTime += gameTime.ElapsedGameTime.TotalSeconds;

                        if (lvlTime > 59)
                        {
                            contMinLvlTime++;
                            lvlTime = 0;
                        }

                        _world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
                        _player.Update(gameTime);
                        if (!_npc.IsDead()) _npc.Update(gameTime);
                        //_enemy2.Update(gameTime);
                        _snowHouse.Update(gameTime);
                        //_snowBall.Update(gameTime);
                        if (!_coin.IsDead()) _coin.Update(gameTime);
                        if (!_coin2.IsDead()) _coin2.Update(gameTime);
                        if (!_coin3.IsDead()) _coin3.Update(gameTime);
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

        public void SaveGame()
        {
            if (File.Exists("DataGame.txt")) 
                File.Delete("DataGame.txt");
            using (StreamWriter writer = new StreamWriter("DataGame.txt"))
            {
                writer.Write($"{level}");
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            Rectangle background = new Rectangle(new Point(0, 0), new Point(1024, 768));
            _spriteBatch.Draw(_background, background, null, Color.White);

            

            //Verifica se e vitoria
            if (isWin)
            {
                Vector2 windowSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

                //Texture2D pixel = new Texture2D(GraphicsDevice, 1, 1);
                //pixel.SetData(new[] { Color.White });
                //_spriteBatch.Draw(pixel, new Rectangle(Point.Zero, windowSize.ToPoint()), new Color(Color.Gold, 0.1f));

                //Desenha mensagem de vitoria
                string win = $"You Win!!!";
                Vector2 winMeasures = _buttonFont.MeasureString(win) / 2f;
                Vector2 windowCenter = windowSize / 2f;
                Vector2 pos = windowCenter - winMeasures;
                _spriteBatch.DrawString(_buttonFont, win, pos, Color.Red);
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

                if (isPause)
                {
                    _spriteBatch.Draw(imgPlay, new Vector2(950f, 40f), Color.White);

                    Vector2 windowSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight / 2);
                    string pauseMsg = $"Paused";
                    Vector2 winMeasures = _buttonFont.MeasureString(pauseMsg) / 2f;
                    Vector2 windowCenter = windowSize / 2f;
                    Vector2 pos = windowCenter - winMeasures;
                    _spriteBatch.DrawString(_buttonFont, pauseMsg, pos, Color.Red);
                }
                else
                    _spriteBatch.Draw(imgPause, new Vector2(950f, 40f), Color.White);

                if (isSoundActive)
                    _spriteBatch.Draw(imgMute, new Vector2(885f, 40f), Color.White);
                else
                    _spriteBatch.Draw(imgUnmuted, new Vector2(885f, 40f), Color.White);

                if (lifeCount == 3)
                {
                    _spriteBatch.Draw(lifeImg, new Vector2(0f, 0f), new Rectangle(0, 0, 0, 0),Color.White, 0, new Vector2(0,0), new Vector2(0, 0), 0,3);
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

                _spriteBatch.Draw(coinImg, new Vector2(300, 40), Color.White);
                _spriteBatch.DrawString(_buttonFont, $"{nrCoins:F0}",new Vector2(350, 50f),Color.White);
                _spriteBatch.DrawString(_buttonFont, $"Time: {contMinLvlTime}:{lvlTime:F0}",new Vector2(600, 50f), Color.White);
                _scene.Draw(_spriteBatch, gameTime);
                if (!_npc.IsDead()) _npc.Draw(_spriteBatch, gameTime);
                _snowHouse.Draw(_spriteBatch, gameTime);
                //_snowBall.Draw(_spriteBatch, gameTime);
                _player.Draw(_spriteBatch, gameTime);
                //_enemy2.Draw(_spriteBatch, gameTime);
                if (!_coin.IsDead()) _coin.Draw(_spriteBatch, gameTime);
                if (!_coin2.IsDead()) _coin2.Draw(_spriteBatch, gameTime);
                if (!_coin3.IsDead()) _coin3.Draw(_spriteBatch, gameTime);
                if (!_gift.IsDead()) _gift.Draw(_spriteBatch, gameTime);
               
                
            }
            else if (activeMenu == false && activeCredits == true)
            {
                Vector2 windowSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight/2);
                string credits = $"Developed by:\nPaulo Macedo 16544\nBruno Carvalho 16614\nRui Cardoso 16624\nEDJD - 2020/2021";
                Vector2 winMeasures = _buttonFont.MeasureString(credits) / 2f;
                Vector2 windowCenter = windowSize / 2f;
                Vector2 pos = windowCenter - winMeasures;
                _spriteBatch.DrawString(_buttonFont, credits, pos, Color.White);
                MenuState._components["back"].Draw(gameTime, _spriteBatch);
            }
            else if(activeMenu == false && activeCredits == false && activeCommands)
            {
                Vector2 windowSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight / 2);
                string commands = $"Commands:\nMove - A and D\nShoot - Enter\nJump - Space\nRestart - R\nMute - V\nPause - P";
                Vector2 winMeasures = _buttonFont.MeasureString(commands) / 2f;
                Vector2 windowCenter = windowSize / 2f;
                Vector2 pos = windowCenter - winMeasures;
                _spriteBatch.DrawString(_buttonFont, commands, pos, Color.White);
                MenuState._components["back"].Draw(gameTime, _spriteBatch);
            }
            else
            {
                if(!isWin && !isLose)
                {
                    Rectangle background2 = new Rectangle(new Point(0, 0), new Point(1024, 768));
                    _spriteBatch.Draw(_background2, background, null, Color.White);
                    _currentState.Draw(gameTime, _spriteBatch);
                }
                
            }

            //Verifica se e derrota
            if (isLose)
            {
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
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
