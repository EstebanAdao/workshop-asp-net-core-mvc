using System;
namespace SalesWebMvc.Services.Exceptions
{
    public class DbConcurrenyException : ApplicationException
    {
        public DbConcurrenyException(string message) : base(message)
        {
        }
    }
}
