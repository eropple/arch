using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArchLib.Graphics
{
    public class Scaling
    {
        private readonly GraphicsDeviceManager _graphicsDeviceManager;

        /// <summary>
        /// The real geometric bounds of the window the game is running in.
        /// </summary>
        public readonly Rectangle RealScreenBounds;
        /// <summary>
        /// The aspect ratio of the real window.
        /// </summary>
        public readonly Single RealAspectRatio;

        /// <summary>
        /// The scaled size of the viewport - for example, a 1920x1080 real
        /// window is going to scale up to the next largest size, which would
        /// be 2560x1440. A 1280x720 real window will remain 1280x720 when scaled.
        /// </summary>
        public readonly Rectangle ScaledScreenBounds;
        /// <summary>
        /// The aspect ratio of the scaled "screen".
        /// </summary>
        public readonly Single ScaledAspectRatio;
        /// <summary>
        /// The ratio of virtual "pixels" to scaled pixels. Used in combination with
        /// the ScaleFactor attribute on drawable items to properly plot the drawn items
        /// on the virtual screen grid.
        /// </summary>
        public readonly Int32 ScaleFactor;

        /// <summary>
        /// The bounds of the virtual screen that games will draw to. Always 1280x720.
        /// </summary>
        public readonly Rectangle VirtualScreenBounds;
        /// <summary>
        /// The aspect ratio of the virtual screen. Should always be the same as ScaledAspectRatio.
        /// </summary>
        public readonly Single VirtualAspectRatio;

        public readonly Matrix TransformationMatrix;

        public readonly Viewport Viewport;

        private readonly Int32 XOffset;
        private readonly Int32 YOffset;
        private readonly Single XScale;
        private readonly Single YScale;

        internal Scaling(GraphicsDeviceManager graphicsDeviceManager)
        {
            _graphicsDeviceManager = graphicsDeviceManager;

            Int32 realScreenWidth = _graphicsDeviceManager.PreferredBackBufferWidth;
            Int32 realScreenHeight = _graphicsDeviceManager.PreferredBackBufferHeight;
            RealScreenBounds = new Rectangle(0, 0, realScreenWidth, realScreenHeight);
            RealAspectRatio = (float) realScreenWidth/(float) realScreenHeight;

            if (realScreenWidth <= 1280 && realScreenHeight <= 720)
            {
                ScaledScreenBounds = new Rectangle(0, 0, 1280, 720);
            }
            else
            {
                ScaledScreenBounds = new Rectangle(0, 0, 2560, 1440);
            }
            ScaledAspectRatio = (float) ScaledScreenBounds.Width/(float)ScaledScreenBounds.Height;

            VirtualScreenBounds = new Rectangle(0, 0, 1280, 720);
            VirtualAspectRatio = (float) VirtualScreenBounds.Width/(float) VirtualScreenBounds.Height;

            // should always be 1 or 2; will have to consider others later
            ScaleFactor = ScaledScreenBounds.Width/VirtualScreenBounds.Width;
            TransformationMatrix = Matrix.CreateScale(ScaleFactor, ScaleFactor, 1);

            Viewport = BuildViewport();
            XScale = (float) VirtualScreenBounds.Width/(float) Viewport.Width;
            XOffset = -Viewport.X;
            YScale = (float) VirtualScreenBounds.Height/(float) Viewport.Height;
            YOffset = -Viewport.Y;
        }

        private Viewport BuildViewport()
        {
            Int32 width = RealScreenBounds.Width;
            Int32 height = (Int32) (width/VirtualAspectRatio + .5f);

            if (height > RealScreenBounds.Height)
            {
                height = RealScreenBounds.Height;
                width = (int) (height*VirtualAspectRatio + .5f);
            }

            return new Viewport
                       {
                           X = (RealScreenBounds.Width/2) - (width/2),
                           Y = (RealScreenBounds.Height/2) - (height/2),
                           Width = width,
                           Height = height,
                           MinDepth = 0,
                           MaxDepth = 1
                       };
        }

        internal void BeforeStart()
        {
            _graphicsDeviceManager.GraphicsDevice.Viewport = Viewport;
        }

        public Vector2 RealCoordsToVirtual(Vector2 realCoords)
        {
            return new Vector2(XOffset + (realCoords.X * XScale),
                YOffset + (realCoords.Y * YScale));
        }
        public Vector2 VirtualCoordsToScaled(Vector2 virtualCoords)
        {
            return virtualCoords*ScaleFactor;
        }
    }
}
