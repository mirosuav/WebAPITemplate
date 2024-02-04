using System.Runtime.CompilerServices;

namespace TodoApi.Tools;
public static class AwaitableExtensions
{
    public static ConfiguredTaskAwaitable KeepContext(this Task t) => t.ConfigureAwait(true);
    public static ConfiguredTaskAwaitable<T> KeepContext<T>(this Task<T> t) => t.ConfigureAwait(true);
    public static ConfiguredTaskAwaitable FreeContext(this Task t) => t.ConfigureAwait(false);
    public static ConfiguredTaskAwaitable<T> FreeContext<T>(this Task<T> t) => t.ConfigureAwait(false);
    public static ConfiguredValueTaskAwaitable KeepContext(this ValueTask t) => t.ConfigureAwait(true);
    public static ConfiguredValueTaskAwaitable<T> KeepContext<T>(this ValueTask<T> t) => t.ConfigureAwait(true);
    public static ConfiguredValueTaskAwaitable FreeContext(this ValueTask t) => t.ConfigureAwait(false);
    public static ConfiguredValueTaskAwaitable<T> FreeContext<T>(this ValueTask<T> t) => t.ConfigureAwait(false);
    public static ConfiguredCancelableAsyncEnumerable<T> KeepContext<T>(this IAsyncEnumerable<T> t) => t.ConfigureAwait(true);
    public static ConfiguredCancelableAsyncEnumerable<T> FreeContext<T>(this IAsyncEnumerable<T> t) => t.ConfigureAwait(false);
    public static ConfiguredCancelableAsyncEnumerable<T> KeepContext<T>(this ConfiguredCancelableAsyncEnumerable<T> t) => t.ConfigureAwait(true);
    public static ConfiguredCancelableAsyncEnumerable<T> FreeContext<T>(this ConfiguredCancelableAsyncEnumerable<T> t) => t.ConfigureAwait(false);

}
