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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace OverDrive
{
    class Stage
    {
        protected HashSet<Entity> entities = new HashSet<Entity>();
        protected HashSet<Entity> pausedEntities = new HashSet<Entity>();
        protected float currentTime = 0.0f;
        protected float inputDelayTime = 0.2f;
        public bool Paused = true;

        public virtual void Init()
        {
            Paused = false;

            Entity[] arr;

            arr = entities.ToArray();
            foreach (Entity e in arr)
            {
                e.Init();
            }

            arr = pausedEntities.ToArray();
            foreach (Entity e in arr)
            {
                e.Init();
            }
        }

        public virtual void Dispose()
        {
            entities.Clear();
        }

        public virtual void Update(float deltaTime)
        {
            Entity[] arr;

            if (!Paused)
            {
                arr = entities.ToArray();
                foreach (Entity e in arr)
                {
                    e.Update(deltaTime);
                    AdditionalEntityUpdates(e, deltaTime);
                }
                AdditionalUpdates(deltaTime);
                if (inputDelayTime > currentTime)
                {
                    currentTime += deltaTime;
                }
                else
                {
                    Game1.Singleton.InputLocked = false;
                }   
            }
            else
            {
                arr = pausedEntities.ToArray();

                foreach (Entity e in arr)
                {
                    e.Update(deltaTime);
                    AdditionaPausedEntityUpdates(e, deltaTime);
                }
                AdditionalPausedUpdates(deltaTime);
             
            }

        }

        public virtual void Draw()
        {
            Entity[] arr;
            
            arr = entities.ToArray();

            foreach (Entity e in arr)
            {
                e.Draw();
            }

            if (Paused)
            {
                arr = pausedEntities.ToArray();

                foreach (Entity e in arr)
                {
                    e.Draw();
                }
            }
        }

        public virtual void AdditionalUpdates(float deltaTime)
        {
            //Nothing to do!
        }

        public virtual void AdditionalEntityUpdates(Entity e, float deltaTime)
        {
            //Nothing to do!
        }

        public virtual void AdditionalPausedUpdates(float deltaTime)
        {
            //Nothing to do!
        }

        public virtual void AdditionaPausedEntityUpdates(Entity e, float deltaTime)
        {
            //Nothing to do!
        }
    }
}
