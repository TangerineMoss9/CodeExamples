using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Quest6
{
    class EnemySprite : Sprite
    {
        bool b = true;
        public Vector2 dir;

        int x;
        int y;

        Random r = new Random();

        public enum GameState { Fleeing,Cruising };

        public GameState currentGameState = GameState.Cruising;
        // Sprite is automated. Direction is same as speed
        public override Vector2 direction
        {
            get { return speed; }

        }


        //Constructor
        public EnemySprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed)
        {
        }

        //Constructor using ms Per Frame
        public EnemySprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
            int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame)
        {
        }



        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            // Move sprite based on direction
            if (b)
            {
                dir = direction;
                b = false;
            }

            position += dir;

            if (position.X < 0)
            {
                position.X = 0;
                if (currentGameState == GameState.Fleeing)
                {
                    currentGameState = GameState.Cruising;

                    x = r.Next(5, 11) / 5;
                    y = r.Next(-10, 11) / 5;

                    while (y == 0)
                    {
                        y = r.Next(-10, 11) / 5;

                    }

                    dir = new Vector2(x, y);
                }
                else
                {
                    dir.X *= -1;
                }

            }
            if (position.Y < 0)
            {
                position.Y = 0;
                if (currentGameState == GameState.Fleeing)
                {
                    currentGameState = GameState.Cruising;

                    x = r.Next(-10, 11) / 5;
                    y = r.Next(5, 11) / 5;

                    while (x == 0)
                    {
                        x = r.Next(-10, 11) / 5;

                    }

                    dir = new Vector2(x, y);
                }
                else
                {
                    dir.Y *= -1;
                }
            }
            if (position.X > clientBounds.Width - frameSize.X)
            {
                position.X = clientBounds.Width - frameSize.X;
                if (currentGameState == GameState.Fleeing)
                {
                    currentGameState = GameState.Cruising;

                    x = r.Next(-10, -5) / 5;
                    y = r.Next(-10, 11) / 5;

                    while (x == 1)
                    {
                        y = r.Next(-10, 11) / 5;

                    }

                    dir = new Vector2(x, y);
                }
                else
                {
                    dir.X *= -1;
                }
            }

            if (position.Y > clientBounds.Height - frameSize.Y)
            {
                position.Y = clientBounds.Height - frameSize.Y;
                if (currentGameState == GameState.Fleeing)
                {
                    currentGameState = GameState.Cruising;

                    x = r.Next(-10, 11) / 5;
                    y = r.Next(-10, -5) / 5;

                    while (x == 0)
                    {
                        x = r.Next(-10, 11) / 5;

                    }

                    dir = new Vector2(x, y);
                }
                else
                {
                    dir.Y *= -1;
                }
            }


            //Call base update (Sprite class)
            base.Update(gameTime, clientBounds);
        }
    }
}
