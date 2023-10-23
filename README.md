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
Output:

```
 _   _ _   _                ____ _               _
| | | | |_| |_ _ __  ___   / ___| |__   ___  ___| | __
| |_| | __| __| '_ \/ __| | |   | '_ \ / _ \/ __| |/ /
|  _  | |_| |_| |_) \__ \ | |___| | | |  __/ (__|   <
|_| |_|\__|\__| .__/|___/  \____|_| |_|\___|\___|_|\_\
              |_|

 -----------------------------------------------------------------------------------------------------
 | Name                        | Value                                                               |
 -----------------------------------------------------------------------------------------------------
 | URL                         | https://www.bing.com                                                |
 -----------------------------------------------------------------------------------------------------
 | Host                        | www.bing.com                                                        |
 -----------------------------------------------------------------------------------------------------
 | Port                        | 443                                                                 |
 -----------------------------------------------------------------------------------------------------
 | IP Address                  | 204.79.197.200                                                      |
 -----------------------------------------------------------------------------------------------------
 | Domain Name                 | www.bing.com                                                        |
 -----------------------------------------------------------------------------------------------------
 | Issuer                      | CN=Microsoft Azure TLS Issuing CA 05, O=Microsoft Corporation, C=US |
 -----------------------------------------------------------------------------------------------------
 | Certificate Start Date      | 2023/7/27 7:57:23                                                   |
 -----------------------------------------------------------------------------------------------------
 | Certificate Expiration Date | 2024/1/23 7:57:23                                                   |
 -----------------------------------------------------------------------------------------------------

 ------------------------------
 | Security Protocol | Result |
 ------------------------------
 | SystemDefault     | YES    |
 ------------------------------
 | Ssl3              | NO     |
 ------------------------------
 | Tls               | NO     |
 ------------------------------
 | Tls11             | NO     |
 ------------------------------
 | Tls12             | YES    |
 ------------------------------
 | Tls13             | NO     |
 ------------------------------
```