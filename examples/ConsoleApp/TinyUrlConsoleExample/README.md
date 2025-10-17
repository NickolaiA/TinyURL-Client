# TinyURL Console Example

This console application demonstrates how to use the HLab.TinyURL.Client library with the comprehensive TinyURLClient class, showcasing both Bearer token and API key authentication methods with full API access including analytics.

## Features

- ✅ **Full TinyURLClient Usage**: Uses the comprehensive client for maximum API access
- 🔗 **URL Shortening**: Create shortened URLs with optional custom aliases
- 📊 **Analytics Integration**: Demonstrates analytics retrieval for created URLs
- 🔐 **Dual Authentication Support**: Bearer Token and API Key methods
- 🛡️ **Input Validation**: Validates URLs and handles errors gracefully
- 📱 **Interactive UI**: User-friendly console interface with emojis
- 🔧 **Error Handling**: Comprehensive error handling with helpful messages

## Prerequisites

- .NET 8.0 or later
- TinyURL API credentials (Bearer Token or API Key)
- Internet connection

## Getting API Credentials

1. Visit [TinyURL API Dashboard](https://tinyurl.com/app/dev)
2. Create an account or sign in
3. Generate your API credentials:
   - **Bearer Token**: For secure production applications (recommended)
   - **API Key**: For simple integrations and testing

## How to Run

1. **Build the application:**
   ```bash
   dotnet build
   ```

2. **Run the application:**
   ```bash
   dotnet run
   ```

3. **Follow the interactive prompts:**
   - Choose authentication method (1 for Bearer Token, 2 for API Key)
   - Enter your credentials
   - Enter the URL you want to shorten
   - Optionally provide a custom alias

## Example Usage

```
🔗 TinyURL Console Example
==========================
This example demonstrates both authentication methods:
1. Bearer Token (recommended for production)
2. API Key (for simple integrations)

Choose authentication method (1=Bearer Token, 2=API Key): 1

Enter your TinyURL Bearer token: your-bearer-token-here

Enter the URL to shorten: https://www.example.com/very/long/url

Enter custom alias (optional, press Enter to skip): my-example

🔄 Creating shortened URL...

✅ Success!
📊 Original URL: https://www.example.com/very/long/url
🔗 Shortened URL: https://tinyurl.com/my-example
🏷️  Custom Alias: my-example
📅 Created: 10/17/2025 2:30:45 PM

💡 Note: For advanced features like analytics, use the TinyURLClient class
💡 See the README.md for comprehensive examples

👋 Press any key to exit...
```

## Authentication Methods

### 1. Bearer Token (Recommended)
- **More Secure**: Credentials sent in HTTP headers
- **Production Ready**: Suitable for enterprise applications
- **Header Format**: `Authorization: Bearer your-token`

### 2. API Key
- **Simple Integration**: Easy to implement
- **URL Parameter**: Passed as query parameter
- **Format**: `?api_token=your-key`

## Error Handling

The application handles various error scenarios:

- **Network Errors**: Connection timeouts, DNS issues
- **Authentication Errors**: Invalid tokens/keys
- **Validation Errors**: Invalid URL formats
- **API Errors**: TinyURL service errors

## Project Structure

- **Program.cs**: Main application logic
- **TinyUrlConsoleExample.csproj**: Project configuration with reference to HLab.TinyURL.Client
- **README.md**: This documentation

## Advanced Usage

This example demonstrates the full `TinyURLClient` capabilities including:

- 📊 Analytics and reporting (demonstrated in the application)
- 📦 Bulk operations (see main project README for examples)
- 🏷️ Domain management
- 📈 Click tracking and analytics

The console application shows how to integrate all these features in a real application.

## Troubleshooting

### Common Issues

1. **"Bearer token is required"**
   - Ensure you have a valid Bearer token from TinyURL
   - Check that you're entering the token correctly

2. **"Network error"**
   - Check your internet connection
   - Verify the TinyURL API is accessible

3. **"TinyURL API Error"**
   - Verify your credentials are valid and active
   - Check if your account has the necessary permissions

4. **"Invalid URL format"**
   - Ensure the URL includes http:// or https://
   - Check that the URL is properly formatted

### Getting Help

- Check the main project README.md for detailed documentation
- Visit [TinyURL API Documentation](https://tinyurl.com/app/dev/documentation)
- Review the test project for more usage examples

## License

This example is part of the HLab.TinyURL.Client project and is licensed under the MIT License.