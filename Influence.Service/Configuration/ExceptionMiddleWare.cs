using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Cosmetic.Model.Dto.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Serilog;

namespace Influence.Service.Configuration
{
    public class ExceptionMiddleWare
    {
        RequestDelegate next;
        private readonly Func<object, Task> _clearCacheHeadersDelegate;

        public ExceptionMiddleWare(RequestDelegate _next)
        {
            next = _next;
            _clearCacheHeadersDelegate = ClearCacheHeaders;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string requestBodyText = "";
            string path = "";
            string customHeader = string.Empty;

            using (MemoryStream requestBodyStream = new MemoryStream())
            {
                using (MemoryStream responseBodyStream = new MemoryStream())
                {
                    Stream originalRequestBody = context.Request.Body;
                    Stream originalResponseBody = context.Response.Body;

                    try
                    {
                        if (context.Request.Headers.TryGetValue("Token", out var traceValue))
                        {
                            customHeader = traceValue;
                        }
                        await context.Request.Body.CopyToAsync(requestBodyStream);
                        requestBodyStream.Seek(0, SeekOrigin.Begin);
                        requestBodyText = new StreamReader(requestBodyStream).ReadToEnd();
                        path = context.Request.Path;

                        if (context.Request.QueryString.HasValue)
                        {
                            path += context.Request.QueryString.Value;
                        }

                        requestBodyStream.Seek(0, SeekOrigin.Begin);
                        context.Request.Body = requestBodyStream;



                        // Stopwatch watch = Stopwatch.StartNew();
                        await next(context);
                        // watch.Stop();
                        string responseBody = "";
                        context.Response.Body = responseBodyStream;
                        responseBodyStream.Seek(0, SeekOrigin.Begin);
                        responseBody = new StreamReader(responseBodyStream).ReadToEnd();
                        responseBodyStream.Seek(0, SeekOrigin.Begin);
                        //if(!path.Contains("payment"))
                        //{
                        //    LogData data = new LogData
                        //    {
                        //        Controller = path,
                        //        Duration = watch.ElapsedMilliseconds,
                        //        IpAdress = context.Connection.RemoteIpAddress.ToString(),
                        //        Message = "",
                        //        Request = requestBodyText,
                        //        Response = responseBody,
                        //        Source = context.Request.Host.HasValue ? context.Request.Host.Value : "",
                        //        Header = customHeader

                        //    };
                        //    logger.Info(JsonConvert.SerializeObject(data));
                        //}

                        await responseBodyStream.CopyToAsync(originalResponseBody);

                    }
                    catch (BusinessException ex)
                    {
                        await HandleAndWrapExceptionAsync(context, ex.Message, !string.IsNullOrEmpty(ex.ResponseCode) ? ex.ResponseCode : ExceptionErrorEnum.NotFound);
                    }
                    catch (Exception ex)
                    {

                        var responseBody = new StreamReader(responseBodyStream).ReadToEnd();
                        string errorMessage = $"{ex.Message}  - {ex?.InnerException?.Message}";
                        Log.Error(errorMessage);

                        var st = new StackTrace(ex, true);
                        var frame = st.GetFrame(0);
                        var line = frame.GetFileLineNumber();
                        //LogData data = new LogData
                        //{
                        //    Controller = path,
                        //    Duration = 0,
                        //    IpAdress = context.Connection.RemoteIpAddress.ToString(),
                        //    Message = errorMessage,
                        //    Request = !path.Contains("payment") ? requestBodyText : "",
                        //    Response = responseBody,
                        //    Source = ex.Source,
                        //    Line = line,
                        //    Header = customHeader


                        //};

                        //logger.Error(JsonConvert.SerializeObject(data));

                        await HandleAndWrapExceptionAsync(context, "Bir hata oluştu", "404");

                    }
                    finally
                    {
                        context.Request.Body = originalRequestBody;
                        context.Response.Body = originalResponseBody;
                    }
                }
            }
        }


        private async Task WriteResponseAsync(HttpContext context, string bodyJson, string statusCode)
        {
            context.Response.Headers.Clear();
            context.Response.Headers.Add("Accept", "application/json");
            context.Response.Headers.Add("Content-Type", "application/json");
            context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With");
            context.Response.Headers.Add("Access-Control-Allow-Methods", "GET,POST,OPTIONS,DELETE,PUT");
            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            context.Response.StatusCode = Int32.Parse(statusCode);
            byte[] data = ASCIIEncoding.ASCII.GetBytes(bodyJson);
            await context.Response.Body.WriteAsync(data, 0, data.Length);
        }

        private async Task HandleAndWrapExceptionAsync(HttpContext httpContext, string exceptionMessage, string responseCode)
        {


            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var responseJson = JsonSerializer.Serialize(new { message = exceptionMessage }, options);
            await WriteResponseAsync(httpContext, responseJson, responseCode);
        }

        private Task ClearCacheHeaders(object state)
        {
            var response = (HttpResponse)state;
            response.Headers[HeaderNames.CacheControl] = "no-cache";
            response.Headers[HeaderNames.Pragma] = "no-cache";
            response.Headers[HeaderNames.Expires] = "-1";
            response.Headers.Remove(HeaderNames.ETag);
            return Task.CompletedTask;
        }
    }
}
