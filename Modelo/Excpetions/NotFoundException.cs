using System;

namespace Model.Excpetions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string msg) : base(msg) {}
    }
}
