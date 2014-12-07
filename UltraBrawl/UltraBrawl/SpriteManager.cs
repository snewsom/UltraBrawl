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


        Camera2d cam;

        SpriteBatch spriteBatch;
        Boolean p3playing = false;
        Boolean p4playing = false;
        Boolean[] ready = {false, false, false, false};

        PlayerPreset[] presets = new PlayerPreset[4];

        PlayerCharacter[] players;
        Vector2[] spawnLocs = {new Vector2(200, 0), new Vector2(800, 0), new Vector2(200, 0), new Vector2(800, 0)};
        Texture2D background;

        List<AutomatedSprite> spriteList = new List<AutomatedSprite>();

        List<Sprite> platformList = new List<Sprite>();

        private Game game;

        FighterFactory factory;

        ParticleEngine2D particleEngine;

        private Texture2D startButton;
        private Texture2D exitButton;
        private Texture2D resumeButton;
        private Texture2D mainmenuButton;
        private Texture2D title;

        //player readies
        private Texture2D[] notReadyTexs;
        private Texture2D[] readyTexs;


        //character buttons
        private Texture2D gokuButton;
        private Texture2D megamanButton;
        private Texture2D ryuButton;


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
        int numPlayers = 2;
        GamePadState[] previousGamePadState = new GamePadState[4];

        MouseState mouseState;
        public GameState gameState;
        public enum GameState
        {
            StartMenu,
            CharSelect,
            Playing,
            Paused
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
            for (int i = 0; i < 4; i++)
            {
                readyTexs[i] = Game.Content.Load<Texture2D>(@"Images/" + (i + 1) + "R");
                notReadyTexs[i] = Game.Content.Load<Texture2D>(@"Images/" + (i + 1) + "NR");
            }


            gokuButton = Game.Content.Load<Texture2D>(@"Images/gokuButton");
            megamanButton = Game.Content.Load<Texture2D>(@"Images/megamanButton");
            ryuButton = Game.Content.Load<Texture2D>(@"Images/ryuButton");

            defaultCursor = Game.Content.Load<Texture2D>(@"Images/DefaultCursor");
            p1Cursor = Game.Content.Load<Texture2D>(@"Images/p1Cursor");
            p2Cursor = Game.Content.Load<Texture2D>(@"Images/p2Cursor");
            p3Cursor = Game.Content.Load<Texture2D>(@"Images/p3Cursor");
            p4Cursor = Game.Content.Load<Texture2D>(@"Images/p4Cursor");


            startMenu = new Vector2[1, 2];
            startMenu[0, 0] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 180, 400);
            startMenu[0, 1] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 180, 600);

            charSelectMenu = new Vector2[1, 3];
            charSelectMenu[0, 0] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 200);
            charSelectMenu[0, 1] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 400);
            charSelectMenu[0, 2] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 600);

            pauseMenu = new Vector2[1, 3];
            pauseMenu[0, 0] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 180, 200);
            pauseMenu[0, 1] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 180, 400);
            pauseMenu[0, 2] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 180, 600);

            switchMenu(startMenu);

            // particle stuff
            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(Game.Content.Load<Texture2D>("Images/circle"));
            textures.Add(Game.Content.Load<Texture2D>("Images/star"));
            textures.Add(Game.Content.Load<Texture2D>("Images/diamond"));
            particleEngine = new ParticleEngine2D(textures, new Vector2(400, 240));

            loadLevel();
        }

        protected void loadLevel()
        {
            platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(100, 700)));
            platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(200, 900)));
            platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(1500, 700)));
            platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(1400, 900)));

            background = Game.Content.Load<Texture2D>(@"Images/background");
            base.LoadContent();
        }

        protected void spawnCharacters()
        {
            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(Game.Content.Load<Texture2D>("Images/circle"));
            textures.Add(Game.Content.Load<Texture2D>("Images/star"));
            textures.Add(Game.Content.Load<Texture2D>("Images/diamond"));
            if (p3playing)
            {
                numPlayers++;
            }
            if (p4playing)
            {
                numPlayers++;
            }
            for (int i = 0; i < numPlayers; i++)
            {
                players[i].spawn(presets[i]);
            }
        }

        

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (gameState == GameState.StartMenu || gameState == GameState.Paused || gameState == GameState.CharSelect)
            {
                navigateMenu();
            }
            if (gameState == GameState.CharSelect)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (GamePad.GetState(gamepads[i]).Buttons.B == ButtonState.Pressed)
                    {
                        resetGame();
                    }
                }
                //p3 press start if playing (not implemented)
                //if (GamePad.GetState(gamepads[2]).Buttons.Start == ButtonState.Pressed && previousGamePadState[2].Buttons.Start == ButtonState.Released)
                if(GamePad.GetState(gamepads[2]).IsConnected && !p3playing)
                {
                    p3playing = true;
                    cursorPositions[2] = charSelectMenu[0, 0];
                }
                //p4 press start if playing (not implemented)
                //if (GamePad.GetState(gamepads[3]).Buttons.Start == ButtonState.Pressed && previousGamePadState[3].Buttons.Start == ButtonState.Released)
                if(GamePad.GetState(gamepads[3]).IsConnected && !p4playing)
                {
                    p4playing = true;
                    cursorPositions[3] = charSelectMenu[0, 0];
                }
                if ((ready[0] && ready[1]) && ((p3playing && !p4playing && ready[2])||(p4playing && !p3playing && ready[3])||(p3playing && p4playing && ready[2] && ready[3])||(!p3playing && !p4playing)))
                {
                        spawnCharacters();
                        gameState = GameState.Playing;
                }
            }
            if (gameState == GameState.Paused)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) || (GamePad.GetState(gamepads[i]).Buttons.Start == ButtonState.Released && previousGamePadState[i].Buttons.Start == ButtonState.Pressed))
                    {
                        for (int j = 0; j < numPlayers; j++)
                        {
                            players[j].update = true;
                        }
                        gameState = GameState.Playing;
                    }
                    previousGamePadState[i] = GamePad.GetState(gamepads[i]);
                }
                navigateMenu();
            }

            else if (gameState == GameState.Playing)
            {
                
                for (int i = 0; i<numPlayers; i++)
                {
                    players[i].Update(gameTime, Game.Window.ClientBounds);

                    if (players[i].fire)
                    {
                        Debug.WriteLine(players[i].flipped);
                        spriteList.Add(new MegamanBuster(Game.Content.Load<Texture2D>(@"Images/megamanBuster"), new Vector2(players[i].hitbox.Center.X, players[i].hitbox.Center.Y), players[i].flipped));
                        players[i].fire = false;
                    }
                }
                for (int i = 0; i < 4; i++)
                {
                    if (GamePad.GetState(gamepads[i]).Buttons.Start == ButtonState.Pressed && previousGamePadState[i].Buttons.Start == ButtonState.Released)
                    {
                        for (int j = 0; j < numPlayers; j++)
                        {
                            players[j].update = false;
                        }
                        switchMenu(pauseMenu);
                        gameState = GameState.Paused;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        for (int j = 0; j < numPlayers; j++)
                        {
                            players[j].update = false;
                        }
                        switchMenu(pauseMenu);
                        gameState = GameState.Paused;
                    }
                    previousGamePadState[i] = GamePad.GetState(gamepads[i]);
                }

                for (int i = 0; i < spriteList.Count; i++)
                {
                    for (int j = 0; j < numPlayers; j++)
                    {
                        if (spriteList.ElementAt(i).collisionRect.Intersects(players[j].collisionRect))
                        {
                            spriteList.ElementAt(i).Collision(players[j]);
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
                        if (i != j)
                        {
                            if (players[i].newHitbox.Intersects(players[j].collisionRect))
                            {
                                players[i].Collision(players[j]);
                            }
                        }
                    }
                }

                foreach (Sprite sprite in platformList)
                {
                    for (int i = 0; i < numPlayers; i++)
                    {
                        if (sprite.collisionRect.Intersects(players[i].collisionRect))
                        {
                            players[i].Collision(sprite);
                            sprite.Collision(players[i]);
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
            if (gameState == GameState.StartMenu)
            {
                game.IsMouseVisible = true;
                spriteBatch.Draw(title, new Vector2((GraphicsDevice.Viewport.Width / 2) - 250, 200), Color.White);
                spriteBatch.Draw(startButton, startMenu[0, 0], Color.White);
                spriteBatch.Draw(exitButton, startMenu[0, 1], Color.White);
                spriteBatch.Draw(defaultCursor, cursorPositions[0], Color.White);
                particleEngine.Draw(spriteBatch);
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
                    else
                    {
                        if (i == 2)
                        {
                            if (p3playing)
                                spriteBatch.Draw(notReadyTexs[i], new Vector2((i % 2 == 0 ? 0 : 1560), (i < 2 ? 20 : 300)), Color.White);
                        }
                        else if (i == 3)
                        {
                            if (p4playing)
                                spriteBatch.Draw(notReadyTexs[i], new Vector2((i % 2 == 0 ? 0 : 1560), (i < 2 ? 20 : 300)), Color.White);
                        }
                        else
                        {
                            spriteBatch.Draw(notReadyTexs[i], new Vector2((i % 2 == 0 ? 0 : 1560), (i < 2 ? 20 : 300)), Color.White);
                        }
                    }
                }
                game.IsMouseVisible = true;
                spriteBatch.Draw(gokuButton, charSelectMenu[0, 0], Color.White);
                spriteBatch.Draw(megamanButton, charSelectMenu[0, 1], Color.White);
                spriteBatch.Draw(ryuButton, charSelectMenu[0, 2], Color.White);
                spriteBatch.Draw(p1Cursor, cursorPositions[0], Color.White);
                spriteBatch.Draw(p2Cursor, cursorPositions[1], Color.White);
                if(p3playing)
                {
                    spriteBatch.Draw(p3Cursor, cursorPositions[2], Color.White);
                }
                if(p4playing)
                {
                    spriteBatch.Draw(p4Cursor, cursorPositions[3], Color.White);
                }
                particleEngine.Draw(spriteBatch);
            }
            if (gameState == GameState.Playing || gameState == GameState.Paused)
            {
                
                game.IsMouseVisible = false;
                LoadGame(gameTime);

                if (players[0].currentHealth <= 0 || players[1].currentHealth <= 0)
                {
                    resetGame();
                }
            } 
            if (gameState == GameState.Paused)
            {
                game.IsMouseVisible = true;
                spriteBatch.Draw(resumeButton, pauseMenu[0, 0], Color.White);
                spriteBatch.Draw(mainmenuButton, pauseMenu[0, 1], Color.White);
                spriteBatch.Draw(exitButton, pauseMenu[0, 2], Color.White);
                spriteBatch.Draw(defaultCursor, cursorPositions[0], Color.White);
                particleEngine.Draw(spriteBatch);
            }
          
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void LoadGame(GameTime gameTime)
        {
            spriteBatch.Draw(background, new Rectangle(0, 0, 1920, 1080), Color.White);
            spriteBatch.DrawString(font, players[0].CHARACTER_NAME + " " + players[0].currentHealth, new Vector2(100, 100), Color.Red);
            spriteBatch.DrawString(font, players[1].currentHealth + " " + players[1].CHARACTER_NAME, new Vector2(1720, 100), Color.Blue);
            if (p3playing)
            {
                spriteBatch.DrawString(font, players[2].CHARACTER_NAME + " " + players[2].currentHealth, new Vector2(100, 300), Color.Green);
            }
            if (p4playing)
            {
                spriteBatch.DrawString(font, players[3].currentHealth + " " + players[3].CHARACTER_NAME, new Vector2(1720, 300), Color.Orange);
            }
            for (int i = 0; i < numPlayers; i++)
            {
                if (players[i].isSuper)
                {
                    particleEngine.EmitterLocation = new Vector2(players[i].collisionRect.Center.X, players[i].collisionRect.Center.Y);
                    particleEngine.Update();
                    particleEngine.Draw(spriteBatch);
                }
               
                players[i].Draw(gameTime, spriteBatch);
            
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
                //cursorLocs[i] = new CursorLoc();
            }
        }
        void navigateMenu()
        {
            
            //mouse controls
            mouseState = Mouse.GetState();
            particleEngine.EmitterLocation = new Vector2(mouseState.X, mouseState.Y);
            particleEngine.Update();

            //gamepad controls
            for (int i = 0; i < 4; i++)
            {
                if (cursorLocs[i].currentItemY + 1 < currentMenu.GetLength(1))
                {
                    if (GamePad.GetState(gamepads[i]).DPad.Down == ButtonState.Pressed && previousGamePadState[i].DPad.Down == ButtonState.Released)
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
                }

                if (cursorLocs[i].currentItemX + 1 < currentMenu.GetLength(0))
                {
                    if (GamePad.GetState(gamepads[i]).DPad.Right == ButtonState.Pressed && previousGamePadState[i].DPad.Right == ButtonState.Released)
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
                }
                if (GamePad.GetState(gamepads[i]).Buttons.A == ButtonState.Released && previousGamePadState[i].Buttons.A == ButtonState.Pressed)
                {
                    if (i == 0)
                    {
                        menuSelect(i, cursorPositions[i]);
                    } else if (currentMenu == charSelectMenu)
                    {
                        menuSelect(i, cursorPositions[i]);
                    }
                }
                cursorPositions[i] = currentMenu[cursorLocs[i].currentItemX, cursorLocs[i].currentItemY];
                previousGamePadState[i] = GamePad.GetState(gamepads[i]);
            }
        }

        private void menuSelect(int playerNum, Vector2 vector2)
        {
            if (gameState == GameState.CharSelect)
            {
                if (cursorLocs[playerNum].currentItemX == 0)
                {
                    if(cursorLocs[playerNum].currentItemY == 0){
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
                    else if (cursorLocs[playerNum].currentItemY == 2)
                    {
                        players[playerNum] = factory.selectCharacter(2);
                        selectedChars[playerNum] = ryuButton;
                        ready[playerNum] = true;
                    }
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
                    for (int j = 0; j < numPlayers; j++)
                    {
                        players[j].update = true;
                    }
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
        }
        private void resetGame()
        {
            for (int j = 0; j < numPlayers; j++)
            {
                ready[j] = false;
                try{
                    players[j].update = false;
                }
                catch (Exception e)
                {
                    //There may or may not be a nullpointer on the characters when resetting the game.
                    //It's okay because we're resetting the game. Yay!
                }
            }
            numPlayers = 2;
            defaultCursorLoc.resetLoc();
            switchMenu(startMenu);
            gameState = GameState.StartMenu;
        }
    }
}
