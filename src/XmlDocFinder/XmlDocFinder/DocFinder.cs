using System;
using System.Collections.Concurrent;
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
        /// Contains previously found XML documentation
        /// paths for hashes of assembly full names.
        /// </summary>
        private static readonly IDictionary<int, string> Cache =
            new ConcurrentDictionary<int, string>();

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

            return TryFindFor(assembly, out var path) 
                ? path 
                : string.Empty;
        }

        public bool TryFindFor(in Assembly assembly, out string path)
        {
            if (assembly == null) { throw new ArgumentNullException(nameof(assembly)); }
            if (string.IsNullOrWhiteSpace(assembly.FullName))
            {
                throw new ArgumentException("Full name of assembly needs to be defined and not only white spaces", nameof(assembly));
            }

            // Check if cache contains previously found
            // path for given assembly
            var hash = assembly.FullName.GetHashCode();
            if (Cache.TryGetValue(hash, out path)) { return true; }

            // Get path for assembly or string.Empty if
            // no XML documentation could be found for
            // given assembly.
            path = FindPath(assembly);

            // Save path into cache
            Cache[hash] = path;

            return true;
        }

        /// <summary>
        /// Returns XML documentation path for <paramref name="assembly"/>
        /// or <see cref="string.Empty"/> if no path could be found.
        /// </summary>
        /// <param name="assembly">Assembly for which to get XML documentation path</param>
        /// <returns>Path to XML documentation path for <paramref name="assembly"/> or <see cref="string.Empty"/> if no path could be found</returns>
        private string FindPath(in Assembly assembly)
        {
            if (assembly == null) { throw new ArgumentNullException(nameof(assembly)); }

            throw new NotImplementedException();
        }
    }
}
