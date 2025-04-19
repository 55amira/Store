﻿using Azure;
using Domain.Exceptions;
using Shared.ErrorsModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Store_Api.Middelware
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        public GlobalErrorHandlingMiddleware (RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = next;
           _logger = logger;
        }

        public async Task InvokeAsync (HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }catch (Exception ex)
            {
                _logger.LogError(ex , ex.Message);

                // 1. Set Status Code For Response 
                // 2. Set Content Type Code For Response 
                // 3. Response Object (Body) 
                // 4. Return Response 


                //context.Response.StatusCode =StatusCodes.Status500InternalServerError ;

                context.Response.ContentType = "application/json";


                var response = new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message
                };

                response.StatusCode = ex switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError,
                };

                context.Response.StatusCode = response.StatusCode;

                await context.Response.WriteAsJsonAsync(response);
            }


        }
    }
}
