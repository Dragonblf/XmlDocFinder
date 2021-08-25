using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dragonblf.XmlDocFinder.DI;

namespace Dragonblf.XmlDocFinder
{
    /// <summary>
    /// Helper class to find paths to XML documentation files for assemblies.
    /// </summary>
    public class DocFinder : IDocFinder
    {
        /// <summary>
        /// Contains the file system wrapper to use.
        /// </summary>
        private IFileSystem _fileSystem;


        /// <summary>
        /// Initializes a new instance of <see cref="DocFinder"/>.
        /// </summary>
        public DocFinder()
        {
            _fileSystem = DIProvider.GetInstance<IFileSystem>();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DocFinder"/>.
        /// </summary>
        /// <param name="fileSystem">File system wrapper to use</param>
        /// <exception cref="ArgumentNullException"></exception>
        internal DocFinder(IFileSystem fileSystem)
        {
            if (fileSystem == null) { throw new ArgumentNullException(nameof(fileSystem)); }

            _fileSystem = fileSystem;
        }


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
