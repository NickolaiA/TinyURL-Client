using System.Net;
using Moq;
using Moq.Protected;
using Shouldly;

namespace HLab.TinyURL.Client.Tests;

public class TinyUrlClientTests : IDisposable
{
    private readonly Mock<HttpMessageHandler> _mockHttpHandler;
    private readonly HttpClient _httpClient;
    private readonly TinyUrlSimpleClient _client;

    public TinyUrlClientTests()
    {
        _mockHttpHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpHandler.Object);
        _client = new TinyUrlSimpleClient(_httpClient);
    }

    [Fact]
    public void Constructor_WithHttpClient_ShouldNotThrow()
    {
        // Arrange & Act
        var client = new TinyUrlSimpleClient(_httpClient);
        
        // Assert
        client.ShouldNotBeNull();
    }

    [Fact]
    public void Constructor_WithNullHttpClient_ShouldThrowArgumentNullException()
    {
        // Arrange, Act & Assert
        Should.Throw<ArgumentNullException>(() => new TinyUrlSimpleClient(null!));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task CreateShortUrlAsync_WithNullOrEmptyUrl_ShouldThrowArgumentNullException(string url)
    {
        // Arrange, Act & Assert
        await Should.ThrowAsync<ArgumentNullException>(
            () => _client.CreateShortUrlAsync(url));
    }

    [Theory]
    [InlineData("not-a-url")]
    [InlineData("ftp://example.com")]
    [InlineData("file://C:/test.txt")]
    [InlineData("javascript:alert('xss')")]
    public async Task CreateShortUrlAsync_WithInvalidUrl_ShouldThrowArgumentException(string invalidUrl)
    {
        // Arrange, Act & Assert
        await Should.ThrowAsync<ArgumentException>(
            () => _client.CreateShortUrlAsync(invalidUrl));
    }

    [Theory]
    [InlineData("abc")]     // Too short
    [InlineData("ab")]      // Too short
    [InlineData("a")]       // Too short
    [InlineData("this-is-a-very-long-alias-that-exceeds-thirty-characters")]  // Too long
    [InlineData("invalid@alias")]   // Invalid characters
    [InlineData("invalid alias")]   // Space not allowed
    [InlineData("invalid.alias")]   // Period not allowed
    public async Task CreateShortUrlAsync_WithInvalidAlias_ShouldThrowArgumentException(string invalidAlias)
    {
        // Arrange, Act & Assert
        await Should.ThrowAsync<ArgumentException>(
            () => _client.CreateShortUrlAsync("https://www.example.com", invalidAlias));
    }

    [Fact]
    public async Task CreateShortUrlAsync_WithValidUrl_ShouldReturnShortUrl()
    {
        // Arrange
        const string expectedShortUrl = "https://tinyurl.com/abc123";
        const string testUrl = "https://www.example.com";
        
        _mockHttpHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedShortUrl)
            });

        // Act
        var result = await _client.CreateShortUrlAsync(testUrl);

        // Assert
        result.ShouldBe(expectedShortUrl);
        
        _mockHttpHandler.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Get &&
                req.RequestUri!.ToString().Contains("url=" + Uri.EscapeDataString(testUrl))),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task CreateShortUrlAsync_WithValidUrlAndAlias_ShouldReturnShortUrl()
    {
        // Arrange
        const string expectedShortUrl = "https://tinyurl.com/myalias";
        const string testUrl = "https://www.example.com";
        const string testAlias = "myalias";
        
        _mockHttpHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedShortUrl)
            });

        // Act
        var result = await _client.CreateShortUrlAsync(testUrl, testAlias);

        // Assert
        result.ShouldBe(expectedShortUrl);
        
        _mockHttpHandler.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Get &&
                req.RequestUri!.ToString().Contains("url=" + Uri.EscapeDataString(testUrl)) &&
                req.RequestUri!.ToString().Contains("alias=" + Uri.EscapeDataString(testAlias))),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task CreateShortUrlAsync_WithOptions_ShouldReturnShortUrl()
    {
        // Arrange
        const string expectedShortUrl = "https://tinyurl.com/optionstest";
        var options = new TinyUrlOptions
        {
            Url = "https://www.example.com",
            Alias = "optionstest"
        };
        
        _mockHttpHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedShortUrl)
            });

        // Act
        var result = await _client.CreateShortUrlAsync(options);

        // Assert
        result.ShouldBe(expectedShortUrl);
    }

    [Fact]
    public async Task CreateShortUrlAsync_WithNullOptions_ShouldThrowArgumentNullException()
    {
        // Arrange, Act & Assert
        await Should.ThrowAsync<ArgumentNullException>(
            () => _client.CreateShortUrlAsync((TinyUrlOptions)null!));
    }

    [Fact]
    public async Task CreateShortUrlAsync_WhenApiReturnsError_ShouldThrowTinyUrlException()
    {
        // Arrange
        const string testUrl = "https://www.example.com";
        
        _mockHttpHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent("Error: Invalid URL")
            });

        // Act & Assert
        await Should.ThrowAsync<TinyUrlException>(
            () => _client.CreateShortUrlAsync(testUrl));
    }

    [Fact]
    public async Task CreateShortUrlAsync_WhenApiReturnsEmptyResponse_ShouldThrowTinyUrlException()
    {
        // Arrange
        const string testUrl = "https://www.example.com";
        
        _mockHttpHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("")
            });

        // Act & Assert
        await Should.ThrowAsync<TinyUrlException>(
            () => _client.CreateShortUrlAsync(testUrl));
    }

    [Fact]
    public async Task CreateShortUrlAsync_WhenApiReturnsErrorMessage_ShouldThrowTinyUrlException()
    {
        // Arrange
        const string testUrl = "https://www.example.com";
        
        _mockHttpHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("Error: Alias already taken")
            });

        // Act & Assert
        await Should.ThrowAsync<TinyUrlException>(
            () => _client.CreateShortUrlAsync(testUrl));
    }

    [Fact]
    public async Task CreateShortUrlAsync_WhenHttpRequestFails_ShouldThrowTinyUrlException()
    {
        // Arrange
        const string testUrl = "https://www.example.com";
        
        _mockHttpHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Network error"));

        // Act & Assert
        await Should.ThrowAsync<TinyUrlException>(
            () => _client.CreateShortUrlAsync(testUrl));
    }

    [Fact]
    public async Task CreateShortUrlAsync_WhenCancelledByToken_ShouldThrowTaskCanceledException()
    {
        // Arrange
        const string testUrl = "https://www.example.com";
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        
        _mockHttpHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new TaskCanceledException("Operation was cancelled"));

        // Act & Assert
        await Should.ThrowAsync<TaskCanceledException>(
            () => _client.CreateShortUrlAsync(testUrl, cancellationToken: cts.Token));
    }

    [Fact]
    public async Task CreateShortUrlAsync_WhenTimedOut_ShouldThrowTinyUrlException()
    {
        // Arrange
        const string testUrl = "https://www.example.com";
        
        _mockHttpHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new TaskCanceledException("Operation was cancelled", new TimeoutException()));

        // Act & Assert
        await Should.ThrowAsync<TinyUrlException>(
            () => _client.CreateShortUrlAsync(testUrl));
    }

    public void Dispose()
    {
        _client?.Dispose();
        _httpClient?.Dispose();
    }
}