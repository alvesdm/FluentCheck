using System;
using Microsoft.AspNetCore.Http;

namespace FluentCheck.HealthCheck
{
    public interface IRegisterRequest
    {
        Action<HttpContext> Response { get; }
        string Url { get; }
    }
}