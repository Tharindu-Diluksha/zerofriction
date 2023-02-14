using System;

namespace ZeroFriction.DB.Domain.Exceptions
{
    public class ConcurrencyException : Exception
    {
        public ConcurrencyException()
            : base("ERROR_CONCURRENCY_VIOLATION")
        {
        }
    }
}
