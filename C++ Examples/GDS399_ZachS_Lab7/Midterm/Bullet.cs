using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Midterm
{
    class Bullet : ModelClass
    {
        float yawAngle = 0;
        float pitchAngle = 0;
        float rollAngle = 0;
        Vector3 direction;

        Matrix rotation = Matrix.Identity;
        Matrix translation = Matrix.Identity;

        public Bullet(Model m, Vector3 Position,
            Vector3 Direction, float yaw, float pitch, float roll)
            : base(m)
        {
            world = Matrix.CreateTranslation(Position);
            yawAngle = yaw;
            pitchAngle = pitch;
            rollAngle = roll;
            direction = Direction;
        }

        public override void Update()
        {
            world *= Matrix.CreateTranslation(direction);
        }
        public override Matrix GetWorld()
        {
            return world * rotation;
        }
    }
}
