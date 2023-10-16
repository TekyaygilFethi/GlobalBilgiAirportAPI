using AirportDistanceCalculator.Data.Base;
using AirportDistanceCalculator.Data.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AirportDistanceCalculator.Business.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<BaseLogClass> _logger;
        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<BaseLogClass> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                var request = context.Request;
                HttpRequestRewindExtensions.EnableBuffering(request);
                var requestBody = string.Empty;


                using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true))
                {
                    request.Body.Position = 0;
                    requestBody = await reader.ReadToEndAsync();
                }

                _logger.LogError(ex, $"Parametreler: Host: {context.Request.Host} - Path: {context.Request.Path} - Method: {context.Request.Method} - requestBody: {requestBody}");

                switch (ex)
                {
                    case AppException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(new ResponseObject<object>
                {
                    Message = ex.Message
                });

                await response.WriteAsync(result);
            }
        }
    }
}
