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
        PlayerController controllerOne;
        PlayerController controllerTwo;
        PlayerController controllerThree;
        PlayerController controllerFour;
        List<PlayerController> controllers = new List<PlayerController>();
        List<PlayerCharacter> players = new List<PlayerCharacter>();

        Boolean p3playing = false;
        Boolean p4playing = false;
        Boolean[] ready = {false, false, false, false};

        PlayerPreset[] presets = new PlayerPreset[4];
        
        PlayerCharacter playerOne;
        PlayerCharacter playerTwo;
        PlayerCharacter playerThree;
        PlayerCharacter playerFour;
        Vector2[] spawnLocs = {new Vector2(300, 0), new Vector2(800, 0), new Vector2(600, 0), new Vector2(600, 0)};
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

        private Texture2D p1Cursor;
        private Texture2D p2Cursor;
        private Texture2D p3Cursor;
        private Texture2D p4Cursor;
        List<Texture2D> cursors = new List<Texture2D>();

        Vector2[] cursorPositions = new Vector2[4];

        PlayerIndex[] gamepads = new PlayerIndex[4];

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
                controllers.Add(new PlayerController(gamepads[i]));
                presets[i] = new PlayerPreset(gamepads[i], controllers.ElementAt(i), spawnLocs[i]);
            }
            font = Game.Content.Load<SpriteFont>("Images/dbzFont");


            startButton = Game.Content.Load<Texture2D>(@"Images/start");
            exitButton = Game.Content.Load<Texture2D>(@"Images/exit");
            resumeButton = Game.Content.Load<Texture2D>(@"Images/resume");
            p1Cursor = Game.Content.Load<Texture2D>(@"Images/CharSelectCursor");
            p2Cursor = Game.Content.Load<Texture2D>(@"Images/CharSelectCursor");
            p3Cursor = Game.Content.Load<Texture2D>(@"Images/CharSelectCursor");
            p4Cursor = Game.Content.Load<Texture2D>(@"Images/CharSelectCursor");


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

            //////////////////////////////////////
            //////////////////////////////////////
            // This will eventually go into the character selection menu.
            playerOne = factory.selectCharacter(0);
            players.Add(playerOne);
            playerTwo = factory.selectCharacter(2);
            players.Add(playerTwo);
            //////////////////////////////////////
            //////////////////////////////////////
            for (int i = 0; i < players.Count; i++)
            {
                players.ElementAt(i).spawn(presets[i]);
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
            if (gameState == GameState.Paused)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) || (GamePad.GetState(gamepads[i]).Buttons.Start == ButtonState.Pressed && previousGamePadState[i].Buttons.Start == ButtonState.Released))
                    {
                        foreach (PlayerCharacter player in players)
                            player.update = true;
                        gameState = GameState.Playing;
                    }
                    previousGamePadState[i] = GamePad.GetState(gamepads[i]);
                }
                navigateMenu();
            }

            else if (gameState == GameState.Playing)
            {
                particleEngine.EmitterLocation = new Vector2(playerOne.collisionRect.Center.X, playerOne.collisionRect.Center.Y);
                particleEngine.Update();
                foreach (PlayerCharacter player in players)
                {
                    player.Update(gameTime, Game.Window.ClientBounds);
                }
                for (int i = 0; i < 4; i++)
                {
                    if (GamePad.GetState(gamepads[i]).Buttons.Start == ButtonState.Pressed && previousGamePadState[i].Buttons.Start == ButtonState.Released)
                    {
                        foreach (PlayerCharacter player in players)
                            player.update = false;
                        switchMenu(pauseMenu);
                        gameState = GameState.Paused;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        foreach (PlayerCharacter player in players)
                            player.update = false;
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
                if (playerOne.currentHealth <= 0 || playerTwo.currentHealth <= 0)
                {
                    Game.Exit();
                }

                    System.Diagnostics.Debug.WriteLine(playerOne.hitbox.Top);
                    if (playerTwo.newHitbox.Intersects(playerOne.collisionRect))
                    {
                        playerTwo.Collision(playerOne);
                    }
                    if (playerOne.newHitbox.Intersects(playerTwo.collisionRect))
                    {
                        playerOne.Collision(playerTwo);
                    }

                foreach (Sprite sprite in platformList)
                {
                    if (sprite.collisionRect.Intersects(playerOne.collisionRect))
                    {
                        playerOne.Collision(sprite);
                        sprite.Collision(playerOne);
                    }
                    if (sprite.collisionRect.Intersects(playerTwo.collisionRect))
                    {
                        playerTwo.Collision(sprite);
                        sprite.Collision(playerTwo);
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
                spriteBatch.Draw(p1Cursor, cursorPositions[0], Color.Red);
                particleEngine.Draw(spriteBatch);
            }
            if (gameState == GameState.CharSelect)
            {
                game.IsMouseVisible = true;
                CharSelectButtonOnePosition = charSelectMenu[0, 0];
                CharSelectButtonTwoPosition = charSelectMenu[0, 1];
                spriteBatch.Draw(startButton, CharSelectButtonOnePosition, Color.White);
                spriteBatch.Draw(exitButton, CharSelectButtonTwoPosition, Color.White);
                spriteBatch.Draw(p1Cursor, cursorPositions[0], Color.Red);
                spriteBatch.Draw(p2Cursor, cursorPositions[1], Color.Blue);
                particleEngine.Draw(spriteBatch);
            }
            if (gameState == GameState.Paused)
            {
                game.IsMouseVisible = true;
                resumeButtonPosition = pauseMenu[0, 0];
                exitButtonPosition = pauseMenu[0, 1];
                spriteBatch.Draw(resumeButton, resumeButtonPosition, Color.White);
                spriteBatch.Draw(exitButton, exitButtonPosition, Color.White);
                spriteBatch.Draw(p1Cursor, cursorPositions[0], Color.Red);
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
            spriteBatch.DrawString(font, playerOne.CHARACTER_NAME + " " + playerOne.currentHealth, new Vector2(100, 100), Color.Black);
            spriteBatch.DrawString(font, playerTwo.currentHealth + " " + playerTwo.CHARACTER_NAME, new Vector2(1080, 100), Color.Black);
            if (playerOne.isSuper)
            {
                particleEngine.Draw(spriteBatch);
            }
            playerOne.Draw(gameTime, spriteBatch);
            playerTwo.Draw(gameTime, spriteBatch);
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
            cursorPositions[1] = currentMenu[0, 1];
        }
        void navigateMenu()
        {
            
            //mouse controls
            mouseState = Mouse.GetState();
            particleEngine.EmitterLocation = new Vector2(mouseState.X, mouseState.Y);
            particleEngine.Update();
            if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
            {
                MouseClicked(mouseState.X, mouseState.Y);
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
                    menuSelect(gamepads[i], cursorPositions[i]);
                }
                previousGamePadState[i] = GamePad.GetState(gamepads[i]);
            }
        }

        private void menuSelect(PlayerIndex playerIndex, Vector2 vector2)
        {
            MouseClicked((int) vector2.X, (int) vector2.Y);
        }

        void MouseClicked(int x, int y)
        {
            //creates a rectangle of 10x10 around the place where the mouse was clicked
            Rectangle mouseClickRect = new Rectangle(x, y, 10, 10);

            //check the startmenu
            if (gameState == GameState.StartMenu)
            {
                Rectangle startButtonRect = new Rectangle((int)startButtonPosition.X, (int)startButtonPosition.Y, 100, 20);
                Rectangle exitButtonRect = new Rectangle((int)exitButtonPosition.X, (int)exitButtonPosition.Y, 100, 20);
                Rectangle playerOneCursor = new Rectangle((int)startButtonPosition.X, (int)startButtonPosition.Y, 100, 20);


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

                if (mouseClickRect.Intersects(startButtonRect)) //player clicked start button
                {
                   spawnCharacters();
                   gameState = GameState.Playing;
                }
                else if (mouseClickRect.Intersects(exitButtonRect)) //player clicked exit button
                {
                    game.Exit();
                }
            }
            if (gameState == GameState.Paused)
            {
                Rectangle resumeButtonRect = new Rectangle((int)resumeButtonPosition.X, (int)resumeButtonPosition.Y, 100, 20);
                Rectangle exitButtonRect = new Rectangle((int)exitButtonPosition.X, (int)exitButtonPosition.Y, 100, 20);

                if (mouseClickRect.Intersects(resumeButtonRect))
                {
                    foreach (PlayerCharacter player in players)
                        player.update = true;
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
