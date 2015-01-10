using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;


namespace UltraBrawl
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {

        //menu variables
        Vector2[,] currentMenu;
        Vector2[,] startMenu;
        Vector2[,] charSelectMenu;
        Vector2[,] pauseMenu;
        Vector2[,] bgSelectMenu;
        Vector2[,] gameOverMenu;
        Vector2[,] previousMenu;
        Vector2[,] nowPlaying; // necessary, do not remove

        long gameOverTimer;
        bool gameOver;

        Camera2d cam;
        public int whoPaused;

        SpriteBatch spriteBatch;
        Boolean[] ready = { false, false, false, false };
        Boolean[] playing = { true, false, false, false };

        PlayerPreset[] presets = new PlayerPreset[4];

        PlayerCharacter[] players;
        Vector2[] spawnLocs = { new Vector2(200, 900), new Vector2(1500, 900), new Vector2(500, 1000), new Vector2(1200, 1000) };
        Texture2D background;

        List<AutomatedSprite> spriteList = new List<AutomatedSprite>();

        List<Sprite> platformList = new List<Sprite>();

        private Game game;

        FighterFactory factory;

        private SoundEffect menuMusic;
        private SoundEffectInstance menuMusicInstance;

        private Boolean guilePlaying;
        private Boolean onGuile;
        private int deadCount = 0;

        private SoundEffect inGameMusic;
        private SoundEffectInstance inGameMusicInstance;

        private Texture2D startButton;
        private Texture2D exitButton;
        private Texture2D resumeButton;
        private Texture2D mainmenuButton;
        private Texture2D title;
        private Texture2D[] healthBars;
        private Texture2D[] healthBgs;
        private Texture2D[] wins;
        private int winner = 0;
        private Texture2D p12hub;
        private Texture2D p34hub;
        private Texture2D blastSheet;
        private Texture2D kamehamehaSheet;
        private Texture2D[] cooldownBar;

        //player readies
        private Texture2D[] notReadyTexs;
        private Texture2D[] readyTexs;
        private Texture2D[] startTexs;

        private Boolean startGame;

        //character buttons
        private Texture2D gokuButton;
        private Texture2D gokuButtonSS;
        private Texture2D megamanButton;
        private Texture2D ryuButton;
        private Texture2D guileButton;
        private Texture2D venomButton;
        private Texture2D zeroButton;
        private Texture2D kazuyaButton;
        private Texture2D lyndisButton;
        private Texture2D jugoButton;
        private Texture2D kenpachiButton;

        private Texture2D bgButton1;
        private Texture2D bgButton2;
        private Texture2D bgButton3;
        private Texture2D bgButton4;
        private Texture2D bgButton5;

        private Texture2D charAllReady;



        private Texture2D[] selectedChars = new Texture2D[4];

        private Texture2D defaultCursor;
        private CursorLoc defaultCursorLoc;
        private Texture2D p1Cursor;
        private Texture2D p2Cursor;
        private Texture2D p3Cursor;
        private Texture2D p4Cursor;
        private Texture2D p1bgCursor;
        private Texture2D p2bgCursor;
        private Texture2D p3bgCursor;
        private Texture2D p4bgCursor;
        private CursorLoc[] cursorLocs;
        List<Texture2D> cursors = new List<Texture2D>();

        Vector2[] cursorPositions = new Vector2[4];

        PlayerIndex[] gamepads = new PlayerIndex[4];
        int numPlayers = 1;
        GamePadState[] previousGamePadState = new GamePadState[4];

        public GameState gameState;
        public enum GameState
        {
            StartMenu,
            CharSelect,
            Playing,
            Paused,
            BgSelect,
            GameOver
        }

        SpriteFont font;

        public SpriteManager(Game game)
            : base(game)
        {
            this.game = game;
            cam = new Camera2d();
            factory = new FighterFactory(game);
            gameState = GameState.StartMenu;
            gamepads[0] = PlayerIndex.One;
            gamepads[1] = PlayerIndex.Two;
            gamepads[2] = PlayerIndex.Three;
            gamepads[3] = PlayerIndex.Four;
            cursors.Add(p1Cursor);
            cursors.Add(p2Cursor);
            cursors.Add(p3Cursor);
            cursors.Add(p4Cursor);
            cursorLocs = new CursorLoc[4];
            defaultCursorLoc = new CursorLoc();
            for (int i = 0; i < 4; i++)
            {
                cursorLocs[i] = new CursorLoc();
            }
            guilePlaying = false;
            onGuile = false;
            startGame = false;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            for (int i = 0; i < 4; i++)
            {
                presets[i] = new PlayerPreset(gamepads[i], spawnLocs[i]);
            }
            font = Game.Content.Load<SpriteFont>("Images/dbzFont");

            players = new PlayerCharacter[4];
            startButton = Game.Content.Load<Texture2D>(@"Images/start");
            exitButton = Game.Content.Load<Texture2D>(@"Images/exit");
            resumeButton = Game.Content.Load<Texture2D>(@"Images/resume");
            mainmenuButton = Game.Content.Load<Texture2D>(@"Images/mainmenu");
            title = Game.Content.Load<Texture2D>(@"Images/title");

            readyTexs = new Texture2D[4];
            notReadyTexs = new Texture2D[4];
            startTexs = new Texture2D[4];
            healthBars = new Texture2D[4];
            healthBgs = new Texture2D[4];

            cooldownBar = new Texture2D[4];


            wins = new Texture2D[5];

            for (int i = 0; i < 4; i++)
            {
                readyTexs[i] = Game.Content.Load<Texture2D>(@"Images/" + (i + 1) + "R");
                notReadyTexs[i] = Game.Content.Load<Texture2D>(@"Images/" + (i + 1) + "NR");
                startTexs[i] = Game.Content.Load<Texture2D>(@"Images/" + (i + 1) + "S");
                wins[i] = Game.Content.Load<Texture2D>(@"Images/" + (i + 1) + "W");
                healthBars[i] = Game.Content.Load<Texture2D>(@"Images/healthBar");
                cooldownBar[i] = Game.Content.Load<Texture2D>(@"Images/healthBar");
            }
            wins[4] = Game.Content.Load<Texture2D>(@"Images/draw");
            healthBgs[0] = Game.Content.Load<Texture2D>(@"Images/p1healthBg");
            healthBgs[1] = Game.Content.Load<Texture2D>(@"Images/p2healthBg");
            healthBgs[2] = Game.Content.Load<Texture2D>(@"Images/p3healthBg");
            healthBgs[3] = Game.Content.Load<Texture2D>(@"Images/p4healthBg");
            p12hub = Game.Content.Load<Texture2D>(@"Images/p12hub");
            p34hub = Game.Content.Load<Texture2D>(@"Images/p34hub");

            menuMusic = game.Content.Load<SoundEffect>("Sound/Carpenter Brut - Roller Mobster Menu Loop");

            menuMusicInstance = menuMusic.CreateInstance();
            menuMusicInstance.IsLooped = true;

            gokuButton = Game.Content.Load<Texture2D>(@"Images/gokuButton");
            gokuButtonSS = Game.Content.Load<Texture2D>(@"Images/gokuButtonSS");
            megamanButton = Game.Content.Load<Texture2D>(@"Images/megamanButton");
            ryuButton = Game.Content.Load<Texture2D>(@"Images/ryuButton");
            guileButton = Game.Content.Load<Texture2D>(@"Images/guileButton");
            venomButton = Game.Content.Load<Texture2D>(@"Images/venomButton");
            zeroButton = Game.Content.Load<Texture2D>(@"Images/zeroButton");
            kazuyaButton = Game.Content.Load<Texture2D>(@"Images/kazuyaButton");
            lyndisButton = Game.Content.Load<Texture2D>(@"Images/lyndisButton");
            jugoButton = Game.Content.Load<Texture2D>(@"Images/jugoButton");
            kenpachiButton = Game.Content.Load<Texture2D>(@"Images/kenpachiButton");

            charAllReady = Game.Content.Load<Texture2D>(@"Images/charAllReady");
            
            bgButton1 = Game.Content.Load<Texture2D>(@"Images/bgButton1");
            bgButton2 = Game.Content.Load<Texture2D>(@"Images/bgButton2");
            bgButton3 = Game.Content.Load<Texture2D>(@"Images/bgButton3");
            bgButton4 = Game.Content.Load<Texture2D>(@"Images/bgButton4");
            bgButton5 = Game.Content.Load<Texture2D>(@"Images/bgButton5");

            defaultCursor = Game.Content.Load<Texture2D>(@"Images/DefaultCursor");
            p1Cursor = Game.Content.Load<Texture2D>(@"Images/p1Cursor");
            p2Cursor = Game.Content.Load<Texture2D>(@"Images/p2Cursor");
            p3Cursor = Game.Content.Load<Texture2D>(@"Images/p3Cursor");
            p4Cursor = Game.Content.Load<Texture2D>(@"Images/p4Cursor");


            p1bgCursor = Game.Content.Load<Texture2D>(@"Images/bgcursor1");
            p2bgCursor = Game.Content.Load<Texture2D>(@"Images/bgcursor2");
            p3bgCursor = Game.Content.Load<Texture2D>(@"Images/bgcursor3");
            p4bgCursor = Game.Content.Load<Texture2D>(@"Images/bgcursor4");


            startMenu = new Vector2[1, 2];
            startMenu[0, 0] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 180, 500);
            startMenu[0, 1] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 180, 600);

            charSelectMenu = new Vector2[5, 2];
            charSelectMenu[0, 0] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 425, 600);
            charSelectMenu[0, 1] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 425, 800);
            charSelectMenu[1, 0] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 175, 600);
            charSelectMenu[1, 1] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 175, 800);
            charSelectMenu[2, 0] = new Vector2((GraphicsDevice.Viewport.Width / 2) + 75, 600);
            charSelectMenu[2, 1] = new Vector2((GraphicsDevice.Viewport.Width / 2) + 75, 800);
            charSelectMenu[3, 0] = new Vector2((GraphicsDevice.Viewport.Width / 2) + 325, 600);
            charSelectMenu[3, 1] = new Vector2((GraphicsDevice.Viewport.Width / 2) + 325, 800);
            charSelectMenu[4, 0] = new Vector2((GraphicsDevice.Viewport.Width / 2) + 575, 600);
            charSelectMenu[4, 1] = new Vector2((GraphicsDevice.Viewport.Width / 2) + 575, 800);

            pauseMenu = new Vector2[1, 3];
            pauseMenu[0, 0] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 180, 200);
            pauseMenu[0, 1] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 180, 400);
            pauseMenu[0, 2] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 180, 600);

            bgSelectMenu = new Vector2[1, 5];
            bgSelectMenu[0, 0] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 75, 100);
            bgSelectMenu[0, 1] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 75, 300);
            bgSelectMenu[0, 2] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 75, 500);
            bgSelectMenu[0, 3] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 75, 700);
            bgSelectMenu[0, 4] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 75, 900);

            gameOverMenu = new Vector2[1, 1];
            gameOverMenu[0, 0] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 180, 500);

            menuMusicInstance.Play();
            switchMenu(startMenu);

            loadLevel();
        }

        protected void loadLevel()
        {

            background = Game.Content.Load<Texture2D>(@"Images/background");
            base.LoadContent();
        }



        protected void spawnCharacters()
        {
            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(Game.Content.Load<Texture2D>("Images/circle"));
            textures.Add(Game.Content.Load<Texture2D>("Images/star"));
            textures.Add(Game.Content.Load<Texture2D>("Images/diamond"));
            for (int i = 1; i < 4; i++)
            {
                if (playing[i])
                {
                    numPlayers = i + 1;
                }
            }
            for (int i = 0; i < numPlayers; i++)
            {
                if (ready[i])
                {
                    //its guile!
                    if (players[i].CHARACTER_ID == 3)
                    {
                        inGameMusic = game.Content.Load<SoundEffect>("Sound/Guile Theme");
                    }
                    players[i].spawn(presets[i]);
                }
            }
        }



        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {

            if (gameState == GameState.StartMenu || gameState == GameState.Paused || gameState == GameState.CharSelect || gameState == GameState.BgSelect || gameState == GameState.GameOver)
            {
                navigateMenu();
            }
            if (gameState == GameState.GameOver)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (playing[i])
                    {
                        if (players[i].currentHealth > 0)
                        {
                            winner = i;
                        }
                    }
                }
            }

            if (gameState == GameState.CharSelect)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (GamePad.GetState(gamepads[i]).Buttons.Start == ButtonState.Pressed && !playing[i])
                    {
                        playing[i] = true;
                        cursorPositions[i] = charSelectMenu[0, 0];
                    }
                    if (GamePad.GetState(gamepads[i]).Buttons.Back == ButtonState.Pressed)
                    {
                        whoPaused = i;
                        previousMenu = charSelectMenu;
                        switchMenu(pauseMenu);
                        gameState = GameState.Paused;
                    }
                }
                for (int i = 0; i < 4; i++)
                {
                    if (playing[i] && !ready[i])
                    {
                        startGame = false;
                    }
                }
                if (startGame)
                {
                    switchMenu(bgSelectMenu);
                    gameState = GameState.BgSelect;
                }
            }
            else if (gameState == GameState.Playing || gameState == GameState.GameOver)
            {

                for (int i = 0; i < numPlayers; i++)
                {
                    if (ready[i])
                    {
                        players[i].Update(gameTime, Game.Window.ClientBounds);

                        if (players[i].isFire)
                        {
                            Debug.WriteLine(players[i].flipped);
                            if (players[i].CHARACTER_ID == 0)
                                spriteList.Add(new GokuKamehameha(kamehamehaSheet, new Vector2(players[i].hitbox.Center.X, players[i].hitbox.Center.Y), players[i].flipped));
                            if (players[i].CHARACTER_ID == 1)
                                spriteList.Add(new MegamanBuster(blastSheet, new Vector2(players[i].hitbox.Center.X, players[i].hitbox.Center.Y), players[i].flipped));
                            if (players[i].CHARACTER_ID == 2)
                                spriteList.Add(new RyuHadouken(blastSheet, new Vector2(players[i].hitbox.Center.X, players[i].hitbox.Center.Y), players[i].flipped));
                            if (players[i].CHARACTER_ID == 3)
                                spriteList.Add(new GuileSonicBoom(blastSheet, new Vector2(players[i].hitbox.Center.X, players[i].hitbox.Center.Y), players[i].flipped));
                            if (players[i].CHARACTER_ID == 7)
                                spriteList.Add(new LyndisArrow(blastSheet, new Vector2(players[i].hitbox.Center.X, players[i].hitbox.Center.Y), players[i].flipped));
                   
                            spriteList.ElementAt(spriteList.Count - 1).myOwner = players[i];
                            players[i].isFire = false;
                        }
                        if (players[i].currentHealth <= 0)
                        {
                            deadCount++;
                        }
                    }
                    else
                    {
                        deadCount++;
                    }
                }
                if (deadCount == numPlayers - 1 && numPlayers > 1 && gameOver == false)
                {
                    gameOver = true;
                    gameOverTimer = System.Environment.TickCount + 2500;
                    switchMenu(gameOverMenu);
                    gameState = GameState.GameOver;
                }
                deadCount = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (ready[i])
                    {
                        if (GamePad.GetState(gamepads[i]).Buttons.Start == ButtonState.Pressed && previousGamePadState[i].Buttons.Start == ButtonState.Released)
                        {
                            whoPaused = i;
                            previousMenu = nowPlaying;
                            for (int j = 0; j < numPlayers; j++)
                            {
                                if (ready[j])
                                {
                                    players[j].pauseChar();
                                }
                            }
                            switchMenu(pauseMenu);
                            gameState = GameState.Paused;
                        }
                        previousGamePadState[i] = GamePad.GetState(gamepads[i]);
                    }
                }

                for (int i = 0; i < spriteList.Count; i++)
                {
                    if (spriteList.ElementAt(i).myOwner.disableProjectile)
                    {
                        spriteList.ElementAt(i).setDisable();
                    }
                    for (int j = 0; j < numPlayers; j++)
                    {
                        if (ready[j])
                        {
                            if (spriteList.ElementAt(i).collisionRect.Intersects(players[j].collisionRect))
                            {
                                spriteList.ElementAt(i).Collision(players[j]);
                            }
                        }
                    }
                    if (spriteList.ElementAt(i).disable)
                    {
                        spriteList.Remove(spriteList.ElementAt(i));
                    }
                    else
                    {
                        spriteList.ElementAt(i).Update(gameTime, Game.Window.ClientBounds);
                    }
                }
                for (int i = 0; i < numPlayers; i++)
                {
                    for (int j = 0; j < numPlayers; j++)
                    {
                        if (ready[i] && ready[j])
                        {
                            if (i != j)
                            {
                                if (players[i].newHitbox.Intersects(players[j].collisionRect))
                                {
                                    players[i].Collision(players[j]);
                                }
                                //cannot run through while blocking
                                if (players[i].collisionRect.Intersects(players[j].newHitbox) && players[j].isBlock && players[i].flipped != players[j].flipped)
                                {
                                    if (players[j].flipped && players[i].collisionRect.Right > players[j].newHitbox.Right + 40)
                                    {
                                        players[i].position.X -= (players[i].speed.X / 12);
                                    }
                                    else if (!players[j].flipped && players[i].collisionRect.Left < players[j].newHitbox.Left - 40)
                                    {
                                        players[i].position.X += (players[i].speed.X / 12);
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (Sprite sprite in platformList)
                {
                    for (int i = 0; i < numPlayers; i++)
                    {
                        if (ready[i])
                        {
                            if (sprite.collisionRect.Intersects(players[i].collisionRect))
                            {
                                players[i].Collision(sprite);
                            }
                        }
                    }
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            cam.Pos = new Vector2(960.0f, 540.0f);
            //spriteBatch.Begin();
            spriteBatch.Begin(SpriteSortMode.Deferred,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        cam.get_transformation(GraphicsDevice));
            spriteBatch.Draw(background, new Rectangle(0, 0, 1920, 1080), Color.White);
            if (gameState == GameState.StartMenu)
            {

                game.IsMouseVisible = false;
                
                spriteBatch.Draw(title, new Vector2((GraphicsDevice.Viewport.Width / 2) - 360, 150), Color.White);
                spriteBatch.Draw(startButton, startMenu[0, 0], Color.White);
                spriteBatch.Draw(exitButton, startMenu[0, 1], Color.White);
                spriteBatch.Draw(defaultCursor, cursorPositions[0], Color.White);

            }
            if (gameState == GameState.CharSelect)
            {
                int readyCount = 0;
                int playingCount = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (ready[i])
                    {
                        spriteBatch.Draw(readyTexs[i], new Vector2((i % 2 == 0 ? 0 : 1560), (i < 2 ? 20 : 300)), Color.White);
                        spriteBatch.Draw(selectedChars[i], new Vector2((i % 2 == 0 ? 10 : 1760), (i < 2 ? 110 : 390)), Color.White);
                        readyCount++;
                        playingCount++;
                    }
                    else if (playing[i])
                    {
                        playingCount++;
                        readyCount--;
                        spriteBatch.Draw(notReadyTexs[i], new Vector2((i % 2 == 0 ? 0 : 1560), (i < 2 ? 20 : 300)), Color.White);
                    }
                    else
                    {
                        readyCount--;
                        playingCount--;
                        spriteBatch.Draw(startTexs[i], new Vector2((i % 2 == 0 ? 0 : 1560), (i < 2 ? 20 : 300)), Color.White);
                    }
                }
                if (readyCount == playingCount)
                {
                    spriteBatch.Draw(charAllReady, new Vector2((GraphicsDevice.Viewport.Width / 2) - 250, 150), Color.White);
                }
                game.IsMouseVisible = false;
                spriteBatch.Draw(gokuButton, charSelectMenu[0, 0], Color.White);
                spriteBatch.Draw(megamanButton, charSelectMenu[0, 1], Color.White);
                spriteBatch.Draw(venomButton, charSelectMenu[1, 0], Color.White);
                spriteBatch.Draw(zeroButton, charSelectMenu[1, 1], Color.White);
                spriteBatch.Draw(ryuButton, charSelectMenu[2, 0], Color.White);
                spriteBatch.Draw(guileButton, charSelectMenu[2, 1], Color.White);
                spriteBatch.Draw(kazuyaButton, charSelectMenu[3, 0], Color.White);
                spriteBatch.Draw(lyndisButton, charSelectMenu[3, 1], Color.White);
                spriteBatch.Draw(jugoButton, charSelectMenu[4, 0], Color.White);
                spriteBatch.Draw(kenpachiButton, charSelectMenu[4, 1], Color.White);

                spriteBatch.Draw(p1Cursor, cursorPositions[0], Color.White);
                if (playing[1])
                {
                    spriteBatch.Draw(p2Cursor, cursorPositions[1], Color.White);
                }
                if (playing[2])
                {
                    spriteBatch.Draw(p3Cursor, cursorPositions[2], Color.White);
                }
                if (playing[3])
                {
                    spriteBatch.Draw(p4Cursor, cursorPositions[3], Color.White);
                }
                
            }
            if (gameState == GameState.BgSelect)
            {
                game.IsMouseVisible = false;
                spriteBatch.Draw(bgButton1, bgSelectMenu[0, 0], Color.White);
                spriteBatch.Draw(bgButton2, bgSelectMenu[0, 1], Color.White);
                spriteBatch.Draw(bgButton3, bgSelectMenu[0, 2], Color.White);
                spriteBatch.Draw(bgButton4, bgSelectMenu[0, 3], Color.White);
                spriteBatch.Draw(bgButton5, bgSelectMenu[0, 4], Color.White);
                if (winner == 0)
                {
                    if (!playing[0])
                        winner = 0;
                    spriteBatch.Draw(p1bgCursor, cursorPositions[0], Color.White);
                }
                if (winner == 1)
                {
                    if (!playing[1])
                        winner = 1;
                    spriteBatch.Draw(p2bgCursor, cursorPositions[1], Color.White);
                }
                if (winner == 2)
                {
                    if (!playing[2])
                        winner = 2;
                    spriteBatch.Draw(p3bgCursor, cursorPositions[2], Color.White);
                }
                if (winner == 3)
                {
                    if (!playing[3])
                        winner = 3;
                    spriteBatch.Draw(p4bgCursor, cursorPositions[3], Color.White);
                }
                if (winner == 4)
                {
                    resetGame();
                }
                
            }
            if (gameState == GameState.Playing || (gameState == GameState.Paused && previousMenu != charSelectMenu) || gameState == GameState.GameOver)
            {
                game.IsMouseVisible = false;
                LoadGame(gameTime);
            }
            if (gameState == GameState.Paused)
            {
                game.IsMouseVisible = false;
                spriteBatch.Draw(resumeButton, pauseMenu[0, 0], Color.White);
                spriteBatch.Draw(mainmenuButton, pauseMenu[0, 1], Color.White);
                spriteBatch.Draw(exitButton, pauseMenu[0, 2], Color.White);
                spriteBatch.Draw(defaultCursor, cursorPositions[0], Color.White);
            }

            if (gameState == GameState.GameOver)
            {

                game.IsMouseVisible = false;

                spriteBatch.Draw(wins[winner], new Vector2((GraphicsDevice.Viewport.Width / 2) - 250, 150), Color.White);
                spriteBatch.Draw(mainmenuButton, startMenu[0, 0], Color.White);
                spriteBatch.Draw(defaultCursor, cursorPositions[0], Color.White);

            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void LoadGame(GameTime gameTime)
        {
            spriteBatch.Draw(p12hub, new Rectangle(625, 0, 680, 150), Color.White);
            if (playing[2] || playing[3])
            {
                spriteBatch.Draw(p34hub, new Rectangle(625, 150, 680, 57), Color.White);
            }
            if (playing[0])
            {
                spriteBatch.Draw(healthBgs[0], new Rectangle(65, 17, 560, 65), Color.White);
                spriteBatch.Draw(healthBars[0], new Rectangle(120, 10, players[0].visibleHealth * 5, 78), Color.DarkRed);
                spriteBatch.Draw(healthBars[0], new Rectangle(120, 10, players[0].currentHealth * 5, 78), Color.LightGray);
                if (players[0].canAOE || players[0].canSmash || players[0].canBeam)
                {
                    spriteBatch.Draw(cooldownBar[0], new Rectangle(120, 65, (int)(players[0].spamTimer - players[0].pauseTime) / 10, 20), Color.Red);
                }
                spriteBatch.DrawString(font, players[0].currentHealth + "%", new Vector2(350, 36), Color.Red);
                spriteBatch.DrawString(font, "p1", new Vector2(700, 35), Color.Red);
                if (players[0].isSuper)
                {
                    spriteBatch.Draw(gokuButtonSS, new Rectangle(630, 17, 65, 65), Color.White);
                }
                else
                {
                    spriteBatch.Draw(selectedChars[0], new Rectangle(630, 17, 65, 65), Color.White);
                }
                spriteBatch.DrawString(font, "p1", new Vector2(players[0].collisionRect.Center.X, players[0].hitbox.Top - 60), Color.Red);
            }
            if (playing[1])
            {
                spriteBatch.Draw(healthBgs[1], new Rectangle(1305, 17, 560, 65), Color.White);
                spriteBatch.Draw(healthBars[1], new Rectangle(1810 - players[1].visibleHealth * 5, 10, players[1].visibleHealth * 5, 78), Color.DarkRed);
                spriteBatch.Draw(healthBars[1], new Rectangle(1810 - players[1].currentHealth * 5, 10, players[1].currentHealth * 5, 78), Color.LightGray);
                if (players[1].canAOE || players[1].canSmash || players[1].canBeam)
                {
                    spriteBatch.Draw(cooldownBar[1], new Rectangle(1810 - (int)(players[1].spamTimer - players[1].pauseTime) / 10, 65, (int)(players[1].spamTimer - players[1].pauseTime) / 10, 20), Color.Blue);
                }
                spriteBatch.DrawString(font, players[1].currentHealth + "%" + " ", new Vector2(1530, 36), Color.Blue);
                spriteBatch.DrawString(font, "p2", new Vector2(1200, 35), Color.Blue);
                if (players[1].isSuper)
                {
                    spriteBatch.Draw(gokuButtonSS, new Rectangle(1235, 17, 65, 65), Color.White);
                }
                else
                {
                    spriteBatch.Draw(selectedChars[1], new Rectangle(1235, 17, 65, 65), Color.White);
                }
                spriteBatch.DrawString(font, "p2", new Vector2(players[1].collisionRect.Center.X, players[1].hitbox.Top - 60), Color.Blue);
            }
            if (playing[2] )
            {
                spriteBatch.Draw(healthBgs[2], new Rectangle(65, 82, 560, 65), Color.White);
                spriteBatch.Draw(healthBars[2], new Rectangle(120, 75, players[2].visibleHealth * 5, 78), Color.DarkRed);
                spriteBatch.Draw(healthBars[2], new Rectangle(120, 75, players[2].currentHealth * 5, 78), Color.LightGray);
                if (players[2].canAOE || players[2].canSmash || players[2].canBeam)
                {
                    spriteBatch.Draw(cooldownBar[2], new Rectangle(120, 130, (int)(players[2].spamTimer - players[2].pauseTime) / 10, 20), Color.Green);
                }
                spriteBatch.DrawString(font, players[2].currentHealth + "%", new Vector2(350, 101), Color.Green);
                spriteBatch.DrawString(font, "p3", new Vector2(700, 100), Color.Green);
                if (players[2].isSuper)
                {
                    spriteBatch.Draw(gokuButtonSS, new Rectangle(630, 82, 65, 65), Color.White);
                }
                else
                {
                    spriteBatch.Draw(selectedChars[2], new Rectangle(630, 82, 65, 65), Color.White);
                }
                spriteBatch.DrawString(font, "p3", new Vector2(players[2].collisionRect.Center.X, players[2].hitbox.Top - 60), Color.Green);
            }
            if (playing[3])
            {
                spriteBatch.Draw(healthBgs[3], new Rectangle(1305, 82, 560, 65), Color.White);
                spriteBatch.Draw(healthBars[3], new Rectangle(1810 - players[3].visibleHealth * 5, 75, players[1].visibleHealth * 5, 78), Color.DarkRed);
                spriteBatch.Draw(healthBars[3], new Rectangle(1810 - players[3].currentHealth * 5, 75, players[1].currentHealth * 5, 78), Color.LightGray);
                if (players[3].canAOE || players[3].canSmash || players[3].canBeam)
                {
                    spriteBatch.Draw(cooldownBar[3], new Rectangle(1810 - (int)(players[3].spamTimer - players[3].pauseTime) / 10, 130, (int)(players[3].spamTimer - players[3].pauseTime) / 10, 20), Color.Orange);
                }
                spriteBatch.DrawString(font, players[3].currentHealth + "%", new Vector2(1530, 101), Color.Orange);
                spriteBatch.DrawString(font, "p4", new Vector2(1200, 100), Color.Orange);
                if (players[3].isSuper)
                {
                    spriteBatch.Draw(gokuButtonSS, new Rectangle(1235, 82, 65, 65), Color.White);
                }
                else 
                {
                    spriteBatch.Draw(selectedChars[3], new Rectangle(1235, 82, 65, 65), Color.White);
                }
                spriteBatch.DrawString(font, "p4", new Vector2(players[3].collisionRect.Center.X, players[3].hitbox.Top - 60), Color.Orange);
            }
            for (int i = 0; i < numPlayers; i++)
            {
                if (ready[i])
                {
                    players[i].Draw(gameTime, spriteBatch);
                }

            }

            foreach (Sprite sprite in spriteList)
                sprite.Draw(gameTime, spriteBatch);
            foreach (Sprite sprite in platformList)
                sprite.Draw(gameTime, spriteBatch);
        }
        void switchMenu(Vector2[,] menu)
        {
            currentMenu = menu;
            
            for (int i = 0; i < 4; i++)
            {
                cursorPositions[i] = currentMenu[0, 0];
                cursorLocs[i].resetLoc();
            }
        }
        void navigateMenu()
        {
            int currentPlayer;
            //gamepad controls
            for (int i = 0; i < 4; i++)
            {
                currentPlayer = i;
                if ((playing[i] && currentMenu == charSelectMenu) || currentMenu == startMenu || currentMenu == bgSelectMenu || currentMenu == gameOverMenu || (currentMenu == bgSelectMenu && i == winner) || (currentMenu == pauseMenu && currentPlayer == whoPaused))
                {
                    if (currentMenu != charSelectMenu && currentMenu != bgSelectMenu)
                    {
                        currentPlayer = 0;
                    }
                    if (cursorLocs[currentPlayer].currentItemY + 1 < currentMenu.GetLength(1))
                    {
                        if (GamePad.GetState(gamepads[i]).DPad.Down == ButtonState.Pressed && previousGamePadState[i].DPad.Down == ButtonState.Released)
                        {
                            cursorLocs[currentPlayer].currentItemY++;
                        }
                        else if (GamePad.GetState(gamepads[i]).ThumbSticks.Left.Y < -.5f && previousGamePadState[i].ThumbSticks.Left.Y >= -.49f)
                        {
                            cursorLocs[currentPlayer].currentItemY++;
                        }
                    }
                    if (cursorLocs[currentPlayer].currentItemX - 1 >= 0)
                    {
                        if (GamePad.GetState(gamepads[i]).DPad.Left == ButtonState.Pressed && previousGamePadState[i].DPad.Left == ButtonState.Released)
                        {
                            cursorLocs[currentPlayer].currentItemX--;
                        }
                        else if (GamePad.GetState(gamepads[i]).ThumbSticks.Left.X < -.5f && previousGamePadState[i].ThumbSticks.Left.X >= -.49f)
                        {
                            cursorLocs[currentPlayer].currentItemX--;
                        }
                    }

                    if (cursorLocs[currentPlayer].currentItemX + 1 < currentMenu.GetLength(0))
                    {
                        if (GamePad.GetState(gamepads[i]).DPad.Right == ButtonState.Pressed && previousGamePadState[i].DPad.Right == ButtonState.Released)
                        {
                            cursorLocs[currentPlayer].currentItemX++;
                        }
                        else if (GamePad.GetState(gamepads[i]).ThumbSticks.Left.X > .5f && previousGamePadState[i].ThumbSticks.Left.X <= .49f)
                        {
                            cursorLocs[currentPlayer].currentItemX++;
                        }
                    }
                    if (cursorLocs[currentPlayer].currentItemY - 1 >= 0)
                    {
                        if (GamePad.GetState(gamepads[i]).DPad.Up == ButtonState.Pressed && previousGamePadState[i].DPad.Up == ButtonState.Released)
                        {
                            cursorLocs[currentPlayer].currentItemY--;
                        }
                        else if (GamePad.GetState(gamepads[i]).ThumbSticks.Left.Y > .5f && previousGamePadState[i].ThumbSticks.Left.Y <= .49f)
                        {
                            cursorLocs[currentPlayer].currentItemY--;
                        }
                    }
                    if (GamePad.GetState(gamepads[i]).Buttons.A == ButtonState.Released && previousGamePadState[i].Buttons.A == ButtonState.Pressed)
                    {
                        menuSelect(currentPlayer, cursorPositions[currentPlayer]);
                    }
                    if (currentMenu == bgSelectMenu)
                    {
                        if (GamePad.GetState(gamepads[currentPlayer]).Buttons.B == ButtonState.Released && previousGamePadState[i].Buttons.B == ButtonState.Pressed)
                        {
                            background = Game.Content.Load<Texture2D>(@"Images/background");
                            startGame = false;
                            gameState = GameState.CharSelect;
                            switchMenu(charSelectMenu);
                        }
                    }
                    if (currentMenu == charSelectMenu)
                    {
                        if (GamePad.GetState(gamepads[currentPlayer]).Buttons.Start == ButtonState.Pressed && previousGamePadState[i].Buttons.Start == ButtonState.Released)
                        {
                            startGame = true;
                        }
                        if (GamePad.GetState(gamepads[currentPlayer]).Buttons.B == ButtonState.Pressed && previousGamePadState[i].Buttons.B == ButtonState.Released)
                        {
                            if (ready[currentPlayer])
                                ready[currentPlayer] = false;
                            else if (playing[currentPlayer])
                                playing[currentPlayer] = false;
                            else
                            {
                                resetGame();
                            }
                        }
                        if (onGuile && !guilePlaying)
                        {
                            guilePlaying = true;
                            menuMusicInstance.Pause();
                            inGameMusic = game.Content.Load<SoundEffect>("Sound/Guile Theme");
                            inGameMusicInstance = inGameMusic.CreateInstance();
                            inGameMusicInstance.IsLooped = true;
                            inGameMusicInstance.Play();
                        }
                        onGuile = false;
                        for (int j = 0; j < 4; j++)
                        {
                            if (cursorLocs[j].currentItemX == 2 && cursorLocs[j].currentItemY == 1)
                            {
                                onGuile = true;
                            }
                        }
                        if (!onGuile && guilePlaying)
                        {
                            guilePlaying = false;
                            inGameMusicInstance.Stop();
                            menuMusicInstance.Resume();
                        }
                    }
                    if (currentMenu == bgSelectMenu)
                    {
                        if (cursorLocs[winner].currentItemX == 0)
                        {
                            if (cursorLocs[winner].currentItemY == 0)
                            {
                                background = Game.Content.Load<Texture2D>(@"Images/background1");
                            }
                            else if (cursorLocs[winner].currentItemY == 1)
                            {
                                background = Game.Content.Load<Texture2D>(@"Images/background2");
                            }
                            else if (cursorLocs[winner].currentItemY == 2)
                            {
                                background = Game.Content.Load<Texture2D>(@"Images/background3");
                            }
                            else if (cursorLocs[winner].currentItemY == 3)
                            {
                                background = Game.Content.Load<Texture2D>(@"Images/background4");
                            }
                            else if (cursorLocs[winner].currentItemY == 4)
                            {
                                background = Game.Content.Load<Texture2D>(@"Images/background5");
                            }
                        }
                        if (cursorLocs[0].currentItemX == 1)
                        {

                        }

                    }
                    if (currentMenu == pauseMenu)
                    {
                        if (GamePad.GetState(gamepads[i]).Buttons.Start == ButtonState.Pressed && previousGamePadState[i].Buttons.Start == ButtonState.Released)
                        {
                            if (previousMenu != charSelectMenu)
                            {
                                for (int j = 0; j < numPlayers; j++)
                                {
                                    if (ready[j])
                                    {
                                        players[j].resumeChar();
                                    }
                                }
                                menuMusicInstance.Stop();
                                inGameMusicInstance.Play();
                                gameState = GameState.Playing;
                            }
                            else
                            {
                                switchMenu(charSelectMenu);
                                gameState = GameState.CharSelect;
                            }
                        }
                    }
                    cursorPositions[currentPlayer] = currentMenu[cursorLocs[currentPlayer].currentItemX, cursorLocs[currentPlayer].currentItemY];
                    previousGamePadState[i] = GamePad.GetState(gamepads[i]);
                }
            }
        }

        private void menuSelect(int playerNum, Vector2 vector2)
        {
            if (gameState == GameState.CharSelect)
            {
                if (playing[playerNum])
                {
                    if (cursorLocs[playerNum].currentItemX == 0)
                    {
                        if (cursorLocs[playerNum].currentItemY == 0)
                        {
                            players[playerNum] = factory.selectCharacter(0);
                            selectedChars[playerNum] = gokuButton;
                            ready[playerNum] = true;
                        }
                        else if (cursorLocs[playerNum].currentItemY == 1)
                        {
                            players[playerNum] = factory.selectCharacter(1);
                            selectedChars[playerNum] = megamanButton;
                            ready[playerNum] = true;
                        }

                    }

                    if (cursorLocs[playerNum].currentItemX == 1)
                    {
                        if (cursorLocs[playerNum].currentItemY == 0)
                        {
                            players[playerNum] = factory.selectCharacter(4);
                            selectedChars[playerNum] = venomButton;
                            ready[playerNum] = true;
                        }
                        else if (cursorLocs[playerNum].currentItemY == 1)
                        {
                            players[playerNum] = factory.selectCharacter(5);
                            selectedChars[playerNum] = zeroButton;
                            ready[playerNum] = true;
                        }

                    }
                    if (cursorLocs[playerNum].currentItemX == 2)
                    {
                        if (cursorLocs[playerNum].currentItemY == 0)
                        {
                            players[playerNum] = factory.selectCharacter(2);
                            selectedChars[playerNum] = ryuButton;
                            ready[playerNum] = true;
                        }
                        else if (cursorLocs[playerNum].currentItemY == 1)
                        {
                            players[playerNum] = factory.selectCharacter(3);
                            selectedChars[playerNum] = guileButton;
                            ready[playerNum] = true;
                        }
                    } 
                    if (cursorLocs[playerNum].currentItemX == 3)
                    {
                        if (cursorLocs[playerNum].currentItemY == 0)
                        {
                            players[playerNum] = factory.selectCharacter(6);
                            selectedChars[playerNum] = kazuyaButton;
                            ready[playerNum] = true;
                        }
                        else if (cursorLocs[playerNum].currentItemY == 1)
                        {
                            players[playerNum] = factory.selectCharacter(7);
                            selectedChars[playerNum] = lyndisButton;
                            ready[playerNum] = true;
                        }
                    }
                    if (cursorLocs[playerNum].currentItemX == 4)
                    {
                        if (cursorLocs[playerNum].currentItemY == 0)
                        {
                            players[playerNum] = factory.selectCharacter(8);
                            selectedChars[playerNum] = jugoButton;
                            ready[playerNum] = true;
                        }
                        else if (cursorLocs[playerNum].currentItemY == 1)
                        {
                            players[playerNum] = factory.selectCharacter(9);
                            selectedChars[playerNum] = kenpachiButton;
                            ready[playerNum] = true;
                        }
                    }
                }

            }
            else if (gameState == GameState.BgSelect)
            {
                if (playerNum == winner)
                {
                    if (cursorLocs[winner].currentItemY == 0)
                    {
                        inGameMusic = game.Content.Load<SoundEffect>("Sound/Carpenter Brut - Le Perv Loop");
                        platformList = new List<Sprite>();
                        platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(150, 650)));
                        platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(250, 875)));
                        platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(1450, 650)));
                        platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(1500, 875)));
                    }
                    else if (cursorLocs[winner].currentItemY == 1)
                    {
                        inGameMusic = game.Content.Load<SoundEffect>("Sound/Kirk Gadget & Valkyrie 1984 - Ghosts Loop");
                        platformList = new List<Sprite>();
                        platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(500, 650)));
                        platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(200, 875)));
                        platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(1100, 650)));
                        platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(1400, 875)));
                        platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(800, 875)));
                    }
                    else if (cursorLocs[winner].currentItemY == 2)
                    {
                        inGameMusic = game.Content.Load<SoundEffect>("Sound/LazerHawk - King of The Streets Loop");
                        platformList = new List<Sprite>();
                        platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(400, 875)));
                        platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(1200, 875)));
                        platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(800, 650)));
                    }
                    else if (cursorLocs[winner].currentItemY == 3)
                    {
                        inGameMusic = game.Content.Load<SoundEffect>("Sound/SelloRekT LA Dreams - Feel The Burn Loop");
                        platformList = new List<Sprite>();
                        platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(400, 875)));
                        platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(1200, 875)));
                    }
                    else if (cursorLocs[winner].currentItemY == 4)
                    {
                        inGameMusic = game.Content.Load<SoundEffect>("Sound/SellorektLA Dreams - LightSpeed Loop");
                        platformList = new List<Sprite>();
                        platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(150, 650)));
                        platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(250, 875)));
                        platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(1450, 650)));
                        platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(1500, 875)));
                    }
                    blastSheet = Game.Content.Load<Texture2D>(@"Images/blasts");
                    kamehamehaSheet = Game.Content.Load<Texture2D>(@"Images/GokuKamehameha");
                    spawnCharacters();
                    //music under spawn characters as spawnCharacters checks if guile is there.
                    if (guilePlaying)
                    {
                        guilePlaying = false;
                        inGameMusicInstance.Stop();
                        menuMusicInstance.Resume();
                    }
                    inGameMusicInstance = inGameMusic.CreateInstance();
                    inGameMusicInstance.IsLooped = true;
                    menuMusicInstance.Stop();
                    inGameMusicInstance.Play();
                    gameState = GameState.Playing;
                }

            }
            else if (gameState == GameState.StartMenu)
            {
                if (cursorLocs[playerNum].currentItemY == 0)
                {
                    switchMenu(charSelectMenu);
                    gameState = GameState.CharSelect;
                }
                else if (cursorLocs[playerNum].currentItemY == 1)
                {
                    game.Exit();
                }
            }
            else if (gameState == GameState.Paused)
            {
                if (cursorLocs[playerNum].currentItemY == 0)
                {
                    if (previousMenu != charSelectMenu)
                    {
                        for (int j = 0; j < numPlayers; j++)
                        {
                            if (ready[j])
                            {
                                players[j].resumeChar();
                            }
                        }
                        menuMusicInstance.Stop();
                        inGameMusicInstance.Play();

                        gameState = GameState.Playing;
                    }
                    else
                    {
                        switchMenu(charSelectMenu);
                        gameState = GameState.CharSelect;
                    }
                }
                else if (cursorLocs[playerNum].currentItemY == 1)
                {
                    resetGame();

                }
                else if (cursorLocs[playerNum].currentItemY == 2)
                {
                    game.Exit();
                }
            }
            else if (gameState == GameState.GameOver)
            {
                if (System.Environment.TickCount > gameOverTimer)
                {
                    resetGame();
                }
            }
        }
        private void resetGame()
        {
            if (winner == 4)
            {
                winner = 0;
            }
            for (int j = 0; j < 4; j++)
            {
                ready[j] = false;
                if (j > 0)
                {
                    playing[j] = false;
                }
                try
                {
                    players[j].stopChar();
                }
                catch (Exception e)
                {
                    //There may or may not be a nullpointer on the characters when resetting the game.
                    //It's okay because we're resetting the game. Yay!
                }
                players[j] = null;
            } 
            try
            {
                inGameMusicInstance.Stop();
            }
            catch (Exception e)
            {
                //same thing pretty much
            }
            gameOver = false;
            menuMusicInstance.Play();
            numPlayers = 1;
            defaultCursorLoc.resetLoc();
            switchMenu(startMenu);
            gameState = GameState.StartMenu;
        }
    }
}
