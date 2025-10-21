# HLab.TinyURL.Client

A .NET 8 client library for TinyURL services that allows you to create shortened URLs programmatically using the official TinyURL API.

## Features

- 🚀 Simple and intuitive async API
- 🔧 Built-in error handling and validation
- 🎯 Support for custom aliases
- 📦 Dependency injection friendly
- 🛡️ Comprehensive exception handling
- ✅ Input validation for URLs and aliases
- � Full analytics and reporting capabilities
- 🏢 Enterprise features with comprehensive API
- �🔄 Compatible with .NET 8+

## Client Options

This library provides two client classes to fit different use cases:

> **⭐ Recommendation:** We highly recommend using `TinyURLClient` for all new projects as it provides comprehensive API access, better error handling, and future-proof functionality.

### TinyURLClient (Recommended)
**Best for:** All applications - from simple URL shortening to enterprise analytics.
- ✅ Complete TinyURL API access
- ✅ Advanced analytics and reporting
- ✅ Bulk operations and batch processing
- ✅ Domain management and custom aliases
- ✅ Enterprise-grade error handling
- ✅ Future-proof with all API features

### TinyUrlSimpleClient (Legacy)
**Best for:** Legacy applications or minimal feature requirements.
- ✅ Basic URL shortening
- ✅ Custom aliases  
- ✅ Basic error handling
- ❌ Limited to basic features only

## Installation

Install the package via NuGet:

```bash
dotnet add package HLab.TinyURL.Client
```

## Authentication

TinyURL API supports two authentication methods for accessing premium features and analytics:

### 1. Bearer Token Authentication (Recommended)

Use Bearer token in the Authorization header for secure API access:

```csharp
using HLab.TinyURL.Client;

// Configure HttpClient with Bearer token
var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Authorization = 
    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "your-bearer-token");

var client = new TinyURLClient(httpClient);

// Or using HttpClientFactory (recommended for production)
builder.Services.AddHttpClient("TinyUrlClient", client =>
{
    client.BaseAddress = new Uri("https://api.tinyurl.com");
    client.DefaultRequestHeaders.Authorization = 
        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "your-bearer-token");
});
```

**HTTP Header Format:**
```
Authorization: Bearer your-bearer-token-here
```

### 2. API Key Authentication (URL Parameter)

Pass the API key as a URL parameter for simple authentication:

```csharp
using HLab.TinyURL.Client;

// Configure base URL with API token parameter
var httpClient = new HttpClient();
var client = new TinyURLClient("https://api.tinyurl.com?api_token=your-api-key", httpClient);

// Or using HttpClientFactory
builder.Services.AddHttpClient("TinyUrlClient", client =>
{
    client.BaseAddress = new Uri("https://api.tinyurl.com?api_token=your-api-key");
});
```

**URL Format:**
```
https://api.tinyurl.com/create?api_token=your-api-key-here
```

### Authentication Comparison

| Method | Security | Use Case | Implementation |
|--------|----------|----------|----------------|
| **Bearer Token** | ✅ Higher (Header-based) | Production apps, Enterprise | `Authorization: Bearer token` |
| **API Key** | ⚠️ Standard (URL parameter) | Simple integrations, Testing | `?api_token=key` |

### Getting Your API Credentials

