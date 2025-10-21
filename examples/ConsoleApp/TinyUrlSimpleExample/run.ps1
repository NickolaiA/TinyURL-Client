#!/usr/bin/env pwsh
# PowerShell script to run the TinyURL Simple Client Console Example

Write-Host "🔗 TinyURL Simple Client Console Example" -ForegroundColor Cyan
Write-Host "=======================================" -ForegroundColor Cyan
Write-Host ""

# Build the project
Write-Host "🔨 Building the project..." -ForegroundColor Yellow
dotnet build --nologo

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Build failed!" -ForegroundColor Red
    exit 1
}

Write-Host "✅ Build successful!" -ForegroundColor Green
Write-Host ""

# Run the project
Write-Host "🚀 Running the example..." -ForegroundColor Yellow
Write-Host ""
dotnet run --no-build

Write-Host ""
Write-Host "👋 Example finished." -ForegroundColor Cyan
