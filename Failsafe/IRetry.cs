using System;
using System.Threading.Tasks;

namespace Failsafe
{
    /// <summary>
    /// Interface for <see cref="Retry"/>.
    /// </summary>
    public interface IRetry
    {
        /// <summary>
        /// Configures this instance to retry on exceptions of given type and, optionally, derived types.
        /// </summary>
        IRetry Catch<TException>(bool catchDerivedExceptions = false, Predicate<TException> exceptionPredicate = null)
            where TException : Exception;

        /// <summary>
        /// Limits retrying to given number of attempts.
        /// </summary>
        IRetry WithMaxTryCount(int maxTryCount);

        /// <summary>
        /// Specifies the delay to wait between retries.
        /// </summary>
        IRetry WithDelay(TimeSpan delay);

        /// <summary>
        /// Executes a function with retry.
        /// </summary>
        TReturn Execute<TReturn>(Func<TReturn> function);

        /// <summary>
        /// Executes an asynchronous function with retry.
        /// </summary>
        Task<TReturn> ExecuteAsync<TReturn>(Func<Task<TReturn>> taskFunction);
    }
}