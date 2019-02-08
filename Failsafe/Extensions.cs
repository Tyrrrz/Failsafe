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
        public static IRetry CatchAnyException(this IRetry retry, Predicate<Exception> exceptionPredicate = null)
        {
            retry.GuardNotNull(nameof(retry));

            return retry.Catch(true, exceptionPredicate);
        }

        /// <summary>
        /// Executes an action with retry.
        /// </summary>
        public static void Execute(this IRetry retry, Action action)
        {
            retry.GuardNotNull(nameof(retry));
            action.GuardNotNull(nameof(action));

            // Execute
            retry.Execute(action.ToObjectFunc());
        }

        /// <summary>
        /// Executes an asynchronous action with retry.
        /// </summary>
        public static Task ExecuteAsync(this IRetry retry, Func<Task> taskFunction)
        {
            retry.GuardNotNull(nameof(retry));
            taskFunction.GuardNotNull(nameof(taskFunction));

            // Execute
            return retry.ExecuteAsync(taskFunction.ToObjectTaskFunc());
        }
    }
}