using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallTracer.Models
{
   public  interface ITraceMetadata
    {
       void TraceDetails(HttpContext context, string level, double responceTime);
        void TraceContent(object request, object reply);
    }
}
