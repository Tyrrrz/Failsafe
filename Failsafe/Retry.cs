using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Failsafe.Internal;

namespace Failsafe
{
    /// <summary>
    /// Executes delegates with retry.
    /// </summary>
    public partial class Retry : IRetry
    {
        private readonly List<Predicate<Exception>> _fullExceptionPredicates = new List<Predicate<Exception>>();

        private int? _maxTryCount;
        private TimeSpan? _delay;

        private bool MatchException(Exception exception)
        {
            // An exception is matched if it's matched against any of the predicates
            foreach (var predicate in _fullExceptionPredicates)
            {
                if (predicate(exception))
                    return true;
            }

            return false;
        }

        /// <inheritdoc />
        public IRetry Catch<TException>(bool catchDerivedExceptions = false,
            Predicate<TException> exceptionPredicate = null) where TException : Exception
        {
            // Create predicate
            var predicate = CreateFullExceptionPredicate(catchDerivedExceptions, exceptionPredicate);

            // Add to the list of predicates
            _fullExceptionPredicates.Add(predicate);

            return this;
        }

        /// <inheritdoc />
        public IRetry WithMaxTryCount(int maxTryCount)
        {
            _maxTryCount = maxTryCount;
            return this;
        }

        /// <inheritdoc />
        public IRetry WithDelay(TimeSpan delay)
        {
            _delay = delay;
            return this;
        }

        /// <inheritdoc />
        public TReturn Execute<TReturn>(Func<TReturn> function)
        {
            function.GuardNotNull(nameof(function));

            // Enter retry loop
            for (var i = 1;; i++)
            {
                try
                {
                    return function.Invoke();
                }
                catch (Exception ex)
                {
                    // If reached max try count - throw
                    if (i >= _maxTryCount)
                        throw;

                    // If exception isn't matched - throw
                    if (!MatchException(ex))
                        throw;

                    // If delay is specified - wait before next retry
                    if (_delay != null)
                        Thread.Sleep(_delay.Value);
                }
            }
        }

        /// <inheritdoc />
        public async Task<TReturn> ExecuteAsync<TReturn>(Func<Task<TReturn>> taskFunction)
        {
            taskFunction.GuardNotNull(nameof(taskFunction));

            // Enter retry loop
            for (var i = 1;; i++)
            {
                try
                {
                    return await taskFunction.Invoke().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    // If reached max try count - throw
                    if (i >= _maxTryCount)
                        throw;

                    // If exception isn't matched - throw
                    if (!MatchException(ex))
                        throw;

                    // If delay is specified - wait before next retry
                    if (_delay != null)
                        await Task.Delay(_delay.Value).ConfigureAwait(false);
                }
            }
        }
    }

    public partial class Retry
    {
        private static Predicate<Exception> CreateFullExceptionPredicate<TException>(
            bool catchDerivedExceptions = false, Predicate<TException> exceptionPredicate = null)
            where TException : Exception
        {
            return ex =>
            {
                // Type matches if the type is the same or if the type is derived and this option is enabled
                var typeMatch = ex.GetType() == typeof(TException) ||
                                catchDerivedExceptions && ex.GetType().IsSubclassOf(typeof(TException));

                // If not matched - return false
                if (!typeMatch)
                    return false;

                // Invoke predicate if it's set
                var predicateMatch = exceptionPredicate?.Invoke((TException) ex) != false;

                // If not matched - return false
                if (!predicateMatch)
                    return false;

                // Return true if reached this point
                return true;
            };
        }
    }

    public partial class Retry
    {
        /// <summary>
        /// Shorthand for creating an instance of <see cref="Retry"/>.
        /// </summary>
        public static IRetry Create() => new Retry();
    }
}