using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OverDrive
{
    class StageManager
    {
        #region Singleton
        private static StageManager singleton = null;
        public static StageManager Singleton
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new StageManager();
                }

                return singleton;
            }
        }
        #endregion
        private Dictionary<string, Stage> stages = new Dictionary<string, Stage>();
        private Stage currentStage = null;
        public string NextStage = null;

        public void Update(float deltaTime)
        {
            
            if (NextStage != null)
            {
                if (currentStage != null)
                {
                    currentStage.Dispose();
                }

                if (NextStage == "Exit")
                {
                    Game1.Singleton.Exit();
                    return;
                }
                else
                {
                    currentStage = Get(NextStage);
                    if (currentStage != null)
                    {
                        currentStage.Init();
                    }

                    NextStage = null;
                }

            }

            if (currentStage != null)
            {
                currentStage.Update(deltaTime);
            }
        }

        public void Draw()
        {
            if (currentStage != null)
            {
                currentStage.Draw();
            }
        }

        public void Add(string stageName, Stage stage)
        {
            stages[stageName] = stage;
            Game1.Singleton.KeyboardState = new Microsoft.Xna.Framework.Input.KeyboardState();
            Game1.Singleton.InputLocked = true;
        }

        public Stage Get(string stageName)
        {
            Stage stage;

            if (stages.TryGetValue(stageName, out stage))
            {
                return stage;
            }

            return null;
        }
    }
}
