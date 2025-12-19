# SmartWarehouse

A cross-platform warehouse management application built with .NET MAUI and Blazor, supporting mobile (iOS, Android, MacCatalyst) and web platforms.

## 🚀 Features

- **Cross-Platform Mobile App** - Native iOS, Android, MacCatalyst, and Windows applications built with .NET MAUI
- **Web Application** - Blazor Server and WebAssembly support with interactive render modes
- **REST API** - FastEndpoints-based RESTful API with Swagger/OpenAPI documentation
- **Real-time Inventory Updates** - SignalR hub broadcasts inventory changes to all connected clients in real-time
- **Platform-Adaptive Barcode Scanning** - Abstracted interface with native mobile and web implementations
- **Data Service Abstraction** - Unified `IInventoryDataService` interface with platform-specific implementations
- **Shared UI Components** - Blazor components shared across mobile and web platforms for consistent UX
- **Form Validation** - DataAnnotations-based validation for inventory items with client-side and server-side validation
- **Connection Resilience** - Automatic reconnection handling for SignalR connections with user-friendly modal dialogs
- **State Persistence** - .NET 10 PersistentState feature for seamless server-to-client state handoff
- **Platform-Aware SignalR** - Intelligent URL resolution for development and production across iOS, MacCatalyst, and Web platforms

## 🏗️ Architecture

SmartWarehouse follows a multi-platform architecture that maximizes code reuse while providing platform-specific optimizations.

### Multi-Platform Architecture

The application consists of three main projects:

- **SmartWarehouse.MAUI** - .NET MAUI application targeting iOS, Android, MacCatalyst, and Windows platforms
- **SmartWarehouse.Web** - Blazor Server and WebAssembly web application with REST API
- **SmartWarehouse.Shared** - Shared library containing reusable Blazor components, models, and service interfaces

### Component Model

The shared component model allows UI components to be written once and run on both mobile and web platforms:

- **Shared Blazor Components** - Components in `SmartWarehouse.Shared/Components` are used across all platforms
- **Platform-Specific Services** - Services implement shared interfaces (e.g., `IBarcodeScanner`) with platform-specific logic
- **Dependency Injection** - Platform-specific implementations are registered at application startup

### Real-Time Synchronization

SignalR `InventoryHub` enables real-time inventory updates:

- Clients connect to the hub for bidirectional communication
- Inventory changes are broadcast to all connected clients (excluding the sender)
- Platform-aware URL resolution handles development and production scenarios
- Connection lifecycle management prevents duplicate initializations
- Automatic reconnection handling ensures reliable connectivity

### REST API Architecture

The web application exposes a RESTful API using FastEndpoints:

- **Feature-Based Organization** - Endpoints organized by feature in `Features/Inventory/`
- **Swagger/OpenAPI Documentation** - Auto-generated API documentation via FastEndpoints.Swagger
- **API Endpoints**:
  - `GET /api/inventory` - Retrieve all inventory items
  - `GET /api/inventory/{id}` - Retrieve inventory item by ID
  - `POST /api/inventory` - Create or update inventory item
- **In-Memory Data Store** - `FakeInventoryDb` provides thread-safe in-memory storage for development
- **Type-Safe Endpoints** - Strongly-typed request/response models with validation

### Data Service Layer

The application uses a unified data service interface for platform-specific implementations:

- **IInventoryDataService** - Shared interface defining inventory operations:
  - `GetItemsAsync()` - Retrieve all items
  - `GetItemByIdAsync(Guid id)` - Retrieve item by ID
  - `SaveItemAsync(InventoryItem item)` - Save or update item
- **Platform Implementations**:
  - `ApiInventoryService` - Web client implementation using HTTP API calls
  - `LocalInventoryService` - Mobile implementation using SQLite for offline-first support

### Platform-Specific Implementations

The application uses abstraction patterns for platform-specific features:

- **Barcode Scanning**: `IBarcodeScanner` interface with implementations:
  - `MobileBarcodeScanner` - Mobile platform implementation
  - `WebBarcodeScanner` - Web platform implementation using JavaScript interop

