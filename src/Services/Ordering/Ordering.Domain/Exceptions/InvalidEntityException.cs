namespace Ordering.Domain.Exceptions
{
    public class InvalidEntityException : ApplicationException
    {
        public InvalidEntityException(string name, string type)
            : base($"Entity \"{name}\" ({type}) was not found.")
        {
        }
    }
}
