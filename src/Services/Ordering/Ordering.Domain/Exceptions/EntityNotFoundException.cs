namespace Ordering.Domain.Exceptions
{
    public class EntityNotFoundException : ApplicationException
    {
        public EntityNotFoundException(string name, string type)
            : base($"Entity \"{name}\" ({type}) was not found.")
        {
        }
    }
}
