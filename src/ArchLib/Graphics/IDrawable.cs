using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArchLib.Graphics
{
    /// <summary>
    /// Provides an interface to draw through a SpriteBatch.
    /// </summary>
    public interface IDrawable
    {
        Rectangle Bounds { get; }

        void Draw(SpriteBatch batch, Vector2 position);
        void Draw(SpriteBatch batch, Vector2 position, Double time);

        void Draw(SpriteBatch batch, Vector2 position, Color color,
                  Single rotation, Vector2 origin, SpriteEffects effects, Single layerDepth);
        void Draw(SpriteBatch batch, Vector2 position, Double time, Color color,
                  Single rotation, Vector2 origin, SpriteEffects effects, Single layerDepth);
    }
}
