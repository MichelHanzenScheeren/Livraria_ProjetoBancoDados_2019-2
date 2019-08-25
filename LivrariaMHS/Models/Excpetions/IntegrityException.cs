using System;

namespace LivrariaMHS.Models.Excpetions
{
    public class IntegrityException : ApplicationException
    {
        public IntegrityException(string message) : base(message)
        {
        }
    }
}
