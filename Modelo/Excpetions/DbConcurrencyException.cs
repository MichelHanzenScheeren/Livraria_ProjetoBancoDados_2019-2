using System;

namespace Model.Excpetions
{
    public class DbConcurrencyException : ApplicationException
    {
        public DbConcurrencyException(string msg) : base(msg) {}
    }
}
