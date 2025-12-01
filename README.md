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

The API will run at `http://localhost:5162`.

**Testing the API:**

-   Swagger UI: `http://localhost:5162/swagger`
-   Test files: Use the `.http` files in `shopping-list-api/` folder

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
