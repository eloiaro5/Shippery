using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shippery.Models.Responses
{
    public class UserResponse : BaseResponse {
        public UserResponse() : base() { }
        public UserResponse(bool s) : base(s) { }
        public UserResponse(bool s, string m) : base(s, m) { }
        public UserResponse(bool s, object d) : base(s, d) { }
        public UserResponse(bool s, string m, object d) : base(s, m, d) { }
    }
    public class CityResponse : BaseResponse {
        public CityResponse() : base() { }
        public CityResponse(bool s) : base(s) { }
        public CityResponse(bool s, string m) : base(s, m) { }
        public CityResponse(bool s, object d) : base(s, d) { }
        public CityResponse(bool s, string m, object d) : base(s, m, d) { }
    }
}
