using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PrimitiveTest.States;

namespace PrimitiveTest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static SpriteFont SpriteFont;

        private Texture2D[] tankTextures;
        private Texture2D backgroundTexture;
        private Texture2D sandBagTexture;

        //private RectPrimitive rect;

        //private CirclePrimitive circle;

        public StaticMap StaticMap;

        private Bot bot1;
        private Bot bot2;

        public static Random random;

        private List<ToggleAction> toggleActions = new List<ToggleAction>();

        private KeyboardState oldKeyboardState;

        private List<Bot> allBots = new List<Bot>();

        private Bot selectedBot;

        private ShotManager shotManager;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);

            random = new Random();

            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 1200;
            
            Content.RootDirectory = "Content";
            
        }

        public static Vector2 GetRandomPosition()
        {
            float x = random.Next(0, 1200);
            float y = random.Next(0, 1200);

            return new Vector2(x, y);
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
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            SpriteFont = Content.Load<SpriteFont>("font");
            
            shotManager = new ShotManager(StaticMap);

            tankTextures = new Texture2D[5];

            tankTextures[0] = Content.Load<Texture2D>("tank_blue");
            tankTextures[1] = Content.Load<Texture2D>("tank_red");
            tankTextures[2] = Content.Load<Texture2D>("tank_green");
            tankTextures[3] = Content.Load<Texture2D>("tank_dark");
            tankTextures[4] = Content.Load<Texture2D>("tank_sand");

            backgroundTexture = Content.Load<Texture2D>("tileGrass1");
            sandBagTexture = Content.Load<Texture2D>("barricadeWood");

            StaticMap = new StaticMap(sandBagTexture);

            StaticMap.AddRectangle(300, 100, 50, 150);
            StaticMap.AddRectangle(300, 250, 200, 200);
            StaticMap.AddRectangle(900, 150, 50, 350);
            StaticMap.AddRectangle(200, 580, 300, 40);
            StaticMap.AddRectangle(700, 580, 300, 40);
            StaticMap.AddRectangle(250, 700, 50, 350);
            StaticMap.AddRectangle(700, 750, 200, 200);
            StaticMap.AddRectangle(850, 950, 50, 150);

            StaticMap.GetNodes();

            //bot1 = new Bot(1, new Vector2(150, 600), StaticMap);
            bot1 = new Bot(Team.Blue, new Vector2(10, 10), StaticMap, shotManager, tankTextures[0]);
            bot2 = new Bot(Team.Red, new Vector2(600, 600), StaticMap, shotManager, tankTextures[1]);

            bot2.Enemy = bot1;
            bot1.Enemy = bot2;

            bot1.SetState(new Wander());
            bot2.SetState(new Seek());

            allBots.Add(bot1);
            allBots.Add(bot2);
            selectedBot = allBots[0];

            toggleActions.Add(new ToggleAction(Keys.N, DrawAllPaths));
            toggleActions.Add(new ToggleAction(Keys.M, DrawAllNodes));

            //shotManager.AddShot(bot1.GetPosition(), bot2.GetPosition());

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState newKeyState = Keyboard.GetState();

            foreach (ToggleAction ta in toggleActions)
            {
                if (oldKeyboardState.IsKeyUp(ta.Key) && newKeyState.IsKeyDown(ta.Key))
                {
                    ta.IsActive = !ta.IsActive;
                }
            }

            if (oldKeyboardState.IsKeyDown(Keys.P) && newKeyState.IsKeyUp(Keys.P))
            {
                SimulateHit(spriteBatch);
            }

            if (oldKeyboardState.IsKeyUp(Keys.NumPad1) && newKeyState.IsKeyDown(Keys.NumPad1))
            {
                selectedBot = allBots[0];
            } else if (oldKeyboardState.IsKeyUp(Keys.NumPad2) && newKeyState.IsKeyDown(Keys.NumPad2))
            {
                selectedBot = allBots[1];
            }

            if (oldKeyboardState.IsKeyDown(Keys.Space) && newKeyState.IsKeyUp(Keys.Space))
            {
                bot1.Fire();
            }

            float frameRate = (float)gameTime.ElapsedGameTime.TotalSeconds;

            bot1.Update(frameRate);
            bot2.Update(frameRate);

            oldKeyboardState = newKeyState;

            shotManager.Update(frameRate);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            DrawBackground();

            //foreach (Node n in path)
            //{
            //    DebugDraw.DrawCircle(spriteBatch, n.Position, Color.Green);

            //    if (n.Parent != null)
            //    {

            //        DebugDraw.DrawLine(spriteBatch, n.Position, n.Parent.Position);
            //    }
            //}

            StaticMap.DrawMap(spriteBatch);

            shotManager.Draw(spriteBatch);

            foreach (var action in toggleActions)
            {
                if (action.IsActive)
                {
                    action.Action.Invoke(spriteBatch); 
                }
            }

            //DebugDraw.DrawCircle(spriteBatch, 150, 600, Color.Blue);
            //DebugDraw.DrawCircle(spriteBatch, 1050, 600, Color.Red);

            //DebugDraw.DrawCircle(spriteBatch, 450, 150);
            //DebugDraw.DrawCircle(spriteBatch, 600, 600);
            //DebugDraw.DrawCircle(spriteBatch, 750, 1050);

            //foreach (var node in StaticMap.NodeMap)
            //{
            //    DebugDraw.DrawCircle(spriteBatch, node.Position, Color.Green);
            //}

            bot1.Draw(spriteBatch);
            bot2.Draw(spriteBatch);

            DebugDraw.DrawText(spriteBatch, 0, 0, bot1.GetState().ToString(), bot1.GetColour());
            DebugDraw.DrawText(spriteBatch, 1000, 0, bot2.GetState().ToString(), bot2.GetColour());

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawBackground()
        {
            int size = backgroundTexture.Width;

            for (int i = 0; i < graphics.PreferredBackBufferWidth; i += size)
            {
                for (int j = 0; j < graphics.PreferredBackBufferHeight; j += size)
                {
                    spriteBatch.Draw(backgroundTexture, new Vector2(i, j), Color.White);
                }
            }
        }

        private void DrawAllNodes(SpriteBatch batch)
        {
            foreach (Node n in StaticMap.NodeMap)
            {
                DebugDraw.DrawCircle(batch, n.Position, 5, Color.Green);
            }
        }

        private void SimulateHit(SpriteBatch batch)
        {
            selectedBot.TakeDamage(1);
        }

        private void DrawAllPaths(SpriteBatch batch)
        {
            for (int i = 0; i < StaticMap.NodeMap.Count; ++i)
            {
                foreach (var neighbour in StaticMap.NodeMap[i].Neighbours)
                {
                    DebugDraw.DrawLine(batch, StaticMap.NodeMap[i].Position, neighbour.Position, Color.White * 0.5f);
                }
            }
        }
    }
}
