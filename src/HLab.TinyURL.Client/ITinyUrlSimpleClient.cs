namespace HLab.TinyURL.Client;

public interface ITinyUrlSimpleClient
{
    /// <summary>
    /// Creates a shortened URL using TinyURL service
    /// </summary>
    /// <param name="url">The URL to shorten</param>
    /// <param name="alias">Optional custom alias for the shortened URL</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The shortened URL</returns>
    Task<string> CreateShortUrlAsync(string url, string? alias = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a shortened URL using TinyURL service with additional options
    /// </summary>
    /// <param name="options">The options for creating the short URL</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The shortened URL</returns>
    Task<string> CreateShortUrlAsync(TinyUrlOptions options, CancellationToken cancellationToken = default);
}
