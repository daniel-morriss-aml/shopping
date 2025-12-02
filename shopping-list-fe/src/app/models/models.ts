export interface User {
    id: number;
    email: string;
    name: string;
    isAdmin: boolean;
    createdAt: string;
}

export interface CreateUserDto {
    email: string;
    password: string;
    name: string;
    isAdmin?: boolean;
}

export interface UpdateUserDto {
    email: string;
    name: string;
    isAdmin: boolean;
}

export interface Category {
    id: number;
    name: string;
    order: number;
    createdAt: string;
}

export interface CreateCategoryDto {
    name: string;
    order: number;
}

export interface UpdateCategoryDto {
    name: string;
    order: number;
}

export interface ShoppingList {
    id: number;
    name: string;
    createdAt: string;
    updatedAt: string;
    sharedWithUserIds: number[];
}

export interface CreateShoppingListDto {
    name: string;
    sharedWithUserIds: number[];
}

export interface UpdateShoppingListDto {
    name: string;
    sharedWithUserIds: number[];
}

export interface ShoppingListItem {
    id: number;
    shoppingListId: number;
    categoryId: number | null;
    categoryName: string;
    name: string;
    quantity: string | null;
    isChecked: boolean;
    addedBy: number;
    addedByName: string;
    createdAt: string;
    order: number;
}

export interface CreateShoppingListItemDto {
    shoppingListId: number;
    categoryId: number | null;
    name: string;
    quantity: string | null;
    addedBy: number;
    order: number;
}

export interface UpdateShoppingListItemDto {
    categoryId: number | null;
    name: string;
    quantity: string | null;
    isChecked: boolean;
    order: number;
}
