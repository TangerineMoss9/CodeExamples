using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Midterm
{
    class ModelClass
    {
        public Model model { get; protected set; }
        protected Matrix world = Matrix.Identity;

        public ModelClass(Model m)
        {
            model = m;

        }
        public virtual void Update() { }

        public virtual void Draw(Camera camera)
        {

            Matrix[] transforms = new Matrix[model.Bones.Count];

            model.CopyAbsoluteBoneTransformsTo(transforms);


            foreach (ModelMesh mesh in model.Meshes)
            {

                foreach (BasicEffect be in mesh.Effects)
                {
                    be.EnableDefaultLighting();
                    be.Projection = camera.projection;
                    be.View = camera.view;

                    be.World = GetWorld() * mesh.ParentBone.Transform;
                }


                mesh.Draw();
            }
        }

        public virtual Matrix GetWorld()
        {
            return world;
        }

        public bool CollidesWith(Model otherModel, Matrix otherWorld)
        {

            foreach (ModelMesh myModelMeshes in model.Meshes)
            {
                foreach (ModelMesh hisModelMeshes in otherModel.Meshes)
                {
                    if (myModelMeshes.BoundingSphere.Transform(
                    GetWorld()).Intersects(
                    hisModelMeshes.BoundingSphere.Transform(otherWorld)))
                        return true;
                }
            }
            return false;
        }
    }

}
