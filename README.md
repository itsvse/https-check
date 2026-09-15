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

Version: 1.0.3+00d3e920c7c2778dbccd213af429453119f03dd3
 -------------------------------------------------------------------------------------------------------
 | Name                        | Value                                                                 |
 -------------------------------------------------------------------------------------------------------
 | URL                         | https://www.itsvse.com                                                |
 -------------------------------------------------------------------------------------------------------
 | Host                        | www.itsvse.com                                                        |
 -------------------------------------------------------------------------------------------------------
 | Port                        | 443                                                                   |
 -------------------------------------------------------------------------------------------------------
 | IP Address                  | 47.253.250.192                                                        |
 -------------------------------------------------------------------------------------------------------
 | Domain Name                 | itsvse.com                                                            |
 -------------------------------------------------------------------------------------------------------
 | Issuer                      | CN=SSL.com RSA SSL subCA, O=SSL Corporation, L=Houston, S=Texas, C=US |
 -------------------------------------------------------------------------------------------------------
 | Certificate Start Date      | 2026/6/10 21:15:07                                                    |
 -------------------------------------------------------------------------------------------------------
 | Certificate Expiration Date | 2026/12/25 21:15:07                                                   |
 -------------------------------------------------------------------------------------------------------

------------------------------
 | Security Protocol | Result |
 ------------------------------
 | Ssl2              | NO     |
 ------------------------------
 | Ssl3              | NO     |
 ------------------------------
 | Tls               | NO     |
 ------------------------------
 | Default           | NO     |
 ------------------------------
 | Tls11             | NO     |
 ------------------------------
 | Tls12             | YES    |
 ------------------------------
 | Tls13             | NO     |
 ------------------------------

 -------------------------
 | Http Version | Result |
 -------------------------
 | 1.0          | YES    |
 -------------------------
 | 1.1          | YES    |
 -------------------------
 | 2.0          | YES    |
 -------------------------
 | 3.0          | NO     |
 -------------------------
```
