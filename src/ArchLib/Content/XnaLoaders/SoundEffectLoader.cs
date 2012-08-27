using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Audio;

namespace ArchLib.Content.XnaLoaders
{
    public static class SoundEffectLoader
    {
        public static SoundEffect LoadFromKey(String key)
        {
            String path = Path.Combine(Arch.Options.ContentRoot, "Sounds", key + ".wav");

            if (File.Exists(path) == false) return null;

            return SoundEffect.FromStream(File.OpenRead(path));
        }
    }
}
