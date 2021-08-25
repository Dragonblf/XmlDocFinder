using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleInjector;

namespace Dragonblf.XmlDocFinder.DI
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
            return container;
        }
    }
}
