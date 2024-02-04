# WebAPITemplate
A template project for ASP.NET Core Web API including all known best practices

## Minimal API
 - Https
 - HSTS header

## [Logging](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-8.0)
- Console, Debug, Events
- Application Insights

## [Health checks](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-8.0)

## CORS
- [Anti-Forgery](https://learn.microsoft.com/en-us/aspnet/core/security/anti-request-forgery?view=aspnetcore-8.0#afwma)

## DDOS
 - Throttling/Rate limiting

## Authorization & Authentication
 - X-API-KEY
 - JWT Bearer token
 
## Error handling
- [IExceptionHandler](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-8.0#iexceptionhandler)
- [Error middleware](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-8.0)
- Result type pattern
  Based on [LanguageExtCore](https://github.com/louthy/language-ext/blob/main/LanguageExt.Core/Common/Result/Result.cs)

## Tests
- [Stress testing](https://learn.microsoft.com/en-us/aspnet/core/test/load-tests?view=aspnetcore-8.0)
- Integration testing on docker