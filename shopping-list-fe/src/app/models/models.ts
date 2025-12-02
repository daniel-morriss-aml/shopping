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
