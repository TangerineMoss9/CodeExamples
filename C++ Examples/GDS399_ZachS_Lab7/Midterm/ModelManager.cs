using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
namespace Midterm
{
    class ModelManager : DrawableGameComponent
    {

        List<ParticleExplosion> explosions = new List<ParticleExplosion>();

        ParticleExplosionSettings particleExplosionSettings = new ParticleExplosionSettings();
        ParticleSettings particleSettings = new ParticleSettings();
        Texture2D explosionTexture;
        Texture2D explosionColorsTexture;
        Effect explosionEffect;

        const int pointsPerKill = 100;

        List<ModelClass> shots = new List<ModelClass>();
        float shotMinZ = -1000;
        public int enemiesLeft;
        public bool lose = false;
        List<ModelClass> models = new List<ModelClass>();

        public ModelManager(Game game)
            : base(game)
        {


        }
        public override void Initialize()
        {

            spawnStaticEnemy(new Vector3(-10, 0, -10));
            spawnStaticEnemy(new Vector3(10, 0, -10));


            Random r = new Random();
            int x = r.Next(-450,450);
            spawnMovingEnemy(new Vector3(x, 0, -490), new Vector3(0,0,1), MathHelper.ToRadians(90));
            x = r.Next(-450,450);
            spawnMovingEnemy(new Vector3(x, 0, -490), new Vector3(0, 0,1 ), MathHelper.ToRadians(270));
            x = r.Next(-450, 450);
            spawnMovingEnemy(new Vector3(x, 0, -490), new Vector3(0, 0, 1), 0);
            x = r.Next(-450, 450);
            spawnMovingEnemy(new Vector3(x, 0, -490), new Vector3(0, 0, 1), MathHelper.ToRadians(180));
            base.Initialize();
        }
        protected override void LoadContent()
        {

            explosionTexture = Game.Content.Load<Texture2D>(@"Test/Textures\Particle");
            explosionColorsTexture = Game.Content.Load<Texture2D>(@"Test/Textures\ParticleColors");
            explosionEffect = Game.Content.Load<Effect>(@"Test/effects\Particle");

            explosionEffect.CurrentTechnique = explosionEffect.Techniques["Technique1"];
            explosionEffect.Parameters["theTexture"].SetValue(explosionTexture);
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {

            UpdateModels();

            UpdateShots();

            UpdateExplosions(gameTime);

            if (enemiesLeft == 0)
            {
                ((Game1)Game).ChangeGameState(Game1.GameState.WIN);
            }

            base.Update(gameTime);
        }

        protected void UpdateModels()
        {
            enemiesLeft = 0;
            for (int i = 0; i < models.Count; ++i)
            {
                
                models[i].Update();
                enemiesLeft++;
                if (models[i].GetWorld().Translation.Z < -490 ||
                    models[i].GetWorld().Translation.Z > 490 ||
                    models[i].GetWorld().Translation.X < -490 ||
                    models[i].GetWorld().Translation.X > 490)
                {
                    ((Game1)Game).ChangeGameState(Game1.GameState.END);
                }

            }
        }
        public override void Draw(GameTime gameTime)
        {

            foreach (ModelClass bm in models)
            {
                bm.Draw(((Game1)Game).camera);
            }
            foreach (ModelClass bm in shots)
            {
                bm.Draw(((Game1)Game).camera);
            }

            foreach (ParticleExplosion pe in explosions)
            {
                pe.Draw(((Game1)Game).camera);
            }
            base.Draw(gameTime);
        }


        public void spawnStaticEnemy(Vector3 pos)
        {
            models.Add(new Enemy(
                Game.Content.Load<Model>(@"test/spaceship"),
                pos, new Vector3(0, 0, 0f), 0, 0, 0));
        }
        public void spawnMovingEnemy(Vector3 pos, Vector3 dir, float yaw)
        {
            models.Add(new Enemy(
                Game.Content.Load<Model>(@"test/spaceship"),
                pos, dir, yaw, 0, 0));
        }

        public void AddShot(Vector3 position, Vector3 direction)
        {
            shots.Add(new Bullet(
            Game.Content.Load<Model>(@"test\ammo"),
            position, direction, 0, 0, 0));
        }

        protected void UpdateShots()
        {
            
            for (int i = 0; i < shots.Count; ++i)
            {

                shots[i].Update();
                if (shots[i].GetWorld().Translation.Z < shotMinZ|| lose == true)
                {
                    shots.RemoveAt(i);
                    --i;
                }
                
                else
                {

                    for (int j = 0; j < models.Count; ++j)
                    {
                        if (shots[i].CollidesWith(models[j].model,
                        models[j].GetWorld()))
                        {

                            explosions.Add(new ParticleExplosion(GraphicsDevice,
                            models[j].GetWorld().Translation,
                            ((Game1)Game).rnd.Next(
                            particleExplosionSettings.minLife,
                            particleExplosionSettings.maxLife),
                            ((Game1)Game).rnd.Next(
                            particleExplosionSettings.minRoundTime,
                            particleExplosionSettings.maxRoundTime),
                            ((Game1)Game).rnd.Next(
                            particleExplosionSettings.minParticlesPerRound,
                            particleExplosionSettings.maxParticlesPerRound),
                            ((Game1)Game).rnd.Next(
                            particleExplosionSettings.minParticles,
                            particleExplosionSettings.maxParticles),


                            explosionColorsTexture, particleSettings,
                            explosionEffect));

                            ((Game1)Game).AddPoints(pointsPerKill);
                            models.RemoveAt(j);
                            shots.RemoveAt(i);
                            --i;
                            break;
                        }
                    }

                    
                }


            }
        }
        protected void UpdateExplosions(GameTime gameTime)
        {
            // Loop through and update explosions
            for (int i = 0; i < explosions.Count; ++i)
            {
                explosions[i].Update(gameTime);
                // If explosion is finished, remove it
                if (explosions[i].IsDead)
                {
                    explosions.RemoveAt(i);
                    --i;
                }
            }
        }
    }
}
