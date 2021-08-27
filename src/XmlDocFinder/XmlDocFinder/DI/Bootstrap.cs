using System.IO.Abstractions;
using SimpleInjector;
using XmlDocFinder.Entities;

namespace XmlDocFinder.DI
{
    /// <summary>
    /// Contains the dependency injection bootstrap for library.
    /// </summary>
    internal static class Bootstrap
    {
        /// <summary>
        /// Apply dependency injection for the base project.
        /// </summary>
        /// <param name="container">Dependency injection container to use</param>
        /// <returns>Dependency injection container</returns>
        internal static Container Initialize(this Container container)
        {
            container.Register<IFileSystem, FileSystem>(Lifestyle.Singleton);
            container.Register<INugetDirectoryPathsProvider, NugetDirectoryPathsProvider>(Lifestyle.Singleton);
            return container;
        }
    }
}
