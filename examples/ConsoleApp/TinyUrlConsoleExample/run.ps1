#!/usr/bin/env pwsh

Write-Host ""
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "   TinyURL Console Example Application" -ForegroundColor Cyan  
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "This application demonstrates how to use the" -ForegroundColor Yellow
Write-Host "HLab.TinyURL.Client library with Bearer token" -ForegroundColor Yellow
Write-Host "or API key authentication." -ForegroundColor Yellow
Write-Host ""
Write-Host "Make sure you have your TinyURL credentials ready!" -ForegroundColor Green
Write-Host "Get them from: https://tinyurl.com/app/dev" -ForegroundColor Blue
Write-Host ""
Read-Host "Press Enter to start the application"
Write-Host ""

try {
    dotnet run
}
catch {
    Write-Host "Error running the application: $_" -ForegroundColor Red
}

Write-Host ""
Read-Host "Press Enter to exit"