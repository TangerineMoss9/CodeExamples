using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Midterm
{
    public class Camera : Microsoft.Xna.Framework.GameComponent
    {
        public Matrix view { get; protected set; }
        public Matrix projection { get; protected set; }

        public Vector3 cameraPosition { get; protected set; }
        Vector3 cameraDirection;
        Vector3 cameraUp;
        Vector3 prevPosition;
        public bool lose = false;

        float speed = 3;

        MouseState prevMouseState;

        public Vector3 GetCameraDirection
        {
            get { return cameraDirection; }
        }

        

        public Camera(Game game, Vector3 pos, Vector3 target, Vector3 up)
            : base(game)
        {
            cameraPosition = pos;
            cameraDirection = target - pos;
            cameraDirection.Normalize();
            cameraUp = up;
            CreateLookAt();
            prevPosition = cameraPosition;


            projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                (float)Game.Window.ClientBounds.Width /
                (float)Game.Window.ClientBounds.Height,
                1, 3000);
        }

        public override void Initialize()
        {
            Mouse.SetPosition(Game.Window.ClientBounds.Width / 2,
                Game.Window.ClientBounds.Height / 2);
            prevMouseState = Mouse.GetState();
            base.Initialize();
            prevPosition = cameraPosition;
        }

        public override void Update(GameTime gameTime)
        {



            if (lose == false)
            {

                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    cameraPosition += new Vector3(cameraDirection.X * speed, 0, cameraDirection.Z * speed);

                }
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                {

                    cameraPosition -= new Vector3(cameraDirection.X * speed, 0, cameraDirection.Z * speed);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    cameraPosition += Vector3.Cross(cameraUp, new Vector3(cameraDirection.X, 0, cameraDirection.Z)) * speed;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    cameraPosition -= Vector3.Cross(cameraUp, new Vector3(cameraDirection.X, 0, cameraDirection.Z)) * speed;
                }

                if (this.cameraPosition.X < -490 || this.cameraPosition.X > 490
                    || this.cameraPosition.Z < -490 || this.cameraPosition.Z > 490)
                {
                    cameraPosition = prevPosition;
                }
            }
            
        

                    cameraDirection = Vector3.Transform(cameraDirection,
                Matrix.CreateFromAxisAngle(cameraUp, (-MathHelper.PiOver4 / 150) *
            (Mouse.GetState().X - prevMouseState.X)));


            //cameraDirection = Vector3.Transform(cameraDirection,
            //    Matrix.CreateFromAxisAngle(Vector3.Cross(cameraUp, cameraDirection),
            //    (MathHelper.PiOver4 / 100) *
            //    (Mouse.GetState().Y - prevMouseState.Y)));

            prevPosition = cameraPosition;
            prevMouseState = Mouse.GetState();


            CreateLookAt();

            base.Update(gameTime);
        }



        private void CreateLookAt()
        {
            view = Matrix.CreateLookAt(cameraPosition,
               cameraPosition + cameraDirection, cameraUp);
        }
    }
}
