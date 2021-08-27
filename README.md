# Welcome to XmlDocFinder 👋
[![Nuget](https://img.shields.io/nuget/v/XmlDocFinder?style=flat-square)](https://www.nuget.org/packages/XmlDocFinder/) [![GitHub Workflow Status](https://img.shields.io/github/workflow/status/Dragonblf/XmlDocFinder/Build%20&%20Test?style=flat-square)](https://github.com/Dragonblf/XmlDocFinder/actions/workflows/coverage-analysis.yml) [![Documentation](https://img.shields.io/badge/documentation-yes-brightgreen.svg?style=flat-square)](https://github.com/Dragonblf/XmlDocFinder) ![GitHub](https://img.shields.io/github/license/Dragonblf/XmlDocFinder?style=flat-square) [![Codecov](https://img.shields.io/codecov/c/github/Dragonblf/XmlDocFinder?style=flat-square&token=6610G1MZ8J)](https://app.codecov.io/gh/Dragonblf/XmlDocFinder/)

> XmlDocFinder offers a  helper class with methods to locate compiler-generated XML documentation file paths for assemblies with various techniques.

## Complete example

```c#
using System;
using XmlDocFinder;

namespace Examples
{
    public class DocFinderExample
    {
        public void Test()
        {
            // Create XML documentation finder instance
            var finder = new DocFinder();
            
            // Get path to XML file containing summary for type
            // parameter object
            var path = finder.FindFor<object>();
            
            // Get path to XML file with return value whether 
            // search was successful or not
            var success = finder.TryFindFor<string>(out path);
            
            // Get assembly which declares object type and search
            // path to XML file containing summary for object
            var assembly = typeof(object).Assembly;
            path = finder.FindFor(assembly);
            
            // Get path to XML file with return value whether 
            // search was successful or not
            assembly = this.GetType().Assembly;
            success = finder.TryFindFor(assembly, out path);
        }
    }
}
```

## Author

👤 **Thomas Trautwein**

* Github: [@dragonblf](https://github.com/dragonblf)

## 📝 License

Copyright © 2021 [Thomas Trautwein](https://github.com/dragonblf).

This project is [MIT](https://github.com/Dragonblf/XmlDocFinder/blob/main/LICENSE) licensed.
