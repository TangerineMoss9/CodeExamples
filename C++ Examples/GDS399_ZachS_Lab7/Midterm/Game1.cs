using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Midterm
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private SpriteFont font;
        private float startTime = 0;
        bool lose = false;

        public enum GameState { START, PLAY, PAUSE, INSTRUCT, END, WIN };
        public GameState currentGameState = GameState.START;
        SplashScreen splashScreen;

        int score = 0;
        float pauseTimer = 0;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        float shotSpeed = 10;
        int shotDelay = 700;
        int shotCountdown = 0;
        SpriteFont scoreFont;

        VertexPositionTexture[] verts;
        VertexBuffer vertexBuffer;

        BasicEffect effect;

        Matrix worldTranslation = Matrix.Identity;
        Matrix worldRotation = Matrix.Identity;

        SamplerState clampTextureAddressMode;

        Texture2D texture;

        ModelManager modelManager;

        public Camera camera { get; protected set; }
        public Random rnd { get; protected set; }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
           
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 1024;

            #if !DEBUG
                graphics.IsFullScreen = true;
            #endif
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            rnd = new Random();

            camera = new Camera(this, new Vector3(0, 0, 20),
                Vector3.Zero, Vector3.Up);
            Components.Add(camera);

            modelManager = new ModelManager(this);
            Components.Add(modelManager);

            splashScreen = new SplashScreen(this);
            Components.Add(splashScreen);
            splashScreen.SetData("Destroy all the enemies before they escape!",
                currentGameState);

            modelManager.Enabled = false;
            modelManager.Visible = false;

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

            scoreFont = Content.Load<SpriteFont>(@"test/Fonts\ScoreFont");


            verts = new VertexPositionTexture[36];

            verts[0] = new VertexPositionTexture(
                new Vector3(500, 500, 500), new Vector2(0, 0));
            verts[1] = new VertexPositionTexture(
                new Vector3(-500, 500, 500), new Vector2(1, 0));
            verts[2] = new VertexPositionTexture(
                new Vector3(500, -500, 500), new Vector2(0, 1));

            verts[3] = new VertexPositionTexture(
                new Vector3(-500, -500, 500), new Vector2(1, 1));
            verts[4] = new VertexPositionTexture(
                new Vector3(500, -500, 500), new Vector2(0, 1));
            verts[5] = new VertexPositionTexture(
                new Vector3(-500, 500, 500), new Vector2(1, 0));

            //side 2
            verts[6] = new VertexPositionTexture(
                new Vector3(-500, 500, 500), new Vector2(0, 0));
            verts[7] = new VertexPositionTexture(
                new Vector3(-500, 500, -500), new Vector2(1, 0));
            verts[8] = new VertexPositionTexture(
                new Vector3(-500, -500, 500), new Vector2(0, 1));

            verts[9] = new VertexPositionTexture(
                new Vector3(-500, -500, -500), new Vector2(1, 1));
            verts[10] = new VertexPositionTexture(
                new Vector3(-500, -500, 500), new Vector2(0, 1));
            verts[11] = new VertexPositionTexture(
                new Vector3(-500, 500, -500), new Vector2(1, 0));

            //side 3
            verts[12] = new VertexPositionTexture(
                new Vector3(-500, 500, -500), new Vector2(0, 0));
            verts[13] = new VertexPositionTexture(
                new Vector3(500, 500, -500), new Vector2(1, 0));
            verts[14] = new VertexPositionTexture(
                new Vector3(-500, -500, -500), new Vector2(0, 1));

            verts[15] = new VertexPositionTexture(
                new Vector3(500, -500, -500), new Vector2(1, 1));
            verts[16] = new VertexPositionTexture(
                new Vector3(-500, -500, -500), new Vector2(0, 1));
            verts[17] = new VertexPositionTexture(
                new Vector3(500, 500, -500), new Vector2(1, 0));

            //side 4
            verts[18] = new VertexPositionTexture(
                new Vector3(500, 500, -500), new Vector2(0, 0));
            verts[19] = new VertexPositionTexture(
                new Vector3(500, 500, 500), new Vector2(1, 0));
            verts[20] = new VertexPositionTexture(
                new Vector3(500, -500, -500), new Vector2(0, 1));

            verts[21] = new VertexPositionTexture(
                new Vector3(500, -500, 500), new Vector2(1, 1));
            verts[22] = new VertexPositionTexture(
                new Vector3(500, -500, -500), new Vector2(0, 1));
            verts[23] = new VertexPositionTexture(
                new Vector3(500, 500, 500), new Vector2(1, 0));

            


            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionTexture), verts.Length, BufferUsage.None);
            vertexBuffer.SetData(verts);

            effect = new BasicEffect(GraphicsDevice);

            clampTextureAddressMode = new SamplerState
            {
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp
            };

            texture = Content.Load<Texture2D>(@"Test/Textures\space");

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
            if (pauseTimer > 1)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.P))
                {
                    pauseTimer = 0;
                    if (currentGameState == GameState.PLAY)
                    {
                        ChangeGameState(GameState.PAUSE);
                    }
                    else if (currentGameState == GameState.PAUSE)
                    {
                        ChangeGameState(GameState.PLAY);
                    }
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.I))
                {
                    pauseTimer = 0;
                    if (currentGameState == GameState.PLAY || currentGameState == GameState.START)
                    {
                        ChangeGameState(GameState.INSTRUCT);
                    }
                    else if (currentGameState == GameState.INSTRUCT)
                    {
                        ChangeGameState(GameState.PLAY);
                    }
                    
                }
            }

            if (currentGameState == GameState.PLAY )
            {
                pauseTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                FireShots(gameTime);
            }
            if (currentGameState == GameState.PAUSE || currentGameState == GameState.START || currentGameState == GameState.INSTRUCT)
            {
                pauseTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            // TODO: Add your update logic here
            FireShots(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SamplerStates[0] = clampTextureAddressMode;

            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.SetVertexBuffer(vertexBuffer);
            // TODO: Add your drawing code here
            effect.World = worldRotation * worldTranslation;

            effect.View = camera.view;
            effect.Projection = camera.projection;
            effect.Texture = texture;
            effect.TextureEnabled = true;

            


                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>
                    (PrimitiveType.TriangleList, verts, 0, 12);
            }

            if (currentGameState == GameState.PLAY)
            {

                // TODO: Add your drawing code here
                spriteBatch.Begin();



                string scoreText = "Score: " + score;
                spriteBatch.DrawString(scoreFont, scoreText,
                 new Vector2(10, 10), Color.Red);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }


        protected void FireShots(GameTime gameTime)
        {
            if (shotCountdown <= 0)
            {

                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {

                    modelManager.AddShot(
                    camera.cameraPosition + new Vector3(0, -1, 0),
                    camera.GetCameraDirection * shotSpeed);

                    shotCountdown = shotDelay;
                }
                else if (Mouse.GetState().RightButton == ButtonState.Pressed)
                {

                    modelManager.AddShot(
                    camera.cameraPosition + new Vector3(0, -1, 0),
                    camera.GetCameraDirection * shotSpeed * 2.5f);

                    shotCountdown = shotDelay + 300;
                }

            }
            else
                shotCountdown -= gameTime.ElapsedGameTime.Milliseconds;
        }
        public void ChangeGameState(GameState state)
        {
            

            currentGameState = state;
            switch (currentGameState)
            {
                case GameState.INSTRUCT:
                    splashScreen.SetData("Instructions", 
                    GameState.INSTRUCT);
                    modelManager.Enabled = false;
                    modelManager.Visible = true;
                    splashScreen.Enabled = true;
                    splashScreen.Visible = true;
                    break;
                case GameState.PLAY:
                    modelManager.Enabled = true;
                    modelManager.Visible = true;
                    splashScreen.Enabled = false;
                    splashScreen.Visible = false;
                    break;
                case GameState.END:
                    splashScreen.SetData("Game Over" +
                    "\nScore: " + score, GameState.END);
                    modelManager.Enabled = false;
                    modelManager.Visible = false;
                    splashScreen.Enabled = true;
                    splashScreen.Visible = true;
                    break;
                case GameState.WIN:
                    splashScreen.SetData("You Win" +
                    "\nScore: " + score, GameState.WIN);
                    modelManager.Enabled = false;
                    modelManager.Visible = false;
                    splashScreen.Enabled = true;
                    splashScreen.Visible = true;
                    break;
                case GameState.PAUSE:
                    splashScreen.SetData("Paused", GameState.PAUSE);
                    modelManager.Enabled = false;
                    modelManager.Visible = true;
                    splashScreen.Enabled = true;
                    splashScreen.Visible = true;
                    break;
            }
        }
        public void AddPoints(int points)
        {
            score += points;
        }

    }
}
