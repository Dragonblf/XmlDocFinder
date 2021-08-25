using Dragonblf.XmlDocFinder;
using System;
using System.IO.Abstractions;
using Xunit;
using System.Reflection;
using FakeItEasy;
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
            _fileSystem = A.Fake<IFileSystem>();
            _testClass = new DocFinder(_fileSystem);
            _assembly = A.Fake<Assembly>();
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
            A.CallTo(() => _assembly.FullName).Returns("path");

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
        public void Call_TryFindFor_WithAssembly_EmptyString()
        {
            A.CallTo(() => _assembly.FullName).Returns("path");

            var result = _testClass.TryFindFor(_assembly, out var path);

            result.ShouldBeTrue();
            path.ShouldBe(string.Empty);
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
            A.CallTo(() => _assembly.FullName).Returns(value);
            Should.Throw<ArgumentException>(() => _testClass.TryFindFor(_assembly, out var path));
        }
    }
}