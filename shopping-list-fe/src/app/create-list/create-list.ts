import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { User } from '../models/models';
import { ShoppingListService } from '../services/shopping-list.service';
import { UserService } from '../services/user.service';

@Component({
    selector: 'app-create-list',
    imports: [CommonModule, FormsModule],
    templateUrl: './create-list.html',
    styleUrl: './create-list.scss',
})
export class CreateList implements OnInit {
    listName = '';
    availableUsers: User[] = [];
    selectedUserIds: number[] = [];
    loading = false;
    error = '';

    constructor(
        private shoppingListService: ShoppingListService,
        private userService: UserService,
        private router: Router
    ) {}

    ngOnInit() {
        this.loadUsers();
    }

    loadUsers() {
        this.userService.getUsers().subscribe({
            next: (users) => {
                this.availableUsers = users;
            },
            error: (err) => {
                console.error('Error loading users:', err);
                this.error = 'Failed to load users';
            },
        });
    }

    toggleUser(userId: number) {
        const index = this.selectedUserIds.indexOf(userId);
        if (index > -1) {
            this.selectedUserIds.splice(index, 1);
        } else {
            this.selectedUserIds.push(userId);
        }
    }

    isUserSelected(userId: number): boolean {
        return this.selectedUserIds.includes(userId);
    }

    onSubmit() {
        if (!this.listName.trim()) {
            this.error = 'Please enter a list name';
            return;
        }

        if (this.selectedUserIds.length === 0) {
            this.error = 'Please select at least one user to share with';
            return;
        }

        this.loading = true;
        this.error = '';

        this.shoppingListService
            .createList({
                name: this.listName.trim(),
                sharedWithUserIds: this.selectedUserIds,
            })
            .subscribe({
                next: (list) => {
                    console.log('List created:', list);
                    this.router.navigate(['/']);
                },
                error: (err) => {
                    console.error('Error creating list:', err);
                    this.error = 'Failed to create list. Please try again.';
                    this.loading = false;
                },
            });
    }

    cancel() {
        this.router.navigate(['/']);
    }
}
