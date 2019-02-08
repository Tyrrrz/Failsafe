# Failsafe

[![Build](https://img.shields.io/appveyor/ci/Tyrrrz/Failsafe/master.svg)](https://ci.appveyor.com/project/Tyrrrz/Failsafe)
[![Tests](https://img.shields.io/appveyor/tests/Tyrrrz/Failsafe/master.svg)](https://ci.appveyor.com/project/Tyrrrz/Failsafe)
[![NuGet](https://img.shields.io/nuget/v/Failsafe.svg)](https://nuget.org/packages/Failsafe)
[![NuGet](https://img.shields.io/nuget/dt/Failsafe.svg)](https://nuget.org/packages/Failsafe)

Failsafe is a library that provides a fluent interface for retrying an operation.

## Download

- [NuGet](https://nuget.org/packages/Failsafe): `Install-Package Failsafe`
- [Continuous integration](https://ci.appveyor.com/project/Tyrrrz/Failsafe)

## Features

- Retry on specific, derived or any exception
- Match exceptions with a predicate
- Limit number of retries
- Configurable delay between retries
- Supports synchronous and asynchronous execution
- Fluent interface
- Targets .NET Framework 4.5+ and .NET Standard 2.0+
- No external dependencies

## Usage

### Basic example

```c#
var result = Retry.Create().CatchAnyException().Execute(FlakyMethod);
```

### Retry only on specific exceptions

```c#
var result = Retry.Create()
	.Catch<InvalidOperationException>() // match specific exception
	.Catch<IOException>(true) // match specific exception and derived from it
	.Catch<FileNotFoundException>(false, ex => ex.FileName == "file.txt") // match specific exception and use predicate
	.Execute(FlakyMethod);
```

### Configure retry limit and delay

```c#
var result = Retry.Create()
	.CatchAnyException()
	.WithMaxTryCount(15) // no more than 15 attempts
	.WithDelay(TimeSpan.FromSeconds(0.2)) // wait 0.2s before trying again
	.Execute(FlakyMethod);
```

## Libraries used

- [NUnit](https://github.com/nunit/nunit)