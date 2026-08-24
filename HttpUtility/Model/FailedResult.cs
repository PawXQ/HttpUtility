using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpUtility.Model
{
    public class FailedResult
    {
        public Error error { get; set; }

        public class Error
        {
            public int code { get; set; }
            public string message { get; set; }
            public Error1[] errors { get; set; }
        }

        public class Error1
        {
            public string message { get; set; }
            public string domain { get; set; }
            public string reason { get; set; }
        }
    }
}
