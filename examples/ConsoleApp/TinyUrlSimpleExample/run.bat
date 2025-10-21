@echo off
REM Batch script to run the TinyURL Simple Client Console Example

echo.
echo 🔗 TinyURL Simple Client Console Example
echo =======================================
echo.

echo 🔨 Building the project...
dotnet build --nologo

if %ERRORLEVEL% neq 0 (
    echo ❌ Build failed!
    exit /b 1
)

echo ✅ Build successful!
echo.

echo 🚀 Running the example...
echo.
dotnet run --no-build

echo.
echo 👋 Example finished.
pause
