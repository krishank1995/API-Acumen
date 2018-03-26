using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallTracer.Models
{
    public class TraceMetadata:ITraceMetadata
    {
        public int Id { get; set; }
        public string Type { get; set; }  // --> Failure,Success,Warning
        public object RequestContent { get; set; }
        public string RequestUri { get; set; }  //
        public string RequestMethod { get; set; }   //
        public DateTime? RequestTimestamp { get; set; } //
        public object ResponseContent { get; set; }
        public int ResponseStatusCode { get; set; } //
        public DateTime? ResponseTimestamp { get; set; } //
        public double ResponceTimeMs { get; set; } //

        public void TraceDetails(HttpContext context,string level, double responceTime)
        {
            Type = level;
            RequestMethod = context.Request.Method;
            RequestUri = context.Request.Path;
            ResponseStatusCode = context.Response.StatusCode;
            ResponceTimeMs = responceTime;
            return this;
        }

        

        public void TraceContent(object request, object reply)
        {
            ResponseContent = reply;
            RequestContent = request;
        }

    }
}

