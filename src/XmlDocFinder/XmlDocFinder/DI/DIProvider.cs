using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleInjector;

namespace XmlDocFinder.DI
{
    /// <summary>
    /// Provides simple dependency injection functionality.
    /// </summary>
    internal static class DIProvider
    {
        /// <summary>
        /// Holds the dependency injection container.
        /// </summary>
        private static readonly Container Container;


        /// <summary>
        /// Initializes functionality for <see cref="DIProvider"/>.
        /// </summary>
        static DIProvider()
        {
            // Create injector container
            Container = new Container();

            // Initialize dependency injection data
            Container.Initialize();
        }


        /// <summary>
        /// Gets an instance of given type <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">Interface to find</typeparam>
        /// <returns>Instance</returns>
        public static T GetInstance<T>() where T : class
        {
            return Container.GetInstance<T>();
        }
    }
}
