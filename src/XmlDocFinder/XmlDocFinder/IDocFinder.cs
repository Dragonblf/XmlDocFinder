using System;
using System.Reflection;

namespace XmlDocFinder
{
    /// <summary>
    /// Interface which defines helper methods to implement
    /// to find paths to XML documentation files for assemblies.
    /// </summary>
    public interface IDocFinder
    {
        /// <summary>
        /// Finds XML documentation file path for assembly declaring
        /// <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type for which declaring assembly to find the XML documentation file path</typeparam>
        /// <returns>Path to XML documentation file if path was found else <see cref="string.Empty"/></returns>
        public string FindFor<T>() where T : notnull;

        /// <summary>
        /// Finds XML documentation file path for assembly declaring
        /// <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type for which declaring assembly to find the XML documentation file path</typeparam>
        /// <param name="path">Path to XML documentation file if path was found else <see cref="string.Empty"/></param>
        /// <returns>True if path to XML documentation file was found otherwise false</returns>
        public bool TryFindFor<T>(out string path) where T : notnull;

        /// <summary>
        /// Finds XML documentation file path for <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">Assembly for which to find its XML documentation file path</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>Path to XML documentation file if path was found else <see cref="string.Empty"/></returns>
        public string FindFor(in Assembly assembly);

        /// <summary>
        /// Finds XML documentation file path for <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">Assembly for which to find its XML documentation file path</param>
        /// <param name="path">Path to XML documentation file if path was found else <see cref="string.Empty"/></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>True if path to XML documentation file was found otherwise false</returns>
        public bool TryFindFor(in Assembly assembly, out string path);
    }
}
