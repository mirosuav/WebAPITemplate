# WebAPITemplate
A template project for ASP.NET Core Web API including all known best practices

## [Minimal API](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/middleware?view=aspnetcore-8.0)
 - Https
 - HSTS header

## [Logging](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-8.0)
- Console, Debug, Events
-[Diagnostics-eventflow](https://github.com/Azure/diagnostics-eventflow) collects event streams from multiple sources and publishes it to output systems.

## [Health checks](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-8.0)
[AspNetCore.Diagnostics.HealthChecks](https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks)

## CORS
- [Anti-Forgery](https://learn.microsoft.com/en-us/aspnet/core/security/anti-request-forgery?view=aspnetcore-8.0#afwma)

## DDOS
 - Throttling/Rate limiting

## Authorization & Authentication
 - X-API-KEY
 - JWT Bearer token
 
## Error handling
- [ProblemDetails](https://learn.microsoft.com/en-us/aspnet/core/web-api/handle-errors?view=aspnetcore-8.0#problem-details-service)
- [IExceptionHandler](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-8.0#iexceptionhandler)
- [Error middleware](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-8.0)
- Result type pattern
  Based on [LanguageExtCore](https://github.com/louthy/language-ext/blob/main/LanguageExt.Core/Common/Result/Result.cs)


## [HttpClient and Resiliency](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-http-call-retries-exponential-backoff-polly)


## Tests
- [Stress testing](https://learn.microsoft.com/en-us/aspnet/core/test/load-tests?view=aspnetcore-8.0)
- Integration testing on docker