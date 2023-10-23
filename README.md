# Https-Check

[![NuGet Version](http://img.shields.io/nuget/v/https-check)](https://www.nuget.org/packages/https-check) [![NuGet Downloads](https://img.shields.io/nuget/dt/https-check)](https://www.nuget.org/packages/https-check) [![GitHub](https://img.shields.io/github/license/itsvse/https-check)](https://img.shields.io/github/license/itsvse/https-check)

Https-Check is a simple ssl/tls transmission security protocol testing tool, which is used to check the security protocol version supported by https websites.

- Supported .NET Core >= 3.1

Installation
-----------------

You can install it globally via the dotnet command.

**Install tool**

```ps
dotnet tool install --global https-check
```

**Uninstall tool**

```ps
dotnet tool uninstall --global https-check
```

Usage
-----------------

You can use it by opening a cmd window and using the following command.

```ps
https-check https://www.bing.com
```