using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CallTracerLibrary.Models
{
    public class AnalysisMetadata
    {
        public string Resource { get; set; }
        public int RequestCount { get; set; }
        public int SuccessCount { get; set; }
        public int ServerFailureCount { get; set; }
        public int ClientFailureCount { get; set; }
        [DisplayFormat(DataFormatString = "{0:#.##}")]
        public double SuccessAverageResponseTime { get; set; }
        [DisplayFormat(DataFormatString = "{0:#.##}")]
        public double ClientFailureAverageResponseTime { get; set; }
        [DisplayFormat(DataFormatString = "{0:#.##}")]
        public double ServerFailureAverageResponseTime { get; set; }

    }
}
