using System;
using System.Threading;
using System.Threading.Tasks;

namespace Axuno.TextTemplating.VirtualFiles
{
public static class SemaphoreSlimExtensions
    {
        public static async Task<IDisposable> LockAsync(this SemaphoreSlim semaphoreSlim)
        {
            await semaphoreSlim.WaitAsync();
            return GetDispose(semaphoreSlim);
        }

        public static async Task<IDisposable> LockAsync(this SemaphoreSlim semaphoreSlim, CancellationToken cancellationToken)
        {
            await semaphoreSlim.WaitAsync(cancellationToken);
            return GetDispose(semaphoreSlim);
        }

        public static async Task<IDisposable> LockAsync(this SemaphoreSlim semaphoreSlim, int millisecondsTimeout)
        {
            await semaphoreSlim.WaitAsync(millisecondsTimeout);
            return GetDispose(semaphoreSlim);
        }

        public static async Task<IDisposable> LockAsync(this SemaphoreSlim semaphoreSlim, int millisecondsTimeout, CancellationToken cancellationToken)
        {
            await semaphoreSlim.WaitAsync(millisecondsTimeout, cancellationToken);
            return GetDispose(semaphoreSlim);
        }

        public static async Task<IDisposable> LockAsync(this SemaphoreSlim semaphoreSlim, TimeSpan timeout)
        {
            await semaphoreSlim.WaitAsync(timeout);
            return GetDispose(semaphoreSlim);
        }

        public static async Task<IDisposable> LockAsync(this SemaphoreSlim semaphoreSlim, TimeSpan timeout, CancellationToken cancellationToken)
        {
            await semaphoreSlim.WaitAsync(timeout, cancellationToken);
            return GetDispose(semaphoreSlim);
        }

        public static IDisposable Lock(this SemaphoreSlim semaphoreSlim)
        {
            semaphoreSlim.Wait();
            return GetDispose(semaphoreSlim);
        }

        public static IDisposable Lock(this SemaphoreSlim semaphoreSlim, CancellationToken cancellationToken)
        {
            semaphoreSlim.Wait(cancellationToken);
            return GetDispose(semaphoreSlim);
        }

        public static IDisposable Lock(this SemaphoreSlim semaphoreSlim, int millisecondsTimeout)
        {
            semaphoreSlim.Wait(millisecondsTimeout);
            return GetDispose(semaphoreSlim);
        }

        public static IDisposable Lock(this SemaphoreSlim semaphoreSlim, int millisecondsTimeout, CancellationToken cancellationToken)
        {
            semaphoreSlim.Wait(millisecondsTimeout, cancellationToken);
            return GetDispose(semaphoreSlim);
        }

        public static IDisposable Lock(this SemaphoreSlim semaphoreSlim, TimeSpan timeout)
        {
            semaphoreSlim.Wait(timeout);
            return GetDispose(semaphoreSlim);
        }

        public static IDisposable Lock(this SemaphoreSlim semaphoreSlim, TimeSpan timeout, CancellationToken cancellationToken)
        {
            semaphoreSlim.Wait(timeout, cancellationToken);
            return GetDispose(semaphoreSlim);
        }

        private static IDisposable GetDispose(this SemaphoreSlim semaphoreSlim)
        {
            return new DisposeAction(() =>
            {
                semaphoreSlim.Release();
            });
        }

        /// <summary>
        /// This class can be used to provide an action when
        /// Dispose method is called.
        /// </summary>
        public class DisposeAction : IDisposable
        {
            private readonly Action _action;

            /// <summary>
            /// Creates a new <see cref="DisposeAction"/> object.
            /// </summary>
            /// <param name="action">Action to be executed when this object is disposed.</param>
            public DisposeAction(Action action)
            {
                Check.NotNull(action, nameof(action));

                _action = action;
            }

            /// <summary>
            /// Invokes the <see cref="Action"/>.
            /// </summary>
            public void Dispose()
            {
                _action();
            }
        }
    }
}
