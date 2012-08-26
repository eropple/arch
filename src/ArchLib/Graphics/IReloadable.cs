using System;
using System.Collections.Generic;

namespace ArchLib.Graphics
{
    /// <summary>
    /// Objects that implement IReloadable have the ability to reload themselves if
    /// their graphical data is lost (as in the case of Android and the destruction
    /// of the GL context when the app switches).
    /// </summary>
    public interface IReloadable
    {
        void Reload();
    }
}
