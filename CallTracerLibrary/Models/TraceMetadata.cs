using Microsoft.AspNetCore.Http;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CallTracerLibrary.Models
{
    [Table("CallTraces")]
    public class TraceMetadata
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; } 
        public string Type { get; set; }  

        public string RequestContent { get; set; }
        public string RequestContentType { get; set; }
        public string RequestHost { get; set; }
        public string RequestUri { get; set; }  
        public string RequestScheme { get; set; }
        public string RequestMethod { get; set; }   
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? RequestTimestamp { get; set; } 

        public string ResponseContent { get; set; }
        public string ResponseContentType { get; set; }
        public int ResponseStatusCode { get; set; } 
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? ResponseTimestamp { get; set; } 
        public double ResponseTimeMs { get; set; } 

    }
}

