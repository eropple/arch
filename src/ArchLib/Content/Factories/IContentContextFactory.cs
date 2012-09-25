using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArchLib.Content.Factories
{
    public interface IContentContextFactory
    {
        ContentContext BuildContext(ContentContext parent = null);
    }
}
