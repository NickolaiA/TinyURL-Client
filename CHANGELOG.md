# Changelog

All notable changes to the HLab.TinyURL.Client project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.1] - 2025-10-21

### Added
- New `TinyUrlSimpleExample` console application demonstrating basic URL shortening
  - Interactive console interface for testing `TinyUrlSimpleClient`
  - No authentication required for simple use cases
  - Comprehensive error handling and input validation
  - Includes PowerShell and batch scripts for easy execution
  - Full documentation in dedicated README.md

### Fixed
- Fixed URL used to access TinyURL API in `TinyUrlSimpleClient` 

### Changed
- Updated main README.md to document both console app examples
  - Added comparison between full-featured and simple examples
  - Included instructions for running both examples
  - Enhanced console app examples section

### Improved
- Enhanced VS Code development experience
  - Added build and run tasks for `TinyUrlSimpleExample`
  - Added debug configurations for simple console app
  - Support for both integrated and external terminal debugging
- Updated solution file to include both console application projects
- Improved project organization and discoverability

## [1.0.0] - 2025-10-17

### Added
- **Core Library Features**
  - `TinyURLClient` - Comprehensive API client with full feature support
    - Complete TinyURL API integration
    - Analytics and reporting capabilities (raw logs, timeline, general, weekdays, hours)
    - Location analytics with geographic data
    - Top sources and languages analytics
    - Bulk operations support
    - URL management (create, update, delete, archive)
    - Domain and alias management
    - Analytics status and sharing controls
  - `TinyUrlSimpleClient` - Lightweight client for basic URL shortening
    - Simple API for creating shortened URLs
    - Custom alias support
    - Built-in input validation
    - No authentication required for basic operations
  - `TinyUrlOptions` - Configuration class for simple client
  - `TinyUrlException` - Custom exception with detailed error information

- **Authentication Support**
  - Bearer token authentication (recommended for production)
  - API key authentication via URL parameter
  - Support for environment variables and configuration files
  - HttpClient authentication header configuration

- **Examples and Documentation**
  - `TinyUrlConsoleExample` - Full-featured console application
    - Demonstrates both authentication methods
    - Interactive UI for URL shortening
    - Analytics demonstration
    - Comprehensive error handling
    - Input validation
    - PowerShell and batch run scripts
  - Comprehensive README.md with usage examples
  - Detailed API reference documentation
  - Authentication guide with best practices
  - HttpClientFactory usage guidelines

- **Development Tools**
  - VS Code debugging configurations
    - Debug console apps in integrated terminal
    - Debug with external terminal support
    - Run without debugging option
  - VS Code tasks
    - Build tasks for library and console apps
    - Run tasks for quick testing
    - Release build and NuGet pack tasks
    - Clean tasks for maintenance
  - Solution file with organized project structure

- **Testing Infrastructure**
  - Unit tests using xUnit framework
  - Mock-based testing with Moq
  - Assertions with Shouldly
  - Test coverage for core functionality
  - Input validation tests
  - Error handling tests

- **Build and Packaging**
  - .NET 8.0 target framework
  - NuGet package configuration
  - Release build configuration
  - Artifact generation support

### Documentation
- Comprehensive README with:
  - Feature overview and client comparison
  - Installation instructions
  - Authentication guide (Bearer token and API key)
  - Usage examples for both clients
  - HttpClientFactory best practices
  - Dependency injection setup
  - Error handling guidelines
  - API reference
  - Testing guide
  - Development and debugging support
  - Console app examples

- Individual README files for console applications
  - Feature descriptions
  - Running instructions
  - Usage examples
  - Project structure documentation

### Technical Details
- Target Framework: .NET 8.0
- Language: C# with nullable reference types enabled
- HTTP Client: Built on System.Net.Http.HttpClient
- JSON Serialization: Newtonsoft.Json
- API Code Generation: NSwag for TinyURLClient
- Testing: xUnit, Moq, Shouldly

### Project Structure
```
TinyURL-Client/
├── src/
│   ├── HLab.TinyURL.Client/          # Main library
│   └── HLab.TinyURL.Client.Tests/    # Unit tests
├── examples/ConsoleApp/
│   ├── TinyUrlConsoleExample/        # Full-featured example
│   └── TinyUrlSimpleExample/         # Simple client example
├── .vscode/                          # VS Code configuration
├── artifacts/                        # Build outputs
├── CHANGELOG.md                      # This file
└── README.md                         # Main documentation
```

### Notes
- First stable release of HLab.TinyURL.Client
- Supports TinyURL API v1
- Requires .NET 8.0 or later
- Tested on Windows, macOS, and Linux

[Unreleased]: https://github.com/NickolaiA/TinyURL-Client/compare/v1.0.0...HEAD
[1.0.0]: https://github.com/NickolaiA/TinyURL-Client/releases/tag/v1.0.0
