import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { CdkTableModule } from '@angular/cdk/table';
import { CdkTreeModule } from '@angular/cdk/tree';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
  MatAutocompleteModule,
  MatBadgeModule,
  MatBottomSheetModule,
  MatButtonModule,
  MatButtonToggleModule,
  MatCardModule,
  MatCheckboxModule,
  MatDatepickerModule,
  MatDialogModule,
  MatDividerModule,
  MatExpansionModule,
  MatGridListModule,
  MatInputModule,
  MatListModule,
  MatMenuModule,
  MatNativeDateModule,
  MatPaginatorModule,
  MatProgressBarModule,
  MatProgressSpinnerModule,
  MatRadioModule,
  MatRippleModule,
  MatSelectModule,
  MatSidenavModule,
  MatSliderModule,
  MatSlideToggleModule,
  MatSnackBarModule,
  MatSortModule,
  MatStepperModule,
  MatTableModule,
  MatTabsModule,
  MatToolbarModule,
  MatTooltipModule,
  MatTreeModule,
} from '@angular/material';
import { BrowserModule } from '@angular/platform-browser';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  exports: [
    CdkTableModule,
    CdkTreeModule,
    MatAutocompleteModule,
    MatBadgeModule,
    MatBottomSheetModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatCardModule,
    MatCheckboxModule,
    MatChipsModule,
    MatStepperModule,
    MatDatepickerModule,
    MatDialogModule,
    MatDividerModule,
    MatExpansionModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatNativeDateModule,
    MatPaginatorModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatRadioModule,
    MatRippleModule,
    MatSelectModule,
    MatSidenavModule,
    MatSliderModule,
    MatSlideToggleModule,
    MatSnackBarModule,
    MatSortModule,
    MatTableModule,
    MatTabsModule,
    MatToolbarModule,
    MatTooltipModule,
    MatTreeModule,
  ]
})

export class DemoMaterialModule { }

import { LeftNavigationComponent } from './components/left-navigation/left-navigation.component';
import { MainComponent } from './main.component';

import { ProfileService } from './services/profile.service';
import { VacationService } from './services/vacation.service';

import { MyProfileComponent } from './components/profile/my-profile/my-profile.component';
import { TopNavigationComponent } from './components/top-navigation/top-navigation.component';
import { RequestVacationComponent } from './components/request-vacation/request-vacation.component';
import { MyVacationsComponent } from './components/profile/my-vacations/my-vacations.component';
import { BannerComponent } from './components/profile/banner/banner.component';
import { ProfileComponent } from './components/profile/profile.component';
import { MainRoutes } from './main.routes';
import { EditProfileComponent } from './components/edit-profile/edit-profile.component';
import { MyFirstInterceptor } from '../app.service';
import { AddNewTeamComponent } from './components/add-new-team/add-new-team.component';
import { EditTeamProfileComponent } from './components/edit-team-profile/edit-team-profile.component';
import { ListOfVacationRequestsComponent } from './components/list-of-vacation-requests/list-of-vacation-requests.component';
import { ListOfTeamsComponent } from './components/list-of-teams/list-of-teams.component';
import { EditService } from './services/edit.service';
import { MatChipsModule, MatIconModule } from '../../../node_modules/@angular/material';
import { VacationRequestComponent } from './components/request-vacation/vacation-request/vacation-request.component';
import { AddProfileComponent } from './components/add-profile/add-profile.component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    HttpClientModule,
    MatChipsModule,
    MatIconModule,
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    HttpClientModule,
    DemoMaterialModule,
    MatNativeDateModule,
    ReactiveFormsModule,

    RouterModule.forChild(MainRoutes),
  ],
  declarations: [
    BannerComponent,
    LeftNavigationComponent,
    MainComponent,
    MyProfileComponent,
    TopNavigationComponent,
    RequestVacationComponent,
    EditProfileComponent,
    AddProfileComponent,
    MyVacationsComponent,
    ProfileComponent,
    AddNewTeamComponent,
    ListOfVacationRequestsComponent,
    ListOfTeamsComponent,
    VacationRequestComponent,
    EditTeamProfileComponent
  ],
  providers: [
    ProfileService,
    VacationService,
    EditService
  ]

})
export class MainModule { }
