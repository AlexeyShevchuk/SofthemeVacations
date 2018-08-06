import { Routes } from '@angular/router';

import { MainComponent } from './main.component';
import { ProfileComponent } from './components/profile/profile.component';
import { EditProfileComponent } from './components/edit-profile/edit-profile.component';
import { ListOfVacationRequestsComponent } from './components/list-of-vacation-requests/list-of-vacation-requests.component';
import { AddNewTeamComponent } from './components/add-new-team/add-new-team.component';
import { EditTeamProfileComponent } from './components/edit-team-profile/edit-team-profile.component';
import { ListOfTeamsComponent } from './components/list-of-teams/list-of-teams.component';
import { RequestVacationComponent } from './components/request-vacation/request-vacation.component';
import { ListOfEmployeesComponent } from './components/list-of-employees/list-of-employees.component';
import { AddNewEmployeeComponent } from './components/add-new-employee/add-new-employee.component';
import { TeamCalendarComponent } from './components/team-calendar/team-calendar.component';
import { AuthGuardService } from '../auth/auth-guard.service';
import { RoleGuardService } from '../auth/role-guard.service';

export const MainRoutes: Routes = [
  {
    path: '',
    component: MainComponent,
    canActivate: [AuthGuardService],
    children: [
      { path: '', component: ProfileComponent },
      {
        path: 'profile', component: ProfileComponent,
        canActivate: [AuthGuardService]
      },
      { path: 'vacation-requests', canActivate: [RoleGuardService], data: { expectedAdminRole: 'Admin', expectedTeamLeadRole: 'TeamLead' }, component: ListOfVacationRequestsComponent },
      { path: 'add-new-team', canActivate: [RoleGuardService], data: { expectedAdminRole: 'Admin' }, component: AddNewTeamComponent },
      { path: 'edit-team/:id', canActivate: [RoleGuardService], data: { expectedAdminRole: 'Admin' }, component: EditTeamProfileComponent },
      { path: 'edit-profile', canActivate: [RoleGuardService], data: { expectedAdminRole: 'Admin' }, component: EditProfileComponent },
      { path: 'edit-profile/:id', canActivate: [RoleGuardService], data: { expectedAdminRole: 'Admin' }, component: EditProfileComponent },
      { path: 'list-of-teams', canActivate: [RoleGuardService], data: { expectedAdminRole: 'Admin' }, component: ListOfTeamsComponent },
      { path: 'request-vacation', component: RequestVacationComponent },
      { path: 'list-of-employees', canActivate: [RoleGuardService], data: { expectedAdminRole: 'Admin', expectedTeamLeadRole: 'TeamLead' }, component: ListOfEmployeesComponent },
      { path: 'add-new-employee', canActivate: [RoleGuardService], data: { expectedAdminRole: 'Admin' }, component: AddNewEmployeeComponent },
      { path: 'team-calendar', canActivate: [RoleGuardService], data: { expectedAdminRole: 'Admin', expectedTeamLeadRole: 'TeamLead' }, component: TeamCalendarComponent }
    ]
  }
];
