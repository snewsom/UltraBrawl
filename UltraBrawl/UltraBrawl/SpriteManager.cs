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

        SpriteBatch spriteBatch;
        PlayerController controllerOne;
        PlayerController controllerTwo;
        List<PlayerCharacter> players = new List<PlayerCharacter>();
        PlayerCharacter playerOne;
        PlayerCharacter playerTwo;

        Vector2 spawnLoc1 = new Vector2(300, 0);
        Vector2 spawnLoc2 = new Vector2(800, 0);
        Vector2 spawnLoc3 = new Vector2(600, 0);
        Vector2 spawnLoc4 = new Vector2(600, 0);

        Sprite platform;

        Texture2D background;

        List<Sprite> spriteList = new List<Sprite>();

        List<Sprite> platformList = new List<Sprite>();

        private Game game;

        ParticleEngine2D particleEngine;

        private Texture2D startButton;
        private Texture2D exitButton;
        private Texture2D resumeButton;
        private Vector2 startButtonPosition;
        private Vector2 exitButtonPosition;
        private Vector2 resumeButtonPosition;
        MouseState mouseState;
        MouseState previousMouseState;
        public GameState gameState;
        public enum GameState
        {
            StartMenu,
            Playing,
            Paused
        }

        SpriteFont font;

        public SpriteManager(Game game)
            : base(game)
        {
            this.game = game;
            gameState = GameState.StartMenu;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            controllerOne = new PlayerController(PlayerIndex.One);
            controllerTwo = new PlayerController(PlayerIndex.Two);
            font = Game.Content.Load<SpriteFont>("Images/dbzFont");


            startButton = Game.Content.Load<Texture2D>(@"Images/start");
            exitButton = Game.Content.Load<Texture2D>(@"Images/exit");

            resumeButton = Game.Content.Load<Texture2D>(@"Images/resume");

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
            playerOne = new Goku(Game.Content.Load<Texture2D>(@"Images/Goku"), Game.Content.Load<SoundEffect>(@"Sound/Dragonball Z Charge Sound"), Game.Content.Load<SoundEffect>(@"Sound/SSloop"), PlayerIndex.One, controllerOne, spawnLoc1);
            players.Add(playerOne);
            playerTwo = new Megaman(Game.Content.Load<Texture2D>(@"Images/Megaman"), Game.Content.Load<SoundEffect>(@"Sound/Dragonball Z Charge Sound"), Game.Content.Load<SoundEffect>(@"Sound/SSloop"), PlayerIndex.Two, controllerTwo, spawnLoc2);
            players.Add(playerTwo);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            
            if (gameState == GameState.StartMenu || gameState == GameState.Paused)
            {
                particleEngine.EmitterLocation = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                particleEngine.Update();
                mouseState = Mouse.GetState();
                if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
                {
                    MouseClicked(mouseState.X, mouseState.Y);
                }
                previousMouseState = mouseState;
            }
            if (gameState == GameState.Paused)
            {
                foreach (PlayerCharacter player in players)
                    player.Update(gameTime, Game.Window.ClientBounds);
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    foreach (PlayerCharacter player in players)
                        player.update = true;
                    gameState = GameState.Playing;
                }
            }

            if (gameState == GameState.Playing)
            {

                foreach (PlayerCharacter player in players)
                {
                    player.Update(gameTime, Game.Window.ClientBounds);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    foreach (PlayerCharacter player in players)
                        player.update = false;
                    gameState = GameState.Paused;
                }

                // update each automated sprite
                foreach (Sprite sprite in spriteList)
                    sprite.Update(gameTime, Game.Window.ClientBounds);

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
                    //foreach (Sprite sprite2 in spriteList)
                    //{
                    //    if (sprite.collisionRect.Intersects(sprite2.collisionRect))
                    //    {
                    //        sprite2.Collision(sprite);
                    //        sprite.Collision(sprite2);
                    //    }
                    //}
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
                startButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 200);
                exitButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 250);
                spriteBatch.Draw(startButton, startButtonPosition, Color.White);
                spriteBatch.Draw(exitButton, exitButtonPosition, Color.White);
                particleEngine.Draw(spriteBatch);
            }
            if (gameState == GameState.Paused)
            {
                game.IsMouseVisible = true;
                resumeButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 200);
                exitButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 250);
                spriteBatch.Draw(resumeButton, resumeButtonPosition, Color.White);
                spriteBatch.Draw(exitButton, exitButtonPosition, Color.White);
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

        void MouseClicked(int x, int y)
        {
            //creates a rectangle of 10x10 around the place where the mouse was clicked
            Rectangle mouseClickRect = new Rectangle(x, y, 10, 10);

            //check the startmenu
            if (gameState == GameState.StartMenu)
            {
                Rectangle startButtonRect = new Rectangle((int)startButtonPosition.X, (int)startButtonPosition.Y, 100, 20);
                Rectangle exitButtonRect = new Rectangle((int)exitButtonPosition.X, (int)exitButtonPosition.Y, 100, 20);

                if (mouseClickRect.Intersects(startButtonRect)) //player clicked start button
                {
                    spawnCharacters();
                    foreach (PlayerCharacter player in players)
                        player.update = true;
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
