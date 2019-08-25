using System;

namespace LivrariaMHS.Models.Excpetions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string msg) : base(msg)
        {
        }
    }
}
