# SmartWarehouse - Environment Setup Guide

This document provides comprehensive instructions for setting up the development environment for the SmartWarehouse project.

## Overview

SmartWarehouse is a .NET MAUI application targeting multiple platforms:
- **iOS** (net10.0-ios26.2)
- **MacCatalyst** (net10.0-maccatalyst26.2)
- **Android** (net10.0-android)
- **Web** (net10.0) - Blazor Server and WebAssembly

## Prerequisites

### Required Software

- **.NET SDK 10.0** or later
- **Visual Studio Code** or **Visual Studio** / **Rider**
- **Git**

### Platform-Specific Requirements

#### macOS (for iOS/MacCatalyst/Android)
- **Xcode 26.2** or later (for iOS/MacCatalyst development)
- **Java SDK 17** (Microsoft OpenJDK 17 recommended)
- **Android SDK** (for Android development)

#### Windows (for Windows/Android)
- **Visual Studio 2022** with .NET MAUI workload
- **Windows SDK**

## .NET SDK Installation

### Verify Installation
```bash
dotnet --version
```

Should show: `10.0.101` or later

### Install .NET MAUI Workload
```bash
# macOS/Linux
sudo dotnet workload install maui

# Windows
dotnet workload install maui
```

### Verify Workload Installation
```bash
dotnet workload list
```

Expected workloads:
- `maui` or `maui-android`
- `android`
- `ios` (macOS only)
- `maccatalyst` (macOS only)

## Platform-Specific Setup

### iOS Development (macOS Only)

