using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Lidgren.Network;
using System.Collections.Generic;
using System.Diagnostics;


namespace NetworkGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        static NetClient Client;

        static List<Character> GameStateList;

        static List<Bullet> Bullets;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        double UpdateTime = 50, Counter = 0;

        bool CanStart = false;

        NetIncomingMessage inc;

        Texture2D playerImg;
        Texture2D bulletImg;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();


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
            string hostip = "127.0.0.1";

            NetPeerConfiguration Config = new NetPeerConfiguration("game");
            Config.AutoFlushSendQueue = false;

            Client = new NetClient(Config);

            NetOutgoingMessage outmsg = Client.CreateMessage();

            Client.Start();

            outmsg.Write((byte)PacketTypes.LOGIN);

            outmsg.Write("MyName");

            Client.Connect(hostip, 14242, outmsg);

            

            Debug.WriteLine("Client Started");

            

            GameStateList = new List<Character>();
            Bullets = new List<Bullet>();

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

            playerImg = Content.Load<Texture2D>("Images/LegoR2");
            bulletImg = Content.Load<Texture2D>("Images/Bullet");

            // TODO: use this.Content to load your game content here
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back 
                == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            if (CanStart == false)
            {
                WaitForStartingInfo();
            }
            else
            {

                foreach (Bullet b in Bullets)
                {
                    b.posX += b.velX;
                    b.posY += b.velY;
                }

                GetInputAndSendItToServer();

                Counter += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (Counter >= UpdateTime)
                {
                    Counter -= UpdateTime;

                    CheckServerMessages();
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

            spriteBatch.Begin();

            foreach (Character ch in GameStateList)
            {
                spriteBatch.Draw(playerImg, new Vector2(ch.X, ch.Y),
                    Color.White);
            }

            foreach (Bullet b in Bullets)
            {
                

                spriteBatch.Draw(bulletImg, new Vector2(b.posX, b.posY), Color.Red);

            }

            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        private void CheckServerMessages()
        {
            while((inc = Client.ReadMessage()) != null){
                if( inc.MessageType == NetIncomingMessageType.ConnectionApproval)
                {

                }
                else if (inc.MessageType == NetIncomingMessageType.Data)
                {
                    byte tByte = inc.ReadByte();
                    if (tByte == (byte)PacketTypes.WORLDSTATE)
                    {
                        GameStateList.Clear();
                        int jii = 0;
                        jii = inc.ReadInt32();
                        for(int i = 0; i < jii; i++)
                        {
                            Character ch = new Character();
                            inc.ReadAllProperties(ch);
                            GameStateList.Add(ch);
                        }
                    }
                    else if (tByte == (byte)PacketTypes.FIREWEAPON)
                    {
                        Bullets.Clear();
                        int jii = 0;
                        jii = inc.ReadInt32();
                        for (int i = 0; i < jii; i++)
                        {
                            Bullet tBullet = new Bullet();
                            inc.ReadAllProperties(tBullet);
                            Bullets.Add(tBullet);
                        }
                    }
                }
            }
        }


        private void WaitForStartingInfo()
        {
            CanStart = false;

            NetIncomingMessage inc;
            

            while (!CanStart)
            {
                if ((inc = Client.ReadMessage()) != null)
                {
                    
                    switch (inc.MessageType)
                    {
                        case NetIncomingMessageType.Data:

                            if(inc.ReadByte() == (byte)
                                PacketTypes.WORLDSTATE)
                            {

                                Debug.WriteLine("Ayoooo it worked");
                                GameStateList.Clear();

                                int count = 0;

                                count = inc.ReadInt32();

                                for(int i = 0; i< count; i++)
                                {
                                    Character ch = new Character();

                                    inc.ReadAllProperties(ch);

                                    GameStateList.Add(ch);
                                }

                                CanStart = true;
                            }
                            break;

                        default:
                            Debug.WriteLine(inc.ReadString() + " Strange message");
                            
                            
                            CanStart = true;
                            break;
                    }
                }
            }
        }

        private static void GetInputAndSendItToServer()
        {
            MoveDirection MoveDir = new MoveDirection();

            MoveDir = MoveDirection.NONE;

            KeyboardState kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.W))
                MoveDir = MoveDirection.UP;
            if (kb.IsKeyDown(Keys.S))
                MoveDir = MoveDirection.DOWN;
            if (kb.IsKeyDown(Keys.A))
                MoveDir = MoveDirection.LEFT;
            if (kb.IsKeyDown(Keys.D))
                MoveDir = MoveDirection.RIGHT;

            if (kb.IsKeyDown(Keys.Q))
            {
                Client.Disconnect("bye bye");
            }

            if (MoveDir != MoveDirection.NONE)
            {
                NetOutgoingMessage outmsg = Client.CreateMessage();

                outmsg.Write((byte)PacketTypes.MOVE);

                outmsg.Write((byte)MoveDir);

                Client.SendMessage(outmsg, NetDeliveryMethod.ReliableOrdered);
                Client.FlushSendQueue();

                MoveDir = MoveDirection.NONE;
            }
            MoveDirection FireDir = new MoveDirection();

            
            FireDir = MoveDirection.NONE;


            if (kb.IsKeyDown(Keys.Right))
                FireDir = MoveDirection.RIGHT;
            else if (kb.IsKeyDown(Keys.Left))
                FireDir = MoveDirection.LEFT;
            else if (kb.IsKeyDown(Keys.Up))
                FireDir = MoveDirection.UP;
            else if (kb.IsKeyDown(Keys.Down))
                FireDir = MoveDirection.DOWN;



            
            if (FireDir != MoveDirection.NONE)
            {
                
                NetOutgoingMessage outmsg = Client.CreateMessage();

                
                outmsg.Write((byte)PacketTypes.FIREWEAPON);

               
                outmsg.Write((byte)FireDir);

                
                Client.SendMessage(outmsg, NetDeliveryMethod.ReliableOrdered);
                Client.FlushSendQueue();

                
                FireDir = MoveDirection.NONE;
            }

        }


    }

    class Character
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; set; }
        public NetConnection Connection { get; set; }


        public Character(string name, int x, int y, NetConnection conn)
        {
            Name = name;
            X = x;
            Y = y;
            Connection = conn;
        }
        public Character()
        {
        }
    }

    class Bullet
    {
        public int posX { get; set; }
        public int posY { get; set; }
        public int velX { get; set; }
        public int velY { get; set; }
        public NetConnection Connection { get; set; }

        public Bullet(int posx, int posy, int velx, int vely, NetConnection conn)
        {
            posX = posx;
            posY = posy;
            velX = velx;
            velY = vely;
            Connection = conn;
        }

        public Bullet()
        {

        }

        public void Move()
        {
            posX += velX;
            posY += velY;
        }
    }


    enum PacketTypes
    {
        LOGIN,
        MOVE,
        WORLDSTATE,
        FIREWEAPON,
        UPDATEBULLETS
    }

    enum MoveDirection
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        NONE
    }


}
