using HLab.TinyURL.Client;
using HLab.TinyURL;
using System.Net.Http.Headers;

namespace TinyUrlConsoleExample;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("🔗 TinyURL Console Example");
        Console.WriteLine("==========================");
        Console.WriteLine("This example demonstrates both authentication methods:");
        Console.WriteLine("1. Bearer Token (recommended for production)");
        Console.WriteLine("2. API Key (for simple integrations)");

        // Choose authentication method
        Console.Write("\nChoose authentication method (1=Bearer Token, 2=API Key): ");
        var choice = Console.ReadLine();

        string? bearerToken = null;
        string? apiKey = null;

        if (choice == "1")
        {
            Console.Write("\nEnter your TinyURL Bearer token: ");
            bearerToken = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(bearerToken))
            {
                Console.WriteLine("❌ Bearer token is required. Please get your token from https://tinyurl.com/app/dev");
                return;
            }
        }
        else if (choice == "2")
        {
            Console.Write("\nEnter your TinyURL API key: ");
            apiKey = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                Console.WriteLine("❌ API key is required. Please get your key from https://tinyurl.com/app/dev");
                return;
            }
        }
        else
        {
            Console.WriteLine("❌ Invalid choice. Please run again and choose 1 or 2.");
            return;
        }

        // Get URL to shorten from user
        Console.Write("\nEnter the URL to shorten: ");
        var urlToShorten = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(urlToShorten))
        {
            Console.WriteLine("❌ URL is required.");
            return;
        }

        // Validate URL format
        if (!Uri.TryCreate(urlToShorten, UriKind.Absolute, out var validUri) || 
            (validUri.Scheme != Uri.UriSchemeHttp && validUri.Scheme != Uri.UriSchemeHttps))
        {
            Console.WriteLine("❌ Invalid URL format. Please include http:// or https://");
            return;
        }

        // Prompt for optional custom alias
        Console.Write("\nEnter custom alias (optional, press Enter to skip): ");
        var customAlias = Console.ReadLine();

        Console.WriteLine("\n🔄 Creating shortened URL...");

        try
        {
            // Using the comprehensive TinyURLClient (recommended)
            // which provides full API access including analytics and advanced features
            using var httpClient = new HttpClient();
            
            if (!string.IsNullOrEmpty(bearerToken))
            {
                // Bearer token authentication (recommended)
                httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", bearerToken);
                httpClient.BaseAddress = new Uri("https://api.tinyurl.com");
            }
            else if (!string.IsNullOrEmpty(apiKey))
            {
                // API key authentication via URL parameter
                httpClient.BaseAddress = new Uri($"https://api.tinyurl.com?api_token={apiKey}");
            }

            httpClient.Timeout = TimeSpan.FromSeconds(30);

            // Create comprehensive TinyURL client
            var tinyUrlClient = new TinyURLClient(httpClient);

            // Create the request using the proper request object
            var createRequest = new CreateTinyURLRequest
            {
                Url = urlToShorten
            };

            // Add custom alias if provided
            if (!string.IsNullOrWhiteSpace(customAlias))
            {
                createRequest.Alias = customAlias;
            }

            // Create the shortened URL
            tinyUrlClient.ReadResponseAsString = true;
            var response = await tinyUrlClient.CreateTinyUrlAsync(createRequest);

            // Display results
            Console.WriteLine("\n✅ Success!");
            Console.WriteLine($"📊 Original URL: {urlToShorten}");
            Console.WriteLine($"🔗 Shortened URL: {response.Data.Tiny_url}");
            
            if (!string.IsNullOrWhiteSpace(customAlias))
            {
                Console.WriteLine($"🏷️  Custom Alias: {customAlias}");
            }

            Console.WriteLine($"📅 Created: {response.Data.Created_at}");
            Console.WriteLine($"  Analytics Enabled: {(response.Data.Analytics != null && response.Data.Analytics.Enabled ? "Yes" : "No")}");

            // Demonstrate analytics capability if we have an alias
            if (!string.IsNullOrWhiteSpace(response.Data.Alias) && response.Data.Analytics != null && response.Data.Analytics.Enabled)
            {
                Console.WriteLine("\n🔄 Fetching analytics...");
                try
                {
                    var analytics = await tinyUrlClient.GeneralAnalyticsAsync(
                        from: DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd"),
                        to: DateTime.Now.ToString("yyyy-MM-dd"),
                        alias: response.Data.Alias,
                        tag: null
                    );

                    Console.WriteLine($"📈 Total Clicks: {analytics.Data?.Total ?? 0}");
                    Console.WriteLine("💡 Analytics feature demonstrated successfully!");
                }
                catch (Exception analyticsEx)
                {
                    Console.WriteLine($"⚠️  Analytics not available: {analyticsEx.Message}");
                    Console.WriteLine("💡 Analytics may require additional permissions or time to populate");
                }
            }

            Console.WriteLine("\n📋 Authentication Methods Demonstrated:");
            if (!string.IsNullOrEmpty(bearerToken))
            {
                Console.WriteLine("✅ Bearer Token authentication (Production recommended)");
                Console.WriteLine("   Header: Authorization: Bearer your-token");
            }
            else
            {
                Console.WriteLine("✅ API Key authentication (Simple integration)"); 
                Console.WriteLine("   URL Parameter: ?api_token=your-key");
            }

            Console.WriteLine("\n✅ This demonstrates the powerful TinyURLClient with full API access!");
            Console.WriteLine("💡 Features available with TinyURLClient:");
            Console.WriteLine("   📊 Advanced analytics and reporting");
            Console.WriteLine("   📦 Bulk operations and batch processing");
            Console.WriteLine("   🏷️  Domain management and enterprise features");
            Console.WriteLine("   🔒 Enhanced error handling and validation");
            Console.WriteLine("\n💡 See the README.md for more advanced features like bulk operations");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"❌ Network error: {ex.Message}");
            Console.WriteLine("💡 Check your internet connection and try again.");
        }
        catch (TaskCanceledException ex)
        {
            Console.WriteLine($"❌ Request timeout: {ex.Message}");
            Console.WriteLine("💡 The request took too long. Try again.");
        }
        catch (TinyUrlException ex)
        {
            Console.WriteLine($"❌ TinyURL API Error: {ex.Message}");
            if (ex.Message.Contains("authentication") || ex.Message.Contains("unauthorized"))
            {
                Console.WriteLine("💡 Check your authentication credentials. Get valid credentials from https://tinyurl.com/app/dev");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Unexpected error: {ex.Message}");
            Console.WriteLine($"💡 Error details: {ex.GetType().Name}");
        }

        Console.WriteLine("\n👋 Press any key to exit...");
        Console.ReadKey();
    }
}
