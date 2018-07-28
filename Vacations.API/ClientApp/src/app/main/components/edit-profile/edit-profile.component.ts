import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { EditService } from '../../services/edit.service';
import { Employee } from './models/employee.model';
import { Team } from './models/team.model';
import { JobTitle } from './models/job-title.model';
import { EmployeeStatus } from './models/employee-status.model';
import { ToastrService } from 'ngx-toastr';
import { ImageService } from '../../services/image.service';
import { environment } from '../../../../environments/environment.prod';

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

  constructor(private imgUploadService: ImageService, private location: Location, private service: EditService, private toast: ToastrService) { }

  fileToUpload: File = null;

  handleFileInput(files: FileList) {
    this.fileToUpload = files.item(0);
  }

 uploadFileToActivity() {
  this.imgUploadService.postFile("http://localhost:2076/api" + "/images/upload",this.fileToUpload).subscribe(data => {
    this.toast.success("File uploaded!","Success")
    }, error => {
      console.log(error);
    });
 }
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
}

// export class EditProfileComponent implements OnInit {
//   title = 'profile';
//   employee: Employee;

//   constructor(private service: EditService) { }

//   ngOnInit() { }
//   //save(): void {
//   //  this.service.updateEmployee(this.employee)
//   //    .subscribe(() => this.location.back();;
//   //}

// }
