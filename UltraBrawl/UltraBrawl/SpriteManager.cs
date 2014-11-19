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


        SpriteFont font;

        public SpriteManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
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

            loadLevel();
        }

        protected void loadLevel(){
            spawnCharacters();

            platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(100, 400)));
            platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(200, 600)));
            platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(800, 600)));
            platformList.Add(new Platform(Game.Content.Load<Texture2D>(@"Images/BlankPlatform"), new Vector2(900, 400)));

            background = Game.Content.Load<Texture2D>(@"Images/background");
            base.LoadContent();
        }

        protected void spawnCharacters()
        {

            playerOne = new Goku(Game.Content.Load<Texture2D>(@"Images/Goku"), Game.Content.Load<SoundEffect>(@"Sound/Dragonball Z Charge Sound"), Game.Content.Load<SoundEffect>(@"Sound/SSloop"), PlayerIndex.One, controllerOne, spawnLoc1);
            playerTwo = new Goku(Game.Content.Load<Texture2D>(@"Images/Goku"), Game.Content.Load<SoundEffect>(@"Sound/Dragonball Z Charge Sound"), Game.Content.Load<SoundEffect>(@"Sound/SSloop"), PlayerIndex.Two, controllerTwo, spawnLoc2);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            playerOne.Update(gameTime, Game.Window.ClientBounds);
            playerTwo.Update(gameTime, Game.Window.ClientBounds);

            if (playerOne.currentHealth <= 0 || playerTwo.currentHealth <= 0)
            {
                Game.Exit();
            }

            // update each automated sprite
            foreach (Sprite sprite in spriteList)
                sprite.Update(gameTime, Game.Window.ClientBounds);

            // collision
            foreach (Sprite sprite in spriteList)
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
                if (playerTwo.collisionRect.Intersects(playerOne.collisionRect))
                {
                    playerTwo.Collision(playerOne);
                    playerOne.Collision(playerTwo);
                }

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
                if (playerTwo.collisionRect.Intersects(playerOne.collisionRect))
                {
                    playerTwo.Collision(playerOne);
                    playerOne.Collision(playerTwo);
                }
                foreach (Sprite sprite2 in spriteList)
                {
                    if (sprite.collisionRect.Intersects(sprite2.collisionRect))
                    {
                        sprite2.Collision(sprite);
                        sprite.Collision(sprite2);
                    }
                }


            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, 1280, 800), Color.White);
            spriteBatch.DrawString(font, playerOne.CHARACTER_NAME +" " + playerOne.currentHealth, new Vector2(100, 100), Color.Black);
            spriteBatch.DrawString(font, playerTwo.currentHealth + " " + playerOne.CHARACTER_NAME, new Vector2(1080, 100), Color.Black);
            playerOne.Draw(gameTime, spriteBatch);
            playerTwo.Draw(gameTime, spriteBatch);
            foreach (Sprite sprite in spriteList)
                sprite.Draw(gameTime, spriteBatch);
            foreach (Sprite sprite in platformList)
                sprite.Draw(gameTime, spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
