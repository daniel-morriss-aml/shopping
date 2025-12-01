# Shopping List App

A full-stack shopping list application built with Angular and .NET

## Project Structure

```
shopping/
├── shopping-list-fe/     # Angular v21 Frontend
└── shopping-list-api/    # .NET Web API Backend
```

## Prerequisites

-   Node.js 20+
-   npm 10+
-   .NET 8 SDK

## Getting Started

### Frontend (Angular)

```bash
cd shopping-list-fe
npm install
npm start
```

The Angular app will run at `http://localhost:4200`.

### Backend (.NET)

```bash
cd shopping-list-api
dotnet run
```

The API will run at `http://localhost:5000` (or the port specified in launchSettings).

## Development

### Frontend Commands

-   `npm start` - Start development server
-   `npm run build` - Build for production
-   `npm test` - Run unit tests
-   `npm run lint` - Run linting

### Backend Commands

-   `dotnet run` - Start the API
-   `dotnet build` - Build the project
-   `dotnet test` - Run unit tests
