namespace HLab.TinyURL.Client;

/// <summary>
/// Exception thrown when TinyURL API operations fail
/// </summary>
public class TinyUrlException : Exception
{
    /// <summary>
    /// Initializes a new instance of the TinyUrlException class
    /// </summary>
    public TinyUrlException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the TinyUrlException class with a specified error message
    /// </summary>
    /// <param name="message">The message that describes the error</param>
    public TinyUrlException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the TinyUrlException class with a specified error message and inner exception
    /// </summary>
    /// <param name="message">The message that describes the error</param>
    /// <param name="innerException">The exception that is the cause of the current exception</param>
    public TinyUrlException(string message, Exception innerException) : base(message, innerException)
    {
    }
}