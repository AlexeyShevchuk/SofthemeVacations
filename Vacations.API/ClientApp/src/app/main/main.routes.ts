import { Routes } from '@angular/router';

import { MainComponent } from './main.component';
import { ProfileComponent } from './components/profile/profile.component';
import { EditProfileComponent } from './components/edit-profile/edit-profile.component';
import { ListOfVacationRequestsComponent } from './components/list-of-vacation-requests/list-of-vacation-requests.component';
import { AddNewTeamComponent } from './components/add-new-team/add-new-team.component';
import { EditTeamProfileComponent } from './components/edit-team-profile/edit-team-profile.component';
import { ListOfTeamsComponent } from './components/list-of-teams/list-of-teams.component';
import { RequestVacationComponent } from './components/request-vacation/request-vacation.component';
import { AddProfileComponent } from './components/add-profile/add-profile.component';

export const MainRoutes: Routes = [
    {
        path: 'main',  component: MainComponent,
        children: [
            { path: '', component: ProfileComponent },
            { path: 'profile', component: ProfileComponent },
            { path: 'vacation-requests', component: ListOfVacationRequestsComponent },
            { path: 'add-new-team', component: AddNewTeamComponent },
            { path: 'edit-team', component: EditTeamProfileComponent },
            { path: 'edit-profile', component: EditProfileComponent },
            { path: 'list-of-teams', component: ListOfTeamsComponent},
            { path: 'request-vacation', component: RequestVacationComponent },
            { path: 'add-new-employee', component: AddProfileComponent }
        ]
    }
];
