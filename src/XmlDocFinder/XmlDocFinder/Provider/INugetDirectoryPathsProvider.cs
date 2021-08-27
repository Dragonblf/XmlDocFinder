using System.Collections.Generic;

namespace XmlDocFinder.Provider
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
