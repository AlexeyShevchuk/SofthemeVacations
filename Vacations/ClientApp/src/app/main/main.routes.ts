import { Routes } from '@angular/router';

import { MainComponent } from './main.component';
import { VacationRequestComponent } from './components/vacation-request/vacation-request.component';
import { MyVacationsComponent } from './components/profile/my-vacations/my-vacations.component';
import { ProfileComponent } from './components/profile/profile.component';

export const MainRoutes: Routes = [
    {
        path: 'main',  component: MainComponent,
        children: [
            { path: '', component: ProfileComponent},
            { path: 'profile', component: ProfileComponent},             
        ]
    }
];
