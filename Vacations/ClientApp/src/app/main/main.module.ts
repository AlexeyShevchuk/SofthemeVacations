import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

import { LeftNavigationComponent } from './components/left-navigation/left-navigation.component';
import { MainComponent } from './main.component';

import { EmployeeService } from './services/employee.service';
import { VacationService } from './services/vacation.service';

import { MyProfileComponent } from './components/profile/my-profile/my-profile.component';
import { TopNavigationComponent } from './components/top-navigation/top-navigation.component';
import { VacationRequestComponent } from './components/vacation-request/vacation-request.component';
import { MyVacationsComponent } from './components/profile/my-vacations/my-vacations.component';
import { BannerComponent } from './components/profile/banner/banner.component';
import { ProfileComponent } from './components/profile/profile.component';
import { MainRoutes } from './main.routes';
import { EditProfileComponent } from './components/edit-profile/edit-profile.component';


@NgModule({
  imports: [ 
    CommonModule,
    RouterModule,
    HttpClientModule,

    RouterModule.forChild(MainRoutes),
  ],
  declarations: [
    BannerComponent,
    LeftNavigationComponent,
    MainComponent,
    MyProfileComponent,
    TopNavigationComponent,
    VacationRequestComponent,
    EditProfileComponent,

    MyVacationsComponent,
    ProfileComponent
  ],
  providers: [
    EmployeeService,
    VacationService
  ]

})
export class MainModule { }
