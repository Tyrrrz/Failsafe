using System.Threading.Tasks;
using Failsafe.Tests.Dummies;
using NUnit.Framework;

namespace Failsafe.Tests
{
    public class RetryTests
    {
        [Test]
        public void Retry_AnyException_Test()
        {
            // Keep track of attempts
            var tryCount = 0;

            // Act
            var result = Retry.Create()
                .CatchAnyException()
                .Execute(() =>
                {
                    tryCount++;

                    if (tryCount < 5)
                        throw new DummyException();

                    return true;
                });

            // Assert
            Assert.That(result);
        }

        [Test]
        public async Task Retry_AnyException_Async_Test()
        {
            // Keep track of attempts
            var tryCount = 0;

            // Act
            var result = await Retry.Create()
                .CatchAnyException()
                .ExecuteAsync(async () =>
                {
                    await Task.Yield();

                    tryCount++;

                    if (tryCount < 5)
                        throw new DummyException();

                    return true;
                });

            // Assert
            Assert.That(result);
        }

        [Test]
        public void Retry_AnyException_ExceedRetryLimit_Test()
        {
            // Keep track of attempts
            const int maxTryCount = 10;
            var tryCount = 0;

            // Act and assert
            Assert.Catch(() =>
            {
                Retry.Create()
                    .WithMaxTryCount(maxTryCount)
                    .CatchAnyException()
                    .Execute(() =>
                    {
                        tryCount++;

                        throw new DummyException();
                    });
            });

            Assert.That(tryCount, Is.EqualTo(maxTryCount));
        }

        [Test]
        public void Retry_SpecificException_Test()
        {
            // Keep track of attempts
            var tryCount = 0;

            // Act
            var result = Retry.Create()
                .Catch<DummyExceptionA>()
                .Catch<DummyExceptionB>()
                .Execute(() =>
                {
                    tryCount++;

                    if (tryCount < 3)
                        throw new DummyExceptionA();

                    if (tryCount < 5)
                        throw new DummyExceptionB();

                    return true;
                });

            // Assert
            Assert.That(result);
        }

        [Test]
        public void Retry_SpecificException_DifferentException_Test()
        {
            // Keep track of attempts
            var tryCount = 0;

            // Act and assert
            Assert.Catch(() =>
            {
                Retry.Create()
                    .Catch<DummyExceptionA>()
                    .Execute(() =>
                    {
                        tryCount++;

                        if (tryCount < 5)
                            throw new DummyExceptionB();
                    });
            });
        }

        [Test]
        public void Retry_SpecificException_DifferentException_DerivedException_Test()
        {
            // Keep track of attempts
            var tryCount = 0;

            // Act and assert
            Assert.Catch(() =>
            {
                Retry.Create()
                    .Catch<DummyException>()
                    .Execute(() =>
                    {
                        tryCount++;

                        if (tryCount < 5)
                            throw new DummyExceptionA();
                    });
            });
        }

        [Test]
        public void Retry_SpecificException_DerivedException_Test()
        {
            // Keep track of attempts
            var tryCount = 0;

            // Act
            var result = Retry.Create()
                .Catch<DummyException>(true)
                .Execute(() =>
                {
                    tryCount++;

                    if (tryCount < 5)
                        throw new DummyExceptionA();

                    return true;
                });

            // Assert
            Assert.That(result);
        }

        [Test]
        public void Retry_SpecificException_DerivedException_SameType_Test()
        {
            // Keep track of attempts
            var tryCount = 0;

            // Act
            var result = Retry.Create()
                .Catch<DummyException>(true)
                .Execute(() =>
                {
                    tryCount++;

                    if (tryCount < 5)
                        throw new DummyException();

                    return true;
                });

            // Assert
            Assert.That(result);
        }

        [Test]
        public void Retry_SpecificException_Predicate_Test()
        {
            // Keep track of attempts
            var tryCount = 0;

            // Act and assert
            Assert.Catch(() =>
            {
                Retry.Create()
                    .Catch<DummyException>(false, e => e.DummyProperty != null)
                    .Execute(() =>
                    {
                        tryCount++;

                        if (tryCount < 5)
                            throw new DummyException("Hello world");

                        throw new DummyException();
                    });
            });

            // Assert
            Assert.That(tryCount, Is.EqualTo(5));
        }
    }
}