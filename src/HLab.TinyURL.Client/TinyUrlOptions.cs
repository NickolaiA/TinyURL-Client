namespace HLab.TinyURL.Client;

/// <summary>
/// Options for creating a shortened URL with TinyURL service
/// </summary>
public class TinyUrlOptions
{
    /// <summary>
    /// Gets or sets the URL to shorten
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional custom alias for the shortened URL
    /// </summary>
    /// <remarks>
    /// Alias must contain only alphanumeric characters, hyphens, and underscores,
    /// and be between 5-30 characters long.
    /// </remarks>
    public string? Alias { get; set; }

    /// <summary>
    /// Initializes a new instance of the TinyUrlOptions class
    /// </summary>
    public TinyUrlOptions()
    {
    }

    /// <summary>
    /// Initializes a new instance of the TinyUrlOptions class with the specified URL
    /// </summary>
    /// <param name="url">The URL to shorten</param>
    public TinyUrlOptions(string url)
    {
        Url = url;
    }

    /// <summary>
    /// Initializes a new instance of the TinyUrlOptions class with the specified URL and alias
    /// </summary>
    /// <param name="url">The URL to shorten</param>
    /// <param name="alias">The custom alias for the shortened URL</param>
    public TinyUrlOptions(string url, string alias)
    {
        Url = url;
        Alias = alias;
    }
}