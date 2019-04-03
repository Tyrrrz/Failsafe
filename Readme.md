# Failsafe

[![Build](https://img.shields.io/appveyor/ci/Tyrrrz/Failsafe/master.svg)](https://ci.appveyor.com/project/Tyrrrz/Failsafe)
[![Tests](https://img.shields.io/appveyor/tests/Tyrrrz/Failsafe/master.svg)](https://ci.appveyor.com/project/Tyrrrz/Failsafe)
[![NuGet](https://img.shields.io/nuget/v/Failsafe.svg)](https://nuget.org/packages/Failsafe)
[![NuGet](https://img.shields.io/nuget/dt/Failsafe.svg)](https://nuget.org/packages/Failsafe)
[![Donate](https://img.shields.io/badge/patreon-donate-yellow.svg)](https://patreon.com/tyrrrz)
[![Donate](https://img.shields.io/badge/buymeacoffee-donate-yellow.svg)](https://buymeacoffee.com/tyrrrz)

Failsafe is a very simple library that provides fluent interface for retrying an operation. It can be configured to catch an arbitrary set of exceptions using various patterns, with optional delay and retry limit. Works with synchronous and asynchronous methods.

## Download

- [NuGet](https://nuget.org/packages/Failsafe): `dotnet add package Failsafe`
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

The following code executes `FlakyMethod`, retrying on any exception until it finally succeeds.

```c#
var result = Retry.Create().CatchAnyException().Execute(FlakyMethod);
```

### Retry only on specific exceptions

```c#
var result = Retry.Create()
    .Catch<InvalidOperationException>() // match specific exception
    .Catch<IOException>(true) // match specific exception and derived from it
    .Catch<HttpRequestException>(false, ex => ex.Message.Contains("403")) // match specific exception and use predicate
    .Execute(FlakyMethod);
```

### Configure retry limit and delay

```c#
var result = Retry.Create()
    .CatchAnyException()
    .WithMaxTryCount(15) // no more than 15 attempts
    .WithDelay(i => TimeSpan.FromSeconds(i*0.2)) // wait 0.2s after first attempt, 0.4s after second, etc
    .Execute(FlakyMethod);
```

## Libraries used

- [ConfigureAwait.Fody](https://github.com/Fody/ConfigureAwait)
- [NUnit](https://github.com/nunit/nunit)

## Donate

If you really like my projects and want to support me, consider donating to me on [Patreon](https://patreon.com/tyrrrz) or [BuyMeACoffee](https://buymeacoffee.com/tyrrrz). All donations are optional and are greatly appreciated. 🙏