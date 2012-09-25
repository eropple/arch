using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArchLib.Content.Factories
{
    public class DefaultContentContextFactory : IContentContextFactory
    {
        public ContentContext BuildContext(ContentContext parent = null)
        {
            return new ContentContext(parent);
        }

        internal static DefaultContentContextFactory BuildContextFactory()
        {
            return new DefaultContentContextFactory();
        }
    }
}
