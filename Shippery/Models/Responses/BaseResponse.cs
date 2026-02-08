using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shippery.Models.Responses
{
    public class BaseResponse
    {
        private bool success;
        private string message;
        private object data;

        public BaseResponse()
        {
            success = false;
            message = "";
            data = null;
        }

        public BaseResponse(bool s):this()
        {
            success = s;
        }

        public BaseResponse(bool s, string m):this(s)
        {
            success = s;
            message = m;
        }

        public BaseResponse(bool s, object d) : this(s)
        {
            success = s;
            data = d;
        }

        public BaseResponse(bool s, string m, object d):this(s,m)
        {
            data = d;
        }

        public bool Success { get => success; }
        public string Message { get => message; }
        public object Data { get => data; }
    }
}
