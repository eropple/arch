using System;
using System.Collections.Generic;
using ArchLib.Options;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArchLib.Runners
{
    public class XnaGame : Game
    {
        public readonly GraphicsDeviceManager GraphicsDeviceManager;
        private readonly StartupOptions Options;

        private Texture2D WhiteTexture;
        private SpriteBatch BackgroundBatch;
        private Viewport ClearingViewport;

        internal XnaGame(StartupOptions options)
            : base()
        {
            Options = options;
            
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            GraphicsDeviceManager.PreferredBackBufferHeight = Options.WindowHeight;
            GraphicsDeviceManager.PreferredBackBufferWidth = Options.WindowWidth;
            GraphicsDeviceManager.IsFullScreen = Options.Fullscreen;

            IsMouseVisible = Options.ShowMouseCursor;

        }

        protected override void LoadContent()
        {
            base.LoadContent();
            WhiteTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            WhiteTexture.SetData(new[] { Color.White });
            
            BackgroundBatch = new SpriteBatch(GraphicsDevice);
            ClearingViewport = new Viewport(0, 0, 1, 1);
        }

        protected override void BeginRun()
        {
            base.BeginRun();
            Arch.FireBeforeStart();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Arch.Update(gameTime.ElapsedGameTime.TotalMilliseconds);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (Arch.Scaling.RealScreenBounds != Arch.Scaling.Viewport.Bounds)
            {
                GraphicsDevice.Viewport = ClearingViewport;
                GraphicsDevice.Clear(Options.LetterboxColor);
                GraphicsDevice.Viewport = Arch.Scaling.Viewport;
            }

            BackgroundBatch.Begin();
            BackgroundBatch.Draw(WhiteTexture, Arch.Scaling.ScaledScreenBounds, Color.Black);
            BackgroundBatch.End();

            Arch.Draw(gameTime.ElapsedGameTime.TotalMilliseconds);
        }
    }
}
