using Shouldly;

namespace HLab.TinyURL.Client.Tests;

public class TinyUrlOptionsTests
{
    [Fact]
    public void Constructor_Default_ShouldHaveEmptyUrl()
    {
        // Arrange & Act
        var options = new TinyUrlOptions();

        // Assert
        options.Url.ShouldBe(string.Empty);
        options.Alias.ShouldBeNull();
    }

    [Fact]
    public void Constructor_WithUrl_ShouldSetUrl()
    {
        // Arrange
        const string testUrl = "https://www.example.com";

        // Act
        var options = new TinyUrlOptions(testUrl);

        // Assert
        options.Url.ShouldBe(testUrl);
        options.Alias.ShouldBeNull();
    }

    [Fact]
    public void Constructor_WithUrlAndAlias_ShouldSetBothProperties()
    {
        // Arrange
        const string testUrl = "https://www.example.com";
        const string testAlias = "myalias";

        // Act
        var options = new TinyUrlOptions(testUrl, testAlias);

        // Assert
        options.Url.ShouldBe(testUrl);
        options.Alias.ShouldBe(testAlias);
    }

    [Fact]
    public void ObjectInitializer_ShouldWork()
    {
        // Arrange & Act
        var options = new TinyUrlOptions
        {
            Url = "https://www.example.com",
            Alias = "myalias"
        };

        // Assert
        options.Url.ShouldBe("https://www.example.com");
        options.Alias.ShouldBe("myalias");
    }

    [Fact]
    public void ObjectInitializer_WithOnlyUrl_ShouldWork()
    {
        // Arrange & Act
        var options = new TinyUrlOptions
        {
            Url = "https://www.example.com"
        };

        // Assert
        options.Url.ShouldBe("https://www.example.com");
        options.Alias.ShouldBeNull();
    }

    [Fact]
    public void Properties_ShouldBeSettable()
    {
        // Arrange
        var options = new TinyUrlOptions();
        const string testUrl = "https://www.example.com";
        const string testAlias = "myalias";

        // Act
        options.Url = testUrl;
        options.Alias = testAlias;

        // Assert
        options.Url.ShouldBe(testUrl);
        options.Alias.ShouldBe(testAlias);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Url_WhenSetToEmptyOrWhitespace_ShouldBeAccepted(string url)
    {
        // Arrange
        var options = new TinyUrlOptions();

        // Act
        options.Url = url;

        // Assert
        options.Url.ShouldBe(url);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Alias_WhenSetToNullOrWhitespace_ShouldBeAccepted(string alias)
    {
        // Arrange
        var options = new TinyUrlOptions();

        // Act
        options.Alias = alias;

        // Assert
        options.Alias.ShouldBe(alias);
    }
}