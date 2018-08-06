import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { ToastrService } from 'ngx-toastr';

import { EditService } from '../../services/edit.service';
import { ImageService } from '../../services/image.service';
import { environment } from '../../../../environments/environment';

import { Team } from '../edit-profile/models/team.model';
import { JobTitle } from '../edit-profile/models/job-title.model';
import { EmployeeStatus } from '../edit-profile/models/employee-status.model';
import { Employee } from '../edit-profile/models/employee.model';
import { EmployeeRole } from '../edit-profile/models/employee-roles.model';
import { UUID } from 'angular2-uuid';

@Component({
  selector: 'app-add-new-employee',
  templateUrl: './add-new-employee.component.html',
  styleUrls: ['./add-new-employee.component.scss']
})
export class AddNewEmployeeComponent implements OnInit {

  employee: Employee = <Employee>{};
  teams: Team[] = [];
  jobTitles: JobTitle[] = [];
  employeeStatuses: EmployeeStatus[] = [];
  employeeRoles: EmployeeRole[] = [];
  date = new Date;

  constructor(private imgUploadService: ImageService,
    private location: Location,
    private service: EditService,
    private toast: ToastrService) { }

  fileToUpload: File = null;
  newFileName: string;
  imgUrl: string;

  handleFileInput(files: FileList) {
    this.fileToUpload = files.item(0);
  }

  uploadFileToActivity() {
    this.imgUploadService.postFile(environment.baseUrl + "/images/upload", this.fileToUpload, this.newFileName).subscribe(data => {
      this.toast.success("File uploaded!", "Success")
    }, error => {
      console.log(error);
    });
  }


  cancel() {
    this.location.back();
  }

  ngOnInit() {
    this.imgUrl = '../../../../assets/user-profile-icon.svg'
    
    const successfnTeams = (response) => {
      this.teams = response;
      console.log(response);
      console.log(this.teams);
    };
    const successfnJobTitles = (response) => {
      this.jobTitles = response;
      console.log(response);
      console.log(this.jobTitles);
    };
    const successfnEmployeeStatus = (response) => {
      this.employeeStatuses = response;
      console.log(response);
      console.log(this.employeeStatuses);
    };
    const successfnEmployeeRole = (response) => {
      this.employeeRoles = response;
      this.toast.success("", "");
      console.log(response);
      console.log(this.employeeRoles);
    };

    const errorfn = () => { };
    const completefn = () => { };

    this.service.getTeam().subscribe(successfnTeams, errorfn, completefn);
    this.service.getJobTitle().subscribe(successfnJobTitles, errorfn, completefn);
    this.service.getEmployeeStatus().subscribe(successfnEmployeeStatus, errorfn, completefn);
    this.service.getEmployeeRole().subscribe(successfnEmployeeRole, errorfn, completefn);
  }

  Save() {
    console.log(this.employee);
    console.log(this.fileToUpload);
    if (this.fileToUpload != null) {
      this.newFileName = UUID.UUID()
      this.employee.ImgUrl = this.newFileName;
      this.uploadFileToActivity();
    }
    this.service.addEmployee(this.employee).subscribe(response => {
      this.employee = response;
      this.location.back();
     this.toast.success("You successfully added new profile", "");
     console.log(this.employeeStatuses);
    });
  }
}
