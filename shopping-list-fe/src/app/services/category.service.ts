import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Category, CreateCategoryDto, UpdateCategoryDto } from '../models/models';

@Injectable({
    providedIn: 'root',
})
export class CategoryService {
    private apiUrl = 'http://localhost:5162/api/categories';

    constructor(private http: HttpClient) {}

    getCategories(): Observable<Category[]> {
        return this.http.get<Category[]>(this.apiUrl);
    }

    getCategory(id: number): Observable<Category> {
        return this.http.get<Category>(`${this.apiUrl}/${id}`);
    }

    createCategory(dto: CreateCategoryDto): Observable<Category> {
        return this.http.post<Category>(this.apiUrl, dto);
    }

    updateCategory(id: number, dto: UpdateCategoryDto): Observable<Category> {
        return this.http.put<Category>(`${this.apiUrl}/${id}`, dto);
    }

    deleteCategory(id: number): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }
}
