# Hashsum

[![Build](https://img.shields.io/appveyor/ci/Tyrrrz/Hashsum/master.svg)](https://ci.appveyor.com/project/Tyrrrz/Hashsum/branch/master)
[![Tests](https://img.shields.io/appveyor/tests/Tyrrrz/Hashsum/master.svg)](https://ci.appveyor.com/project/Tyrrrz/Hashsum/branch/master/tests)
[![Coverage](https://img.shields.io/codecov/c/gh/Tyrrrz/Hashsum/master.svg)](https://codecov.io/gh/Tyrrrz/Hashsum)
[![NuGet](https://img.shields.io/nuget/v/Hashsum.svg)](https://nuget.org/packages/Hashsum)
[![NuGet](https://img.shields.io/nuget/dt/Hashsum.svg)](https://nuget.org/packages/Hashsum)
[![Donate](https://img.shields.io/badge/patreon-donate-yellow.svg)](https://patreon.com/tyrrrz)
[![Donate](https://img.shields.io/badge/buymeacoffee-donate-yellow.svg)](https://buymeacoffee.com/tyrrrz)

Hashsum is a library for generating checksums based on arbitrary sets of values. It works by invariantly formatting values into a string buffer and calculating hash based on resulting string.

## Download

- [NuGet](https://nuget.org/packages/Hashsum): `dotnet add package Hashsum`
- [Continuous integration](https://ci.appveyor.com/project/Tyrrrz/Hashsum)

## Features

- Convenient fluent interface
- Culture and format invariant
- Pluggable hashing algorithm
- Targets .NET Framework 4.5+ and .NET Standard 1.3+
- No external dependencies

## Usage

```c#
var checksum = new ChecksumBuilder()
    .Mutate("hello world")
    .Mutate(12345678)
    .Mutate(10e-5)
    .Mutate(DateTime.Now)
    .Calculate()
    .ToString(); // 4vKHCvfiiF/RLEfiqMnCkzQ8IYGu4K8erlWyzdsvqrU=
```

## Libraries used

- [NUnit](https://github.com/nunit/nunit)

## Donate

If you really like my projects and want to support me, consider donating to me on [Patreon](https://patreon.com/tyrrrz) or [BuyMeACoffee](https://buymeacoffee.com/tyrrrz). All donations are optional and are greatly appreciated. 🙏