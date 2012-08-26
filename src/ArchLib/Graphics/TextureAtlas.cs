using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using ArchLib.Utility.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArchLib.Graphics
{
    public class TextureAtlas : IReloadable, IDisposable
    {
        /// <summary>
        /// The string path that corresponds to this texture resource. May differ from platform
        /// to platform in content and usage.
        /// </summary>
        public readonly String ResourcePath;

        /// <summary>
        /// XNA texture used in the atlas.
        /// </summary>
        public readonly Texture2D BackingTexture;

        public readonly Int32 ScaleFactor;

        private readonly Dictionary<String, TextureRegion> _dict;
        public readonly LookupTable<String, TextureRegion> Entries;

        public TextureAtlas(String resourcePath, Int32 scaleFactor, Texture2D backingTexture,
                            ICollection<Tuple<String, Rectangle>> entries)
        {
            ResourcePath = resourcePath;
            BackingTexture = backingTexture;
            ScaleFactor = scaleFactor;

            _dict = new Dictionary<String, TextureRegion>(entries.Count);
            foreach (var entry in entries)
            {
                _dict.Add(entry.Item1, new TextureRegion(BackingTexture, ScaleFactor, entry.Item2));
            }
            Entries = new LookupTable<String, TextureRegion>(_dict);
        }

        public TextureRegion this[String key]
        {
            get
            {
                TextureRegion t;
                return _dict.TryGetValue(key, out t) ? t : null;
            }
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



        public static TextureAtlas LoadFromKey(String key)
        {
            String pathBase = Path.Combine(Arch.Options.ContentRoot, 
                "Atlases", key);

            String imagePath = null;
            ICollection<Tuple<String, Rectangle>> rects;

            if (Arch.Scaling.ScaleFactor == 2)
            {
                String retinaPath = pathBase + "@2x.atlas";
                if (File.Exists(retinaPath))
                {
                    ParseXml(retinaPath, out imagePath, out rects);
                    Texture2D tex = Texture2D.FromStream(Arch.Graphics.GraphicsDevice, File.OpenRead(imagePath));
                    return new TextureAtlas(retinaPath, 2, tex, rects);
                }
            }

            String normalPath = pathBase + ".atlas";
            if (File.Exists(normalPath))
            {
                ParseXml(normalPath, out imagePath, out rects);
                Texture2D tex = Texture2D.FromStream(Arch.Graphics.GraphicsDevice, File.OpenRead(imagePath));
                return new TextureAtlas(normalPath, 1, tex, rects);
            }

            if (Arch.Scaling.ScaleFactor != 2)
            {
                String retinaPath = pathBase + "@2x.atlas";
                if (File.Exists(retinaPath))
                {
                    ParseXml(retinaPath, out imagePath, out rects);
                    Texture2D tex = Texture2D.FromStream(Arch.Graphics.GraphicsDevice, File.OpenRead(imagePath));
                    return new TextureAtlas(retinaPath, 2, tex, rects);
                }
            }

            return null;
        }


        private static void ParseXml(String filename, 
            out String imagePath, out ICollection<Tuple<String, Rectangle>> rects)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(filename);

            var root = xmlDoc["TextureAtlas"];
            imagePath = Path.Combine(Arch.Options.ContentRoot, "Textures", "Atlases", 
                root.Attributes["imagePath"].Value);

            rects = new List<Tuple<String, Rectangle>>(root.ChildNodes.Count);

            foreach (XmlElement item in root.ChildNodes)
            {
                if (item.Name != "sprite") continue;

                String name = item.Attributes["n"].Value;

                Int32 x = Convert.ToInt32(item.Attributes["x"].Value);
                Int32 y = Convert.ToInt32(item.Attributes["y"].Value);
                Int32 w = Convert.ToInt32(item.Attributes["w"].Value);
                Int32 h = Convert.ToInt32(item.Attributes["h"].Value);

                var rect = new Rectangle(x, y, w, h);

                rects.Add(Tuple.Create(name, rect));
            }
        }
    }
}
