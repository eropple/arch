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

        public static Boolean Contains(this Rectangle rect, Vector2 vector)
        {
            return !(vector.X < rect.Left || vector.X > rect.Right || vector.Y < rect.Top || vector.Y > rect.Bottom);
        }
    }
}
