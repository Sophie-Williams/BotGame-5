using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

        //private RectPrimitive rect;

        //private CirclePrimitive circle;

        public StaticMap StaticMap;

        private Bot bot1;

        public static  Random random;

        private List<Node> path;

        private float pathTimer;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);

            random = new Random();

            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 1200;
            Content.RootDirectory = "Content";

            
        }

        public Vector2 GetRandomPosition()
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
            StaticMap = new StaticMap();

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
            bot1 = new Bot(1, new Vector2(10, 10), StaticMap);

            path = StaticMap.GetShortestPath(Vector2.Zero, new Vector2(1000, 1000));

            //rect = new RectPrimitive(GraphicsDevice, 500, 200, new Vector2(GraphicsDevice.Viewport.Width / 2.0f, GraphicsDevice.Viewport.Height / 2.0f), Color.White);

            //circle = new CirclePrimitive(GraphicsDevice, 192, ShapeType.Outline);

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            pathTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (pathTimer > 5)
            {
                pathTimer = 0;
                //path = StaticMap.GetShortestPath(bot1.GetPosition(), GetRandomPosition());
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                float x, y;
                x = random.Next(0, 1000);
                y = random.Next(0, 1000);

                bot1.SetTarget(x, y);
            }

            //if (Mouse.GetState().LeftButton == ButtonState.Released)
            //{
            //    bot1.SetTarget(Mouse.GetState().X, Mouse.GetState().Y);
            //}

            // TODO: Add your update logic here

            float frameRate = (float)gameTime.ElapsedGameTime.TotalSeconds;

            bot1.FollowPath(path);

            if (Vector2.Distance(bot1.GetPosition(), bot1.GetTarget()) < 20)
            {
                path = StaticMap.GetShortestPath(bot1.GetPosition(), GetRandomPosition());
            }

            bot1.Update(frameRate);

            //path = StaticMap.GetShortestPath(Vector2.Zero, bot1.GetPosition());

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            //for (int i = 0; i < StaticMap.NodeMap.Count; ++i)
            //{
            //    foreach (var neighbour in StaticMap.NodeMap[i].Neighbours)
            //    {
            //        DebugDraw.DrawLine(spriteBatch, StaticMap.NodeMap[i].Position, neighbour.Position);
            //    }
            //}

            foreach (Node n in path)
            {
                DebugDraw.DrawCircle(spriteBatch, n.Position, Color.Green);

                if (n.Parent != null)
                {

                    DebugDraw.DrawLine(spriteBatch, n.Position, n.Parent.Position);
                }
            }

            StaticMap.DrawMap(spriteBatch);

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

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
