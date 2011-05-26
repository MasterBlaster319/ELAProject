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

namespace ELAProject
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
       
        Texture2D spaceTexture;
        Rectangle ViewportRect;
        GameObject Frat;
        SpriteBatch spriteBatch;
        
        KeyboardState previousKeyboardState = Keyboard.GetState();
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);


            spaceTexture = Content.Load<Texture2D>("Sprites\\space");
            ViewportRect = new Rectangle(0, 0,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);
            
            Frat = new GameObject(Content.Load<Texture2D>("Sprites\\frat"));
            Frat.position = new Vector2(250, graphics.GraphicsDevice.Viewport.Height - 250);



            // TODO: use this.Content to load your game content here
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            if (previousKeyboardState.IsKeyDown(Keys.S))
            {
                Frat.position.Y = Frat.position.Y + 1.75f;
            }
            else if (previousKeyboardState.IsKeyDown(Keys.A))
                Frat.position.X = Frat.position.X - 1.75f;
            else if (previousKeyboardState.IsKeyDown(Keys.W))
                Frat.position.Y = Frat.position.Y - 1.75f;
            else if (previousKeyboardState.IsKeyDown(Keys.D))
                Frat.position.X = Frat.position.X + 1.75f;
            
            
            
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            spriteBatch.Draw(spaceTexture, ViewportRect, Color.White);
            
            spriteBatch.Draw(Frat.sprite,
                Frat.position,
                null,
                Color.White);
            
            
            
            
            
            spriteBatch.End();
            


            // TODO: Add your drawing code here

            
            base.Draw(gameTime);
            
        }
    }
}
