import { Routes } from '@angular/router';
import { CreateList } from './create-list/create-list';

export const routes: Routes = [
    { path: '', redirectTo: '/create-list', pathMatch: 'full' },
    { path: 'create-list', component: CreateList },
];
