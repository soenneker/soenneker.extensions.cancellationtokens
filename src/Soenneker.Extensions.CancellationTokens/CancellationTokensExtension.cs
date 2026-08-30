using System.Runtime.CompilerServices;
using System.Threading;

namespace Soenneker.Extensions.CancellationTokens;

/// <summary>
/// Provides allocation-aware cancellation-token composition.
/// </summary>
public static class CancellationTokensExtension
{
    /// <summary>
    /// Returns a linked token if both are cancelable; otherwise returns whichever is cancelable (or default if neither).
    /// Avoids allocating a CTS unless strictly necessary.
    /// </summary>
    /// <param name="first">The first token to combine.</param>
    /// <param name="second">The second token to combine.</param>
    /// <param name="cts">Receives the linked source when one is allocated; otherwise <see langword="null"/>. The caller owns and must dispose it.</param>
    /// <returns>A token canceled when either input cancels.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CancellationToken Link(this CancellationToken first, CancellationToken second, out CancellationTokenSource? cts)
    {
        cts = null;

        // Fast path: identical tokens (same CTS + state) → no link needed
        if (first == second)
            return first;

        // Already canceled? Combined should be canceled; linking adds no value.
        if (first.IsCancellationRequested) 
            return first;

        if (second.IsCancellationRequested)
            return second;

        if (!second.CanBeCanceled)
            return first;

        if (!first.CanBeCanceled) 
            return second;

        // Both can cancel → allocate a linked CTS
        cts = CancellationTokenSource.CreateLinkedTokenSource(first, second);
        return cts.Token;
    }
}
