import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateShoppingListDto, ShoppingList } from '../models/models';

@Injectable({
    providedIn: 'root',
})
export class ShoppingListService {
    private apiUrl = 'http://localhost:5162/api/shopping-lists';

    constructor(private http: HttpClient) {}

    getLists(): Observable<ShoppingList[]> {
        return this.http.get<ShoppingList[]>(this.apiUrl);
    }

    getList(id: number): Observable<ShoppingList> {
        return this.http.get<ShoppingList>(`${this.apiUrl}/${id}`);
    }

    createList(dto: CreateShoppingListDto): Observable<ShoppingList> {
        return this.http.post<ShoppingList>(this.apiUrl, dto);
    }

    updateList(id: number, dto: CreateShoppingListDto): Observable<ShoppingList> {
        return this.http.put<ShoppingList>(`${this.apiUrl}/${id}`, dto);
    }

    deleteList(id: number): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }
}
