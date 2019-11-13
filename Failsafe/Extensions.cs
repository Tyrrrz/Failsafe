using System;
using System.Threading.Tasks;
using Failsafe.Internal;

namespace Failsafe
{
    /// <summary>
    /// Extensions for <see cref="Failsafe"/>.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Configures this instance to retry on exceptions of any type.
        /// </summary>
        public static IRetry CatchAnyException(this IRetry retry, Predicate<Exception>? exceptionPredicate = null) =>
            retry.Catch(true, exceptionPredicate);

        /// <summary>
        /// Specifies a constant delay between retries.
        /// </summary>
        public static IRetry WithDelay(this IRetry retry, TimeSpan delay) => retry.WithDelay(_ => delay);

        /// <summary>
        /// Executes an action with retry.
        /// </summary>
        public static void Execute(this IRetry retry, Action action) => retry.Execute(action.ToObjectFunc());

        /// <summary>
        /// Executes an asynchronous action with retry.
        /// </summary>
        public static Task ExecuteAsync(this IRetry retry, Func<Task> taskFunction) => retry.ExecuteAsync(taskFunction.ToObjectTaskFunc());
    }
}