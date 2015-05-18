using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteManager spriteManager;
        int _total_frames = 0;
        float _elapsed_time = 0.0f;
        int _fps = 0;
        //ParticleEngine2D particleEngine;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            // resize the game
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = true;
            spriteManager = new SpriteManager(this);
            Components.Add(spriteManager);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            // Put the name of the font
            // _spr_font = Content.Load("kootenay");

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            SoundEffect bgEffect;
            bgEffect = Content.Load<SoundEffect>(@"Sound/DBZ Loop");
            SoundEffectInstance introInstance = bgEffect.CreateInstance();
            introInstance.IsLooped = true;
            introInstance.Volume = 0.5f;
            //introInstance.Play();

            // particle stuff
            //List<Texture2D> textures = new List<Texture2D>();
           // textures.Add(Content.Load<Texture2D>("Images/circle"));
           // textures.Add(Content.Load<Texture2D>("Images/star"));
           // textures.Add(Content.Load<Texture2D>("Images/diamond"));
           // particleEngine = new ParticleEngine2D(textures, new Vector2(400, 240));

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Update
            _elapsed_time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // 1 Second has passed
            if (_elapsed_time >= 1000.0f)
            {
                _fps = _total_frames;
                _total_frames = 0;
                _elapsed_time = 0;
            }

            // TODO: Add your update logic here
            //i did dat ^ ^ right dat for particalz
           // particleEngine.EmitterLocation = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
           // particleEngine.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            _total_frames++;

            //System.Diagnostics.Debug.WriteLine("FPS =" + _fps);

            //particleEngine.Draw(spriteBatch);
 

            base.Draw(gameTime);
        }

        private void DrawScenery()
        {
            //spriteBatch.Begin();

            //spriteBatch.End();
        }
    }
}
