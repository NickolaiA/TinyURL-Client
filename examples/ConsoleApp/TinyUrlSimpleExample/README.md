# TinyURL Simple Client Console Example

This console application demonstrates the usage of the `TinyUrlSimpleClient` class for basic URL shortening without authentication.

## Features

- **Simple URL Shortening**: Create shortened URLs without requiring API credentials
- **Custom Aliases**: Optional support for custom URL aliases
- **No Authentication**: Works without API keys or bearer tokens
- **User-Friendly**: Interactive console interface with clear prompts and error messages

## Key Differences from TinyURLClient

### TinyUrlSimpleClient (This Example)
- ✅ No authentication required
- ✅ Simple and quick to use
- ✅ Perfect for basic URL shortening
- ❌ No analytics or reporting
- ❌ No bulk operations
- ❌ Limited API features

### TinyURLClient (Other Example)
- ✅ Full API access
- ✅ Analytics and reporting
- ✅ Bulk operations
- ✅ Advanced features
- ❌ Requires API authentication

## Running the Example

### Using VS Code Tasks

```powershell
# Build the project
dotnet build

# Run the project
dotnet run
```

### Using PowerShell Script

```powershell
.\run.ps1
```

### Using Batch File

```cmd
run.bat
```

## Usage Example

```
🔗 TinyURL Simple Client Example
==================================
This example demonstrates the simple TinyUrlSimpleClient
for basic URL shortening without authentication.

Enter the URL to shorten: https://www.example.com/very/long/url

Enter custom alias (optional, 5-30 characters, press Enter to skip): mylink

🔄 Creating shortened URL...

✅ Success!
📊 Original URL: https://www.example.com/very/long/url
🔗 Shortened URL: https://tinyurl.com/mylink
🏷️  Custom Alias: mylink
```

## When to Use This Client

Use `TinyUrlSimpleClient` when:
- You need quick and simple URL shortening
- You don't need analytics or tracking
- You don't want to set up API credentials
- You're building a simple tool or script

For advanced features like analytics, bulk operations, and enhanced control, use the `TinyURLClient` demonstrated in the `TinyUrlConsoleExample` project.

## Project Structure

```
TinyUrlSimpleExample/
├── Program.cs                    # Main application logic
├── TinyUrlSimpleExample.csproj   # Project file
├── README.md                     # This file
├── run.ps1                       # PowerShell run script
└── run.bat                       # Batch run script
```

## Error Handling

The example includes comprehensive error handling for:
- Invalid URL formats
- Invalid alias formats
- Network errors
- Request timeouts
- TinyURL service errors

## Dependencies

- .NET 8.0
- HLab.TinyURL.Client library
