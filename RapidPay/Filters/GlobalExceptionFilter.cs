﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using RapidPay.Application.Common;
using System.Net;

namespace RapidPay.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "An unhandled exception occurred.");

            var result = new BaseResult<object>
            {
                Success = false,
                Message = "An unexpected error occurred.",
                Errors = new List<string> { context.Exception.Message }
            };

            context.Result = new ObjectResult(result)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }
}
