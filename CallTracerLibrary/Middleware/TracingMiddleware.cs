using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CallTracerLibrary.DataProviders;
using CallTracerLibrary.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using MongoDB.Driver;

namespace CallTracerLibrary.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project.
    public class TracingMiddleware
    {
        private Stopwatch _timeKeeper;
        private readonly RequestDelegate _next;
        private IRepository<TraceMetadata, int> _repository;
        private static int _defaultPageSize = 20;

        public TracingMiddleware(RequestDelegate next, IRepository<TraceMetadata, int> repository)
        {
            _repository = repository;
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            TraceMetadata _trace = new TraceMetadata();
            if (httpContext.Request.Path != "/trace" && httpContext.Request.Path != "/ui")
            {
                _trace.RequestContentType = httpContext.Request.ContentType;
                _trace.ResponseContentType = httpContext.Response.ContentType;
                _trace.RequestTimestamp = DateTime.Now;
                _timeKeeper = Stopwatch.StartNew();

                #region Retrieve Request-Responce Content 
                _trace.RequestContent= await FormatRequest(httpContext.Request);
                var originalBodyStream = httpContext.Response.Body;

                using (var responseBody = new MemoryStream())
                {
                    httpContext.Response.Body = responseBody;
                    try
                    {
                        await _next(httpContext);
                        _timeKeeper.Stop();

                        if (httpContext.Response.StatusCode != 204 && (httpContext.Response.ContentType == "application/json; charset=utf-8"))
                        {
                            _trace.ResponseContent = await FormatResponse(httpContext.Response);
                            _trace.ResponseContentType = httpContext.Response.ContentType;
                            await responseBody.CopyToAsync(originalBodyStream);
                        }
                        else
                        {
                            _trace.ResponseContent = "N/A";
                            _trace.ResponseContentType = "N/A";
                        }

                        MapMetadata(_trace, httpContext);
                        _repository.SaveAsync(_trace);
                    }

                    catch (Exception ex)
                    {
                        _timeKeeper.Stop();
                        MapMetadata(_trace, httpContext);
                        _trace.ResponseContent = ex.ToString();
                        _trace.ResponseContentType = "Exception";
                        _trace.ResponseStatusCode = 500;
                        _repository.SaveAsync(_trace);
                        httpContext.Response.Body = originalBodyStream;
                        throw;
                    }

                }

                #endregion


            }
           
            
            // Request Trace Logs should be Configurable .
            else if (httpContext.Request.Path == "/trace")
            {
                IEnumerable<TraceMetadata> asyncDocumentsFiltered;
                IEnumerable<TraceMetadata> asyncDocumentsPaged;
                IEnumerable<TraceMetadata> asyncDocuments;

                // Pagination Params
                int pageSize, pageNumber, recordsToSkip;
                var pageSizeStr = httpContext.Request.Query["size"].ToString();
                var pageNumberStr = httpContext.Request.Query["page"].ToString();
                int.TryParse(pageSizeStr, out pageSize);
                int.TryParse(pageNumberStr, out pageNumber);
                pageSize = pageSize == 0 ? pageSize = _defaultPageSize : pageSize;
                recordsToSkip = pageSize * pageNumber;

                // Get All documents.
                asyncDocuments = await _repository.GetAll();

                //Apply Filters
                asyncDocumentsFiltered = FilterTraces(asyncDocuments,httpContext);

                //Apply Pagination
                asyncDocumentsPaged = asyncDocumentsFiltered.Skip(recordsToSkip).Take(pageSize);
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(asyncDocumentsPaged);
                httpContext.Response.ContentType = "Application/json";
                await httpContext.Response.WriteAsync(json);
            }

            // UI --> Endpoint should be Configurable.
            else if (httpContext.Request.Path == "/ui")
            {
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = System.IO.Path.Combine(currentDirectory, "FrontEnd", "templete.html");
                string readContents;

                using (StreamReader streamReader = new StreamReader(filePath, Encoding.UTF8))
                {
                    readContents = streamReader.ReadToEnd();
                }

                httpContext.Response.ContentType = "text/html";
                await httpContext.Response.WriteAsync(readContents);
            }
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            request.EnableRewind();
            var body = request.Body;
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            body.Seek(0, SeekOrigin.Begin);
            request.Body = body;

            if (bodyAsText.Length == 0)
            {
                return "RequestBody=Empty";
            }
            else
            {
                return $"RequestBody={bodyAsText}";
            }
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return $"Response:{text}";
        }

        void MapMetadata (TraceMetadata trace, HttpContext httpContext)
        {
            trace.RequestContentType = httpContext.Request.ContentType;
            trace.ResponseTimestamp = DateTime.Now;
            trace.ResponseTimeMs = _timeKeeper.Elapsed.TotalMilliseconds;
            trace.ResponseStatusCode = httpContext.Response.StatusCode;
            trace.RequestMethod = httpContext.Request.Method;
            trace.RequestUri = httpContext.Request.Path;
            trace.RequestHost = httpContext.Request.Host.ToString();
            trace.Type = httpContext.Response.StatusCode < 500 ? "Regular" : "Error";
           
        }

        public IEnumerable<TraceMetadata> FilterTraces (IEnumerable<TraceMetadata> inputDocs,HttpContext httpContext)
        {
            #region filters

            //Timeframe Filter
            if (httpContext.IsFilterApplicable("starttimestamp") && httpContext.IsFilterApplicable("stoptimestamp"))
            {
                DateTime.TryParse(httpContext.Request.Query["starttimestamp"].ToString(), out DateTime stamp1);
                DateTime.TryParse(httpContext.Request.Query["stoptimestamp"].ToString(), out DateTime stamp2);
                inputDocs = from doc in inputDocs
                            where DateTime.Compare(doc.RequestTimestamp, stamp1) > 0 && DateTime.Compare(doc.RequestTimestamp, stamp2) < 0
                            select doc;
            }

            //Type Filter
            if (httpContext.IsFilterApplicable("type"))
            {
                var traceType = httpContext.Request.Query["type"].ToString();
                inputDocs = from doc in inputDocs
                         where string.Compare(doc.Type, traceType, StringComparison.OrdinalIgnoreCase) == 0
                         select doc;
            }

            //Hostfilter
            if (httpContext.IsFilterApplicable("host"))
            {
                var reqHost = httpContext.Request.Query["host"].ToString();
                inputDocs = from doc in inputDocs
                            where string.Compare(doc.RequestHost, reqHost, StringComparison.OrdinalIgnoreCase) == 0
                            select doc;
            }

            //Method Filter
            if (httpContext.IsFilterApplicable("method"))
            {
                var reqMethod = httpContext.Request.Query["method"].ToString();
                inputDocs = from doc in inputDocs
                            where string.Compare(doc.RequestMethod, reqMethod, StringComparison.OrdinalIgnoreCase) == 0
                            select doc;
            }

            //Response Code Filter
            if (httpContext.IsFilterApplicable("code"))
            {
                int.TryParse(httpContext.Request.Query["code"].ToString(), out int code);
                inputDocs = from doc in inputDocs
                            where doc.ResponseStatusCode==code
                            select doc;
            }

            #endregion

            return inputDocs;
        }

        public bool IsFilterApplicable (string key,HttpContext httpContext)
        {
            if(httpContext.Request.Query.ContainsKey(key))
            {
                return true;
            }

            return false;
           
        }

        public void GenricLinq ()
        {
            //Expression<Func<TraceMetadata, bool>> whereClause = a => a.Type == traceType;
            //var x = frSomeList.Where(whereClause);
        }


    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class TracingMiddlewareExtensions
    {
        public static IApplicationBuilder UseTracingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TracingMiddleware>();
        }

        public static bool IsFilterApplicable(this HttpContext httpContext, string key)
        {
            if (httpContext.Request.Query.ContainsKey(key))
            {
                return true;
            }

            return false;
        }
    }
}