using System.Collections.Generic;
using System.IO.Abstractions;

namespace XmlDocFinder.Entities
{
    internal interface INugetDirectoryPathsProvider
    {
        /// <summary>
        /// Returns paths to every nuget like directory
        /// on the current machine.
        /// </summary>
        /// <returns>Paths to nuget like directories</returns>
        public IReadOnlyCollection<string> GetNugetLikeDirectoryPaths();
    }
}
