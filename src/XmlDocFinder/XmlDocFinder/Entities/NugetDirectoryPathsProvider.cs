using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Abstractions;
using System.Reflection;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using NuGet.Configuration;

namespace XmlDocFinder.Entities
{
    internal class NugetDirectoryPathsProvider : INugetDirectoryPathsProvider
    {
        /// <summary>
        /// Contains the file system wrapper to use.
        /// </summary>
        private readonly IFileSystem _fileSystem;


        /// <summary>
        /// Initializes a new instance of <see cref="NugetDirectoryPathsProvider"/>.
        /// </summary>
        /// <param name="fileSystem">File system wrapper to use</param>
        public NugetDirectoryPathsProvider(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }


        /// <inheritdoc cref="INugetDirectoryPathsProvider.GetNugetLikeDirectoryPaths"/>
        public IReadOnlyCollection<string> GetNugetLikeDirectoryPaths()
        {
            var paths = new HashSet<string>();

            // Load machine wide default settings
            var settings = Settings.LoadDefaultSettings(null);

            // Add global nuget packages folder
            var globalPath = SettingsUtility.GetGlobalPackagesFolder(settings);
            if (globalPath != string.Empty &&
                _fileSystem.Directory.Exists(globalPath))
            {
                paths.Add(globalPath);
            }

            // Add fallback nuget packages folder
            foreach (var fallbackPath in SettingsUtility.GetFallbackPackageFolders(settings))
            {
                if (fallbackPath != string.Empty &&
                    _fileSystem.Directory.Exists(fallbackPath))
                {
                    paths.Add(fallbackPath);
                }
            }

            // Add package sources like package source
            // for microsoft SDKs
            var section = settings.GetSection("packageSources");
            foreach (var sectionItem in section.Items)
            {
                if (sectionItem is SourceItem sourceItem &&
                    !sourceItem.IsEmpty() &&
                    sourceItem.Value != string.Empty &&
                    _fileSystem.Directory.Exists(sourceItem.Value))
                {
                    paths.Add(sourceItem.Value);
                }
            }

            // Add nuget path entries from executing assembly
            // dev runtime config
            var executingAssembly = Assembly.GetExecutingAssembly();
            foreach (var configPath in GetDirectoriesFromRunTimeConfig(executingAssembly))
            {
                if (configPath != string.Empty &&
                    _fileSystem.Directory.Exists(configPath))
                {
                    paths.Add(configPath);
                }
            }

            // Add nuget path entries from calling assembly
            // dev runtime config
            var callingAssembly = Assembly.GetCallingAssembly();
            foreach (var configPath in GetDirectoriesFromRunTimeConfig(callingAssembly))
            {
                if (configPath != string.Empty &&
                    _fileSystem.Directory.Exists(configPath))
                {
                    paths.Add(configPath);
                }
            }

            return paths;
        }

        /// <summary>
        /// Tries to parse nuget like directory paths out of
        /// the dev runtime config file of <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">Assembly which dev runtime config file should be read</param>
        /// <returns>Parsed paths to nuget like directories</returns>
        private IReadOnlyCollection<string> GetDirectoriesFromRunTimeConfig(Assembly assembly)
        {
            Debug.Assert(assembly != null);

            var paths = new HashSet<string>();

            // Skip if no location is defined
            if (string.IsNullOrWhiteSpace(assembly.Location)) { return paths; }

            // Get assembly information
            var assemblyName = assembly.GetName();
            var version = assemblyName.Version;

            // Skip if no version is defined
            if (version == null) { return paths; }

            // Search for runtime config file
            var directory = _fileSystem.Path.GetDirectoryName(assembly.Location);
            foreach (var configPath in _fileSystem.Directory.EnumerateFiles(directory, "*.runtimeconfig.dev.json"))
            {
                // Parse config file
                var json = File.ReadAllText(configPath);
                var config = JsonConvert.DeserializeObject<RuntimeConfig>(json);

                // Go through every possible location
                // and search for the paths containing
                // the keyword "nuget"
                foreach (var cachePath in config.RuntimeOptions.AdditionalProbingPaths)
                {
                    if (cachePath.Contains("nuget", StringComparison.CurrentCultureIgnoreCase))
                    {
                        var processedCachePath = _fileSystem.Path.Combine(cachePath, " ");
                        processedCachePath = processedCachePath.Substring(0, processedCachePath.Length - 1);
                        paths.Add(processedCachePath);
                    }
                }
            }

            return paths;
        }


        /// <summary>
        /// Represents runtime config.
        /// </summary>
        private class RuntimeConfig
        {
            /// <summary>
            /// Contains the runtime options.
            /// </summary>
            [JsonPropertyName("additionalProbingPaths")]
            public RuntimeOptions RuntimeOptions { get; set; }
        }

        /// <summary>
        /// Represents runtime options.
        /// </summary>
        private class RuntimeOptions
        {
            /// <summary>
            /// Contains paths from which it's possible
            /// to get XML documentation files for
            /// assemblies.
            /// </summary>
            [JsonPropertyName("runtimeOptions")]
            public IList<string> AdditionalProbingPaths { get; } = new List<string>();
        }
    }
}
