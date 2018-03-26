using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CallTracer.DataProviders;
using CallTracer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace CallTracer.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class TracingMiddleware
    {
        private Stopwatch _timeKeeper;
        private readonly RequestDelegate _next;
        private ITraceMetadata _trace;
        private ITraceRepository _repository;

        public TracingMiddleware(RequestDelegate next,ITraceMetadata traceMetadata,ITraceRepository repository)
        {
            _trace = traceMetadata;
            _next = next;
            _repository = repository;
        }

        public async Task Invoke(HttpContext httpContext)
        {
        
            _timeKeeper = Stopwatch.StartNew();
            await _next(httpContext);
            _timeKeeper.Stop();
            _repository.SaveTrace(_trace.TraceDetails(httpContext, "level", _timeKeeper.Elapsed.TotalMilliseconds)); // --> Do the context to class mapping here

        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class TracingMiddlewareExtensions
    {
        public static IApplicationBuilder UseTracingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TracingMiddleware>();
        }
    }
}
