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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Singleton
        private static Game1 singleton = null;
        public static Game1 Singleton
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new Game1();
                }

                return singleton;
            }
        }
        #endregion
        public GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;
        public KeyboardState KeyboardState = Keyboard.GetState();
        public KeyboardState PreviousKeyboardState = Keyboard.GetState();
        public MouseState MouseState = Mouse.GetState();
        public MouseState PreviousMouseState = Mouse.GetState();
        public Random Random = new Random();

        public bool InputLocked = false;

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
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

            base.Initialize();
#if !DEBUG
            if (!Graphics.IsFullScreen)
            {
                Graphics.ToggleFullScreen();
            }
#endif
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            StageManager sm = StageManager.Singleton;

            sm.Add("Intro", new Stages.Intro());
            sm.Add("Title", new Stages.Title());
            sm.Add("Menu", new Stages.Menu());
            sm.Add("Shop", new Stages.Shop());
            sm.Add("Bank", new Stages.Bank());
            sm.Add("Easy", new Stages.Easy());
            sm.Add("Normal", new Stages.Normal());
            sm.Add("Hard", new Stages.Hard());
            sm.Add("Result", new Stages.Result());
            sm.Add("Ending", new Stages.Ending());

            sm.NextStage = "Intro";

            CarGarage garage = CarGarage.Singleton;

            garage.Add("Red", new Cars.Red());
            garage.Add("Yellow", new Cars.Yellow());
            garage.Add("Green", new Cars.Green());
            garage.Add("Orange", new Cars.Orange());
            garage.Add("Bus_Blue", new Cars.Bus_Blue());
            garage.Add("Bus_Green", new Cars.Bus_Green());
            garage.Add("Rival_Easy", new Cars.Rival_Easy());
            garage.Add("Rival_Normal", new Cars.Rival_Normal());
            garage.Add("Rival_Hard", new Cars.Rival_Hard());

            Player player = Player.Singleton;
            player.PurchaseCar("Red");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            if (!InputLocked)
            {
                KeyboardState = Keyboard.GetState();
                MouseState = Mouse.GetState();
            }
            else
            {
                KeyboardState = new KeyboardState();
                MouseState = new MouseState();
            }

            float deltaTime = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            StageManager.Singleton.Update(deltaTime);

            PreviousKeyboardState = KeyboardState;
            PreviousMouseState = MouseState;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            SpriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            StageManager.Singleton.Draw();

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
