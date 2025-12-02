import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CreateList } from './create-list/create-list';

@Component({
    selector: 'app-root',
    imports: [RouterOutlet, CreateList],
    templateUrl: './app.html',
    styleUrl: './app.scss',
})
export class App {
    protected readonly title = signal('shopping-list-fe');
}
