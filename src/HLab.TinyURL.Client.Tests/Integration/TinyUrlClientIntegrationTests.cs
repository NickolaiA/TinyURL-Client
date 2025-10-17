using Shouldly;

namespace HLab.TinyURL.Client.Tests.Integration;

/// <summary>
/// Integration tests for TinyUrlClient that test the actual functionality
/// These tests are marked with [Fact(Skip = ...)] to avoid making real API calls during regular test runs
/// To run these tests, remove the Skip parameter and ensure you have internet connectivity
/// </summary>
public class TinyUrlClientIntegrationTests : IDisposable
{
    private readonly TinyUrlSimpleClient _client;

    public TinyUrlClientIntegrationTests()
    {
        _client = new TinyUrlSimpleClient();
    }

    [Fact(Skip = "Integration test - requires internet connection and makes real API calls")]
    public async Task CreateShortUrlAsync_WithValidUrl_ShouldReturnValidShortUrl()
    {
        // Arrange
        const string validUrl = "https://www.microsoft.com";

        // Act
        var result = await _client.CreateShortUrlAsync(validUrl);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
        result.ShouldStartWith("https://tinyurl.com/");
    }

    [Theory(Skip = "Integration test - requires internet connection and makes real API calls")]
    [InlineData("valid-alias-123")]
    [InlineData("test_alias")]
    [InlineData("alias-with-hyphens")]
    [InlineData("UPPERCASE")]
    [InlineData("lowercase")]
    [InlineData("Mixed_Case-123")]
    public void ValidAlias_ShouldNotThrowException(string validAlias)
    {
        // Arrange & Act & Assert
        Should.NotThrow(async () => 
        {
            // We're just testing that the alias validation doesn't throw
            // We'll catch any TinyUrlException that might occur from the actual API call
            try
            {
                await _client.CreateShortUrlAsync("https://www.example.com", validAlias);
            }
            catch (TinyUrlException)
            {
                // Expected if alias is already taken or other API errors
                // We only care about ArgumentException not being thrown for validation
            }
        });
    }

    [Fact(Skip = "Integration test - requires internet connection and makes real API calls")]
    public async Task CreateShortUrlAsync_WithOptions_ShouldWork()
    {
        // Arrange
        var options = new TinyUrlOptions
        {
            Url = "https://www.example.com"
        };

        // Act
        var result = await _client.CreateShortUrlAsync(options);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
        result.ShouldStartWith("https://tinyurl.com/");
    }

    [Fact]
    public void Dispose_ShouldNotThrow()
    {
        // Arrange & Act & Assert
        Should.NotThrow(() => _client.Dispose());
    }

    [Fact]
    public void MultipleDispose_ShouldNotThrow()
    {
        // Arrange & Act & Assert
        Should.NotThrow(() =>
        {
            _client.Dispose();
            _client.Dispose(); // Should not throw on multiple disposes
        });
    }

    public void Dispose()
    {
        _client?.Dispose();
    }
}