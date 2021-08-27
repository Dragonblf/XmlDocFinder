using System;
using System.Collections.Generic;
using System.IO.Abstractions;
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

            return paths;
        }
    }
}
