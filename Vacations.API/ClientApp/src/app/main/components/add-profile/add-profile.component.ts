import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { EditService } from '../../services/edit.service';
import { Employee } from './models/employee.model';
import { Team } from './models/team.model';
import { JobTitle } from './models/job-title.model';
import { EmployeeStatus } from './models/employee-status.model';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-add-profile',
  templateUrl: './add-profile.component.html',
  styleUrls: ['./add-profile.component.scss']
})
export class AddProfileComponent implements OnInit {

  employee: Employee = <Employee>{};
  teams: Team[] = [];
  jobTitles: JobTitle[] = [];
  employeeStatuses: EmployeeStatus[] = [];

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

    const errorfn = () => { };
    const completefn = () => { };

    this.service.getEmployee().subscribe(successfnEmployee, errorfn, completefn);
    this.service.getTeam().subscribe(successfnTeams, errorfn, completefn);
    this.service.getJobTitle().subscribe(successfnJobTitles, errorfn, completefn);
    this.service.getEmployeeStatus().subscribe(successfnEmployeeStatus, errorfn, completefn);
  }

  Save() {
    this.service.updateEmployee(this.employee);
    this.location.back();
  }
}
