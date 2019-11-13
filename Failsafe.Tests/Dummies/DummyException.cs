using System;

namespace Failsafe.Tests.Dummies
{
    public class DummyException : Exception
    {
        public string? DummyProperty { get; }

        public DummyException(string? dummyProperty = null)
        {
            DummyProperty = dummyProperty;
        }
    }
}