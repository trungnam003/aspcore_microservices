using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.SeedWord
{
    public class ApiErrorResult<T> : ApiResult<T>
    {
        public List<string> Errors { get; set; } 
        public ApiErrorResult() : this("Something wrong happened. Please try again later")
        {

        }

        public ApiErrorResult(string message) : base(false, message)
        {
            Errors = new List<string>();
        }

        public ApiErrorResult(List<string> errors) : base(false)
        {
            Errors = errors;
        }
    }
}
