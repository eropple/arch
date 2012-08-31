using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ArchLib.Content.XnaLoaders;
using ArchLib.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace ArchLib.Content
{
    /// <summary>
    /// A "scope" for loading content objects. ContentContexts may have parent contexts
    /// that will be checked for a requested object before attempting to load them into
    /// itself.
    /// </summary>
    public class ContentContext : IDisposable
    {
        private static readonly List<ContentContext> ContextList = new List<ContentContext>(20); 
        public static readonly ReadOnlyCollection<ContentContext> Contexts = new ReadOnlyCollection<ContentContext>(ContextList); 

        public readonly ContentContext Parent;

        private readonly Dictionary<String, Texture> _textures = new Dictionary<String, Texture>();
        private readonly Dictionary<String, TextureAtlas> _atlases = new Dictionary<String, TextureAtlas>();
        private readonly Dictionary<String, BitmapFont> _fonts = new Dictionary<String, BitmapFont>();
        private readonly Dictionary<String, Song> _songs = new Dictionary<String, Song>();
        private readonly Dictionary<String, SoundEffect> _sounds = new Dictionary<String, SoundEffect>(); 

        public ContentContext(ContentContext parent)
        {
            Parent = parent;

            if (parent == null) ContextList.Add(this);
        }

        public ContentContext CreateChildContext()
        {
            var child = new ContentContext(this);
            ContextList.Add(child);

            return child;
        }


        #region Texture Methods
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
        #endregion

        #region Texture Atlas Methods
        /// <summary>
        /// Retrieves a TextureAtlas object by key if it is loaded into this context or any
        /// parent context. Returns null if not found.
        /// </summary>
        /// <param name="key">The TextureAtlas object's key.</param>
        /// <returns>a TextureAtlas object if already loaded, or null.</returns>
        public TextureAtlas GetTextureAtlasIfLoaded(String key)
        {
            TextureAtlas t;
            return _atlases.TryGetValue(key, out t) ? t : null;
        }

        /// <summary>
        /// Returns a TextureAtlas object corresponding to the given key, loading it from disk
        /// if necessary. If one cannot be found, an exception is thrown.
        /// </summary>
        /// <param name="key">The TextureAtlas object's key.</param>
        /// <exception cref="ContentLoadException">If no texture atlas found.</exception>
        /// <returns>a TextureAtlas object.</returns>
        public TextureAtlas GetTextureAtlas(String key)
        {
            TextureAtlas t;
            t = _atlases.TryGetValue(key, out t) ? t : null;
            if (t == null && Parent != null) Parent.GetTextureAtlasIfLoaded(key);

            if (t == null)
            {
                t = TextureAtlas.LoadFromKey(key);
                if (t == null) throw new ContentNotFoundException<TextureAtlas>(key);

                _atlases.Add(key, t);
            }
            return t;
        }
        #endregion

        #region Bitmap Font Methods
        /// <summary>
        /// Retrieves a BitmapFont object by key if it is loaded into this context or any
        /// parent context. Returns null if not found.
        /// </summary>
        /// <param name="key">The BitmapFont object's key.</param>
        /// <returns>a BitmapFont object if already loaded, or null.</returns>
        public BitmapFont GetFontIfLoaded(String key)
        {
            BitmapFont f;
            return _fonts.TryGetValue(key, out f) ? f : null;
        }

        /// <summary>
        /// Returns a BitmapFont object corresponding to the given key, loading it from disk
        /// if necessary. If one cannot be found, an exception is thrown.
        /// </summary>
        /// <param name="key">The BitmapFont object's key.</param>
        /// <exception cref="ContentLoadException">If no texture atlas found.</exception>
        /// <returns>a BitmapFont object.</returns>
        public BitmapFont GetFont(String key)
        {
            BitmapFont f;
            f = _fonts.TryGetValue(key, out f) ? f : null;
            if (f == null && Parent != null) Parent.GetFontIfLoaded(key);

            if (f == null)
            {
                f = BitmapFont.LoadFromKey(key);
                if (f == null) throw new ContentNotFoundException<BitmapFont>(key);

                _fonts.Add(key, f);
            }
            return f;
        }
        #endregion

        #region Song Methods
        /// <summary>
        /// Retrieves a Song object by key if it is loaded into this context or any
        /// parent context. Returns null if not found.
        /// </summary>
        /// <param name="key">The Song object's key.</param>
        /// <returns>a Song object if already loaded, or null.</returns>
        public Song GetSongIfLoaded(String key)
        {
            Song s;
            return _songs.TryGetValue(key, out s) ? s : null;
        }

        /// <summary>
        /// Returns a Song object corresponding to the given key, loading it from disk
        /// if necessary. If one cannot be found, an exception is thrown.
        /// </summary>
        /// <param name="key">The Song object's key.</param>
        /// <exception cref="ContentLoadException">If no texture atlas found.</exception>
        /// <returns>a Song object.</returns>
        public Song GetSong(String key)
        {
            Song s;
            s = _songs.TryGetValue(key, out s) ? s : null;
            if (s == null && Parent != null) Parent.GetSongIfLoaded(key);

            if (s == null)
            {
                s = SongLoader.LoadFromKey(key);
                if (s == null) throw new ContentNotFoundException<Song>(key);

                _songs.Add(key, s);
            }
            return s;
        }
        #endregion
        
        #region SoundEffect Methods
        /// <summary>
        /// Retrieves a SoundEffect object by key if it is loaded into this context or any
        /// parent context. Returns null if not found.
        /// </summary>
        /// <param name="key">The SoundEffect object's key.</param>
        /// <returns>a SoundEffect object if already loaded, or null.</returns>
        public SoundEffect GetSoundEffectIfLoaded(String key)
        {
            SoundEffect s;
            return _sounds.TryGetValue(key, out s) ? s : null;
        }

        /// <summary>
        /// Returns a SoundEffect object corresponding to the given key, loading it from disk
        /// if necessary. If one cannot be found, an exception is thrown.
        /// </summary>
        /// <param name="key">The SoundEffect object's key.</param>
        /// <exception cref="ContentLoadException">If no texture atlas found.</exception>
        /// <returns>a SoundEffect object.</returns>
        public SoundEffect GetSoundEffect(String key)
        {
            SoundEffect s;
            s = _sounds.TryGetValue(key, out s) ? s : null;
            if (s == null && Parent != null) Parent.GetSoundEffectIfLoaded(key);

            if (s == null)
            {
                s = SoundEffectLoader.LoadFromKey(key);
                if (s == null) throw new ContentNotFoundException<SoundEffect>(key);

                _sounds.Add(key, s);
            }
            return s;
        }
        #endregion

        public void Dispose()
        {
            foreach (var t in _textures.Values) t.Dispose();
            foreach (var a in _atlases.Values) a.Dispose();
            foreach (var f in _fonts.Values) f.Dispose();
            foreach (var s in _songs.Values) s.Dispose();
            foreach (var s in _sounds.Values) s.Dispose();
        }
    }
}