1. Visit [TinyURL API Dashboard](https://tinyurl.com/app/dev)
2. Create an account or sign in
3. Generate your API credentials:
   - **Bearer Token**: For secure production applications
   - **API Key**: For simple integrations and testing

### Environment Variables (Security Best Practice)

Store your credentials securely using environment variables:

```csharp
// Using Bearer Token from environment
var bearerToken = Environment.GetEnvironmentVariable("TINYURL_BEARER_TOKEN");
httpClient.DefaultRequestHeaders.Authorization = 
    new AuthenticationHeaderValue("Bearer", bearerToken);

// Using API Key from environment
var apiKey = Environment.GetEnvironmentVariable("TINYURL_API_KEY");
var baseUrl = $"https://api.tinyurl.com?api_token={apiKey}";
var client = new TinyURLClient(baseUrl, httpClient);

// Or in appsettings.json
{
  "TinyUrl": {
    "BearerToken": "your-bearer-token",
    "ApiKey": "your-api-key"
  }
}
```

### Anonymous Usage (Limited Features)

For basic URL shortening without analytics, you can use the library without authentication:

```csharp
// No authentication - limited to basic features
var client = new TinyUrlSimpleClient();
var shortUrl = await client.CreateShortUrlAsync("https://example.com");
```

> **⚠️ Note:** Authentication is required for analytics, bulk operations, custom domains, and other advanced TinyURL features.

## Usage

### Basic URL Shortening (Recommended)

```csharp
using HLab.TinyURL.Client;

// ✅ BEST PRACTICE: Use HttpClientFactory (in DI container)
public class UrlShorteningService
{
    private readonly IHttpClientFactory _httpClientFactory;
    
    public UrlShorteningService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<string> ShortenAsync(string url)
    {
        using var httpClient = _httpClientFactory.CreateClient();
        var client = new TinyURLClient(httpClient);
        
        var result = await client.CreateTinyUrlAsync(new TinyUrlRequest 
        { 
            Url = url 
        });
        return result.TinyUrl;
    }
}

// Alternative for console apps (not recommended for production)
var httpClient = new HttpClient(); // ⚠️ Consider using HttpClientFactory instead
var client = new TinyURLClient(httpClient);

try
{
    var result = await client.CreateTinyUrlAsync(new TinyUrlRequest 
    { 
        Url = "https://www.example.com" 
    });
    Console.WriteLine($"Shortened URL: {result.TinyUrl}");
}
finally
{
    client.Dispose();
    httpClient.Dispose();
}
```

### Alternative: Simple Client (Legacy)

For minimal feature requirements, you can use the legacy simple client:

```csharp
using HLab.TinyURL.Client;

// Legacy simple client
var client = new TinyUrlSimpleClient();

try
{
    string shortUrl = await client.CreateShortUrlAsync("https://www.example.com");
    Console.WriteLine($"Shortened URL: {shortUrl}");
}
finally
{
    client.Dispose();
}
```

### Custom Aliases

```csharp
using HLab.TinyURL.Client;

var httpClient = new HttpClient();
var client = new TinyURLClient(httpClient);

try
{
    // Shorten a URL with custom alias using TinyURLClient
    var result = await client.CreateTinyUrlAsync(new TinyUrlRequest 
    { 
        Url = "https://www.example.com",
        Alias = "my-custom-alias" 
    });
    Console.WriteLine($"Custom shortened URL: {result.TinyUrl}");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Invalid alias: {ex.Message}");
}
catch (TinyUrlException ex)
{
    Console.WriteLine($"TinyURL error: {ex.Message}");
}
finally
{
    client.Dispose();
}
```

### Advanced Usage with Analytics and Enterprise Features

For applications requiring analytics and advanced features:

```csharp
using HLab.TinyURL.Client;

// ✅ PRODUCTION APPROACH: Using HttpClientFactory in a service
public class TinyUrlAnalyticsService
{
    private readonly IHttpClientFactory _httpClientFactory;
    
    public TinyUrlAnalyticsService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<(string Url, int Clicks)> CreateAndTrackAsync(string url, string alias)
    {
        using var httpClient = _httpClientFactory.CreateClient("TinyUrlAnalytics");
        var client = new TinyURLClient(httpClient);
        
        // Create shortened URL with advanced options
        var createRequest = new CreateRequest
        {
            Url = url,
            Alias = alias,
            Tags = new[] { "marketing", "campaign-2024" }
        };
        
        var response = await client.CreateTinyUrlAsync(createRequest);
        
        // Get analytics for the URL
        var analytics = await client.GeneralAnalyticsAsync(
            from: DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd"),
            to: DateTime.Now.ToString("yyyy-MM-dd"),
            alias: alias,
            tag: null
        );
        
        return (response.Data.TinyUrl, analytics.Data.TotalClicks);
    }
}

// Configure HttpClient with authentication in Program.cs
// See Authentication section above for details on Bearer token vs API key
builder.Services.AddHttpClient("TinyUrlAnalytics", client =>
{
    client.DefaultRequestHeaders.Authorization = 
        new AuthenticationHeaderValue("Bearer", "your-bearer-token");
    client.BaseAddress = new Uri("https://api.tinyurl.com");
});
```

### Using Options Pattern (Simple Client)

```csharp
using HLab.TinyURL.Client;

var client = new TinyUrlSimpleClient();

var options = new TinyUrlOptions
{
    Url = "https://www.example.com",
    Alias = "my-alias"
};

try
{
    string shortUrl = await client.CreateShortUrlAsync(options);
    Console.WriteLine($"Shortened URL: {shortUrl}");
}
catch (TinyUrlException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
finally
{
    client.Dispose();
}
```

### Dependency Injection

```csharp
// TinyURLClient (Recommended) - Comprehensive features
services.AddHttpClient<TinyURLClient>((serviceProvider, httpClient) =>
{
    // Configure API authentication (see Authentication section for details)
    httpClient.DefaultRequestHeaders.Authorization = 
        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "your-bearer-token");
    httpClient.BaseAddress = new Uri("https://api.tinyurl.com");
});

// Or register as scoped service
services.AddScoped<TinyURLClient>(provider =>
{
    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient();
    return new TinyURLClient(httpClient);
});

// Alternative: Simple client for legacy applications
services.AddHttpClient<TinyUrlSimpleClient>();

// Or configure simple client manually
services.AddSingleton<TinyUrlSimpleClient>(provider =>
{
    var httpClient = provider.GetRequiredService<HttpClient>();
    return new TinyUrlSimpleClient(httpClient);
});
```

## Configuration

### TinyURLClient Configuration (Recommended)

The comprehensive client should be configured using HttpClientFactory for production applications:

```csharp
// ✅ RECOMMENDED: Configure using HttpClientFactory
// In Program.cs or Startup.cs
builder.Services.AddHttpClient<TinyURLClient>("TinyUrlClient", client =>
{
    client.DefaultRequestHeaders.Authorization = 
        new AuthenticationHeaderValue("Bearer", "your-bearer-token");
    client.BaseAddress = new Uri("https://api.tinyurl.com");
    client.Timeout = TimeSpan.FromSeconds(30);
});

// Usage in your service
public class MyService
{
    private readonly IHttpClientFactory _httpClientFactory;
    
    public MyService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<string> CreateUrlAsync(string url)
    {
        using var httpClient = _httpClientFactory.CreateClient("TinyUrlClient");
        var client = new TinyURLClient(httpClient);
        
        var result = await client.CreateTinyUrlAsync(new TinyUrlRequest { Url = url });
        return result.TinyUrl;
    }
}

// ⚠️ For console apps only (not recommended for production)
var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Authorization = 
    new AuthenticationHeaderValue("Bearer", "your-bearer-token");
var client = new TinyURLClient(httpClient);
```

### Simple Client Configuration (Legacy)

The simple client supports configuration through the Options pattern:

```csharp
var options = new TinyUrlOptions
{
    BaseUrl = "https://api.tinyurl.com",
    Timeout = TimeSpan.FromSeconds(30),
    RetryCount = 3,
    ValidateSsl = true
};

var client = new TinyUrlSimpleClient(httpClient, options);
```

### Using Statement Best Practices

```csharp
// ✅ BEST: HttpClientFactory in DI container
public async Task<string> RecommendedApproach(IHttpClientFactory factory)
{
    using var httpClient = factory.CreateClient("TinyUrlClient");
    using var client = new TinyURLClient(httpClient);
    
    var result = await client.CreateTinyUrlAsync(new TinyUrlRequest 
    { 
        Url = "https://www.example.com" 
    });
    return result.TinyUrl;
}

// ⚠️ ACCEPTABLE: For simple console apps only
using var httpClient = new HttpClient(); // Consider HttpClientFactory for production
using var client = new TinyURLClient(httpClient);

var result = await client.CreateTinyUrlAsync(new TinyUrlRequest 
{ 
    Url = "https://www.example.com" 
});
Console.WriteLine($"Shortened URL: {result.TinyUrl}");
```

## HttpClientFactory Best Practices

### ⚠️ Important: Use HttpClientFactory Instead of HttpClient

**Always use `IHttpClientFactory` instead of creating `HttpClient` instances directly** to avoid common issues:

#### Why HttpClientFactory?

1. **Socket Exhaustion Prevention**: Direct `HttpClient` creation can exhaust available sockets
2. **DNS Changes Handling**: HttpClientFactory respects DNS TTL and handles DNS changes properly  
3. **Connection Pooling**: Efficient connection reuse and management
4. **Memory Management**: Proper disposal and lifecycle management
5. **Configuration Centralization**: Centralized timeout, retry, and policy configuration

#### Recommended Implementation

```csharp
// ✅ RECOMMENDED: Using HttpClientFactory
public class TinyUrlService
{
    private readonly IHttpClientFactory _httpClientFactory;
    
    public TinyUrlService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<string> ShortenUrlAsync(string url)
    {
        // Create HttpClient from factory
        using var httpClient = _httpClientFactory.CreateClient("TinyUrlClient");
        var client = new TinyURLClient(httpClient);
        
        var result = await client.CreateTinyUrlAsync(new TinyUrlRequest { Url = url });
        return result.TinyUrl;
    }
}

// Configure in Program.cs or Startup.cs
builder.Services.AddHttpClient("TinyUrlClient", client =>
{
    client.BaseAddress = new Uri("https://api.tinyurl.com");
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Authorization = 
        new AuthenticationHeaderValue("Bearer", "your-bearer-token");
});
```

#### What NOT to Do

```csharp
// ❌ AVOID: Creating HttpClient directly
public async Task<string> BadExample()
{
    using var httpClient = new HttpClient(); // This can cause socket exhaustion!
    var client = new TinyURLClient(httpClient);
    // ... rest of code
}

// ❌ AVOID: Singleton HttpClient without proper lifecycle management
private static readonly HttpClient _httpClient = new HttpClient(); // Memory leaks!
```

#### Advanced HttpClientFactory Configuration

```csharp
// Configure with retry policies, timeouts, and custom handlers
builder.Services.AddHttpClient<TinyURLClient>("TinyUrlClient")
    .ConfigureHttpClient(client =>
    {
        client.BaseAddress = new Uri("https://api.tinyurl.com");
        client.Timeout = TimeSpan.FromSeconds(30);
    })
    .AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(GetCircuitBreakerPolicy());

// Named client registration for TinyURLClient
builder.Services.AddTransient<ITinyUrlService>(provider =>
{
    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient("TinyUrlClient");
    return new TinyURLClient(httpClient);
});
```

## Error Handling

Both clients provide comprehensive error handling:

```csharp
try
{
    string shortUrl = await client.CreateShortUrlAsync("https://www.example.com");
    Console.WriteLine($"Success: {shortUrl}");
}
catch (TinyUrlException ex)
{
    Console.WriteLine($"TinyURL API Error: {ex.Message}");
    Console.WriteLine($"Error Code: {ex.ErrorCode}");
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"Network Error: {ex.Message}");
}
catch (TaskCanceledException ex)
{
    Console.WriteLine($"Timeout Error: {ex.Message}");
}
```

## API Reference

### TinyURLClient (Recommended)

A comprehensive API client that provides full access to TinyURL's API features including analytics and advanced operations.

> **⭐ This is the recommended client for all new projects.**

A comprehensive API client that provides full access to TinyURL's API features including analytics and advanced operations.

#### Key Features

- 📊 **Analytics**: Raw logs, timeline analytics, browser/OS/device analytics
- 📈 **Reporting**: Weekdays popularity, hourly analytics, top sources and languages
- 🌍 **Location Analytics**: Geographic data and insights  
- 📦 **Bulk Operations**: Handle multiple URLs at once
- 🏷️ **Domain Management**: Custom domains and aliases
- 🔒 **Advanced Features**: Full API access with authentication

#### Constructor

- `TinyURLClient(HttpClient httpClient)` - Creates instance with provided HttpClient

#### Main Method Categories

**Analytics Methods:**
- `RawLogsAnalyticsAsync()` - Get raw analytics logs
- `TimelineAnalyticsAsync()` - Get timeline-based analytics 
- `GeneralAnalyticsAsync()` - Get browser, OS, and device analytics
- `WeekdaysPopularityAnalyticsAsync()` - Get weekday popularity data
- `HourPopularityAnalyticsAsync()` - Get hourly popularity data
- `TopSourcesAnalyticsAsync()` - Get top traffic sources
- `TopLanguagesAnalyticsAsync()` - Get top languages analytics
- `LocationAnalyticsAsync()` - Get geographic analytics

**URL Management Methods:**
- `CreateTinyUrlAsync()` - Create shortened URLs
- `UpdateTinyUrlAsync()` - Update existing URLs
- `ChangeTinyUrlDomainAsync()` - Change URL domain
- `ArchiveTinyUrlAsync()` / `UnarchiveTinyUrlAsync()` - Archive management

**Domain & Alias Methods:**
- `CreateAliasAsync()` - Create custom aliases
- `GetDomainsAsync()` - Get available domains
- `GetTinyUrlsByDomainAsync()` - Get URLs by domain

All methods support cancellation tokens and follow async/await patterns.

### TinyUrlSimpleClient (Legacy)

A simple, lightweight client for basic URL shortening operations. **Consider using TinyURLClient instead for better features and future compatibility.**

#### Methods

- `CreateShortUrlAsync(string url, string? alias = null, CancellationToken cancellationToken = default)`
- `CreateShortUrlAsync(TinyUrlOptions options, CancellationToken cancellationToken = default)`

#### Constructors

- `TinyUrlSimpleClient()` - Creates a new instance with internal HttpClient
- `TinyUrlSimpleClient(HttpClient httpClient)` - Creates a new instance with provided HttpClient

### TinyUrlOptions

Properties:
- `Url` (required) - The URL to shorten
- `Alias` (optional) - Custom alias (5-30 characters, alphanumeric, hyphens, underscores only)

### TinyUrlException

Custom exception thrown when TinyURL API operations fail.

## Alias Rules

Custom aliases must follow these rules:
- 5-30 characters in length
- Only alphanumeric characters, hyphens (-), and underscores (_)
- Case-sensitive

## Error Handling

The library provides comprehensive error handling:

- `ArgumentNullException` - When URL is null or empty
- `ArgumentException` - When URL format is invalid or alias doesn't meet requirements
- `TinyUrlException` - When TinyURL service returns an error
- `HttpRequestException` - When HTTP communication fails

## Testing

The library includes comprehensive unit tests using xUnit, Moq, and Shouldly:

```bash
# Run all tests
dotnet test

# Run tests with detailed output
dotnet test --verbosity normal

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Test Categories

- **Unit Tests**: Mock HTTP responses for reliable testing
- **Integration Tests**: Test against actual TinyURL API (requires network)
- **Validation Tests**: Test input validation and error handling

### Testing Your Implementation

```csharp
// Example unit test using Moq
[Fact]
public async Task CreateShortUrlAsync_ShouldReturnShortUrl_WhenValidUrl()
{
    // Arrange
    var mockHttpClient = new Mock<HttpClient>();
    var client = new TinyUrlSimpleClient(mockHttpClient.Object);
    
    // Act & Assert
    var result = await client.CreateShortUrlAsync("https://www.example.com");
    result.ShouldNotBeNullOrEmpty();
}
```

## Development and Debugging

### VS Code Debugging Support

This repository includes comprehensive VS Code debugging configurations for the console application example.

#### Quick Start

1. **Open in VS Code:**
   ```bash
   code .
   ```

2. **Debug the Console App:**
   - Press `F5` and select **"Debug TinyURL Console App"**
   - Or go to Run and Debug panel and choose from available configurations

#### Available Debug Configurations

| Configuration | Description | Use Case |
|---------------|-------------|----------|
| **Debug TinyURL Console App** | Full debugging with breakpoints | Development and troubleshooting |
| **Debug TinyURL Console App (External Terminal)** | Debug in external terminal | Better terminal interaction |
| **Run TinyURL Console App (No Debug)** | Run without debugger | Performance testing |

#### VS Code Tasks

Access via `Ctrl+Shift+P` → `Tasks: Run Task`:

**Development Tasks:**
- **`build-console-app`** - Build the console application (Debug mode)
- **`build-main-library`** - Build the main TinyURL library (Debug mode)  
- **`run-console-app`** - Run the console application

**Release & Packaging:**
- **`build-library-release`** - Build library in Release mode
- **`pack-nuget-release`** - Create NuGet package in Release mode
- **`build-and-pack-release`** - 🚀 Build and pack Release (recommended)

**Maintenance:**
- **`clean-all`** - Clean build outputs
- **`clean-artifacts`** - Clean artifacts directory

#### Debugging Tips

**Useful Breakpoints:**
- **Authentication Logic** - Line ~50 in `Program.cs`
- **TinyURLClient Creation** - Line ~95 in `Program.cs`
- **API Request** - Line ~110 in `Program.cs`
- **Response Handling** - Line ~125 in `Program.cs`

**Variables to Watch:**
- `bearerToken` / `apiKey` - Authentication credentials
- `createRequest` - Request object sent to API
- `response.Data` - API response data structure

**Console Integration:**
- Uses VS Code integrated terminal for seamless user input
- Interactive prompts work during debugging sessions
- Can test with real or mock credentials

#### Project Structure for Debugging

```
TinyURL-nuget/
├── .vscode/
│   ├── launch.json          # Debug configurations
│   ├── tasks.json           # Build and run tasks
│   ├── settings.json        # VS Code workspace settings
│   └── extensions.json      # Recommended extensions
├── artifacts/               # 📦 Generated NuGet packages (Release builds)
│   └── HLab.TinyURL.Client.x.x.x.nupkg
├── examples/ConsoleApp/TinyUrlConsoleExample/
│   ├── Program.cs           # 🎯 Main debugging target
│   ├── .vscode/             # Console app specific configs
│   └── DEBUG_GUIDE.md       # Detailed debugging guide
└── src/HLab.TinyURL.Client/ # Main library source
```

#### Required Extensions

The workspace will recommend installing:
- **C# Dev Kit** (`ms-dotnettools.csdevkit`)
- **C#** (`ms-dotnettools.csharp`)
- **.NET Runtime** (`ms-dotnettools.vscode-dotnet-runtime`)

#### Console App Examples

This project includes two console application examples:

##### 1. TinyUrlConsoleExample (Full Featured)

The `examples/ConsoleApp/TinyUrlConsoleExample/` demonstrates:
- ✅ **Full TinyURLClient Integration** with analytics
- 🔐 **Both Authentication Methods** (Bearer token + API key)
- 📊 **Analytics Demonstration** with error handling
- 🛡️ **Input Validation** and comprehensive error handling
- 📱 **Interactive UI** for testing different scenarios

See [`examples/ConsoleApp/TinyUrlConsoleExample/README.md`](examples/ConsoleApp/TinyUrlConsoleExample/README.md) for detailed documentation.

##### 2. TinyUrlSimpleExample (Basic/Legacy)

The `examples/ConsoleApp/TinyUrlSimpleExample/` demonstrates:
- ✅ **TinyUrlSimpleClient Usage** for basic URL shortening
- 🔓 **No Authentication Required** (public API)
- 🎯 **Simple and Straightforward** implementation
- 🛡️ **Basic Error Handling** and validation
- 📱 **Interactive UI** for quick testing

See [`examples/ConsoleApp/TinyUrlSimpleExample/README.md`](examples/ConsoleApp/TinyUrlSimpleExample/README.md) for detailed documentation.

**Run the Examples:**

```bash
# Full-featured example (with authentication and analytics)
cd examples/ConsoleApp/TinyUrlConsoleExample
dotnet run

# Simple example (no authentication required)
cd examples/ConsoleApp/TinyUrlSimpleExample
dotnet run
```

## License

This project is licensed under the MIT License.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.