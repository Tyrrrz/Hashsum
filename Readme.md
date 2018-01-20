# Hashsum

[![Build](https://img.shields.io/appveyor/ci/Tyrrrz/Hashsum/master.svg)](https://ci.appveyor.com/project/Tyrrrz/Hashsum)
[![Tests](https://img.shields.io/appveyor/tests/Tyrrrz/Hashsum/master.svg)](https://ci.appveyor.com/project/Tyrrrz/Hashsum)
[![NuGet](https://img.shields.io/nuget/v/Hashsum.svg)](https://nuget.org/packages/Hashsum)
[![NuGet](https://img.shields.io/nuget/dt/Hashsum.svg)](https://nuget.org/packages/Hashsum)

Hashsum can be used to easily calculate checksums based on arbitrary sets of values.

## Download

- Using NuGet: `Install-Package Hashsum`
- [Continuous integration](https://ci.appveyor.com/project/Tyrrrz/Hashsum)

## Features

- Convenient fluent interface
- Culture and format invariant

## Usage

```c#
using (var builder = new ChecksumBuilder())
{
    var checksum = builder
        .Mutate("hello world")
        .Mutate(12345678)
        .Mutate(10e-5)
        .Mutate(DateTime.Now)
        .Calculate();
}
```