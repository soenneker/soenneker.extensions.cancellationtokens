[![](https://img.shields.io/nuget/v/soenneker.extensions.cancellationtokens.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.cancellationtokens/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.cancellationtokens/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.cancellationtokens/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.extensions.cancellationtokens.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.cancellationtokens/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.cancellationtokens/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.cancellationtokens/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Extensions.CancellationTokens

Combines two cancellation tokens while avoiding a linked `CancellationTokenSource` allocation when one is unnecessary.

## Installation

```bash
dotnet add package Soenneker.Extensions.CancellationTokens
```

## Usage

```csharp
using Soenneker.Extensions.CancellationTokens;

CancellationToken combined = requestAborted.Link(
    timeoutSource.Token,
    out CancellationTokenSource? linkedSource);

using (linkedSource)
{
    await PerformWork(combined);
}
```

The `out` source is owned by the caller. Keep it alive for the entire operation and dispose it afterward to unregister callbacks from the input tokens. It is `null` whenever no linked source was needed; disposing a nullable source with `using` is safe.

## Allocation behavior

`first.Link(second, out linkedSource)` follows these paths:

| Inputs | Returned token | `linkedSource` |
| --- | --- | --- |
| Tokens are identical | That token | `null` |
| Either token is already canceled | An already-canceled input token | `null` |
| Only one token can be canceled | The cancelable token | `null` |
| Neither token can be canceled | The default/non-cancelable token | `null` |
| Both distinct tokens can be canceled | A token linked to both inputs | Newly allocated source |

When a linked source is created, canceling either parent cancels the returned token. Canceling the linked source cancels only the combined token, not either parent. Disposal removes the link; it does not dispose or cancel the input sources.

This helper composes cancellation signals only. Work must still observe the returned token, and cancellation does not imply rollback or immediate interruption.