- **SignalR URL Resolution**: `HubUrlHelper` utility handles cross-platform URL resolution:
  - Automatically detects development vs production environments
  - Resolves localhost addresses for iOS Simulator and MacCatalyst
  - Handles port detection and scheme preservation (HTTP/HTTPS)
  - Supports both web and mobile platforms seamlessly

### .NET 10 Features

The application leverages .NET 10 capabilities:

- **PersistentState** - Automatically persists component state during server-to-client handoff, eliminating double-fetching
- **Interactive Render Modes** - Supports both Blazor Server and WebAssembly render modes
- **Nullable Reference Types** - Type safety enabled throughout the codebase

### Architecture Diagram

```mermaid
graph TB
    subgraph Clients["Client Platforms"]
        Mobile[".NET MAUI Mobile<br/>iOS/Android/MacCatalyst/Windows"]
        Web["Web Browser<br/>Blazor Server/WebAssembly"]
    end
    
    subgraph Shared["Shared Layer"]
        Components["Blazor Components<br/>ItemEditor.razor"]
        Models["Data Models<br/>InventoryItem.cs"]
        Interfaces["Service Interfaces<br/>IBarcodeScanner<br/>IInventoryDataService"]
        Helpers["Utility Services<br/>HubUrlHelper"]
    end
    
    subgraph Services["Platform Services"]
        MobileScanner["MobileBarcodeScanner"]
        WebScanner["WebBarcodeScanner"]
        LocalDataService["LocalInventoryService<br/>SQLite"]
        ApiDataService["ApiInventoryService<br/>HTTP Client"]
    end
    
    subgraph Server["Server Layer"]
        API["FastEndpoints REST API<br/>/api/inventory"]
        Hub["SignalR InventoryHub<br/>Real-time Updates"]
        WebApp["Blazor Server App"]
        DataStore["FakeInventoryDb<br/>In-Memory Store"]
    end
    
    Mobile --> Components
    Web --> Components
    Components --> Models
    Components --> Interfaces
    Components --> Helpers
    Interfaces --> MobileScanner
    Interfaces --> WebScanner
    Interfaces --> LocalDataService
    Interfaces --> ApiDataService
    Mobile --> MobileScanner
    Web --> WebScanner
    Mobile --> LocalDataService
    ApiDataService --> API
    Components --> Hub
    Components --> API
    Helpers --> Hub
    API --> DataStore
    Hub --> DataStore
    Hub --> Mobile
    Hub --> Web
    Web --> WebApp
```

## 🔌 API Documentation

The application exposes a RESTful API for inventory management. API documentation is available via Swagger UI when running the web application.

### Base URL
- Development: `http://localhost:5055` or `https://localhost:7229`
- Swagger UI: `http://localhost:5055/swagger` or `https://localhost:7229/swagger`

### Endpoints

#### Get All Inventory Items
```http
GET /api/inventory
```

**Response:** `200 OK`
```json
[
  {
    "id": "guid",
    "name": "string",
    "quantity": 0,
    "sku": "string"
  }
]
```

#### Get Inventory Item by ID
```http
GET /api/inventory/{id}
```

**Parameters:**
- `id` (path, required): Inventory item GUID

**Response:** `200 OK`
```json
{
  "id": "guid",
  "name": "string",
  "quantity": 0,
  "sku": "string"
}
```

**Error Response:** `404 Not Found` - Item not found

#### Create or Update Inventory Item
```http
POST /api/inventory
```

**Request Body:**
```json
{
  "id": "guid",  // Optional: empty GUID for new items
  "name": "string",
  "quantity": 0,
  "sku": "string"
}
```

**Response:** `200 OK`
```json
{
  "id": "guid",
  "name": "string",
  "quantity": 0,
  "sku": "string"
}
```

**Validation:**
- `name`: Required, non-empty string
- `quantity`: Required, integer between 1 and 1000
- `sku`: Required, non-empty string

### Real-Time Updates

Inventory changes are broadcast to all connected clients via SignalR:

**Hub:** `/inventoryhub`

**Event:** `ReceiveItemUpdate`
```json
{
  "itemId": "guid",
  "quantity": 0
}
```

## 📋 Prerequisites

