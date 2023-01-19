## AspNetCore.Testing.Expressions

Get intellisense support for your WebApplicationFactory-based tests.

```csharp
var prm = 1;
var response = await client.GetAsync((WeatherForecastController c) => c.GetWithRouteParam(prm));
```