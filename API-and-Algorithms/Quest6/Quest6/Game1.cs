using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;

namespace Quest6
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //Pointers to Graphics 
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        int x;
        int y;
        float NyDir;
        float NxDir;



        Random r = new Random();
        
        
       


        //NEW-> Connects and Manages our Sprite classes (calls Updates/Draws)
        public SpriteManager spriteManager;

        

        public Game1()
        {
            //Typical instantiation and settings
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //Typical instantiation
            spriteManager = new SpriteManager(this);

            //NEW ->This connects us to the SpriteManager class
            Components.Add(spriteManager);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            for (int i = 0; i < 20; i++) { 
            x = r.Next(0, 500);
            y = r.Next(0, 500);
            NxDir = r.Next(1, 20) / 10;
            NyDir = r.Next(1, 20) / 10;
            spriteManager.spawn(x, y, NxDir, NyDir);
            }




        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {


            //if (respawnTime >= 2)
            //{
            //    x = r.Next(0, 500);
            //    y = r.Next(0, 500);
            //    NxDir = r.Next(1, 20) / 10;
            //    NyDir = r.Next(1, 20) / 10;

            //    spriteManager.spawn(x, y, NxDir, NyDir);

            //    respawnTime = 0;
            //}



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //Clear the screen
            GraphicsDevice.Clear(Color.White);
            
            //Base draw handles all base work for drawing
            base.Draw(gameTime);
        }
    }
}
