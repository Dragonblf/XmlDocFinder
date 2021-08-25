using Dragonblf.XmlDocFinder;
using System;
using System.IO.Abstractions;
using Xunit;
using System.Reflection;
using NSubstitute;
using Shouldly;

namespace XmlDocFinder.Tests
{
    public class DocFinderTests
    {
        private DocFinder _testClass;

        private Assembly _assembly;

        private IFileSystem _fileSystem;


        public DocFinderTests()
        {
            _assembly = Substitute.For<Assembly>();
            _fileSystem = Substitute.For<IFileSystem>();
            _testClass = new DocFinder(_fileSystem);

            _assembly.FullName.Returns(Guid.NewGuid().ToString());
        }


        [Fact]
        public void Can_Construct()
        {
            var instance = new DocFinder();
            instance.ShouldNotBeNull();
        }

        [Fact]
        public void Call_FindForT_WithType_EmptyString()
        {
            var path = _testClass.FindFor<object>();

            path.ShouldBe(string.Empty);
        }

        [Fact]
        public void Call_TryFindForT_WithType_EmptyString()
        {
            var result = _testClass.TryFindFor<object>(out var path);

            result.ShouldBeTrue();
            path.ShouldBe(string.Empty);
        }

        [Fact]
        public void Call_FindFor_WithAssembly_EmptyString()
        {
            var path = _testClass.FindFor(_assembly);
            path.ShouldBe(string.Empty);
        }

        [Fact]
        public void Call_FindFor_WithNull_ArgumentNullException()
        {
            Assembly assembly = null;
            Should.Throw<ArgumentNullException>(() => _testClass.FindFor(assembly));
        }

        [Fact]
        public void Call_TryFindFor_WithNull_ArgumentNullException()
        {
            Assembly assembly = null;
            Should.Throw<ArgumentNullException>(() => _testClass.TryFindFor(assembly, out var path));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Call_TryFindFor_WithWrongFullName_ArgumentException(string value)
        {
            _assembly.FullName.Returns(value);
            Should.Throw<ArgumentException>(() => _testClass.TryFindFor(_assembly, out var path));
        }

        [Fact]
        public void Call_TryFindFor_WithAssemblyLocation_Works()
        {
            var assemblyName = new AssemblyName { Name = "file" };
            var baseDirectory = "path/to";
            var pathToFileWithoutExtension = $"{baseDirectory}/file";
            var dllFilePath = $"{pathToFileWithoutExtension}.dll";
            var xmlFilePath = $"{pathToFileWithoutExtension}.xml";

            _assembly.Location.Returns(dllFilePath);
            _assembly.GetName().Returns(assemblyName);
            _fileSystem.Path.GetDirectoryName(dllFilePath).Returns(baseDirectory);
            _fileSystem.Path.Combine(baseDirectory, $"{assemblyName.Name}.xml").Returns(xmlFilePath);
            _fileSystem.File.Exists(xmlFilePath).Returns(true);

            var result = _testClass.TryFindFor(_assembly, out var path);
            result.ShouldBeTrue();
            path.ShouldBe(xmlFilePath);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Call_TryFindFor_WithAssemblyCodebase1_Works(string assemblyLocation)
        {
            var assemblyName = new AssemblyName { Name = "file" };
            var baseDirectory = "path/to";
            var pathToFileWithoutExtension = $"{baseDirectory}/file";
            var dllFilePath = $"{pathToFileWithoutExtension}.dll";
            var xmlFilePath = $"{pathToFileWithoutExtension}.xml";
            var dllFileCodebasePath = $"file:///c/{pathToFileWithoutExtension}.dll";
            var xmlFileCodebasePath = $"c/{pathToFileWithoutExtension}.xml";

            // No location
            _assembly.Location.Returns(assemblyLocation);
            _assembly.GetName().Returns(assemblyName);
            _fileSystem.Path.GetDirectoryName(dllFilePath).Returns(baseDirectory);
            _fileSystem.Path.Combine(baseDirectory, $"{assemblyName.Name}.xml").Returns(xmlFilePath);
            _fileSystem.File.Exists(xmlFilePath).Returns(true);

            _assembly.CodeBase.Returns(dllFileCodebasePath);
            _fileSystem.Path.GetDirectoryName(dllFileCodebasePath)
                .Returns($"file:///c/{baseDirectory}");
            _fileSystem.Path.Combine($"c/{baseDirectory}", $"{assemblyName.Name}.xml")
                .Returns(xmlFileCodebasePath);
            _fileSystem.File.Exists(xmlFileCodebasePath).Returns(true);

            var result = _testClass.TryFindFor(_assembly, out var path);
            result.ShouldBeTrue();
            path.ShouldBe(xmlFileCodebasePath);
        }

        [Fact]
        public void Call_TryFindFor_WithAssemblyCodebase2_Works()
        {
            var assemblyName = new AssemblyName { Name = "file" };
            var baseDirectory = "path/to";
            var pathToFileWithoutExtension = $"{baseDirectory}/file";
            var dllFilePath = $"{pathToFileWithoutExtension}.dll";
            var xmlFilePath = $"{pathToFileWithoutExtension}.xml";
            var dllFileCodebasePath = $"file:///c/{pathToFileWithoutExtension}.dll";
            var xmlFileCodebasePath = $"c/{pathToFileWithoutExtension}.xml";

            // No location
            _assembly.Location.Returns(dllFilePath);
            _assembly.GetName().Returns(assemblyName);
            _fileSystem.Path.GetDirectoryName(dllFilePath).Returns(baseDirectory);
            _fileSystem.Path.Combine(baseDirectory, $"{assemblyName.Name}.xml").Returns(xmlFilePath);
            _fileSystem.File.Exists(xmlFilePath).Returns(false);

            _assembly.CodeBase.Returns(dllFileCodebasePath);
            _fileSystem.Path.GetDirectoryName(dllFileCodebasePath)
                .Returns($"file:///c/{baseDirectory}");
            _fileSystem.Path.Combine($"c/{baseDirectory}", $"{assemblyName.Name}.xml")
                .Returns(xmlFileCodebasePath);
            _fileSystem.File.Exists(xmlFileCodebasePath).Returns(true);

            var result = _testClass.TryFindFor(_assembly, out var path);
            result.ShouldBeTrue();
            path.ShouldBe(xmlFileCodebasePath);
        }
    }
}