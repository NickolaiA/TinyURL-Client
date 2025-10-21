using HLab.TinyURL.Client;

namespace TinyUrlSimpleExample;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("🔗 TinyURL Simple Client Example");
        Console.WriteLine("==================================");
        Console.WriteLine("This example demonstrates the simple TinyUrlSimpleClient");
        Console.WriteLine("for basic URL shortening without authentication.\n");

        // Get URL to shorten from user
        Console.Write("Enter the URL to shorten: ");
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
        Console.Write("\nEnter custom alias (optional, 5-30 characters, press Enter to skip): ");
        var customAlias = Console.ReadLine();

        Console.WriteLine("\n🔄 Creating shortened URL...");

        try
        {
            // Using the simple TinyUrlSimpleClient (no authentication required)
            // This uses the public TinyURL API for basic shortening
            using var tinyUrlClient = new TinyUrlSimpleClient();

            string shortUrl;
            
            if (!string.IsNullOrWhiteSpace(customAlias))
            {
                // Create with custom alias
                shortUrl = await tinyUrlClient.CreateShortUrlAsync(urlToShorten, customAlias);
            }
            else
            {
                // Create without alias
                shortUrl = await tinyUrlClient.CreateShortUrlAsync(urlToShorten);
            }

            // Display results
            Console.WriteLine("\n✅ Success!");
            Console.WriteLine($"📊 Original URL: {urlToShorten}");
            Console.WriteLine($"🔗 Shortened URL: {shortUrl}");
            
            if (!string.IsNullOrWhiteSpace(customAlias))
            {
                Console.WriteLine($"🏷️  Custom Alias: {customAlias}");
            }

            Console.WriteLine("\n📋 Client Used:");
            Console.WriteLine("✅ TinyUrlSimpleClient (Simple, no authentication)");
            Console.WriteLine("   - No API key or token required");
            Console.WriteLine("   - Basic URL shortening functionality");
            Console.WriteLine("   - Perfect for simple use cases");
            
            Console.WriteLine("\n💡 Differences from TinyURLClient:");
            Console.WriteLine("   ❌ No authentication required");
            Console.WriteLine("   ❌ No analytics or reporting");
            Console.WriteLine("   ❌ No bulk operations");
            Console.WriteLine("   ❌ Limited customization options");
            Console.WriteLine("   ✅ Simple and easy to use");
            Console.WriteLine("   ✅ No account setup needed");

            Console.WriteLine("\n💡 For advanced features (analytics, bulk operations, etc.),");
            Console.WriteLine("   use the TinyURLClient with authentication as shown in");
            Console.WriteLine("   the TinyUrlConsoleExample project.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"❌ Invalid input: {ex.Message}");
            if (ex.ParamName == "alias")
            {
                Console.WriteLine("💡 Alias must be 5-30 characters long and contain only");
                Console.WriteLine("   letters, numbers, hyphens, and underscores.");
            }
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
            Console.WriteLine($"❌ TinyURL Error: {ex.Message}");
            if (ex.Message.Contains("alias"))
            {
                Console.WriteLine("💡 The alias might already be taken. Try a different one.");
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
