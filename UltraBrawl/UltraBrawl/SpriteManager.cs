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


        Camera2d cam;

        SpriteBatch spriteBatch;
        Boolean[] ready = { false, false, false, false };
        Boolean[] playing = { true, false, false, false };

        PlayerPreset[] presets = new PlayerPreset[4];

        PlayerCharacter[] players;
        Vector2[] spawnLocs = { new Vector2(200, 0), new Vector2(800, 0), new Vector2(200, 0), new Vector2(800, 0) };
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
        private int winner = 4;
        private Texture2D p12hub;
        private Texture2D p34hub;
        private Texture2D blastSheet;
        private Texture2D[] venomBar;

        //player readies
        private Texture2D[] notReadyTexs;
        private Texture2D[] readyTexs;
        private Texture2D[] startTexs;

        private Boolean startGame;

        //character buttons
        private Texture2D gokuButton;
        private Texture2D megamanButton;
        private Texture2D ryuButton;
        private Texture2D guileButton;
        private Texture2D venomButton;
        private Texture2D zeroButton;
        private Texture2D kazuyaButton;
        private Texture2D lyndisButton;

        private Texture2D bgCursor;
        private Texture2D bgButton1;
        private Texture2D bgButton2;
        private Texture2D bgButton3;
        private Texture2D bgButton4;
        private Texture2D bgButton5;



        private Texture2D[] selectedChars = new Texture2D[4];

        private Texture2D defaultCursor;
        private CursorLoc defaultCursorLoc;
        private Texture2D p1Cursor;
        private Texture2D p2Cursor;
        private Texture2D p3Cursor;
        private Texture2D p4Cursor;
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
                presets[i] = new PlayerPreset(gamepads[i], new PlayerController(gamepads[i]), spawnLocs[i]);
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

            venomBar = new Texture2D[4];


            wins = new Texture2D[5];

            for (int i = 0; i < 4; i++)
            {
                readyTexs[i] = Game.Content.Load<Texture2D>(@"Images/" + (i + 1) + "R");
                notReadyTexs[i] = Game.Content.Load<Texture2D>(@"Images/" + (i + 1) + "NR");
                startTexs[i] = Game.Content.Load<Texture2D>(@"Images/" + (i + 1) + "S");
                wins[i] = Game.Content.Load<Texture2D>(@"Images/" + (i + 1) + "W");
                healthBars[i] = Game.Content.Load<Texture2D>(@"Images/healthBar");
                venomBar[i] = Game.Content.Load<Texture2D>(@"Images/healthBar");
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
            megamanButton = Game.Content.Load<Texture2D>(@"Images/megamanButton");
            ryuButton = Game.Content.Load<Texture2D>(@"Images/ryuButton");
            guileButton = Game.Content.Load<Texture2D>(@"Images/guileButton");
            venomButton = Game.Content.Load<Texture2D>(@"Images/venomButton");
            zeroButton = Game.Content.Load<Texture2D>(@"Images/zeroButton");
            kazuyaButton = Game.Content.Load<Texture2D>(@"Images/kazuyaButton");
            lyndisButton = Game.Content.Load<Texture2D>(@"Images/lyndisButton");


            bgCursor = Game.Content.Load<Texture2D>(@"Images/bgCursor");
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


            startMenu = new Vector2[1, 2];
            startMenu[0, 0] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 180, 500);
            startMenu[0, 1] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 180, 600);

            charSelectMenu = new Vector2[4, 2];
            charSelectMenu[0, 0] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 600, 600);
            charSelectMenu[0, 1] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 600, 800);
            charSelectMenu[1, 0] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 400, 600);
            charSelectMenu[1, 1] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 400, 800);
            charSelectMenu[2, 0] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 200, 600);
            charSelectMenu[2, 1] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 200, 800);
            charSelectMenu[3, 0] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 000, 600);
            charSelectMenu[3, 1] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 000, 800);

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
                    if (GamePad.GetState(gamepads[i]).Buttons.B == ButtonState.Pressed)
                    {
                        resetGame();
                    }
                    if (GamePad.GetState(gamepads[i]).Buttons.Start == ButtonState.Pressed && !playing[i])
                    {
                        playing[i] = true;
                        cursorPositions[i] = charSelectMenu[0, 0];
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

                        if (players[i].fire)
                        {
                            Debug.WriteLine(players[i].flipped);
                            if (players[i].CHARACTER_ID == 0)
                                spriteList.Add(new GokuKamehameha(blastSheet, new Vector2(players[i].hitbox.Center.X, players[i].hitbox.Center.Y), players[i].flipped));
                            if (players[i].CHARACTER_ID == 1)
                                spriteList.Add(new MegamanBuster(blastSheet, new Vector2(players[i].hitbox.Center.X, players[i].hitbox.Center.Y), players[i].flipped));
                            if (players[i].CHARACTER_ID == 2)
                                spriteList.Add(new RyuHadouken(blastSheet, new Vector2(players[i].hitbox.Center.X, players[i].hitbox.Center.Y), players[i].flipped));
                            if (players[i].CHARACTER_ID == 3)
                                spriteList.Add(new GuileSonicBoom(blastSheet, new Vector2(players[i].hitbox.Center.X, players[i].hitbox.Center.Y), players[i].flipped));
                            if (players[i].CHARACTER_ID == 7)
                                spriteList.Add(new LyndisArrow(blastSheet, new Vector2(players[i].hitbox.Center.X, players[i].hitbox.Center.Y), players[i].flipped));

                            players[i].fire = false;
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
                if (deadCount == numPlayers - 1 && numPlayers > 1)
                {
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
                                //sprite.Collision(players[i]);
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
                for (int i = 0; i < 4; i++)
                {
                    if (ready[i])
                    {
                        spriteBatch.Draw(readyTexs[i], new Vector2((i % 2 == 0 ? 0 : 1560), (i < 2 ? 20 : 300)), Color.White);
                        spriteBatch.Draw(selectedChars[i], new Vector2((i % 2 == 0 ? 10 : 1760), (i < 2 ? 110 : 390)), Color.White);
                    }
                    else if (playing[i])
                    {
                        spriteBatch.Draw(notReadyTexs[i], new Vector2((i % 2 == 0 ? 0 : 1560), (i < 2 ? 20 : 300)), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(startTexs[i], new Vector2((i % 2 == 0 ? 0 : 1560), (i < 2 ? 20 : 300)), Color.White);
                    }
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
                spriteBatch.Draw(bgCursor, cursorPositions[0], Color.White);
                
            }
            if (gameState == GameState.Playing || gameState == GameState.Paused || gameState == GameState.GameOver)
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
            spriteBatch.Draw(healthBgs[0], new Rectangle(65, 17, 560, 65), Color.White);
            spriteBatch.Draw(healthBars[0], new Rectangle(120, 10, players[0].visibleHealth * 5, 78), Color.DarkRed);
            spriteBatch.Draw(healthBars[0], new Rectangle(120, 10, players[0].currentHealth * 5, 78), Color.LightGray);
            if (players[0].canAOE)
            {
                //Debug.WriteLine(players[0].spamTimer-System.Environment.TickCount);
                spriteBatch.Draw(venomBar[0], new Rectangle(120, 65, (int)(players[0].spamTimer-System.Environment.TickCount)/10, 20), Color.Yellow);
            }
            spriteBatch.DrawString(font, players[0].currentHealth + "%", new Vector2(350, 36), Color.Red);
            spriteBatch.DrawString(font, "p1", new Vector2(700, 35), Color.Red);
            spriteBatch.Draw(selectedChars[0], new Rectangle(630, 17, 65, 65), Color.White);

            if (playing[1])
            {
                spriteBatch.Draw(healthBgs[1], new Rectangle(1305, 17, 560, 65), Color.White);
                spriteBatch.Draw(healthBars[1], new Rectangle(1810 - players[1].visibleHealth * 5, 10, players[1].visibleHealth * 5, 78), Color.DarkRed);
                spriteBatch.Draw(healthBars[1], new Rectangle(1810 - players[1].currentHealth * 5, 10, players[1].currentHealth * 5, 78), Color.LightGray);
                //VENOM COOLDOWN HERE DON'T FORGET
                if (players[1].canAOE)
                {
                    //Debug.WriteLine(players[0].spamTimer-System.Environment.TickCount);
                    spriteBatch.Draw(venomBar[0], new Rectangle(1810 - (int)(players[1].spamTimer - System.Environment.TickCount) / 10, 65, (int)(players[1].spamTimer - System.Environment.TickCount) / 10, 20), Color.Yellow);
                }
                spriteBatch.DrawString(font, players[1].currentHealth + "%" + " ", new Vector2(1530, 36), Color.Blue);
                spriteBatch.DrawString(font, "p2", new Vector2(1200, 35), Color.Blue);
                spriteBatch.Draw(selectedChars[1], new Rectangle(1235, 17, 65, 65), Color.White);
            }
            if (playing[2])
            {
                spriteBatch.Draw(healthBgs[2], new Rectangle(65, 82, 560, 65), Color.White);
                spriteBatch.Draw(healthBars[2], new Rectangle(120, 75, players[2].visibleHealth * 5, 78), Color.DarkRed);
                spriteBatch.Draw(healthBars[2], new Rectangle(120, 75, players[2].currentHealth * 5, 78), Color.LightGray);
                if (players[2].canAOE)
                {
                    //Debug.WriteLine(players[0].spamTimer-System.Environment.TickCount);
                    spriteBatch.Draw(venomBar[2], new Rectangle(120, 130, (int)(players[2].spamTimer - System.Environment.TickCount) / 10, 20), Color.Yellow);
                }
                spriteBatch.DrawString(font, players[2].currentHealth + "%", new Vector2(350, 101), Color.Green);
                spriteBatch.DrawString(font, "p3", new Vector2(700, 100), Color.Green);
                spriteBatch.Draw(selectedChars[2], new Rectangle(630, 82, 65, 65), Color.White);
            }
            if (playing[3])
            {
                spriteBatch.Draw(healthBgs[3], new Rectangle(1305, 82, 560, 65), Color.White);
                spriteBatch.Draw(healthBars[3], new Rectangle(1810 - players[3].visibleHealth * 5, 75, players[3].visibleHealth * 5, 78), Color.DarkRed);
                spriteBatch.Draw(healthBars[3], new Rectangle(1810 - players[3].currentHealth * 5, 75, players[3].currentHealth * 5, 78), Color.LightGray);
                //VENOM COOLDOWN HERE DON'T FORGET
                if (players[3].canAOE)
                {
                    //Debug.WriteLine(players[0].spamTimer-System.Environment.TickCount);
                    spriteBatch.Draw(venomBar[0], new Rectangle(1810 - (int)(players[3].spamTimer - System.Environment.TickCount) / 10, 130, (int)(players[3].spamTimer - System.Environment.TickCount) / 10, 20), Color.Yellow);
                }
                spriteBatch.DrawString(font, players[3].currentHealth + "%", new Vector2(1530, 101), Color.Orange);
                spriteBatch.DrawString(font, "p4", new Vector2(1200, 100), Color.Orange);
                spriteBatch.Draw(selectedChars[3], new Rectangle(1235, 82, 65, 65), Color.White);
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
            if (playing[0])
            spriteBatch.DrawString(font, "p1", new Vector2(players[0].collisionRect.Center.X, players[0].hitbox.Top - 60), Color.Red);
            if (playing[1])
            spriteBatch.DrawString(font, "p2", new Vector2(players[1].collisionRect.Center.X, players[1].hitbox.Top - 60), Color.Blue);
            if (playing[2])
            spriteBatch.DrawString(font, "p3", new Vector2(players[2].collisionRect.Center.X, players[2].hitbox.Top - 60), Color.Green);
            if (playing[3])
            spriteBatch.DrawString(font, "p4", new Vector2(players[3].collisionRect.Center.X, players[3].hitbox.Top - 60), Color.Orange);
        }
        void switchMenu(Vector2[,] menu)
        {
            currentMenu = menu;
            for (int i = 0; i < 4; i++)
            {
                cursorPositions[i] = currentMenu[0, 0];
                cursorLocs[i].resetLoc();
                //cursorLocs[i] = new CursorLoc();
            }
        }
        void navigateMenu()
        {
            //gamepad controls
            for (int i = 0; i < 4; i++)
            {
                if (playing[i])
                {
                    if (cursorLocs[i].currentItemY + 1 < currentMenu.GetLength(1))
                    {
                        if (GamePad.GetState(gamepads[i]).DPad.Down == ButtonState.Pressed && previousGamePadState[i].DPad.Down == ButtonState.Released)
                        {
                            cursorLocs[i].currentItemY++;
                        }
                        else if (GamePad.GetState(gamepads[i]).ThumbSticks.Left.Y < -.5f && previousGamePadState[i].ThumbSticks.Left.Y >= -.49f)
                        {
                            cursorLocs[i].currentItemY++;
                        }
                    }
                    if (cursorLocs[i].currentItemX - 1 >= 0)
                    {
                        if (GamePad.GetState(gamepads[i]).DPad.Left == ButtonState.Pressed && previousGamePadState[i].DPad.Left == ButtonState.Released)
                        {
                            cursorLocs[i].currentItemX--;
                        }
                        else if (GamePad.GetState(gamepads[i]).ThumbSticks.Left.X < -.5f && previousGamePadState[i].ThumbSticks.Left.X >= -.49f)
                        {
                            cursorLocs[i].currentItemX--;
                        }
                    }

                    if (cursorLocs[i].currentItemX + 1 < currentMenu.GetLength(0))
                    {
                        if (GamePad.GetState(gamepads[i]).DPad.Right == ButtonState.Pressed && previousGamePadState[i].DPad.Right == ButtonState.Released)
                        {
                            cursorLocs[i].currentItemX++;
                        }
                        else if (GamePad.GetState(gamepads[i]).ThumbSticks.Left.X > .5f && previousGamePadState[i].ThumbSticks.Left.X <= .49f)
                        {
                            cursorLocs[i].currentItemX++;
                        }
                    }
                    if (cursorLocs[i].currentItemY - 1 >= 0)
                    {
                        if (GamePad.GetState(gamepads[i]).DPad.Up == ButtonState.Pressed && previousGamePadState[i].DPad.Up == ButtonState.Released)
                        {
                            cursorLocs[i].currentItemY--;
                        }
                        else if (GamePad.GetState(gamepads[i]).ThumbSticks.Left.Y > .5f && previousGamePadState[i].ThumbSticks.Left.Y <= .49f)
                        {
                            cursorLocs[i].currentItemY--;
                        }
                    }
                    if (GamePad.GetState(gamepads[i]).Buttons.A == ButtonState.Released && previousGamePadState[i].Buttons.A == ButtonState.Pressed)
                    {
                        if (i == 0)
                        {
                            menuSelect(i, cursorPositions[i]);
                        }
                        else if (currentMenu == charSelectMenu)
                        {
                            menuSelect(i, cursorPositions[i]);
                        }
                    }
                    if (currentMenu == bgSelectMenu)
                    {
                        if (GamePad.GetState(gamepads[i]).Buttons.B == ButtonState.Released && previousGamePadState[i].Buttons.B == ButtonState.Pressed)
                        {
                            background = Game.Content.Load<Texture2D>(@"Images/background");
                            startGame = false;
                            gameState = GameState.CharSelect;
                            switchMenu(charSelectMenu);
                        }
                    }
                    if (currentMenu == charSelectMenu)
                    {
                        if (GamePad.GetState(gamepads[0]).Buttons.Start == ButtonState.Pressed && previousGamePadState[0].Buttons.Start == ButtonState.Released)
                        {
                            startGame = true;
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
                        if (cursorLocs[0].currentItemX == 0)
                        {
                            if (cursorLocs[0].currentItemY == 0)
                            {
                                background = Game.Content.Load<Texture2D>(@"Images/background1");
                            }
                            else if (cursorLocs[0].currentItemY == 1)
                            {
                                background = Game.Content.Load<Texture2D>(@"Images/background2");
                            }
                            else if (cursorLocs[0].currentItemY == 2)
                            {
                                background = Game.Content.Load<Texture2D>(@"Images/background3");
                            }
                            else if (cursorLocs[0].currentItemY == 3)
                            {
                                background = Game.Content.Load<Texture2D>(@"Images/background4");
                            }
                            else if (cursorLocs[0].currentItemY == 4)
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
                    }
                    cursorPositions[i] = currentMenu[cursorLocs[i].currentItemX, cursorLocs[i].currentItemY];
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
                        }
                        else if (cursorLocs[playerNum].currentItemY == 1)
                        {
                        }
                    }
                }

            }
            else if (gameState == GameState.BgSelect)
            {
                if (cursorLocs[0].currentItemY == 0)
                {
                    inGameMusic = game.Content.Load<SoundEffect>("Sound/Carpenter Brut - Le Perv Loop");
                    platformList = new List<Sprite>();
                    platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(150, 700)));
                    platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(250, 900)));
                    platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(1450, 700)));
                    platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(1500, 900)));
                }
                }
                else if (cursorLocs[0].currentItemY == 1)
                {
                    inGameMusic = game.Content.Load<SoundEffect>("Sound/Kirk Gadget & Valkyrie 1984 - Ghosts Loop");
                    platformList = new List<Sprite>();
                    platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(500, 600)));
                    platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(200, 800)));
                    platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(1100, 600)));
                    platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(1400, 800)));
                    //platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(800, 460)));
                    platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(800, 800)));
                }
                else if (cursorLocs[0].currentItemY == 2)
                {
                    inGameMusic = game.Content.Load<SoundEffect>("Sound/LazerHawk - King of The Streets Loop");
                    platformList = new List<Sprite>();
                    //platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(100, 700)));
                    platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(400, 900)));
                    //platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(1500, 700)));
                    platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(1200, 900)));
                    platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(800, 700)));
                }
                else if (cursorLocs[0].currentItemY == 3)
                {
                    inGameMusic = game.Content.Load<SoundEffect>("Sound/SelloRekT LA Dreams - Feel The Burn Loop");
                    platformList = new List<Sprite>();
                    //platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(100, 700)));
                    platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(400, 900)));
                    //platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(1500, 700)));
                    platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(1200, 900)));
                }
                else if (cursorLocs[0].currentItemY == 4)
                {
                    inGameMusic = game.Content.Load<SoundEffect>("Sound/SellorektLA Dreams - LightSpeed Loop");
                    platformList = new List<Sprite>();
                    platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(150, 700)));
                    platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(250, 900)));
                    platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(1450, 700)));
                    platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(1500, 900)));
                }
                blastSheet = Game.Content.Load<Texture2D>(@"Images/blasts");
                spawnCharacters();
                //music under spawn characters as spawnCharacters checks if guile is there.
                if(guilePlaying)
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
                if (cursorLocs[playerNum].currentItemY == 0)
                {
                    resetGame();
                }
            }
        }
        private void resetGame()
        {
            winner = 4;
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
            menuMusicInstance.Play();
            numPlayers = 1;
            defaultCursorLoc.resetLoc();
            switchMenu(startMenu);
            gameState = GameState.StartMenu;
        }
    }
}