#### 1. Install Xcode
- Download from [Apple App Store](https://apps.apple.com/us/app/xcode/id497799835)
- **Required Version:** Xcode 26.2 or later for .NET 10
- Launch Xcode and accept license agreements

#### 2. Install Xcode Command Line Tools
```bash
xcode-select --install
```

#### 3. Verify Xcode Installation
```bash
xcodebuild -version
```

Should show: `Xcode 26.2` or later

#### 4. Configure Simulators
- Open Xcode → Settings → Components
- Install required iOS simulator runtimes

### MacCatalyst Development (macOS Only)

MacCatalyst uses the same setup as iOS. Once iOS is configured, MacCatalyst is ready.

**Note:** Minimum supported macOS version is 15.0 (Sequoia)

### Android Development

#### 1. Install Java SDK

**macOS (using Homebrew):**
```bash
brew install --cask microsoft-openjdk@17
```

**Windows:**
- Download [Microsoft OpenJDK 17](https://learn.microsoft.com/en-us/java/openjdk/download)
- Install and note the installation path

**Linux:**
```bash
# Ubuntu/Debian
sudo apt-get install openjdk-17-jdk

# Fedora
sudo dnf install java-17-openjdk-devel
```

#### 2. Set JAVA_HOME Environment Variable

**macOS/Linux:**
Add to `~/.zshrc` or `~/.bashrc`:
```bash
export JAVA_HOME=$(/usr/libexec/java_home -v 17)
```

**Windows:**
- System Properties → Environment Variables
- Add `JAVA_HOME` pointing to Java installation directory

**Verify:**
```bash
echo $JAVA_HOME  # macOS/Linux
echo %JAVA_HOME% # Windows
java -version
```

#### 3. Install Android SDK

**Option A: Using MSBuild Target (Recommended)**
```bash
cd SmartWarehouse.Mobile
dotnet build \
  -t:InstallAndroidDependencies \
  -f:net10.0-android \
  -p:AndroidSdkDirectory="$HOME/Library/Android/sdk" \
  -p:JavaSdkDirectory="$JAVA_HOME" \
  -p:AcceptAndroidSDKLicenses=True
```

**Option B: Using Android Studio**
1. Download [Android Studio](https://developer.android.com/studio)
2. Install Android SDK through Android Studio
3. Note the SDK location (typically `~/Library/Android/sdk` on macOS)

#### 4. Set ANDROID_HOME Environment Variable

**macOS/Linux:**
Add to `~/.zshrc` or `~/.bashrc`:
```bash
export ANDROID_HOME=$HOME/Library/Android/sdk
export PATH=$PATH:$ANDROID_HOME/platform-tools
export PATH=$PATH:$ANDROID_HOME/tools
```

**Windows:**
- System Properties → Environment Variables
- Add `ANDROID_HOME` pointing to Android SDK directory
- Add `%ANDROID_HOME%\platform-tools` and `%ANDROID_HOME%\tools` to PATH

**Verify:**
```bash
echo $ANDROID_HOME  # macOS/Linux
adb version         # Should show Android Debug Bridge version
```

#### 5. Accept Android Licenses
```bash
cd $ANDROID_HOME/cmdline-tools/latest/bin
./sdkmanager --licenses
# Accept all licenses by typing 'y'
```

#### 6. Install Android Emulator (Optional but Recommended)

**macOS (Apple Silicon):**
```bash
cd $ANDROID_HOME/cmdline-tools/latest/bin
./sdkmanager --install emulator
./sdkmanager --install "system-images;android-35;google_apis;arm64-v8a"
```

**macOS (Intel):**
```bash
cd $ANDROID_HOME/cmdline-tools/latest/bin
./sdkmanager --install emulator
./sdkmanager --install "system-images;android-35;google_apis;x86_64"
```

**Create Emulator:**
```bash
cd $ANDROID_HOME/cmdline-tools/latest/bin
# Apple Silicon
./avdmanager create avd -n MyAndroidDevice-API35 -k "system-images;android-35;google_apis;arm64-v8a"

# Intel
./avdmanager create avd -n MyAndroidDevice-API35 -k "system-images;android-35;google_apis;x86_64"
```

### Web Development

No additional setup required beyond .NET SDK installation.

## Project Configuration

### Target Frameworks

**SmartWarehouse.Mobile:**
```xml
<TargetFrameworks>net10.0-android;net10.0-ios26.2;net10.0-maccatalyst26.2</TargetFrameworks>
```

**SmartWarehouse.Shared:**
```xml
<TargetFramework>net10.0</TargetFramework>
```

**SmartWarehouse.Web:**
```xml
<TargetFramework>net10.0</TargetFramework>
```

### Package Versions

- **MAUI:** 10.0.0
- **ASP.NET Core:** 10.0.1
- **Microsoft.Extensions.Logging.Debug:** 10.0.0

## Build Commands

### Build All Projects
```bash
dotnet build
```

### Build Specific Platform
```bash
# iOS
dotnet build SmartWarehouse.Mobile/SmartWarehouse.Mobile.csproj -f net10.0-ios26.2

# MacCatalyst
dotnet build SmartWarehouse.Mobile/SmartWarehouse.Mobile.csproj -f net10.0-maccatalyst26.2

# Android
dotnet build SmartWarehouse.Mobile/SmartWarehouse.Mobile.csproj -f net10.0-android

# Web
dotnet build SmartWarehouse.Web/SmartWarehouse.Web/SmartWarehouse.Web.csproj
```

### Restore Packages
```bash
dotnet restore
```

## Debugging

### iOS Debugging (macOS)
- Use iOS Simulator or physical device
- Full debugging support: breakpoints, step-through, variable inspection
- Requires Xcode 26.2+

### MacCatalyst Debugging (macOS)
- Debug directly on macOS
- Same debugging capabilities as iOS
- Requires macOS 15.0+

### Android Debugging
- **Emulator:** Run Android emulator and debug from IDE
- **Physical Device:** Connect via USB with USB debugging enabled
- Full debugging support on both macOS and Windows

### Web Debugging
- Standard ASP.NET Core debugging
- Browser DevTools integration
- Hot reload support

## Troubleshooting

### Common Issues

#### 1. Xcode Version Mismatch
**Error:** `This version of .NET for iOS requires Xcode 26.2`

**Solution:**
- Update Xcode to version 26.2 or later
- Verify with: `xcodebuild -version`

#### 2. Android SDK Not Found
**Error:** `XA5300: The Android SDK directory could not be found`

**Solution:**
- Verify `ANDROID_HOME` environment variable is set
- Run: `echo $ANDROID_HOME` (macOS/Linux) or `echo %ANDROID_HOME%` (Windows)
- Install Android SDK using instructions above

#### 3. Java SDK Not Found
**Error:** `Failed to get the Java SDK version`

**Solution:**
- Install Microsoft OpenJDK 17
- Set `JAVA_HOME` environment variable
- Verify with: `java -version`

#### 4. Framework Compatibility Errors
**Error:** `Package X is not compatible with net8.0`

**Solution:**
- Ensure all projects target .NET 10.0
- Update package versions to match .NET 10 requirements

#### 5. MacCatalyst Minimum Version Error
**Error:** `SupportedOSPlatformVersion value '14.0' is lower than minimum '15.0'`

**Solution:**
- Update `SupportedOSPlatformVersion` to 15.0 in project file:
```xml
<SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'maccatalyst'">15.0</SupportedOSPlatformVersion>
```

## Environment Verification

### Quick Check Script
```bash
# Check .NET SDK
dotnet --version

# Check workloads
dotnet workload list

# Check Xcode (macOS)
xcodebuild -version

# Check Java
java -version
echo $JAVA_HOME

# Check Android SDK
echo $ANDROID_HOME
adb version
```

### Expected Output
- .NET SDK: `10.0.101` or later
- Workloads: `maui`, `android`, `ios`, `maccatalyst` (macOS)
- Xcode: `26.2` or later (macOS)
- Java: `openjdk version "17.x.x"`
- Android SDK: Path should be set, `adb` should be available

## IDE Configuration

### Visual Studio Code
1. Install **.NET MAUI Extension**
2. Install **C# Dev Kit**
3. Configure Android/iOS paths via extension settings

### Visual Studio / Rider
- .NET MAUI workload includes all necessary tools
- Automatic environment detection

## Additional Resources

- [.NET MAUI Documentation](https://learn.microsoft.com/dotnet/maui/)
- [Android SDK Installation](https://aka.ms/dotnet-android-install-sdk)
- [Xcode Requirements](https://aka.ms/xcode-requirement)
- [.NET MAUI Release Versions](https://github.com/dotnet/maui/wiki/Release-Versions)

## Project Structure

```
SmartWarehouse/
├── SmartWarehouse.Mobile/          # MAUI mobile app
│   ├── Platforms/                  # Platform-specific code
│   ├── Resources/                  # App resources
│   └── SmartWarehouse.Mobile.csproj
├── SmartWarehouse.Shared/          # Shared components
│   └── SmartWarehouse.Shared.csproj
├── SmartWarehouse.Web/             # Web application
│   ├── SmartWarehouse.Web/         # Server project
│   └── SmartWarehouse.Web.Client/ # Blazor WebAssembly
└── docs/                           # Documentation
```

## Support

For issues or questions:
1. Check this documentation
2. Review [.NET MAUI Troubleshooting](https://learn.microsoft.com/dotnet/maui/troubleshooting/)
3. Check project-specific error messages and logs

