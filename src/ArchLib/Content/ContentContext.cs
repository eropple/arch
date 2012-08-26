using System;
using System.Collections.Generic;
using ArchLib.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ArchLib.Content
{
    /// <summary>
    /// A "scope" for loading content objects. ContentContexts may have parent contexts
    /// that will be checked for a requested object before attempting to load them into
    /// itself.
    /// </summary>
    public class ContentContext : IDisposable
    {
        private static readonly List<ContentContext> ReloadList = new List<ContentContext>(20); 

        public readonly ContentContext Parent;

        private readonly Dictionary<String, Texture> _textures = new Dictionary<String, Texture>();

        public ContentContext(ContentContext parent)
        {
            Parent = parent;

            if (parent == null) ReloadList.Add(this);
        }

        public ContentContext CreateChildContext()
        {
            var child = new ContentContext(this);
            ReloadList.Add(child);

            return child;
        }


        /// <summary>
        /// Retrieves a Texture object by key if it is loaded into this context or any
        /// parent context. Returns null if not found.
        /// </summary>
        /// <param name="key">The Texture object's key.</param>
        /// <returns>a Texture object if already loaded, or null.</returns>
        public Texture GetTextureIfLoaded(String key)
        {
            Texture t;
            return _textures.TryGetValue(key, out t) ? t : null;
        }

        /// <summary>
        /// Returns a Texture object corresponding to the given key, loading it from disk
        /// if necessary. If one cannot be found, an exception is thrown.
        /// </summary>
        /// <param name="key">The Texture object's key.</param>
        /// <exception cref="ContentLoadException">If no texture found.</exception>
        /// <returns>a Texture object.</returns>
        public Texture GetTexture(String key)
        {
            Texture t;
            t = _textures.TryGetValue(key, out t) ? t : null;
            if (t == null && Parent != null) Parent.GetTextureIfLoaded(key);

            if (t == null)
            {
                t = Texture.LoadFromKey(key);
                if (t == null) throw new ContentNotFoundException<Texture>(key);

                _textures.Add(key, t);
            }
            return t;
        }

        public void Dispose()
        {
            foreach(var t in _textures.Values) t.Dispose();
        }
    }
}
