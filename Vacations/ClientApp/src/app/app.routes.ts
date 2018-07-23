import { Routes } from '@angular/router';
import { AuthComponent } from './auth/auth.component';
import { MainComponent } from './main/main.component';

export const AppRoutes: Routes = [
    { path: 'auth', component: AuthComponent },
    { path: '', redirectTo: 'main', pathMatch: 'full' },
    { path: 'main', component: MainComponent },
    { path: '**', component: AuthComponent }
];
