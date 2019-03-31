using System;
using System.Threading.Tasks;

namespace Failsafe.Internal
{
    internal static class Extensions
    {
        public static Func<object> ToObjectFunc(this Action action)
        {
            return () =>
            {
                action.Invoke();
                return null;
            };
        }

        public static Func<Task<object>> ToObjectTaskFunc(this Func<Task> func)
        {
            return async () =>
            {
                await func.Invoke();
                return null;
            };
        }
    }
}