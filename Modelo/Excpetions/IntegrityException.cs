using System;

namespace Model.Excpetions
{
    public class IntegrityException : ApplicationException
    {
        public IntegrityException(string message) : base(message) {}
    }
}
