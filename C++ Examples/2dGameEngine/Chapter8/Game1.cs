using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using TileEngine;

namespace Chapter8
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player player;

        SpriteFont pericles8;
        Vector2 scorePosition = new Vector2(20, 580);
        Vector2 livePosition = new Vector2(700, 580);


        enum GameState { TitleScreen, Playing, PlayerDead, GameOver };
        GameState gameState = GameState.TitleScreen;

        Vector2 gameOverPosition = new Vector2(350, 300);   
        Vector2 livesPosition = new Vector2(600, 580);      
        Texture2D titleScreen;                              
        

        public List<SoundEffect> sounds = new List<SoundEffect>();

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

            this.graphics.PreferredBackBufferWidth = 800;
            this.graphics.PreferredBackBufferHeight = 600;
            this.graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            sounds.Add(Content.Load<SoundEffect>(@"Sounds/jump"));
            sounds.Add(Content.Load<SoundEffect>(@"Sounds/door"));
            sounds.Add(Content.Load<SoundEffect>(@"Sounds/death"));
            sounds.Add(Content.Load<SoundEffect>(@"Sounds/eDeath"));

            sound.Initialize(sounds);



            TileMap.Initialize(Content.Load<Texture2D>(@"Textures\PlatformTiles"));
            
            //TileMap.SetTileAtCell(3, 3, 1, 10);
            //TileMap.SetTileAtCell(0, 0, 1, 8);
            //TileMap.SetTileAtCell(0, 0, 0, 4);

            
            Camera.WorldRectangle = new Rectangle(0, 0, 160 * 48, 12 * 48);

            Camera.Position = Vector2.Zero; 

            
            Camera.ViewPortWidth = 800;
            Camera.ViewPortHeight = 600;

            player = new Player(Content);
            
            LevelManager.Initialize(Content, player);
            LevelManager.LoadLevel(0);
            pericles8 = Content.Load<SpriteFont>(@"Fonts\Pericles8");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            KeyboardState keyState = Keyboard.GetState();


            // TODO: Add your update logic here
            if (gameState == GameState.TitleScreen)
            {
                if (keyState.IsKeyDown(Keys.Enter))
                {
                    gameState = GameState.Playing;
                }
            }


            if (gameState == GameState.Playing)
            {
                player.Update(gameTime);

                LevelManager.Update(gameTime);

                if (player.LivesRemaining == 0)
                {
                    gameState = GameState.GameOver;
                }
            }

            if (gameState == GameState.GameOver)
            {
                if (keyState.IsKeyDown(Keys.M))
                {
                    gameState = GameState.TitleScreen;
                    player.Score = 0;
                    player.LivesRemaining = 3;
                }
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);


            if (gameState == GameState.TitleScreen)
            {
                spriteBatch.DrawString(
                    pericles8,
                    "Press enter to play",
                    new Vector2(300, 200),
                    Color.White);
            }



            if (gameState == GameState.Playing)
            {
                TileMap.Draw(spriteBatch);

                player.Draw(spriteBatch);


                LevelManager.Draw(spriteBatch);

                spriteBatch.DrawString(
                    pericles8,
                    "Score: " + player.Score.ToString(),
                    scorePosition,
                    Color.White);

                spriteBatch.DrawString(
                    pericles8,
                    "Lives: " + player.LivesRemaining.ToString(),
                    livePosition,
                    Color.White);

                
            }

            if (gameState == GameState.GameOver)
            {
                spriteBatch.DrawString(
                    pericles8,
                    "Game Over, Press M to return to menu",
                    new Vector2(300, 200),
                    Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
