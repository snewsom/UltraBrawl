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
        int currentItemX = 0;
        int currentItemY = 0;



        SpriteBatch spriteBatch;
        Boolean p3playing = false;
        Boolean p4playing = false;
        Boolean[] ready = {false, false, false, false};

        PlayerPreset[] presets = new PlayerPreset[4];

        PlayerCharacter[] players;
        Vector2[] spawnLocs = {new Vector2(200, 0), new Vector2(800, 0), new Vector2(200, 600), new Vector2(800, 600)};
        Texture2D background;

        List<Sprite> spriteList = new List<Sprite>();

        List<Sprite> platformList = new List<Sprite>();

        private Game game;

        FighterFactory factory;

        ParticleEngine2D particleEngine;

        private Texture2D startButton;
        private Texture2D exitButton;
        private Texture2D resumeButton;

        private Vector2 startButtonPosition;
        private Vector2 exitButtonPosition;
        private Vector2 CharSelectButtonOnePosition;
        private Vector2 CharSelectButtonTwoPosition;

        private Texture2D defaultCursor;
        private Texture2D p1Cursor;
        private Texture2D p2Cursor;
        private Texture2D p3Cursor;
        private Texture2D p4Cursor;
        List<Texture2D> cursors = new List<Texture2D>();

        Vector2[] cursorPositions = new Vector2[4];

        PlayerIndex[] gamepads = new PlayerIndex[4];
        int numPlayers = 2;
        GamePadState[] previousGamePadState = new GamePadState[4];

        private Vector2 resumeButtonPosition;
        MouseState mouseState;
        MouseState previousMouseState;
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

            defaultCursor = Game.Content.Load<Texture2D>(@"Images/DefaultCursor");
            p1Cursor = Game.Content.Load<Texture2D>(@"Images/p1Cursor");
            p2Cursor = Game.Content.Load<Texture2D>(@"Images/p2Cursor");
            p3Cursor = Game.Content.Load<Texture2D>(@"Images/p3Cursor");
            p4Cursor = Game.Content.Load<Texture2D>(@"Images/p4Cursor");


            startMenu = new Vector2[1, 2];
            startMenu[0, 0] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 200);
            startMenu[0, 1] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 250);

            charSelectMenu = new Vector2[1, 2];
            charSelectMenu[0,0] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 600);
            charSelectMenu[0,1] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 650);

            pauseMenu = new Vector2[1, 2];
            pauseMenu[0, 0] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 200);
            pauseMenu[0, 1] = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 250);

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
            platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(100, 400)));
            platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(200, 600)));
            platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(800, 600)));
            platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(900, 400)));

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
                if (GamePad.GetState(gamepads[2]).Buttons.Start == ButtonState.Pressed && previousGamePadState[2].Buttons.Start == ButtonState.Released)
                {
                    p3playing = true;
                    cursorPositions[2] = charSelectMenu[0, 0];
                }
                if (GamePad.GetState(gamepads[3]).Buttons.Start == ButtonState.Pressed && previousGamePadState[3].Buttons.Start == ButtonState.Released)
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
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) || (GamePad.GetState(gamepads[i]).Buttons.Start == ButtonState.Pressed && previousGamePadState[i].Buttons.Start == ButtonState.Released))
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
                particleEngine.EmitterLocation = new Vector2(players[0].collisionRect.Center.X, players[0].collisionRect.Center.Y);
                particleEngine.Update();
                for (int i = 0; i<numPlayers; i++)
                {
                    players[i].Update(gameTime, Game.Window.ClientBounds);
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

                // update each automated sprite
                foreach (Sprite sprite in spriteList)
                {
                    sprite.Update(gameTime, Game.Window.ClientBounds);
                }
                if (players[0].currentHealth <= 0 || players[1].currentHealth <= 0)
                {
                    switchMenu(startMenu);
                    gameState = GameState.StartMenu;
                }

                    System.Diagnostics.Debug.WriteLine(players[0].hitbox.Top);
                    if (players[1].newHitbox.Intersects(players[0].collisionRect))
                    {
                        players[1].Collision(players[0]);
                    }
                    if (players[0].newHitbox.Intersects(players[1].collisionRect))
                    {
                        players[0].Collision(players[1]);
                    }

                foreach (Sprite sprite in platformList)
                {
                    if (sprite.collisionRect.Intersects(players[0].collisionRect))
                    {
                        players[0].Collision(sprite);
                        sprite.Collision(players[0]);
                    }
                    if (sprite.collisionRect.Intersects(players[1].collisionRect))
                    {
                        players[1].Collision(sprite);
                        sprite.Collision(players[1]);
                    }
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            if (gameState == GameState.StartMenu)
            {
                game.IsMouseVisible = true;
                startButtonPosition = startMenu[0, 0];
                exitButtonPosition = startMenu[0, 1];
                spriteBatch.Draw(startButton, startButtonPosition, Color.White);
                spriteBatch.Draw(exitButton, exitButtonPosition, Color.White);
                spriteBatch.Draw(defaultCursor, cursorPositions[0], Color.White);
                particleEngine.Draw(spriteBatch);
            }
            if (gameState == GameState.CharSelect)
            {
                game.IsMouseVisible = true;
                CharSelectButtonOnePosition = charSelectMenu[0, 0];
                CharSelectButtonTwoPosition = charSelectMenu[0, 1];
                spriteBatch.Draw(startButton, CharSelectButtonOnePosition, Color.White);
                spriteBatch.Draw(exitButton, CharSelectButtonTwoPosition, Color.White);
                spriteBatch.Draw(p1Cursor, cursorPositions[0], Color.White);
                spriteBatch.Draw(p2Cursor, cursorPositions[1], Color.White);
                if(p3playing)
                {
                    Debug.WriteLine("PLAYER 3 PLAYING");
                    spriteBatch.Draw(p3Cursor, cursorPositions[2], Color.Green);
                }
                if(p4playing)
                {
                    spriteBatch.Draw(p4Cursor, cursorPositions[3], Color.White);
                }
                particleEngine.Draw(spriteBatch);
            }
            if (gameState == GameState.Paused)
            {
                game.IsMouseVisible = true;
                resumeButtonPosition = pauseMenu[0, 0];
                exitButtonPosition = pauseMenu[0, 1];
                spriteBatch.Draw(resumeButton, resumeButtonPosition, Color.White);
                spriteBatch.Draw(exitButton, exitButtonPosition, Color.White);
                spriteBatch.Draw(defaultCursor, cursorPositions[0], Color.White);
                particleEngine.Draw(spriteBatch);
            }
            if (gameState == GameState.Playing)
            {
          
                game.IsMouseVisible = false;
                LoadGame(gameTime);
               
            }
          
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void LoadGame(GameTime gameTime)
        {
            spriteBatch.Draw(background, new Rectangle(0, 0, 1280, 800), Color.White);
            spriteBatch.DrawString(font, players[0].CHARACTER_NAME + " " + players[0].currentHealth, new Vector2(100, 100), Color.Black);
            spriteBatch.DrawString(font, players[1].currentHealth + " " + players[1].CHARACTER_NAME, new Vector2(1080, 100), Color.Black);
            if (players[0].isSuper)
            {
                particleEngine.Draw(spriteBatch);
            }
            players[0].Draw(gameTime, spriteBatch);
            players[1].Draw(gameTime, spriteBatch);
            foreach (Sprite sprite in spriteList)
                sprite.Draw(gameTime, spriteBatch);
            foreach (Sprite sprite in platformList)
                sprite.Draw(gameTime, spriteBatch);
        }
        void switchMenu(Vector2[,] menu)
        {
            currentItemX = 0;
            currentItemY = 0;
            currentMenu = menu;
            cursorPositions[0] = currentMenu[0, 0];
            if (currentMenu == charSelectMenu)
            {
                cursorPositions[1] = currentMenu[0, 1];
                cursorPositions[2] = currentMenu[0, 1];
                cursorPositions[3] = currentMenu[0, 1];
            }
        }
        void navigateMenu()
        {
            
            //mouse controls
            mouseState = Mouse.GetState();
            particleEngine.EmitterLocation = new Vector2(mouseState.X, mouseState.Y);
            particleEngine.Update();
            if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
            {
                MouseClicked(mouseState.X, mouseState.Y, 0);
            }
            previousMouseState = mouseState;

            //gamepad controls
            for (int i = 0; i < 4; i++)
            {
                if (currentItemY + 1 < currentMenu.GetLength(1))
                {
                    if (GamePad.GetState(gamepads[i]).DPad.Down == ButtonState.Pressed && previousGamePadState[i].DPad.Down == ButtonState.Released)
                    {
                        currentItemY++;
                        cursorPositions[i] = currentMenu[currentItemX, currentItemY];
                    }
                }
                if (currentItemX - 1 >= 0)
                {
                    if (GamePad.GetState(gamepads[i]).DPad.Left == ButtonState.Pressed && previousGamePadState[i].DPad.Left == ButtonState.Released)
                    {
                        currentItemX--;
                        cursorPositions[i] = currentMenu[currentItemX, currentItemY];
                    }
                }

                if (currentItemX + 1 < currentMenu.GetLength(0))
                {
                    if (GamePad.GetState(gamepads[i]).DPad.Right == ButtonState.Pressed && previousGamePadState[i].DPad.Right == ButtonState.Released)
                    {
                        currentItemX++;
                        cursorPositions[i] = currentMenu[currentItemX, currentItemY];
                    }
                }
                if (currentItemY - 1 >= 0)
                {
                    if (GamePad.GetState(gamepads[i]).DPad.Up == ButtonState.Pressed && previousGamePadState[i].DPad.Up == ButtonState.Released)
                    {
                        currentItemY--;
                        cursorPositions[i] = currentMenu[currentItemX, currentItemY];
                    }
                }
                if (GamePad.GetState(gamepads[i]).Buttons.A == ButtonState.Pressed && previousGamePadState[i].Buttons.A == ButtonState.Released)
                {
                    if (i == 0)
                    {
                        menuSelect(i, cursorPositions[i]);
                    } else if (currentMenu == charSelectMenu)
                    {
                        menuSelect(i, cursorPositions[i]);
                    }
                }
                previousGamePadState[i] = GamePad.GetState(gamepads[i]);
            }
        }

        private void menuSelect(int playerNum, Vector2 vector2)
        {
            MouseClicked((int) vector2.X, (int) vector2.Y, playerNum);
        }

        void MouseClicked(int x, int y, int playerNum)
        {
            //creates a rectangle of 10x10 around the place where the mouse was clicked
            Rectangle mouseClickRect = new Rectangle(x, y, 10, 10);

            //check the startmenu
            if (gameState == GameState.StartMenu)
            {
                Rectangle startButtonRect = new Rectangle((int)startButtonPosition.X, (int)startButtonPosition.Y, 100, 20);
                Rectangle exitButtonRect = new Rectangle((int)exitButtonPosition.X, (int)exitButtonPosition.Y, 100, 20);
                Rectangle defaultCursor = new Rectangle((int)startButtonPosition.X, (int)startButtonPosition.Y, 100, 20);


                if (mouseClickRect.Intersects(startButtonRect)) //player clicked start button
                {
                    switchMenu(charSelectMenu);
                    gameState = GameState.CharSelect;
                }
                else if (mouseClickRect.Intersects(exitButtonRect)) //player clicked exit button
                {
                    game.Exit();
                }
            }
            if (gameState == GameState.CharSelect)
            {
                Rectangle startButtonRect = new Rectangle((int)CharSelectButtonOnePosition.X, (int)CharSelectButtonOnePosition.Y, 100, 20);
                Rectangle exitButtonRect = new Rectangle((int)CharSelectButtonTwoPosition.X, (int)CharSelectButtonTwoPosition.Y, 100, 20);

                if (mouseClickRect.Intersects(startButtonRect)) //a player clicked char1
                {
                    players[playerNum] = factory.selectCharacter(0);
                    ready[playerNum] = true;
                }
                else if (mouseClickRect.Intersects(exitButtonRect)) //a player clicked char2
                {
                    players[playerNum] = factory.selectCharacter(1);
                    ready[playerNum] = true;
                }
            }
            if (gameState == GameState.Paused)
            {
                Rectangle resumeButtonRect = new Rectangle((int)resumeButtonPosition.X, (int)resumeButtonPosition.Y, 100, 20);
                Rectangle exitButtonRect = new Rectangle((int)exitButtonPosition.X, (int)exitButtonPosition.Y, 100, 20);

                if (mouseClickRect.Intersects(resumeButtonRect))
                {
                    for (int j = 0; j < numPlayers; j++)
                    {
                        players[j].update = true;
                    }
                    gameState = GameState.Playing;
                }
                else if (mouseClickRect.Intersects(exitButtonRect)) //player clicked exit button
                {
                    game.Exit();
                }
            }
        }
    }
}
