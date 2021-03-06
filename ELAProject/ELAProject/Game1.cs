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
    /// <summary> w
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        Texture2D spaceTexture;
        Rectangle ViewportRect;
        GameObject Frat;
        SpriteBatch spriteBatch;
        GameObject[] Rounds;
        const int MAXROUND = 20;
        KeyboardState previousKeyboardState = Keyboard.GetState();

        const int MAXENEMIES = 7;
        const float MAXENEMYHEIGHT = 0.89f;
        const float MINENEMYHEIGHT = 0.01f;
        const float MAXENEMYVELOCITY = 5.0f;
        const float MINENEMYVELOCITY = 1.0f;
        Random random = new Random();
        GameObject[] enemies;

        int score;
        SpriteFont font;
        Vector2 scoreDrawPoint = new Vector2(
            0.1f,
            0.1f);
        MediaLibrary sampleMediaLibrary;
        Random rand;


        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        
        
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.a
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

            Rounds = new GameObject[MAXROUND];
            for (int i = 0; i < MAXROUND; i++)
            {
                Rounds[i] = new GameObject(Content.Load<Texture2D>("Sprites\\round"));
            }

            enemies = new GameObject[MAXENEMIES];
            for (int z = 0; z < MAXENEMIES; z++)
            {
                enemies[z] = new GameObject(
                    Content.Load<Texture2D>("Sprites\\enemy"));
            }
            ViewportRect = new Rectangle(0, 0,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);

            font = Content.Load<SpriteFont>("Fonts\\GameFont");
            {
                sampleMediaLibrary = new MediaLibrary();
                rand = new Random();

                MediaPlayer.Stop();


                int i = rand.Next(0, sampleMediaLibrary.Albums.Count - 1);

                MediaPlayer.Play(sampleMediaLibrary.Albums[i].Songs[0]);
            }
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
            KeyboardState keyboardState = Keyboard.GetState();
            // Trying to get the round to fire in the direction of the arrow key pressed when space and one of the arrows is pressed


            if (keyboardState.IsKeyDown(Keys.A))
                Frat.position.X = Frat.position.X - 5f;
            
            else if (keyboardState.IsKeyDown(Keys.W))
                Frat.position.Y = Frat.position.Y - 5f;
            
            else if (keyboardState.IsKeyDown(Keys.D))
                Frat.position.X = Frat.position.X + 5f;
            
            if (keyboardState.IsKeyDown(Keys.S))
            {
                Frat.position.Y = Frat.position.Y + 5f;
            }

            if (keyboardState.IsKeyDown(Keys.Space) && previousKeyboardState.IsKeyUp(Keys.Space))

                FireRound();

            // TODO: Add your update logic here

            UpdateEnemies();
            UpdateRounds();
            previousKeyboardState = keyboardState;
            base.Update(gameTime);
        }

        public void UpdateEnemies()
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy.alive)
                {
                    enemy.position += enemy.velocity;
                    if (!ViewportRect.Contains(new Point(
                    (int)enemy.position.X,
                    (int)enemy.position.Y)))
                    {
                        enemy.alive = false;
                    }
                }
                else
                {
                    enemy.alive = true;
                    enemy.position = new Vector2(
                        ViewportRect.Right,
                        MathHelper.Lerp(
                        (float)ViewportRect.Height * MINENEMYHEIGHT,
                        (float)ViewportRect.Height * MAXENEMYHEIGHT,
                        (float)random.NextDouble()));
                    enemy.velocity = new Vector2(
                        MathHelper.Lerp(
                        -MINENEMYVELOCITY,
                        -MAXENEMYVELOCITY,
                        (float)random.NextDouble()), 0);
                }
            }
        }


        public void FireRound()
        {
            foreach (GameObject round in Rounds)
            {
                if (!round.alive)
                {
                    round.alive = true;
                    round.position = Frat.position - round.center;
                    round.velocity = new Vector2(
                        (float)Math.Cos(Frat.rotation),
                        (float)Math.Sin(Frat.rotation))
                        * 5.0f;
                    return;


                }
            }
        }

        public void UpdateRounds()
        {
            foreach (GameObject round in Rounds)
            {
                if (round.alive)
                {
                    round.position += round.velocity;
                    if (!ViewportRect.Contains(new Point(
                        (int)round.position.X,
                        (int)round.position.Y)))
                    {
                        round.alive = false;
                        continue;
                    }
                    Rectangle roundRect = new Rectangle(
                        (int)round.position.X,
                        (int)round.position.Y,
                        round.sprite.Width,
                        round.sprite.Height);

                    foreach (GameObject enemy in enemies)
                    {
                        Rectangle enemyRect = new Rectangle(
                            (int)enemy.position.X,
                            (int)enemy.position.Y,
                            enemy.sprite.Width,
                            enemy.sprite.Height);
                        if (roundRect.Intersects(enemyRect))
                        {
                            round.alive = false;
                            enemy.alive = false;
                            score += 5;
                            break;
                        }
                    }
                }
            }
        }





        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            
            spriteBatch.Begin();
            spriteBatch.Draw(spaceTexture, ViewportRect, Color.White);
            foreach (GameObject round in Rounds)
            {
                if (round.alive)
                {
                    spriteBatch.Draw(round.sprite,
                        round.position, Color.White);
                }
            }
            
            spriteBatch.Draw(Frat.sprite,
                Frat.position,
                null,
                Color.White,
                Frat.rotation,
                Frat.center,
                1.0f,
                SpriteEffects.None,
                0);
                


            foreach (GameObject enemy in enemies)
            {
                if (enemy.alive)
                {
                    spriteBatch.Draw(enemy.sprite,
                        enemy.position, Color.White);

                }

            }

            spriteBatch.DrawString(font, "Score: " + score.ToString(),
                new Vector2(scoreDrawPoint.X * ViewportRect.Width,
                    scoreDrawPoint.Y * ViewportRect.Height),
                    Color.Green);



            spriteBatch.End();
            


            // TODO: Add your drawing code here

            
            base.Draw(gameTime);
            
        }
    }
}
