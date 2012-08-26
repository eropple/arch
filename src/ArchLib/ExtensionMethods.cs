using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArchLib
{
    public static class ExtensionMethods
    {
        internal static void BeginScaled(this SpriteBatch batch)
        {
            batch.Begin(SpriteSortMode.Deferred,
                BlendState.NonPremultiplied, null, null, null, null,
                Arch.Scaling.TransformationMatrix);
        }


        public static Int32 GetScaleFactor(this GraphicsDeviceManager gdm)
        {
            return 1;
        }
    }
}
