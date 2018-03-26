using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallTracerLibrary.Models
{
    public class TraceMetadata
    {
        public int Id { get; set; } //Find better alternative to this ID // Guid maybe --> Genric Id 
        public string Type { get; set; }  // --> Failure,Success,Warning
        public string RequestContent { get; set; }
        public string RequestUri { get; set; }  //
        public string RequestMethod { get; set; }   //
        public DateTime? RequestTimestamp { get; set; } //
        public string ResponseContent { get; set; }
        public int ResponseStatusCode { get; set; } //
        public DateTime? ResponseTimestamp { get; set; } //
        public double ResponseTimeMs { get; set; } //

    }
}

