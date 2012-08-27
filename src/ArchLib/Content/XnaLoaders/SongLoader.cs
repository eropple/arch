using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework.Media;

namespace ArchLib.Content.XnaLoaders
{
    public static class SongLoader
    {
        // TODO: This probably won't work for MonoGame.
#if XNA && WINDOWS

        private static readonly ConstructorInfo SongConstructor;
        static SongLoader()
        {
            var songType = typeof (Song);
            var stringType = typeof (String);
            var intType = typeof (Int32);

            SongConstructor = songType.GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance, null,
                new[] {stringType, stringType, intType}, null);
        }
#endif


        public static Song LoadFromKey(String key)
        {
#if XNA && WINDOWS
            String path = Path.Combine(Arch.Options.ContentRoot, "Music", key + ".mp3");

            if (File.Exists(path) == false)
                return null;

            return (Song)SongConstructor.Invoke(new Object[] { key, path, 0 });
#endif
        }
    }
}
