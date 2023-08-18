using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LifeLogger.Models.DTO
{
    public class APIResponse
    {
        public bool IsSuccess { get; set; } = true;   
        public HttpStatusCode StatusCode { get; set; }
        public List<string> ErrorMessage { get; set; }
        public object Result { get; set; }
    }
}