using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArchLib.Graphics
{
    public class TextureRegion : IDrawable
    {
        /// <summary>
        /// The XNA texture reference that backs this region.
        /// </summary>
        public readonly Texture2D BackingTexture;
        /// <summary>
        /// The bounds of this TextureRegion within its BackingTexture.
        /// </summary>
        public readonly Rectangle Bounds;
        /// <summary>
        /// The scale factor for this drawable object. If it matches Arch.Scaling.ScaleFactor,
        /// then it will be drawn at 1:1 pixel:texel; otherwise, it will be scaled up or down
        /// as appropriate.
        /// </summary>
        public readonly Int32 ScaleFactor;

        /// <summary>
        /// The ratio of ScaleFactor / Arch.Scaling.ScaleFactor. Will be used for determining
        /// destination rectangles.
        /// </summary>
        protected readonly Single ScaleRatio;

        public TextureRegion(Texture2D backingTexture, Int32 scaleFactor, Rectangle bounds)
        {
            BackingTexture = backingTexture;
            ScaleFactor = scaleFactor;
            Bounds = bounds;

            ScaleRatio = (float)ScaleFactor/(float)Arch.Scaling.ScaleFactor;
        }

        /// <summary>
        /// Draws the given texture region at the requested position.
        /// </summary>
        /// <param name="batch">The SpriteBatch with which to draw this TextureRegion.</param>
        /// <param name="position">The position, in virtual coordinates, to draw this TextureRegion.</param>
        public void Draw(SpriteBatch batch, Vector2 position)
        {
            Draw(batch, position, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
        }

        public void Draw(SpriteBatch batch, Vector2 position, Color color, 
            Single rotation, Vector2 origin, SpriteEffects effects, Single layerDepth)
        {
            Vector2 scaledPosition = Arch.Scaling.VirtualCoordsToScaled(position);
            if (ScaleFactor == Arch.Scaling.ScaleFactor)
            {
                // scaled and virtual are the same
                batch.Draw(BackingTexture, scaledPosition, Bounds, color, rotation, origin, 1.0f, effects, layerDepth);
            }
            else
            {
                // scaled and virtual aren't the same, so we have to multiply by ScaleRatio
                Rectangle r = new Rectangle((Int32)Math.Floor(scaledPosition.X),
                    (Int32)Math.Floor(scaledPosition.Y), 
                    (Int32)(Bounds.Width * ScaleRatio), 
                    (Int32)(Bounds.Height * ScaleRatio));

                batch.Draw(BackingTexture, r, Bounds, color, rotation, origin, effects, layerDepth);
            }
        }
    }
}