### Required
- **.NET SDK 10.0** or later ([Download](https://dotnet.microsoft.com/download))
- **Git**

### Platform-Specific Requirements

#### macOS (for iOS/MacCatalyst/Android)
- **Xcode 26.2** or later ([App Store](https://apps.apple.com/us/app/xcode/id497799835))
- **Java SDK 17** (Microsoft OpenJDK 17 recommended)
- **Android SDK** (for Android development)

#### Windows (for Windows/Android)
- **Visual Studio 2022** with .NET MAUI workload
- **Windows SDK**

> 📖 **Detailed Setup Instructions:** See [Environment Setup Guide](docs/environment-setup.md) for complete installation and configuration steps.

## 🛠️ Quick Start

### 1. Clone the Repository
```bash
git clone <repository-url>
cd smartwarehouse
```

### 2. Install .NET MAUI Workload
```bash
# macOS/Linux
sudo dotnet workload install maui

# Windows
dotnet workload install maui
```

### 3. Restore Dependencies
```bash
dotnet restore
```

### 4. Build the Solution
```bash
dotnet build
```

### 5. Run the Application

**Mobile (iOS - macOS only):**
```bash
dotnet build SmartWarehouse.MAUI/SmartWarehouse.MAUI.csproj -f net10.0-ios26.2
# Then run from Xcode or IDE
```

**Mobile (MacCatalyst - macOS only):**
```bash
dotnet build SmartWarehouse.MAUI/SmartWarehouse.MAUI.csproj -f net10.0-maccatalyst26.2
# Then run from IDE
```

**Mobile (Android):**
```bash
dotnet build SmartWarehouse.MAUI/SmartWarehouse.MAUI.csproj -f net10.0-android
# Then run on emulator or device
```

**Web:**
```bash
cd SmartWarehouse.Web/SmartWarehouse.Web
dotnet run
```

Navigate to:
- Web Application: `https://localhost:7229` or `http://localhost:5055`
- Swagger API Documentation: `http://localhost:5055/swagger` or `https://localhost:7229/swagger`

## 📁 Project Structure

```
SmartWarehouse/
├── SmartWarehouse.MAUI/            # .NET MAUI mobile application
│   ├── Components/                 # Blazor components for mobile
│   │   ├── Pages/                  # Mobile-specific pages
│   │   └── Routes.razor            # Routing configuration
│   ├── Platforms/                  # Platform-specific implementations
│   │   ├── Android/                # Android platform code
│   │   ├── iOS/                    # iOS platform code
│   │   ├── MacCatalyst/            # MacCatalyst platform code
│   │   └── Windows/                # Windows platform code
│   ├── Resources/                  # App icons, images, fonts
│   ├── Services/                   # Mobile-specific services
│   │   ├── MobileBarcodeScanner.cs # Mobile barcode scanner implementation
│   │   └── LocalInventoryService.cs # SQLite-based local inventory service
│   ├── MauiProgram.cs             # MAUI app configuration and DI setup
│   └── SmartWarehouse.MAUI.csproj
│
├── SmartWarehouse.Shared/          # Shared components and models
│   ├── Components/                 # Reusable Blazor components
│   │   ├── Layout/                 # Layout components (MainLayout, NavMenu, ReconnectModal)
│   │   └── Pages/                  # Shared pages
│   │       ├── ItemEditor.razor    # Inventory item editor with barcode scanning
│   │       └── Counter.razor      # Example counter component
│   ├── Models/                     # Data models
│   │   └── InventoryItem.cs        # Inventory item model with validation
│   ├── Services/                   # Shared services/interfaces
│   │   ├── IBarcodeScanner.cs      # Barcode scanning abstraction interface
│   │   ├── IInventoryDataService.cs # Inventory data service interface
│   │   └── HubUrlHelper.cs         # Cross-platform SignalR URL resolution utility
│   └── SmartWarehouse.Shared.csproj
│
├── SmartWarehouse.Web/             # Web application
│   ├── SmartWarehouse.Web/         # Blazor Server project
│   │   ├── Components/             # Server-side components
│   │   │   ├── Pages/              # Web pages (Home, Weather, Error, NotFound)
│   │   │   └── Layout/             # Layout components
│   │   ├── Features/               # Feature-based API endpoints
│   │   │   └── Inventory/         # Inventory management endpoints
│   │   │       ├── GetList/       # GET /api/inventory endpoint
│   │   │       ├── GetById/       # GET /api/inventory/{id} endpoint
│   │   │       └── SaveItem/      # POST /api/inventory endpoint
│   │   ├── Data/                   # Data access layer
│   │   │   └── FakeInventoryDb.cs # In-memory database implementation
│   │   ├── Hubs/                   # SignalR hubs
│   │   │   └── InventoryHub.cs     # Real-time inventory update hub
│   │   └── Program.cs              # Web app configuration, DI, and API setup
│   └── SmartWarehouse.Web.Client/  # Blazor WebAssembly project
│       └── Services/               # Client-side services
│           ├── WebBarcodeScanner.cs # Web barcode scanner implementation
│           └── ApiInventoryService.cs # HTTP API client service
│
├── docs/                           # Documentation
│   └── environment-setup.md        # Environment setup guide
│
└── SmartWarehouse.sln             # Solution file
```

## 🎯 Target Frameworks

| Project | Target Frameworks |
|---------|------------------|
| **SmartWarehouse.MAUI** | `net10.0-android`, `net10.0-ios26.2`, `net10.0-maccatalyst26.2`, `net10.0-windows10.0.19041.0` |
| **SmartWarehouse.Shared** | `net10.0` |
| **SmartWarehouse.Web** | `net10.0` |
| **SmartWarehouse.Web.Client** | `net10.0` |

## 📦 Key Dependencies

- **.NET MAUI** 10.0.0 - Cross-platform mobile framework
- **ASP.NET Core** 10.0.1 - Web application framework
- **Blazor** 10.0.1 - Web UI framework with Server and WebAssembly render modes
- **FastEndpoints** 7.1.1 - High-performance REST API framework
- **FastEndpoints.Swagger** 7.1.1 - Swagger/OpenAPI integration for FastEndpoints
- **SignalR** - Real-time bidirectional communication (included in ASP.NET Core)
- **SQLite** - Local database for mobile offline-first support
- **Microsoft.Extensions.Logging.Debug** 10.0.0 - Debug logging provider
- **Data Annotations** - Form validation (included in .NET)
- **Dependency Injection** - Service registration and resolution (included in .NET)

## 🔧 Development

### Building Specific Platforms

```bash
# Build iOS
dotnet build SmartWarehouse.MAUI/SmartWarehouse.MAUI.csproj -f net10.0-ios26.2

# Build MacCatalyst
dotnet build SmartWarehouse.MAUI/SmartWarehouse.MAUI.csproj -f net10.0-maccatalyst26.2

# Build Android
dotnet build SmartWarehouse.MAUI/SmartWarehouse.MAUI.csproj -f net10.0-android

# Build Web
dotnet build SmartWarehouse.Web/SmartWarehouse.Web/SmartWarehouse.Web.csproj
```

### Running Tests
```bash
# Add test commands here when tests are added
```

### Code Style
- Follow C# coding conventions
- Use nullable reference types (enabled)
- Prefer explicit types over `var` for clarity

## 🐛 Troubleshooting

### Common Issues

**Xcode Version Mismatch:**
- Ensure Xcode 26.2+ is installed for iOS/MacCatalyst development
- Verify with: `xcodebuild -version`

**Android SDK Not Found:**
- Install Android SDK following [Environment Setup Guide](docs/environment-setup.md)
- Set `ANDROID_HOME` environment variable

**Java SDK Not Found:**
- Install Microsoft OpenJDK 17
- Set `JAVA_HOME` environment variable

**Build Errors:**
- Run `dotnet clean` then `dotnet restore`
- Verify all workloads are installed: `dotnet workload list`

> 📖 **More Troubleshooting:** See [Environment Setup Guide](docs/environment-setup.md#troubleshooting) for detailed solutions.

## 📚 Documentation

- [Environment Setup Guide](docs/environment-setup.md) - Complete setup instructions for all platforms
- [.NET MAUI Documentation](https://learn.microsoft.com/dotnet/maui/)
- [Blazor Documentation](https://learn.microsoft.com/aspnet/core/blazor/)

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📝 License

[Add your license here]

## 👥 Authors

[Add author information here]

## 🙏 Acknowledgments

- Built with [.NET MAUI](https://dotnet.microsoft.com/apps/maui)
- UI components using [Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)

