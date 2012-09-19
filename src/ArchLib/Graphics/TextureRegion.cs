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
        private readonly Rectangle _bounds;
        /// <summary>
        /// The scale factor for this drawable object. If it matches Arch.Scaling._scaleFactor,
        /// then it will be drawn at 1:1 pixel:texel; otherwise, it will be scaled up or down
        /// as appropriate.
        /// </summary>
        private readonly Int32 _scaleFactor;

        /// <summary>
        /// The ratio of _scaleFactor / Arch.Scaling._scaleFactor. Will be used for determining
        /// destination rectangles.
        /// </summary>
        protected readonly Single ScaleRatio;

        public TextureRegion(Texture2D backingTexture, Int32 scaleFactor, Rectangle bounds)
        {
            BackingTexture = backingTexture;
            _scaleFactor = scaleFactor;
            _bounds = bounds;

            ScaleRatio = (float)Arch.Scaling.ScaleFactor / (float)_scaleFactor;
        }

        public Rectangle Bounds { get { return _bounds; } }
        public Int32 ScaleFactor { get { return _scaleFactor; } }

        /// <summary>
        /// Draws the given texture region at the requested position.
        /// </summary>
        /// <param name="batch">The SpriteBatch with which to draw this TextureRegion.</param>
        /// <param name="position">The position, in virtual coordinates, to draw this TextureRegion.</param>
        public void Draw(SpriteBatch batch, Vector2 position)
        {
            Draw(batch, position, 0.0, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
        }

        public void Draw(SpriteBatch batch, Vector2 position, double time)
        {
            Draw(batch, position, time, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
        }

        public void Draw(SpriteBatch batch, Vector2 position, Color color, 
            Single rotation, Vector2 origin, SpriteEffects effects, Single layerDepth)
        {
            Draw(batch, position, 0.0, color, rotation, origin, effects, layerDepth);
        }

        public void Draw(SpriteBatch batch, Vector2 position, Double time, Color color, 
            Single rotation, Vector2 origin, SpriteEffects effects, Single layerDepth)
        {
            Vector2 scaledPosition = Arch.Scaling.VirtualCoordsToScaled(position);
            if (_scaleFactor == Arch.Scaling.ScaleFactor)
            {
                // scaled and virtual are the same
                batch.Draw(BackingTexture, scaledPosition, _bounds, color, rotation, origin, 1.0f, effects, layerDepth);
            }
            else
            {
                // scaled and virtual aren't the same, so we have to multiply by ScaleRatio
                Rectangle r = new Rectangle((Int32)Math.Floor(scaledPosition.X),
                    (Int32)Math.Floor(scaledPosition.Y),
                    (Int32)(_bounds.Width * ScaleRatio),
                    (Int32)(_bounds.Height * ScaleRatio));

                batch.Draw(BackingTexture, r, _bounds, color, rotation, origin, effects, layerDepth);
            }
        }
    }
}
