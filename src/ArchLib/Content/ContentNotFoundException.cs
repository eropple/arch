using System;
using System.Collections.Generic;

namespace ArchLib.Content
{
    public class ContentNotFoundException<TContentType> : Exception
    {
        internal ContentNotFoundException(String key)
            : base(String.Format("Could not find or load a {0} corresponding to the key '{1}'.", typeof(TContentType).Name, key))
        {
            
        }
    }
}
