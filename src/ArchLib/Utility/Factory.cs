using System;
using System.Collections.Generic;
using ArchLib.Runners;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ArchLib.Utility
{
    public sealed class Factory
    {
        internal Factory()
        {
        }

        public SpriteBatch BuildSpriteBatch()
        {
            return new SpriteBatch(Arch.Graphics.GraphicsDevice);
        }
    }
}
