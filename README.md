# Shopping List App

A full-stack shopping list application built with Angular and .NET for managing shared shopping lists.

## Project Structure

```
shopping/
├── shopping-list-fe/     # Angular v21 Frontend
└── shopping-list-api/    # .NET 8 Web API Backend with SQLite
```

## Prerequisites

-   Node.js 20+
-   npm 10+
-   .NET 8 SDK

## Database Schema

The application uses SQLite with Entity Framework Core and includes:

-   **Users** - User accounts with email, password, admin flag
-   **Categories** - Item categories (Produce, Dairy, Meat, etc.)
-   **ShoppingLists** - Shopping list containers with shared access
-   **ShoppingListItems** - Individual items with quantity, checked status, category
-   **ShoppingListUsers** - Many-to-many relationship for list sharing

## API Endpoints

### Categories (`/api/categories`)

-   `GET /api/categories` - List all categories
-   `GET /api/categories/{id}` - Get category by ID
-   `POST /api/categories` - Create new category
-   `PUT /api/categories/{id}` - Update category
-   `DELETE /api/categories/{id}` - Delete category

### Users (`/api/users`)

-   `GET /api/users` - List all users
-   `GET /api/users/{id}` - Get user by ID
-   `POST /api/users` - Create new user
-   `PUT /api/users/{id}` - Update user
-   `DELETE /api/users/{id}` - Delete user

### Shopping Lists (`/api/shopping-lists`)

-   `GET /api/shopping-lists` - List all shopping lists
-   `GET /api/shopping-lists/{id}` - Get shopping list by ID
-   `POST /api/shopping-lists` - Create new shopping list
-   `PUT /api/shopping-lists/{id}` - Update shopping list
-   `DELETE /api/shopping-lists/{id}` - Delete shopping list

### Shopping List Items (`/api/shopping-list-items`)

-   `GET /api/shopping-lists/{listId}/items` - Get items for a list
-   `GET /api/shopping-list-items/{id}` - Get item by ID
-   `POST /api/shopping-list-items` - Create new item
-   `PUT /api/shopping-list-items/{id}` - Update item
-   `DELETE /api/shopping-list-items/{id}` - Delete item

## Getting Started

### Backend (.NET) - Start First

**Important:** The backend must be started before the frontend to ensure the API is available.

1. Navigate to the backend directory:

```bash
cd shopping-list-api
```

2. Build the project (first time or after code changes):

```bash
dotnet build
```

3. Run the API:

```bash
dotnet run
```

The API will start at `http://localhost:5162`.

**Verify it's running:**

-   Open `http://localhost:5162/swagger` in your browser
-   You should see the Swagger API documentation

### Frontend (Angular) - Start After Backend

1. Open a new terminal and navigate to the frontend directory:

```bash
cd shopping-list-fe
```

2. Install dependencies (first time only):

```bash
npm install
```

3. Start the development server:

```bash
npm start
```

The Angular app will run at `http://localhost:4200`.

**Note:** The frontend will make API calls to `http://localhost:5162`, so ensure the backend is running.

### Testing the API

-   Swagger UI: `http://localhost:5162/swagger`
-   Test files: Use the `.http` files in `shopping-list-api/` folder (requires VS Code REST Client extension)

### Creating Initial Data

Before using the app, you need to create users. Use one of these methods:

**Option 1: Using PowerShell**

```powershell
# Create first user
Invoke-RestMethod -Uri http://localhost:5162/api/users -Method Post -ContentType "application/json" -Body '{"email":"you@example.com","password":"password123","name":"Your Name","isAdmin":true}'

# Create second user
Invoke-RestMethod -Uri http://localhost:5162/api/users -Method Post -ContentType "application/json" -Body '{"email":"partner@example.com","password":"password123","name":"Partner Name","isAdmin":false}'
```

**Option 2: Using Swagger UI**

-   Navigate to `http://localhost:5162/swagger`
-   Find POST `/api/users` endpoint
-   Click "Try it out" and fill in the user details

**Option 3: Using .http files**

-   Open `shopping-list-api/Users.http` in VS Code
-   Click "Send Request" above the POST requests

## Development

### Frontend Commands

-   `npm start` - Start development server
-   `npm run build` - Build for production
-   `npm test` - Run unit tests
-   `npm run lint` - Run linting

### Backend Commands

-   `dotnet run` - Start the API
-   `dotnet build` - Build the project
-   `dotnet ef migrations add <name>` - Create new migration
-   `dotnet ef database update` - Apply migrations

### Database

The SQLite database file (`shoppinglist.db`) is created automatically on first run in the `shopping-list-api` folder.
