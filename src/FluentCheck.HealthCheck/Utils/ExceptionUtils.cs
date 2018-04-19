using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentCheck.HealthCheck.Utils
{
    public class ExceptionUtils
    {
        private static IEnumerable<Exception> GetInnerExceptions(Exception ex)
        {
            if (ex == null)
                throw new ArgumentNullException("ex");

            var innerException = ex;
            do
            {
                yield return innerException;
                innerException = innerException.InnerException;
            }
            while (innerException != null);
        }

        public static IEnumerable<string> GetExceptions(Exception ex)
        {
            return GetInnerExceptions(ex).Select(e => e.Message);
        }
    }
}