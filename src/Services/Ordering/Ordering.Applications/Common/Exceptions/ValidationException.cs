using FluentValidation.Results;

namespace Ordering.Application.Common.Exceptions
{
    public class AppValidationException : ApplicationException
    {
        public IDictionary<string, string[]> Errors { get; }
        public AppValidationException() : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }
        public AppValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            Errors = failures.GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }
    }
}
