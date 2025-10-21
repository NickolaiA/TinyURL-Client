using System.Text;

namespace HLab.TinyURL.Client;

/// <summary>
/// A client for interacting with TinyURL services
/// </summary>
public class TinyUrlSimpleClient : IDisposable, ITinyUrlSimpleClient
{
    private readonly HttpClient _httpClient;
    private readonly bool _disposeHttpClient;
    private const string CreateApiUrl = "https://tinyurl.com/api-create.php";

    /// <summary>
    /// Initializes a new instance of the TinyUrlClient class
    /// </summary>
    public TinyUrlSimpleClient() : this(new HttpClient())
    {
        _disposeHttpClient = true;
    }

    /// <summary>
    /// Initializes a new instance of the TinyUrlClient class with a custom HttpClient
    /// </summary>
    /// <param name="httpClient">The HttpClient to use for requests</param>
    public TinyUrlSimpleClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _disposeHttpClient = false;
    }

    /// <summary>
    /// Creates a shortened URL using TinyURL service
    /// </summary>
    /// <param name="url">The URL to shorten</param>
    /// <param name="alias">Optional custom alias for the shortened URL</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The shortened URL</returns>
    /// <exception cref="ArgumentNullException">Thrown when url is null or empty</exception>
    /// <exception cref="ArgumentException">Thrown when url is not a valid URI</exception>
    /// <exception cref="TinyUrlException">Thrown when the TinyURL service returns an error</exception>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails</exception>
    public async Task<string> CreateShortUrlAsync(string url, string? alias = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentNullException(nameof(url));

        // Validate URL format
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) ||
            (uri.Scheme != "http" && uri.Scheme != "https"))
        {
            throw new ArgumentException("Invalid URL format. URL must be a valid HTTP or HTTPS URL.", nameof(url));
        }

        try
        {
            var requestUrl = BuildRequestUrl(url, alias);

            var response = await _httpClient.GetAsync(requestUrl, cancellationToken);
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new TinyUrlException($"TinyURL API returned error: {response.StatusCode} - {responseContent}");
            }

            // TinyURL returns the shortened URL directly as plain text
            var shortUrl = responseContent.Trim();

            // Validate the response
            if (string.IsNullOrWhiteSpace(shortUrl))
            {
                throw new TinyUrlException("TinyURL API returned an empty response");
            }

            // Check if the response contains an error message
            if (shortUrl.StartsWith("Error", StringComparison.OrdinalIgnoreCase) ||
                shortUrl.Contains("Invalid") ||
                !shortUrl.StartsWith("https://tinyurl.com/"))
            {
                throw new TinyUrlException($"TinyURL API returned an error: {shortUrl}");
            }

            return shortUrl;
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            throw new TinyUrlException("Request to TinyURL API timed out", ex);
        }
        catch (HttpRequestException ex)
        {
            throw new TinyUrlException($"Failed to communicate with TinyURL API: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Creates a shortened URL using TinyURL service with additional options
    /// </summary>
    /// <param name="options">The options for creating the short URL</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The shortened URL</returns>
    public async Task<string> CreateShortUrlAsync(TinyUrlOptions options, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        return await CreateShortUrlAsync(options.Url, options.Alias, cancellationToken);
    }

    /// <summary>
    /// Builds the request URL for the TinyURL API
    /// </summary>
    /// <param name="url">The URL to shorten</param>
    /// <param name="alias">Optional custom alias</param>
    /// <returns>The complete request URL</returns>
    private static string BuildRequestUrl(string url, string? alias)
    {
        var requestUrl = new StringBuilder(CreateApiUrl);
        requestUrl.Append("?url=");
        requestUrl.Append(Uri.EscapeDataString(url));

        if (!string.IsNullOrWhiteSpace(alias))
        {
            // Validate alias format (alphanumeric characters, hyphens, and underscores)
            if (!IsValidAlias(alias))
            {
                throw new ArgumentException(
                    "Alias must contain only alphanumeric characters, hyphens, and underscores, and be between 5-30 characters long.",
                    nameof(alias));
            }

            requestUrl.Append("&alias=");
            requestUrl.Append(Uri.EscapeDataString(alias));
        }

        return requestUrl.ToString();
    }

    /// <summary>
    /// Validates the alias format
    /// </summary>
    /// <param name="alias">The alias to validate</param>
    /// <returns>True if the alias is valid, false otherwise</returns>
    private static bool IsValidAlias(string alias)
    {
        if (alias.Length < 5 || alias.Length > 30)
            return false;

        foreach (char c in alias)
        {
            if (!char.IsLetterOrDigit(c) && c != '-' && c != '_')
                return false;
        }

        return true;
    }

    /// <summary>
    /// Disposes the HttpClient if it was created internally
    /// </summary>
    public void Dispose()
    {
        if (_disposeHttpClient)
        {
            _httpClient?.Dispose();
        }
    }
}