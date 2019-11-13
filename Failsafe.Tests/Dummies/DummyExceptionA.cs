namespace Failsafe.Tests.Dummies
{
    public class DummyExceptionA : DummyException
    {
        public DummyExceptionA(string? dummyProperty = null) : base(dummyProperty)
        {
        }
    }
}