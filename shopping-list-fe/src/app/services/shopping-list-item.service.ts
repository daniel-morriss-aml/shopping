import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
    CreateShoppingListItemDto,
    ShoppingListItem,
    UpdateShoppingListItemDto,
} from '../models/models';

@Injectable({
    providedIn: 'root',
})
export class ShoppingListItemService {
    private apiUrl = 'http://localhost:5162/api';

    constructor(private http: HttpClient) {}

    getItemsForList(listId: number): Observable<ShoppingListItem[]> {
        return this.http.get<ShoppingListItem[]>(`${this.apiUrl}/shopping-lists/${listId}/items`);
    }

    getItem(id: number): Observable<ShoppingListItem> {
        return this.http.get<ShoppingListItem>(`${this.apiUrl}/shopping-list-items/${id}`);
    }

    createItem(dto: CreateShoppingListItemDto): Observable<ShoppingListItem> {
        return this.http.post<ShoppingListItem>(`${this.apiUrl}/shopping-list-items`, dto);
    }

    updateItem(id: number, dto: UpdateShoppingListItemDto): Observable<ShoppingListItem> {
        return this.http.put<ShoppingListItem>(`${this.apiUrl}/shopping-list-items/${id}`, dto);
    }

    deleteItem(id: number): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/shopping-list-items/${id}`);
    }
}
