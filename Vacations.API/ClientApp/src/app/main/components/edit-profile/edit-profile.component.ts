import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { ToastrService } from 'ngx-toastr';

import { EditService } from '../../services/edit.service';
import { Employee } from './models/employee.model';
import { Team } from './models/team.model';
import { JobTitle } from './models/job-title.model';
import { EmployeeStatus } from './models/employee-status.model';

import { employeeRole } from './models/employee-roles.model';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.scss']
})
export class EditProfileComponent implements OnInit {

  employee: Employee = <Employee>{};
  teams: Team[] = [];
  jobTitles: JobTitle[] = [];
  employeeStatuses: EmployeeStatus[] = [];
  employeeRoles: employeeRole[] =[];
  date = new Date;

  constructor(private location: Location, private service: EditService, private toast: ToastrService) { }

  cancel() {
    this.location.back();
  }

  ngOnInit() {
    const successfnEmployee = (response) => {
      this.employee = response;
      this.toast.success("", "");
      console.log(response);
      console.log(this.employee);
    };
    const successfnTeams = (response) => {
      this.teams = response;
      this.toast.success("", "");
      console.log(response);
      console.log(this.teams);
    };
    const successfnJobTitles = (response) => {
      this.jobTitles = response;
      this.toast.success("", "");
      console.log(response);
      console.log(this.jobTitles);
    };
    const successfnEmployeeStatus = (response) => {
      this.employeeStatuses = response;
      this.toast.success("", "");
      console.log(response);
      console.log(this.employeeStatuses);
    };
    const successfnEmployeeRole = (response) => {
      this.employeeRoles = response;
      this.toast.success("", "");
      console.log(response);
      console.log(this.employeeStatuses);
    };

    const errorfn = () => { };
    const completefn = () => { };

    this.service.getEmployee().subscribe(successfnEmployee, errorfn, completefn);
    this.service.getTeam().subscribe(successfnTeams, errorfn, completefn);
    this.service.getJobTitle().subscribe(successfnJobTitles, errorfn, completefn);
    this.service.getEmployeeStatus().subscribe(successfnEmployeeStatus, errorfn, completefn);
    //this.service.getEmployeeRole().subscribe(successfnEmployeeRole, errorfn, completefn);
  }

  Save() {
    console.log(this.employee);
    this.service.updateEmployee(this.employee).subscribe(response => this.employee = response);;
    this.location.back();
    this.toast.success("You successfully edit profile", "");
    console.log(this.employeeStatuses);
  }
}
