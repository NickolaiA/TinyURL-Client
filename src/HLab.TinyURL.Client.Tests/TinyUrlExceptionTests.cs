using Shouldly;

namespace HLab.TinyURL.Client.Tests;

public class TinyUrlExceptionTests
{
    [Fact]
    public void Constructor_Default_ShouldCreateException()
    {
        // Arrange & Act
        var exception = new TinyUrlException();

        // Assert
        exception.ShouldNotBeNull();
        exception.Message.ShouldNotBeNull();
    }

    [Fact]
    public void Constructor_WithMessage_ShouldSetMessage()
    {
        // Arrange
        const string testMessage = "Test error message";

        // Act
        var exception = new TinyUrlException(testMessage);

        // Assert
        exception.Message.ShouldBe(testMessage);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_ShouldSetBothProperties()
    {
        // Arrange
        const string testMessage = "Test error message";
        var innerException = new InvalidOperationException("Inner exception");

        // Act
        var exception = new TinyUrlException(testMessage, innerException);

        // Assert
        exception.Message.ShouldBe(testMessage);
        exception.InnerException.ShouldBe(innerException);
    }

    [Fact]
    public void Exception_ShouldBeThrowableAndCatchable()
    {
        // Arrange
        const string testMessage = "Test exception";

        // Act & Assert
        var thrownException = Should.Throw<TinyUrlException>(() => 
        {
            throw new TinyUrlException(testMessage);
        });

        thrownException.Message.ShouldBe(testMessage);
    }

    [Fact]
    public void Exception_ShouldBeInstanceOfException()
    {
        // Arrange & Act
        var exception = new TinyUrlException();

        // Assert
        exception.ShouldBeAssignableTo<Exception>();
    }
}