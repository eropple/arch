using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ArchLib.Utility
{
    public sealed class Factory
    {
        internal Factory()
        {
        }

        public ContentManager BuildContentManager()
        {
#if XNA
            return new ContentManager(XnaRunner.Instance.Services, "Content");
#else
            throw new Exception();
#endif
        }

        public SpriteBatch BuildSpriteBatch()
        {
            return new SpriteBatch(Arch.Graphics.GraphicsDevice);
        }
    }
}
