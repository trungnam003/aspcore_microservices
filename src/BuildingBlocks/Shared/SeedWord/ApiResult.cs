using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.SeedWord
{
    public class ApiResult<T>
    {
        public bool IsSuccessded { get; set; }

        public string Message { get; set; }

        public T Data { get; }

        [JsonConstructor]
        public ApiResult(bool isSucceeded, string message = null)
        {
            IsSuccessded = isSucceeded;
            Message = message;
        }

        public ApiResult(bool isSucceeded, T data, string message = null)
        {
            IsSuccessded = isSucceeded;
            Message = message;
            Data = data;
        }
    }
}
