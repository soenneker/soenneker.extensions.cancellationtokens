[![](https://img.shields.io/nuget/v/soenneker.extensions.cancellationtokens.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.cancellationtokens/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.cancellationtokens/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.cancellationtokens/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.extensions.cancellationtokens.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.cancellationtokens/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.cancellationtokens/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.cancellationtokens/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Extensions.CancellationTokens

A collection of helpful CancellationToken extension methods.

## Installation

```bash
dotnet add package Soenneker.Extensions.CancellationTokens
```

## Quick start

```csharp
using Soenneker.Extensions.CancellationTokens;

// Given an existing CancellationToken named first:
var result = first.Link(second, cts);
```

## Common operations

- `Link()` - Returns a linked token if both are cancelable; otherwise returns whichever is cancelable (or default if neither). Avoids allocating a CTS unless strictly necessary.
