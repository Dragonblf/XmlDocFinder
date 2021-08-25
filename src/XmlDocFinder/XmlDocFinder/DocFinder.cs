using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dragonblf.XmlDocFinder
{
    /// <summary>
    /// Helper class to find paths to XML documentation files for assemblies.
    /// </summary>
    public class DocFinder : IDocFinder
    {
        public string FindFor<T>() where T : notnull => FindFor(typeof(T).Assembly);

        public bool TryFindFor<T>(out string path) where T : notnull => TryFindFor(typeof(T).Assembly, out path);

        public string FindFor(in Assembly assembly)
        {
            if (assembly == null) { throw new ArgumentNullException(nameof(assembly)); }

            throw new NotImplementedException();
        }

        public bool TryFindFor(in Assembly assembly, out string path)
        {
            if (assembly == null) { throw new ArgumentNullException(nameof(assembly)); }

            throw new NotImplementedException();
        }
    }
}
