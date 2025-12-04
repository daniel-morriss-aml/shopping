import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Router } from '@angular/router';
import { User } from '../models/models';
import { ShoppingListService } from '../services/shopping-list.service';
import { UserService } from '../services/user.service';

@Component({
    selector: 'app-create-list',
    imports: [
        CommonModule,
        FormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        MatCheckboxModule,
        MatCardModule,
        MatProgressSpinnerModule,
        MatListModule,
    ],
    templateUrl: './create-list.html',
    styleUrl: './create-list.scss',
})
export class CreateList implements OnInit {
    private shoppingListService = inject(ShoppingListService);
    private userService = inject(UserService);
    private router = inject(Router);

    listName = signal('');
    availableUsers = signal<User[]>([]);
    selectedUserIds = signal<number[]>([]);
    loading = signal(false);
    loadingUsers = signal(true);
    error = signal('');

    ngOnInit() {
        this.loadUsers();
    }

    loadUsers() {
        this.loadingUsers.set(true);
        this.userService.getUsers().subscribe({
            next: (users) => {
                this.availableUsers.set(users);
                this.loadingUsers.set(false);
            },
            error: (err) => {
                console.error('Error loading users:', err);
                this.error.set('Failed to load users: ' + (err.message || JSON.stringify(err)));
                this.loadingUsers.set(false);
            },
        });
    }

    toggleUser(userId: number) {
        const currentIds = this.selectedUserIds();
        const index = currentIds.indexOf(userId);
        if (index > -1) {
            this.selectedUserIds.set(currentIds.filter((id) => id !== userId));
        } else {
            this.selectedUserIds.set([...currentIds, userId]);
        }
    }

    isUserSelected(userId: number): boolean {
        return this.selectedUserIds().includes(userId);
    }

    onSubmit() {
        if (!this.listName().trim()) {
            this.error.set('Please enter a list name');
            return;
        }

        if (this.selectedUserIds().length === 0) {
            this.error.set('Please select at least one user to share with');
            return;
        }

        this.loading.set(true);
        this.error.set('');

        this.shoppingListService
            .createList({
                name: this.listName().trim(),
                sharedWithUserIds: this.selectedUserIds(),
            })
            .subscribe({
                next: (list) => {
                    console.log('List created:', list);
                    this.router.navigate(['/']);
                },
                error: (err) => {
                    console.error('Error creating list:', err);
                    this.error.set('Failed to create list. Please try again.');
                    this.loading.set(false);
                },
            });
    }

    cancel() {
        this.router.navigate(['/']);
    }
}
