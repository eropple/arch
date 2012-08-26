using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArchLib.Graphics
{
    public class Texture : TextureRegion, IReloadable, IDisposable
    {
        /// <summary>
        /// The string path that corresponds to this texture resource. May differ from platform
        /// to platform in content and usage.
        /// </summary>
        public readonly String ResourcePath;

        public Texture(String resourcePath, Texture2D backingTexture,
            Int32 scaleFactor)
            : base(backingTexture, scaleFactor, backingTexture.Bounds)
        {
            ResourcePath = resourcePath;
        }

        public void Reload()
        {
            // TODO: implement asset reloading for Android
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            BackingTexture.Dispose();
        }

        public static Texture LoadFromKey(String key)
        {
            String pathBase = Path.Combine(Arch.Options.ContentRoot, "Textures", key);

            if (Arch.Scaling.ScaleFactor == 2)
            {
                String retinaPath = pathBase + "@2x.png";
                if (File.Exists(retinaPath))
                {
                    Texture2D tex = Texture2D.FromStream(Arch.Graphics.GraphicsDevice, File.OpenRead(retinaPath));
                    return new Texture(retinaPath, tex, 2); // it's a retina texture
                }
            }

            String normalPath = pathBase + ".png";
            if (File.Exists(normalPath))
            {
                Texture2D tex = Texture2D.FromStream(Arch.Graphics.GraphicsDevice, File.OpenRead(normalPath));
                return new Texture(normalPath, tex, 1); // it's a normal texture
            }
            
            if (Arch.Scaling.ScaleFactor != 2) // we'd rather take 1x if it's available
            {
                String retinaPath = pathBase + "@2x.png";
                if (File.Exists(retinaPath))
                {
                    Texture2D tex = Texture2D.FromStream(Arch.Graphics.GraphicsDevice, File.OpenRead(retinaPath));
                    return new Texture(retinaPath, tex, 2); // it's a retina texture
                }
            }

            return null; // failboat'd
        }
    }
}
